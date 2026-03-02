using System.Collections.Generic;
using UnityEngine;

public class DamnationAura : BaseWeaponController
{
    [Header("Settings")]
    [SerializeField] private LayerMask enemyLayer;

    [Header("Combat Stats")]
    [SerializeField] private float damage;
    [SerializeField] private float cooldown;

    [Header("Area Stats")]
    [SerializeField] private float size;

    private List<Collider2D> hitBuffer = new List<Collider2D>();
    private ContactFilter2D filter = new ContactFilter2D();

    protected override void Awake()
    {
        base.Awake();
        filter.SetLayerMask(enemyLayer);
        filter.useTriggers = true;
    }

    protected override void Update()
    {
        base.Update();
        transform.localScale = Vector3.one * size * 2; // Size upgrade
    }

    protected override float GetCooldown() => cooldown;

    protected override void Attack()
    {
        Vector2 attackPos = transform.position;

        // Yakındaki düşmanları bulur
        int hitCount = Physics2D.OverlapCircle(attackPos, size, filter, hitBuffer);

        for (int i = 0; i < hitCount; i++)
        {
            var enemyCollider = hitBuffer[i];
            if (enemyCollider != null && enemyCollider.TryGetComponent(out IEnemy enemyController))
            {
                if (enemyController.IsDead) return;
                enemyController.TakeDamage(damage);
            }
        }
    }

    // --- Upgrade Methods ---
    public void UpgradeDamage(float amount) => damage += amount;
    public void UpgradeSize(float amount) => size += amount;
    public void UpgradeCooldown(float amount) => cooldown = Mathf.Max(0.1f, cooldown - amount);

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, size);
    }
}
