using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade", menuName = "Upgrade")]
public class UpgradeData : ScriptableObject
{
    public string upgradeName;
    [TextArea] public string upgradeDescription;
    public Sprite upgradeIcon;
}