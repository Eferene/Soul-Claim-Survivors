using NaughtyAttributes;
using UnityEngine;

public abstract class EnemyData : ScriptableObject
{
    [Header("Basic Info")]
    public string enemyName;
    public int baseHealth;
    public float speed;
    public GameObject enemyPrefab;
    [Range(0,4)] public int enemyHardness;
    public int expGain;

    [Header("Attack Info")]
    public int attackDamage;
    public float attackDamagePercentage = 10; // +- oranını belirler.
    public float attackRange;
    public float attackCooldown;
    public Vector3 attackOffset;

    [Header("Stats Values")]
    public int scoreGain;
    public float scoreGainPercentage = 10; // +- oranını belirler.

    [Header("Effects")]
    public Color[] colors = new Color[2];
}

public enum ThrowDamageType
{
    Instant,
    OverTime
}

public enum EnemyType
{
    Melee,
    Projectile,
    Raycast,
    Throw,
    Suicide
}