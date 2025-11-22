using UnityEngine;

public class ScytheAttack : MonoBehaviour
{
    [SerializeField] float radius = 3f;
    [SerializeField] float damage = 10f;
    [SerializeField] float angle = 180f; // 180 derece alan

    [SerializeField] private bool attackRight = true;

    private void Update()
    {
        DoAttack();
    }

    void DoAttack()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);

        Vector2 attackDir = attackRight ? Vector2.right : Vector2.left;

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                Vector2 dirToEnemy = hit.transform.position - transform.position;

                float a = Vector2.Angle(attackDir, dirToEnemy);

                if (a < angle / 2f)
                {
                    hit.GetComponent<EnemyController>().TakeDamage(damage);
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);

        // Debug: çizgi ile attackDir göster
        Vector2 dir = attackRight ? (Vector2)transform.right : -(Vector2)transform.right;
        Gizmos.DrawLine(transform.position, (Vector2)transform.position + dir * radius);
    }
}
