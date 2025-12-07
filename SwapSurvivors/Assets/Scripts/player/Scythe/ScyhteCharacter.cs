using System.Collections.Generic;
using UnityEngine;

public class ScyhteCharacter : BaseCharacterController
{
    [SerializeField] private float offset;                      // Tırpanın karakterden ne kadar uzakta başlayacağını belirler
    [SerializeField] private LayerMask enemyLayer;

    // --- Attack State ---
    private bool isRight = true;
    private bool rightCheck = true;
    private Vector3 attackPos;

    private List<Collider2D> hitBuffer = new List<Collider2D>();
    ContactFilter2D filter = new ContactFilter2D();

    // --- Visuals ---
    [Header("Visuals")]
    [SerializeField] private Animator scytheAnimator; // Tırpanın Animatorü
    [SerializeField] private GameObject scythe;       // Tırpan objesi
    [SerializeField] private Transform staticScythe;  // Seviye 3 için dönen tırpan objesi
    Vector3 scytheScale;

    protected override float GetCooldown() => playerManager.CurrentCooldown;

    // --- Unity Methods ---
    protected override void Awake()
    {
        base.Awake();
        filter.SetLayerMask(enemyLayer);
        filter.useTriggers = true;

        scytheScale = new Vector3(playerManager.CurrentRange, playerManager.CurrentRange);
        scythe.transform.localScale = scytheScale;
    }

    protected override void Update()
    {
        base.Update();
        if (playerManager.CharacterLevel == 3)
        {
            LevelThreeAttack();
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Attack()
    {
        switch (playerManager.CharacterLevel)
        {
            case 1: // level 1
                staticScythe.gameObject.SetActive(false);
                LevelOneAttack();
                break;
            case 2: // level 2
                staticScythe.gameObject.SetActive(false);
                LevelTwoAttack();
                break;
            case 3: // level 3
                break;
            default: // default level 1
                staticScythe.gameObject.SetActive(false);
                LevelOneAttack();
                break;
        }
    }

    private void SetPositionsAndScales()
    {
        rightCheck = isRight;

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

    public void DoScytheHit()
    {
        // Yakındaki düşmanları bulur
        int hitCount = Physics2D.OverlapCircle(attackPos, playerManager.CurrentRange, filter, hitBuffer);

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
            if (rightCheck == isEnemyOnRight)
                ApplyDamage(enemy, playerManager.GiveDamageCharacter());
        }
    }


    private void LevelOneAttack()
    {
        SetPositionsAndScales();
        scytheAnimator.SetTrigger("Attack");
        isRight = !isRight;
    }

    private void LevelTwoAttack()
    {
        isRight = true;
        SetPositionsAndScales();
        scytheAnimator.SetTrigger("Attack");
    }

    private void LevelTwoAttackSecond()
    {
        isRight = false;
        SetPositionsAndScales();
        scytheAnimator.SetTrigger("Attack");
    }

    private void LevelThreeAttack()
    {
        scythe.SetActive(false);
        staticScythe.gameObject.SetActive(true);
        staticScythe.RotateAround(transform.position, Vector3.forward, -(360f / playerManager.CurrentCooldown) * Time.deltaTime);
    }

    public bool CheckCombo()
    {
        if (playerManager.CharacterLevel == 2 && isRight)
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
        Vector3 rightDir = transform.right * playerManager.CurrentRange;
        Vector3 leftDir = -transform.right * playerManager.CurrentRange;


        Gizmos.DrawLine(attackPos, attackPos + (isRight ? leftDir : rightDir));

    }
}