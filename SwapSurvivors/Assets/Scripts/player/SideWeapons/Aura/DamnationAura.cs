using System.Collections.Generic;
using UnityEngine;

public class DamnationAura : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float damage;
    [SerializeField] private float cooldown;
    [SerializeField] private float size;
    [SerializeField] private LayerMask enemyLayer;

    // --- Tırpan Kodundaki Optimize Fieldlar ---
    private List<Collider2D> hitBuffer = new List<Collider2D>();
    private ContactFilter2D filter = new ContactFilter2D();
    private float timer;

    private void Awake()
    {
        filter.SetLayerMask(enemyLayer);
        filter.useTriggers = true;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= cooldown)
        {
            DoAttack();
            timer = 0;
        }

        transform.localScale = Vector3.one * size * 2; // Size upgrade
    }

    private void DoAttack()
    {
        Vector2 attackPos = transform.position;

        // Yakındaki düşmanları bulur
        int hitCount = Physics2D.OverlapCircle(attackPos, size, filter, hitBuffer);

        for (int i = 0; i < hitCount; i++)
        {
            var enemyCollider = hitBuffer[i];
            if (enemyCollider != null)
                ApplyDamage(enemyCollider, damage);
        }
    }

    private void ApplyDamage(Collider2D enemy, float damageAmount)
    {
        if (enemy.TryGetComponent(out IEnemy enemyController))
        {
            if (enemyController.IsDead) return;
            enemyController.TakeDamage(damageAmount);
        }
    }

    public void UpgradeDamage(float amount) => damage += amount;
    public void UpgradeSize(float amount) => size += amount;
    public void UpgradeCooldown(float amount) => cooldown = Mathf.Max(0.1f, cooldown - amount);

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, size);
    }
}
