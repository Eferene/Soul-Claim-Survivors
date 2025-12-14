using UnityEngine;

[CreateAssetMenu(fileName = "New Suicide Enemy", menuName = "Enemies/Suicide Enemy")]
public class SuicideEnemyData : EnemyData
{
    [Header("Suicide Info")]
    public float explodeDuration = 0.5f;
    public float explodeAreaRadius = 2.5f;
    public GameObject explodeArea;
}