using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "ShopData", menuName = "Shop/New Shop")]
public class ShopData : ScriptableObject
{
    private bool UpgradeShop() => shopType == ShopType.UpgradeShop;
    private bool ItemShop() => shopType == ShopType.ItemShop;

    [Header("Shop General Settings")]
    public string shopName;
    public ShopType shopType;

    [Header("Items")]
    [ShowIf("UpgradeShop")] public UpgradeData[] upgrades;
}

public enum ShopType
{
    UpgradeShop,
    ItemShop
}
