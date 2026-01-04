using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopData", menuName = "Shop/New Shop")]
public class ShopData : ScriptableObject
{
    private bool UpgradeShop() => shopType == ShopType.UpgradeShop;
    private bool ItemShop() => shopType == ShopType.ItemShop;
    private bool WhellOfFortune() => shopType == ShopType.WhellOfFortune;

    [Header("Shop General Settings")]
    public string shopName;
    public ShopType shopType;

    [Header("Items")]
    [ShowIf("UpgradeShop")] public UpgradeData[] upgrades;
    [ShowIf("WheelOfFortune")] public UpgradeData[] wofUpgrades;
}

public enum ShopType
{
    UpgradeShop,
    ItemShop,
    WhellOfFortune
}
