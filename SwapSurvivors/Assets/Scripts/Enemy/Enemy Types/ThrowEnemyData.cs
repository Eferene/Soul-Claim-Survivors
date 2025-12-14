using UnityEngine;

[CreateAssetMenu(fileName = "New Throw Enemy", menuName = "Enemies/Throw Enemy")]
public class ThrowEnemyData : EnemyData
{
    [Header("Throw Info")] // Throw attacklar için damage, cooldown Attack Info kısmında belirtilir.
    public GameObject throwPrefab;
    public float throwRadius = 5f;
    public int throwCount = 3;
    public float damageRadius = 1f;
    public ThrowDamageType throwDamageType;
    public float damageOverTimeDuration = 2f;
    public float overTimeDamageInterval = 0.1f;
}