using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Enemy Pattern", menuName = "Wave/Enemy Pattern")]
public class EnemyPattern : ScriptableObject
{
    public List<EnemyPatternObject> enemySequence = new List<EnemyPatternObject>();
}

[System.Serializable]
public class EnemyPatternObject
{
    public EnemyData enemyData;
    public int quantity;
}