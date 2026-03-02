using System.Collections.Generic;
using UnityEngine;

public class FireZoneBase : BaseWeaponController
{
    [Header("References")]
    [SerializeField] private GameObject fireZonePrefab;
    [SerializeField] private LayerMask enemyLayer;

    [Header("Spawn Stats")]
    [SerializeField] private float spawnCooldown; // Zone ne sıklıkta spawn olacak
    [SerializeField] private int projectileCount;
    [SerializeField] private float spawnRadius;

    [Header("Zone Stats")]
    [SerializeField] private float damage;
    [SerializeField] private float size;
    [SerializeField] private float duration;
    [SerializeField] private float damageCooldown; // Zone ne sıklıkta hasar verecek

    private int zonesLimit = 10;
    private List<GameObject> activeZones = new List<GameObject>();

    protected override float GetCooldown() => spawnCooldown;

    protected override void Attack()
    {
        for (int i = 0; i < projectileCount; i++)
            SpawnFireZone();
    }

    void SpawnFireZone()
    {
        Vector2 spawnPos = (Vector2)transform.position + Random.insideUnitCircle * spawnRadius; // Rastgele bir konum

        GameObject instance = Instantiate(fireZonePrefab, spawnPos, Quaternion.identity);
        if (instance.TryGetComponent(out FireZone fzScript))
            fzScript.Init(damage, size, damageCooldown, enemyLayer);

        ManageProjectileLimit(instance, activeZones, zonesLimit);
        Destroy(instance, duration);
    }
}
