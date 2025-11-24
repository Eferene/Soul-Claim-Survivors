using UnityEngine;

public class ScyhteCharacter : BaseCharacterController
{
    [Header("Tırpan Özellikleri")]
    [SerializeField] private float cooldownMultiplier = 1.0f; // Tırpan için saldırı hızı çarpanı
    [SerializeField] private float damageMultiplier = 1.0f;         // Tırpan için hasar çarpanı
    [SerializeField] private float speedMultiplier = 1.0f;          // Tırpan karakteri için hız çarpanı
    [SerializeField] private float rangeMultiplier = 5f;
    [SerializeField] private float offset; // Tırpanın karakterden ne kadar uzakta başlayacağını belirler
    [SerializeField] private LayerMask enemyLayer;

    private float currentCooldown;
    private float currentRange;
    private float currentDamage;
    private float currentSpeed;

    // --- Attack State ---
    private bool isRight = true;
    private Vector3 attackDir;

    // --- Unity Methods ---
    protected override void Awake()
    {
        base.Awake();
        ApplyUpgrades();
    }

    protected override void Attack()
    {
        float damage = PlayerStats.Instance.GiveDamage();

        attackDir = transform.position + new Vector3(isRight ? offset : -offset, 0f, 0f);

        // Yakındaki düşmanları bul
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackDir, currentRange, enemyLayer);

        foreach (var enemy in enemies)
        {
            Vector2 dir = enemy.transform.position - transform.position;
            dir.Normalize();

            bool isEnemyOnRight = Vector2.Dot(dir, transform.right) > 0;

            if (isRight == isEnemyOnRight)
                ApplyDamage(enemy, damage);
        }

        isRight = !isRight;
    }

    void ApplyDamage(Collider2D enemy, float damage)
    {
        enemyController = enemy.GetComponent<EnemyController>();
        enemyController.TakeDamage(damage);
        Debug.Log($"{enemy.name} gelen {damage} hasarı yedi.");
    }

    void ApplyUpgrades()
    {
        currentCooldown = attacCooldown * cooldownMultiplier;  // Tırpan için saldırı hızı
        currentRange = attackRange * rangeMultiplier;           // Tırpan için saldırı menzili
        currentDamage = PlayerStats.Instance.PlayerDamage * damageMultiplier * upgradesDamageMultiplier; // Tırpan için hasar
        currentSpeed = playerSpeed * speedMultiplier;               // Tırpan karakteri için hız
    }

    private void OnDrawGizmosSelected()
    {
        // Sağ taraf vurulacaksa mavi, sol taraf vurulacaksa kırmızı çizelim.
        Gizmos.color = Color.red;

        // Karakterin yönü
        Vector3 rightDir = transform.right * currentRange;
        Vector3 leftDir = -transform.right * currentRange;


        Gizmos.DrawLine(attackDir, attackDir + (isRight ? leftDir : rightDir));

    }
}