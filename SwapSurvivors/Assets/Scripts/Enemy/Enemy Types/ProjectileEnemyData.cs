using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile Enemy", menuName = "Enemies/Projectile Enemy")]
public class ProjectileEnemyData : EnemyData
{
    [Header("Projectile Info")]
    public GameObject projectilePrefab;
    public float projectileSpeed;
}
