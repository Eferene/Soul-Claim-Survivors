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
    public int goldGain;
    public float goldGainPercentage = 50; // +- oranını belirler.

    [Header("Effects")]
    public Color[] colors = new Color[2];

    [Header("Elite Values")]
    public float healthMultiplier = 1.5f;
    public float damageMultiplier = 1.5f;
    public float speedMultiplier = 1.2f;
    public float expMultiplier = 1.5f;
    public float scoreMultiplier = 1.5f;
    public float goldMultiplier = 1.5f;
    public float eliteScaleMultiplier = 1.2f;
    public Vector2 crownOffset = new Vector2(0, 0.5f);
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