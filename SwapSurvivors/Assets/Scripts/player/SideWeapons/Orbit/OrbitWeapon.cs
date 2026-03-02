using System.Collections.Generic;
using UnityEngine;

public class OrbitWeapon : BaseWeaponController
{
    [Header("References")]
    [SerializeField] private GameObject projectilePrefab;

    [Header("Combat Stats")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float cooldown = 4f;
    [SerializeField] private float duration = 2f;

    [Header("Shape & Movement")]
    [SerializeField] private float radius = 2f;
    [SerializeField] private float rotationSpeed = 150f;
    [SerializeField] private int projectileCount = 3;

    protected override float GetCooldown() => Mathf.Max(cooldown, duration + 0.1f);

    protected override void Attack()
    {
        if (projectilePrefab == null) return;

        float angle = 360f / projectileCount;

        for (int i = 0; i < projectileCount; i++)
        {
            float angleDeg = i * angle; // Derece hesabı
            float angleRad = angleDeg * Mathf.Deg2Rad; // Dereceyi radyana çevir

            Vector2 offset = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * radius; // Cos(θ) = x, Sin(θ) = y
            Vector3 spawnPos = transform.position + (Vector3)offset;
            Quaternion rotation = Quaternion.Euler(0, 0, angleDeg);

            GameObject instance = Instantiate(projectilePrefab, spawnPos, rotation);
            instance.transform.SetParent(transform);

            if (instance.TryGetComponent(out OrbitProjectile opScript))
                opScript.Init(rotationSpeed, damage);

            Destroy(instance, duration);
        }
    }

    // --- Upgrade Methods ---
    public void UpgradeProjectileCount(int amount) => projectileCount += amount;
    public void UpgradeProjectileDuration(int amount) => duration += amount;
    public void UpgradeProjectileDamage(int amount) => damage += amount;
}
