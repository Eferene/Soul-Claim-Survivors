using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Stats/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    [Header("Base Attack Stats")]
    public float BaseMaxHealth = 100f;
    public float BaseDamage = 50f;
    public float BaseSpeed = 5f;
    public float BaseAttackCooldown = 1f;
    public float BaseRange = 1f;

    [Header("Combat Config")]
    public float DamageRangePercentage = 10f; // Hasar dalgalanma oranı

    [Header("Initial Upgrade Stats")]
    public float InitialDamageMultiplier = 1f;
    public float InitialRangeMultiplier = 1f;
    public float InitialSpeedMultiplier = 1f;
    public float InitialAttackSpeedMultiplier = 1f;
    public float InitialCritChance = 1f;
    public float InitialCritDamageMultiplier = 2f;
    public float InitialArmorPercentage = 0f;
    public float InitialLifeStealRate = 0f;
    public float InitialHealthRegen = 0f;
    public float InitialLuckPercentage = 1f;
}