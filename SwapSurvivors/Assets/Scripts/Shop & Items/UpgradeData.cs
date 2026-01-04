using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade", menuName = "Upgrades/Upgrade")]
public class UpgradeData : ScriptableObject
{
    public string upgradeName;
    [TextArea] public string upgradeDescription;
    public Sprite upgradeIcon;
    public UpgradeTypes upgradeType;
    public IncreaseValues[] increaseValues = new IncreaseValues[5];
    public int cost;
}

[System.Serializable]
public class IncreaseValues
{
    public float min;
    public float max;
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