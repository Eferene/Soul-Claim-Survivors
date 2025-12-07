using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Stats/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    [Header("Base Stats")]
    public float BaseMaxHealth = 100f;
    public float BaseDamage = 50f;
    public float BaseSpeed = 5f;
    public float BaseAttackCooldown = 1f;
    public float BaseRange = 1f;

    [Header("Combat Config")]
    public float DamageRangePercentage = 10f; // Hasar dalgalanma oranı
}