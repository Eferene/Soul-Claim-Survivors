using UnityEngine;

public class ShotgunCharacter : BaseCharacterController
{
    [Header("Shotgun Stats")]
    [SerializeField] private float detectionRadius = 10.0f;
    [SerializeField] private float spreadAngle = 30.0f;
    [SerializeField] private float bulletSpeed = 20.0f;
    [SerializeField] private float bulletExplosionRadius = 1.0f;
    [SerializeField] private int bulletCount = 5;

    [Header("Components")]
    [SerializeField] private Transform WeaponHolder;
    [SerializeField] private Transform FirePoint;
    [SerializeField] private GameObject BulletPrefab;
    [SerializeField] private LayerMask enemyLayer;

    private Transform currentTarget;

    protected override float GetCooldown() => playerManager.CurrentCooldown;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Attack()
    {
        switch (playerManager.CharacterLevel)
        {
            case 1:
                LevelOneAttack();
                break;
            case 2:
                LevelTwoAttack();
                break;
            case 3:
                LevelThreeAttack();
                break;
            default:
                LevelOneAttack();
                break;
        }
    }

    protected override void Update()
    {
        base.Update();

        // En yakın düşmanı bulur
        FindClosestEnemy();

        // Hedef varsa silahı hedefe döndürür
        if (currentTarget != null)
        {
            RotateWeaponToTarget();
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void ApplyAttack()
    {
        if (currentTarget != null)
            base.ApplyAttack();
    }

    private void FireBullet()
    {
        if (currentTarget == null) return;

        Quaternion aimRotation = FirePoint.rotation;
        float randomZ = Random.Range(-spreadAngle / 2f, spreadAngle / 2f);
        Quaternion bulletRotation = Quaternion.Euler(0, 0, aimRotation.eulerAngles.z + randomZ);

        GameObject bullet = Instantiate(BulletPrefab, FirePoint.position, bulletRotation);
        bullet.transform.parent = this.transform;

        // Bullet bileşenini alıp gerekli ayarları yapar
        if (bullet.TryGetComponent(out ShotgunBullet bulletScript))
        {
            bulletScript.Setup(playerManager.CurrentDamage, bulletSpeed, playerManager.CurrentSpeed, bulletExplosionRadius, (playerManager.CurrentDamage / 2));
        }
    }

    private void FindClosestEnemy()
    {
        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, detectionRadius, enemyLayer);

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var enemy in enemiesInRange)
        {
            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);

            if (distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                closestEnemy = enemy.transform;
            }
        }

        currentTarget = closestEnemy;
    }

    private void RotateWeaponToTarget()
    {
        if (currentTarget == null) return;

        // Düşmana giden yol vektörü
        Vector3 direction = currentTarget.position - WeaponHolder.position;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float newAngle = Mathf.MoveTowardsAngle(WeaponHolder.rotation.eulerAngles.z, targetAngle, 5f);

        WeaponHolder.rotation = Quaternion.Euler(0, 0, newAngle);

        // Silahın yukarı/aşağı yönünü ayarlar
        Vector3 localScale = Vector3.one;
        if (newAngle > 90 || newAngle < -90)
        {
            localScale.y = -1;
        }
        else
        {
            localScale.y = 1;
        }
        WeaponHolder.localScale = localScale;
    }

    private void LevelOneAttack()
    {
        for (int i = 0; i < bulletCount; i++)
            FireBullet();
    }

    private void LevelTwoAttack()
    {
        for (int i = 0; i < bulletCount; i++)
            FireBullet();
        Invoke(nameof(DelayedSecondSalvo), 0.2f);
    }

    private void DelayedSecondSalvo()
    {
        for (int i = 0; i < bulletCount; i++)
            FireBullet();
    }

    private void LevelThreeAttack()
    {
        for (int i = 0; i < bulletCount; i++)
            FireBullet();
        Invoke(nameof(DelayedSecondSalvo), 0.2f);
    }

    private void OnDrawGizmosSelected()
    {
        if (playerManager == null)
            return;

        // Algılama menzili
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        // Mermi menzili
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(FirePoint.position, playerManager.CurrentRange);
    }
}
