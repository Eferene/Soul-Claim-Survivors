using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class WaveController : MonoBehaviour
{
    public List<Wave> waves = new List<Wave>();
    public int edgeLength = 50; // Alan kare olacak.
    public TextMeshProUGUI waveText;
    private int currentWave = 1;

    void Start()
    {
        NewWave();
    }

    public void NewWave()
    {
        waveText.text = "Wave " + currentWave;
        Transform enemiesParent = GameObject.FindGameObjectWithTag("Enemies").transform;
        for(int i = 0; i < waves[currentWave - 1].enemiesToSpawn.Count; i++)
        {
            int count = Convert.ToInt32(UnityEngine.Random.Range(waves[currentWave - 1].enemiesToSpawn[i].count * (1 - waves[currentWave - 1].enemiesToSpawn[i].countPercentage / 100f), waves[currentWave - 1].enemiesToSpawn[i].count * (1 + waves[currentWave - 1].enemiesToSpawn[i].countPercentage / 100f)));
            for(int j = 0; j < count; j++)
            {
                int attempts = 0;
                while (attempts < 50)
                {
                    Vector3 newPoint = GetRandomPoint();
                    if (!IsVisibleByCamera(newPoint))
                    {
                        GameObject newEnemy = Instantiate(waves[currentWave - 1].enemiesToSpawn[i].enemy.enemyPrefab, newPoint, Quaternion.identity, enemiesParent);
                        newEnemy.name += " " + j;
                        break;
                    }
                    attempts++;
                }
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(edgeLength, edgeLength, edgeLength));
    }

    Vector3 GetRandomPoint()
    {
        Vector3 half = new Vector3(edgeLength, edgeLength, edgeLength) / 2;
        return new Vector3(
            UnityEngine.Random.Range(-half.x, half.x),
            UnityEngine.Random.Range(-half.y, half.y),
            -1
        );
    }

    bool IsVisibleByCamera(Vector2 point)
    {
        Vector2 view = Camera.main.WorldToViewportPoint(point);
        return view.x > 0 && view.x < 1 && view.y > 0 && view.y < 1;
    }
}

[System.Serializable]
public class Wave
{
    public List<EnemyToSpawn> enemiesToSpawn = new List<EnemyToSpawn>();
    public int difficultyMultiplier = 1;
}

[System.Serializable]
public class EnemyToSpawn
{
    public EnemyData enemy;
    public int count;
    public int countPercentage = 10;
}