using UnityEngine;

public class ScyhteCharacter : BaseCharacterController
{
    [Header("Tırpan Özellikleri")]
    [SerializeField] private float cooldownPercantage = 0f;     // Tırpan için saldırı hızı yüzdesi
    [SerializeField] private float healthMultiplier = 1.0f;     // Tırpan karakteri için hız
    [SerializeField] private float damageMultiplier = 1.0f;     // Tırpan için hasar çarpanı
    [SerializeField] private float rangeMultiplier = 5.0f;      // Tırpan için menzil çarpanı
    [SerializeField] private float speedMultiplier = 1.0f;      // Tırpan karakteri için hız çarpanı
    [SerializeField] private float offset;                      // Tırpanın karakterden ne kadar uzakta başlayacağını belirler
    [SerializeField] private LayerMask enemyLayer;

    // Her çağrıldığında güncellenmesi için property olarak tanımlandı
    private float ScytheCooldown => PlayerStats.Instance.AttackCooldown - (PlayerStats.Instance.AttackCooldown * (cooldownPercantage / 100));
    private float ScytheMaxHealth => PlayerStats.Instance.PlayerMaxHealth * healthMultiplier;
    public float ScytheDamage => PlayerStats.Instance.PlayerDamage * damageMultiplier;
    public float ScytheRange => PlayerStats.Instance.AttackRange * rangeMultiplier;
    private float ScytheSpeed => PlayerStats.Instance.PlayerSpeed * speedMultiplier;    // Tırpan karakteri için hız

    // --- Attack State ---
    private bool isRight = true;
    private Vector3 attackPos;

    // --- Visuals ---
    [Header("Visuals")]
    [SerializeField] private Animator scytheAnimator; // Tırpanın Animator'ını buraya sürükle
    [SerializeField] private GameObject scytheTransform; // Tırpan objesinin Transform'u (yön çevirmek için)
    Vector3 scytheScale;
    [SerializeField] private Transform staticScythe;

    // --- Unity Methods ---
    protected override void Awake()
    {
        base.Awake();
        playerSpeed = ScytheSpeed;
    }

    protected override void Update()
    {
        base.Update();
        if (PlayerStats.Instance.CharacterLevel == 3)
        {
            LevelThreeAttack();
        }
    }

    protected override void Attack()
    {
        switch (PlayerStats.Instance.CharacterLevel)
        {
            case 1:
                staticScythe.gameObject.SetActive(false);
                LevelOneAttack();
                break;
            case 2:
                staticScythe.gameObject.SetActive(false);
                LevelTwoAttack();
                break;
            case 3:
                break;
            default:
                staticScythe.gameObject.SetActive(false);
                LevelOneAttack();
                break;
        }
    }

    private void DoScytheHit()
    {
        float currentOffset = isRight ? offset : -offset;

        attackPos = transform.position + new Vector3(currentOffset, 0f, 0f);

        // Tırpanı pozisyona koy
        scytheTransform.transform.position = attackPos;

        // Yön skalası
        scytheScale = new Vector3(ScytheRange, ScytheRange);
        scytheScale.x = isRight ? Mathf.Abs(scytheScale.x) : -Mathf.Abs(scytheScale.x);
        scytheTransform.transform.localScale = scytheScale;

        // Görseli aktif et
        scytheTransform.gameObject.SetActive(true);

        // Yakındaki düşmanları bul
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPos, ScytheRange, enemyLayer);

        foreach (var enemy in enemies)
        {
            Vector2 dir = enemy.transform.position - transform.position;
            dir.Normalize();

            bool isEnemyOnRight = Vector2.Dot(dir, transform.right) > 0;

            if (isRight == isEnemyOnRight)
                ApplyDamage(enemy, PlayerStats.Instance.GiveDamage(ScytheDamage));
        }
    }


    private void LevelOneAttack()
    {
        DoScytheHit();
        scytheAnimator.SetTrigger("Attack");
        isRight = !isRight;
    }

    private void LevelTwoAttack()
    {
        isRight = true;
        DoScytheHit();
        scytheAnimator.SetTrigger("Attack");
    }

    private void LevelTwoAttackSecond()
    {
        isRight = false;
        DoScytheHit();
        scytheAnimator.SetTrigger("Attack");
    }

    private void LevelThreeAttack()
    {
        staticScythe.gameObject.SetActive(true);
        staticScythe.RotateAround(transform.position, Vector3.forward, -100 * Time.deltaTime);
    }

    public bool CheckCombo()
    {
        if (PlayerStats.Instance.CharacterLevel == 2 && isRight)
        {
            LevelTwoAttackSecond();
            return true;
        }

        return false;
    }

    protected override float GetCooldown() => ScytheCooldown;

    void ApplyDamage(Collider2D enemy, float damage)
    {
        if (enemy.TryGetComponent(out EnemyController enemyController))
        {
            enemyController.TakeDamage(damage);
            Debug.Log($"{enemy.name} gelen {damage} hasarı yedi.");
        }
    }

    protected override void FixedUpdate()
    {
        playerSpeed = ScytheSpeed;
        base.FixedUpdate();
    }

    private void OnDrawGizmosSelected()
    {
        // Sağ taraf vurulacaksa mavi, sol taraf vurulacaksa kırmızı çizelim.
        Gizmos.color = Color.red;

        // Karakterin yönü
        Vector3 rightDir = transform.right * ScytheRange;
        Vector3 leftDir = -transform.right * ScytheRange;


        Gizmos.DrawLine(attackPos, attackPos + (isRight ? leftDir : rightDir));

    }
}