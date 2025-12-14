using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade", menuName = "Upgrade")]
public class UpgradeData : ScriptableObject
{
    public string upgradeName;
    [TextArea] public string upgradeDescription;
    public Sprite upgradeIcon;
    public UpgradeTypes upgradeType;
}

public enum UpgradeTypes
{
    Damage,
    Range,
    Speed,
    AttackSpeed,
    Luck,
    MaxHealth,
    HealthRegen,
    LifeSteal,
    Armor,
    CriticalHitChance,
    CriticalHitDamage
}