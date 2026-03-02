using System.Collections.Generic;
using UnityEngine;

public abstract class BaseWeaponController : MonoBehaviour
{
    // --- References ---
    protected Rigidbody2D rb;

    // --- Combat ---
    private float lastAttackTime = 0f;

    // --- Unity Methods ---
    protected virtual void Awake() => rb = GetComponent<Rigidbody2D>();

    protected virtual void Update() => ApplyAttack();

    protected virtual void ApplyAttack()
    {
        if (Time.time >= lastAttackTime + GetCooldown())
        {
            Attack();
            lastAttackTime = Time.time;
        }
    }

    protected virtual void ManageProjectileLimit(GameObject newProj, List<GameObject> activeList, int limit)
    {
        activeList.Add(newProj);

        if (activeList.Count > limit)
        {
            GameObject oldest = activeList[0];
            activeList.RemoveAt(0);
            if (oldest != null)
                Destroy(oldest);
        }
    }

    // --- Abstract Methods ---
    protected abstract void Attack();
    protected abstract float GetCooldown();
}
