using UnityEngine;

public class OrbitWeapon : BaseWeaponController
{
    [Header("Stats")]
    [SerializeField] private GameObject orbitWeapon;
    [SerializeField] private float projectileCount = 3;
    [SerializeField] private float projectileDuration = 2;
    [SerializeField] private float projectileDamage = 10;
    [SerializeField] private float orbitRadius = 2f;
    [SerializeField] private float projectileSpeed = 150;

    protected override float GetCooldown() => playerManager.CurrentCooldown + projectileDuration;

    protected override void Attack()
    {
        if (orbitWeapon == null) return;

        float angle = 360f / projectileCount;

        for (int i = 0; i < projectileCount; i++)
        {
            float angleDeg = i * angle; // Derece hesabı
            float angleRad = angleDeg * Mathf.Deg2Rad; // Dereceyi radyana çevir

            Vector2 offset = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * orbitRadius; // Cos(θ) = x, Sin(θ) = y
            Vector3 spawnPos = transform.position + (Vector3)offset;
            Quaternion rotation = Quaternion.Euler(0, 0, angleDeg);

            GameObject instance = Instantiate(orbitWeapon, spawnPos, rotation);
            instance.transform.SetParent(transform);

            if (instance.TryGetComponent(out OrbitProjectile opScript))
                opScript.Init(projectileSpeed, projectileDamage);

            Destroy(instance, projectileDuration);
        }
    }

    // --- Upgrade Methods ---
    public void UpgradeProjectileCount(int amount) => projectileCount += amount;
    public void UpgradeProjectileDuration(int amount) => projectileDuration += amount;
    public void UpgradeProjectileDamage(int amount) => projectileDamage += amount;
}
