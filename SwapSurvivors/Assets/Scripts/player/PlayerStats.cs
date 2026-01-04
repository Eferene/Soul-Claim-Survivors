using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Stats/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    [Header("Base Attack Stats")]
    public float MaxHealth = 100f;
    public float AttackDamage = 50f;
    public float MovementSpeed = 5f;
    public float AttackCooldown = 1f;
    public float AttackRange = 1f;

    [Header("Combat Settings")]
    [Range(0f, 1f)]
    public float DamageVariance = 0.1f;

    [Header("Starting Bonuses")]
    public float StartingDamageBonus = 0f;
    public float StartingRangeBonus = 0f;
    public float StartingSpeedBonus = 0f;
    public float StartingAttackSpeedBonus = 0f;

    [Header("Starting Stats")]
    [Range(0f, 100f)]
    public float BaseCritChance = 0f;
    public float BaseCritMultiplier = 2f;
    public float BaseArmorPercent = 0f;
    public float BaseLifeStealPercent = 0f;
    public float BaseHealthRegenPercent = 0f;
    public float BaseLuck = 0f;
    public float BasePickUpRange = 5f;
}