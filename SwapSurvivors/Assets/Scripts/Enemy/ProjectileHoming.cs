using UnityEngine;

public class ProjectileHoming : ProjectileBase
{
    public float damage = 10f;
    private Transform target;
    private Rigidbody2D targetRb;
    // oyuncuya kaçma şansı tanıyan dönüş limiti
    public float turnSpeed = 180f;
    private Vector2 moveDir;

    protected override void Start()
    {
        base.Start();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            target = playerObj.transform;
            targetRb = playerObj.GetComponent<Rigidbody2D>();
            moveDir = (target.position - transform.position).normalized;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected override void Move()
    {
        if (target == null) // hedef yoksa düz git
        {
            transform.Translate(moveDir * speed * Time.deltaTime, Space.World);
            return;
        }

        Vector2 targetPos = target.position;
        if (targetRb != null)
        {
            targetPos += targetRb.linearVelocity * 0.15f; // basit öngörü
        }

        Vector2 toTarget = (targetPos - (Vector2)transform.position).normalized;

        // dönüş miktarı
        float currentAngle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
        float targetAngle = Mathf.Atan2(toTarget.y, toTarget.x) * Mathf.Rad2Deg;

        // sınırlı dönme
        float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, turnSpeed * Time.deltaTime);

        float rad = newAngle * Mathf.Deg2Rad;
        moveDir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));

        transform.rotation = Quaternion.Euler(0, 0, newAngle);
        transform.Translate(moveDir * speed * Time.deltaTime, Space.World);
    }

    protected override void OnHitTarget(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerStats.Instance.DecreaseHealth(damage);
            Destroy(gameObject);
        }
    }
}
