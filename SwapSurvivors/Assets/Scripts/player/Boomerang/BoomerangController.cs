using UnityEngine;

public class BoomerangController : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int projectileCount = 6;
    [SerializeField] private float rotationSpeed = 720f;

    // Stats
    private float boomerangDamage;
    private float boomerangRange;
    private float boomerangSpeed;
    private int characterLevel;

    // Internal
    private float projectileDamage;
    private bool isReturning = false;
    private Vector3 startPosition;

    // References
    private GameObject boomerangCharacter;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Bu metod, bumerang özelliklerini ayarlamak için çağrılır
    public void Setup(float damage, float range, float speed, int level, GameObject owner)
    {
        boomerangDamage = damage;
        boomerangRange = range;
        boomerangSpeed = speed;
        characterLevel = level;
        boomerangCharacter = owner;

        projectileDamage = damage * 0.5f;
        startPosition = transform.position;
        isReturning = false;

        rb.linearVelocity = transform.right * speed;
    }

    private void Update()
    {
        transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime); // Bumerang dönme

        // Menzil ve durum kontrolü
        if (!isReturning && Vector3.Distance(startPosition, transform.position) >= boomerangRange)
        {
            if (characterLevel >= 2)
                SpawnProjectiles();

            isReturning = true;
        }

        // Geri dönüyorsa oyuncuya doğru hareket et
        if (isReturning)
            ReturnMovement();
    }

    private void ReturnMovement()
    {
        Vector3 directionToPlayer = (boomerangCharacter.transform.position - transform.position).normalized;
        rb.linearVelocity = directionToPlayer * boomerangSpeed;

        // Sadece geri dönüyorsa mesafeyi kontrol et
        if (Vector2.Distance(boomerangCharacter.transform.position, transform.position) < 0.5f)
            ReturnToPool(gameObject);
    }

    private void SpawnProjectiles()
    {
        float angleStep = 360f / projectileCount;

        for (int i = 0; i < projectileCount; i++)
        {
            float currentAngle = i * angleStep;

            float dirX = Mathf.Cos(currentAngle * Mathf.Deg2Rad);
            float dirY = Mathf.Sin(currentAngle * Mathf.Deg2Rad);
            Vector3 spawnDirection = new Vector3(dirX, dirY, 0);

            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

            if (projectile.TryGetComponent(out ProjectileController script))
            {
                projectileDamage = boomerangDamage / 2;
                script.Setup(projectileDamage, boomerangSpeed, spawnDirection);
            }
        }
    }

    private void ReturnToPool(GameObject obj)
    {
        rb.linearVelocity = Vector2.zero;
        isReturning = false;
        obj.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Düşmanla çarpışma kontrolü
        if (collision.CompareTag("Enemy"))
        {
            if (collision.TryGetComponent(out EnemyController enemyController))
            {
                if (enemyController.IsDead) return;
                enemyController.TakeDamage(boomerangDamage);
            }
        }
    }
}
