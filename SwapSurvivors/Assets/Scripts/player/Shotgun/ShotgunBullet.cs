
using UnityEngine;

public class ShotgunBullet : MonoBehaviour
{
    private float bulletDamage;
    private float bulletRange;
    private float bulletExplosionRange;
    private Vector3 startPosition;

    // Bu metod, mermi özelliklerini ayarlamak için çağrılır
    public void Setup(float bulletDamage, float bulletSpeed, float bulletRange, float bulletExplosionRange)
    {
        this.bulletDamage = bulletDamage;
        this.bulletRange = bulletRange;
        this.bulletExplosionRange = bulletExplosionRange;
        startPosition = transform.position;

        // Hız ayarı
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            bulletSpeed = bulletSpeed * Random.Range(0.8f, 1.2f);
            rb.linearVelocity = transform.right * bulletSpeed;
        }
    }

    private void Update()
    {
        // Mermi menzilini aştıysa yok et
        if (Vector3.Distance(startPosition, transform.position) >= bulletRange)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Düşmanla çarpışma kontrolü
        if (collision.CompareTag("Enemy"))
        {
            if (collision.TryGetComponent(out EnemyController enemyController))
            {
                if (enemyController.IsDead) return;
                float newBulletDamage = PlayerStats.Instance.GiveDamage(bulletDamage);
                enemyController.TakeDamage(newBulletDamage);
                //Debug.Log($"{collision.name} gelen {newBulletDamage} hasarı yedi.");

                if (PlayerStats.Instance.CharacterLevel == 3)
                    BulletExplosion();

                Destroy(gameObject);
            }
        }

        // Mermi engelle çarpıştığında yok et
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            if (PlayerStats.Instance.CharacterLevel == 3)
                BulletExplosion();
            Destroy(gameObject);
        }
    }

    private void BulletExplosion()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, bulletExplosionRange);

        foreach (var enemy in enemies)
        {
            if (enemy.CompareTag("Enemy"))
            {
                if (enemy.TryGetComponent(out EnemyController enemyController))
                {
                    float newBulletDamage = PlayerStats.Instance.GiveDamage(bulletDamage);
                    enemyController.TakeDamage(newBulletDamage);
                    Debug.Log($"{enemy.name} gelen {newBulletDamage} hasarı yedi.");
                }
            }
        }
    }
}