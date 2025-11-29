using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Enemy/New Enemy")]
public class EnemyData : ScriptableObject
{
    [Header("Basic Info")]
    public string enemyName;
    public int baseHealth;
    public float speed;
    public GameObject enemyPrefab;

    [Header("Attack Info")]
    public float attackDamage;
    public float attackDamagePercentage = 10; // +- oranını belirler.
    public float attackRange;
    public float attackCooldown;

    [Header("Projectile Info")]
    public GameObject projectilePrefab;
    public float projectileSpeed;

    [Header("Stats Values")]
    public int scoreGain;
    public float scoreGainPercentage = 10; // +- oranını belirler.

    [Header("Throw Info")] // Throw attackalr için damage, cooldown Attack Info kısmında belirtilir.
    public GameObject throwPrefab;
    public float throwRadius = 5f;
    public int throwCount = 3;
    public float damageRadius = 1f;
    public ThrowDamageType throwDamageType;
    public float damageOverTimeDuration = 2f;
    public float overTimeDamageInterval = 0.1f;
}

public enum ThrowDamageType
{
    Instant,
    OverTime
}