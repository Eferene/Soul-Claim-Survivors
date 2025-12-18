using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Wave Config", menuName = "Wave/Wave")]
public class WaveConfig : ScriptableObject
{
    public List<EnemyPattern> enemiesToSpawn = new List<EnemyPattern>();
    public float difficultyMultiplier = 1f;
    public int waveDurationSec = 60;
    public int rewardTokens = 1; 
}