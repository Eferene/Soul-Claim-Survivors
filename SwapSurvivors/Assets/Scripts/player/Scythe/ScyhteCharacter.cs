using System.Collections.Generic;
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
    protected override float GetCooldown() => ScytheCooldown;
    private float ScytheMaxHealth => PlayerStats.Instance.PlayerMaxHealth * healthMultiplier;
    public float ScytheDamage => PlayerStats.Instance.PlayerDamage * damageMultiplier;
    public float ScytheRange => PlayerStats.Instance.AttackRange * rangeMultiplier;
    private float ScytheSpeed => PlayerStats.Instance.PlayerSpeed * speedMultiplier;    // Tırpan karakteri için hız

    // --- Attack State ---
    private bool isRight = true;
    private Vector3 attackPos;

    private List<Collider2D> hitBuffer = new List<Collider2D>();
    ContactFilter2D filter = new ContactFilter2D();

    // --- Visuals ---
    [Header("Visuals")]
    [SerializeField] private Animator scytheAnimator; // Tırpanın Animatorü
    [SerializeField] private GameObject scythe;       // Tırpan objesi
    [SerializeField] private Transform staticScythe;  // Seviye 3 için dönen tırpan objesi
    Vector3 scytheScale;

    // --- Unity Methods ---
    protected override void Awake()
    {
        base.Awake();
        filter.SetLayerMask(enemyLayer);
        filter.useTriggers = true;

        scytheScale = new Vector3(ScytheRange, ScytheRange);
        scythe.transform.localScale = scytheScale;
    }

    protected override void Update()
    {
        base.Update();
        if (PlayerStats.Instance.CharacterLevel == 3)
        {
            LevelThreeAttack();
        }
    }

    protected override void FixedUpdate()
    {
        playerSpeed = ScytheSpeed;
        base.FixedUpdate();
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

    private void SetPositionsAndScales()
    {
        // offset değerini yöne göre ayarlar
        float currentOffset = isRight ? offset : -offset;

        // Saldırı pozisyonunu hesaplar
        attackPos = transform.position + new Vector3(currentOffset, 0f, 0f);

        // Tırpan pozisyonunu hesaplar
        int additionalOffset = isRight ? 1 : -1;
        Vector3 scythePos = transform.position + new Vector3(currentOffset + additionalOffset, 0f, 0f);
        scythe.transform.position = scythePos;

        // Tırpan yönünü hesaplar
        scytheScale.x = isRight ? Mathf.Abs(scytheScale.x) : -Mathf.Abs(scytheScale.x);
        scythe.transform.localScale = scytheScale;

        // Tırpanı aktif eder
        scythe.gameObject.SetActive(true);
    }

    private void DoScytheHit()
    {
        SetPositionsAndScales();

        // Yakındaki düşmanları bulur
        int hitCount = Physics2D.OverlapCircle(attackPos, ScytheRange, filter, hitBuffer);

        for (int i = 0; i < hitCount; i++)
        {
            var enemy = hitBuffer[i];
            if (enemy == null) continue;

            // Düşmanın karaktere göre konumunu belirler
            Vector2 dir = enemy.transform.position - transform.position;
            dir.Normalize();

            // Düşmanın karakterin sağında mı solunda mı olduğunu kontrol eder
            bool isEnemyOnRight = Vector2.Dot(dir, transform.right) > 0;

            // Sadece tırpanın vurduğu taraftaki düşmanlara hasar uygular
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
        scythe.SetActive(false);
        staticScythe.gameObject.SetActive(true);
        staticScythe.RotateAround(transform.position, Vector3.forward, -(360f / ScytheCooldown) * Time.deltaTime);
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

    void ApplyDamage(Collider2D enemy, float damage)
    {
        if (enemy.TryGetComponent(out EnemyController enemyController))
        {
            enemyController.TakeDamage(damage);
            Debug.Log($"{enemy.name} gelen {damage} hasarı yedi.");
        }
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