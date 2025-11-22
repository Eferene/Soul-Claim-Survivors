using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public EnemyData enemyData;
    private int currentHealth;
    private BaseAttackType attackType;
    Rigidbody2D rb;
    Transform playerTransform;

    // Cooldown
    private float lastAttackTime;
    private bool canAttack = true; 

    void Start()
    {
        currentHealth = enemyData.baseHealth;
        attackType = GetComponent<BaseAttackType>();
        rb = GetComponent<Rigidbody2D>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null) playerTransform = playerObj.transform;
    }

    void Update()
    {
        // Cooldown ile vurup vuramayacağını kontrol etme.
        if(Time.time - lastAttackTime >= enemyData.attackCooldown)
        {
            canAttack = true;
        }

        // Basit saldırı
        if (attackType != null && canAttack)
        {
            attackType.Attack(transform, GameObject.FindGameObjectWithTag("Player").transform, enemyData.attackDamage, enemyData.attackRange);
            lastAttackTime = Time.time;
            canAttack = false;
        }
    }

    void FixedUpdate()
    {
        // Hareket
        if (playerTransform == null)
        {
            var playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) playerTransform = playerObj.transform;
        }

        if (rb != null && playerTransform != null && enemyData != null)
        {
            Vector2 currentPos = rb.position;
            Vector2 targetPos = (Vector2)playerTransform.position;
            Vector2 direction = (targetPos - currentPos).normalized;
            rb.MovePosition(currentPos + direction * enemyData.speed * Time.fixedDeltaTime);
        }
    }
}