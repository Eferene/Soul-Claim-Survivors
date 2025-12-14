using UnityEngine;

[CreateAssetMenu(fileName = "ShopData", menuName = "Shop/New Shop")]
public class ShopData : ScriptableObject
{
    [Header("Shop General Settings")]
    public string shopName;
    public int itemQuantity;

    [Header("Items")]
    public int x;
}
