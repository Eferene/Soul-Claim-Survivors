using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public abstract class EnemyControllerBase<T> : MonoBehaviour, IEnemy where T : EnemyData
{
    public T enemyData;

    protected EnemyAttack attackType;
    protected Rigidbody2D rb;
    protected Transform playerTransform;
    protected Material mainMat;
    protected Transform wsCanvas;
    protected PlayerManager playerManager;
    protected SpriteRenderer spriteRenderer;

    // Cooldown
    [SerializeField] private float lastAttackTime;
    [SerializeField] private bool canAttack = true;

    public bool isAttacking = false;
    [SerializeField] private bool isFreezed = false;

    private bool isCritical = false;
    private float currentHealth;
    
    // Elite-specific stats (for this instance only)
    private int eliteAttackDamage;
    private float eliteSpeed;
    private int eliteExpGain;
    private int eliteScoreGain;
    private int eliteGoldGain;
    private bool isElite = false;

    [Header("Resources")]
    [SerializeField] Material flashMat;
    [SerializeField] TextMeshProUGUI damageTMP;
    [SerializeField] GameObject effectPrefab;
    [SerializeField] GameObject expPrefab;

    public bool IsDead => currentHealth <= 0;


    protected virtual void Awake()
    {
        attackType = GetComponent<EnemyAttack>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainMat = spriteRenderer.material;
        spriteRenderer.material = mainMat;
        wsCanvas = GameObject.FindGameObjectWithTag("WorldSpaceCanvas").transform;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) playerTransform = playerObj.transform;
        playerManager = playerObj.GetComponent<PlayerManager>();
    }

    protected virtual void OnEnable()
    {
        currentHealth = enemyData.baseHealth;
        canAttack = true;
        isAttacking = false;
        spriteRenderer.material = mainMat;
        playerManager.OnDamageHitOccurred += IsCritical;
    }

    protected virtual void Update()
    {
        // Cooldown ile vurup vuramayacağını kontrol etme.
        if (Time.time - lastAttackTime >= enemyData.attackCooldown)
        {
            canAttack = true;
        }

        // Basit saldırı
        if (attackType != null && canAttack && !isAttacking)
        {
            int damageToUse = isElite ? eliteAttackDamage : enemyData.attackDamage;
            bool attackSuccessful = attackType.Attack(transform, playerTransform, damageToUse, enemyData.attackDamagePercentage, enemyData.attackRange);
            if (attackSuccessful)
            {
                lastAttackTime = Time.time;
                canAttack = false;
            }
        }
    }

    protected virtual void FixedUpdate()
    {
        if (IsDead) return;
        Move();
    }

    protected virtual bool CanMove()
    {
        if (playerTransform == null) playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        
        return isFreezed == false && rb != null && playerTransform != null && enemyData != null;
    }

    protected void MoveTowardsPlayer()
    {
        if (CanMove())
        {
            Vector2 currentPos = rb.position;
            Vector2 targetPos = (Vector2)playerTransform.position;
            Vector2 direction = (targetPos - currentPos).normalized;
            if (direction.x < 0)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }
            else
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }

            float speedToUse = isElite ? eliteSpeed : enemyData.speed;
            rb.linearVelocity = direction * speedToUse;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    protected void KeepDistanceMovement()
    {
        if (!CanMove())
        {
            if (rb != null) rb.linearVelocity = Vector2.zero;
            return;
        }

        if (rb != null && playerTransform != null && enemyData != null && Vector2.Distance(rb.position, playerTransform.position) > enemyData.attackRange)
        {
            if (isAttacking) return;

            Vector2 currentPos = rb.position;
            Vector2 targetPos = (Vector2)playerTransform.position;
            Vector2 direction = (targetPos - currentPos).normalized;

            float speedToUse = isElite ? eliteSpeed : enemyData.speed;
            rb.linearVelocity = direction * speedToUse;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (!IsDead) StartCoroutine(Flash());
        AudioManager.Instance.PlayEnemyHurtSFX();

        TextMeshProUGUI newText = Instantiate(damageTMP, wsCanvas);
        newText.text = damage.ToString();
        if (isCritical) newText.color = Color.red;
        else newText.color = Color.white;
        newText.transform.position = transform.position + new Vector3(0, 1.5f, 0f);

        if (currentHealth <= 0)
            Die();
    }

    private void IsCritical(bool critical) => isCritical = critical;

    private void Die()
    {
        // Skor kazanç hesaplama
        int scoreGainValue = isElite ? eliteScoreGain : enemyData.scoreGain;
        float min = scoreGainValue * (1f - enemyData.scoreGainPercentage / 100f);
        float max = scoreGainValue * (1f + enemyData.scoreGainPercentage / 100f);
        float fScoreGain = Random.Range(min, max);
        int scoreGain = Mathf.RoundToInt(fScoreGain);
        playerManager.AddScore(scoreGain);

        // Altın kazanç hesaplama
        int goldGainValue = isElite ? eliteGoldGain : enemyData.goldGain;
        min = goldGainValue * (1f - enemyData.goldGainPercentage / 100f);
        max = goldGainValue * (1f + enemyData.goldGainPercentage / 100f);
        float fGoldGain = Random.Range(min, max);
        int goldGain = Mathf.RoundToInt(fGoldGain);
        playerManager.GainGold(goldGain);

        DieEffect();
        GetComponent<SpriteRenderer>().material = mainMat;

        Exp newExp = Instantiate(expPrefab, transform.position, Quaternion.identity).GetComponent<Exp>();
        int expGainValue = isElite ? eliteExpGain : enemyData.expGain;
        newExp.expAmount = expGainValue;
        EnemyPool.Instance.ReturnEnemyToPool(this.gameObject);
    }

    public void DieEffect()
    {
        GameObject newEffect = Instantiate(effectPrefab, transform.position, Quaternion.identity);

        var mainSettings = newEffect.GetComponent<ParticleSystem>().main;
        mainSettings.startColor = new ParticleSystem.MinMaxGradient(enemyData.colors[0], enemyData.colors[1]);

        Destroy(newEffect, 1f);
    }

    private IEnumerator Flash()
    {
        GetComponent<SpriteRenderer>().material = flashMat;
        yield return new WaitForSeconds(0.075f);
        GetComponent<SpriteRenderer>().material = mainMat;
    }

    private void OnDrawGizmosSelected()
    {
        if (enemyData == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + enemyData.attackOffset, enemyData != null ? enemyData.attackRange : 1f);
    }

    public virtual void ApplyFreeze(float duration)
    {
        StartCoroutine(FreezeCoroutine(duration));
    }

    IEnumerator FreezeCoroutine(float duration)
    {
        isFreezed = true;
        yield return new WaitForSeconds(duration);
        isFreezed = false;
    }

    public virtual void MakeElite()
    {
        isElite = true;
        currentHealth *= enemyData.healthMultiplier;
        eliteAttackDamage = Mathf.RoundToInt(enemyData.attackDamage * enemyData.damageMultiplier);
        eliteSpeed = enemyData.speed * enemyData.speedMultiplier;
        eliteExpGain = Mathf.RoundToInt(enemyData.expGain * enemyData.expMultiplier);
        eliteScoreGain = Mathf.RoundToInt(enemyData.scoreGain * enemyData.scoreMultiplier);
        eliteGoldGain = Mathf.RoundToInt(enemyData.goldGain * enemyData.goldMultiplier);
        transform.localScale *= enemyData.eliteScaleMultiplier;

        // Taç objesi ekleme
        GameObject crown = Instantiate(Resources.Load<GameObject>("Crown"));
        crown.transform.parent = this.transform;
        crown.transform.localPosition = new Vector3(enemyData.crownOffset.x, enemyData.crownOffset.y, 0);
    }

    #region ABSTRACT METHODS

    protected abstract void Move();

    #endregion
}