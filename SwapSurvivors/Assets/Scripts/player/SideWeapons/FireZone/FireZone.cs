using System.Collections.Generic;
using UnityEngine;

public class FireZone : BaseWeaponController
{
    private float damage;
    private float size;
    private float cooldown;

    private List<Collider2D> hitBuffer = new List<Collider2D>();
    private ContactFilter2D filter = new ContactFilter2D();

    public void Init(float damage, float size, float cooldown, LayerMask enemyLayer)
    {
        this.damage = damage;
        this.size = size;
        this.cooldown = cooldown;

        filter.SetLayerMask(enemyLayer);
        filter.useTriggers = true;

        transform.localScale = Vector3.one * size * 2;
    }

    protected override float GetCooldown() => cooldown;

    protected override void Attack()
    {
        Vector2 attackPos = transform.position;
        int hitCount = Physics2D.OverlapCircle(attackPos, size, filter, hitBuffer);
        for (int i = 0; i < hitCount; i++)
        {
            var enemyCollider = hitBuffer[i];
            if (enemyCollider != null && enemyCollider.TryGetComponent(out IEnemy enemyController))
            {
                if (enemyController.IsDead) continue;
                enemyController.TakeDamage(damage);
            }
        }
    }

}
