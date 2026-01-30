using UnityEngine;

public abstract class BaseWeaponController : MonoBehaviour
{
    // --- References ---
    protected Rigidbody2D rb;
    protected PlayerManager playerManager;

    // --- Combat ---
    private float lastAttackTime = 0f;

    // --- Unity Methods ---
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerManager = GetComponentInParent<PlayerManager>();
    }

    protected virtual void Update()
    {
        ApplyAttack();
    }

    protected virtual void ApplyAttack()
    {
        if (Time.time >= lastAttackTime + GetCooldown())
        {
            Attack();
            lastAttackTime = Time.time;
        }
    }

    // --- Abstract Methods ---
    protected abstract void Attack();
    protected abstract float GetCooldown();
}
