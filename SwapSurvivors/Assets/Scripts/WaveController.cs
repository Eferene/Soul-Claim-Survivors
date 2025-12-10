using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;

public class WaveController : MonoBehaviour
{
    public static WaveController Instance;

    [Header("Main Wave Settings")]
    public List<WaveConfig> waves = new List<WaveConfig>();

    [Header("General Settings")]
    public int edgeLength = 50; // Alan kare olacak.
    public TextMeshProUGUI waveText;
    private int currentWave = 0;
    private bool isEndlessMode = false;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        StartCoroutine(GameLoop());
    }

    IEnumerator GameLoop()
    {
        while(currentWave < waves.Count)
        {
            WaveConfig cWave = waves[currentWave];

            yield return StartCoroutine(ExecuteMainWaves(cWave)); 
            currentWave++;

            yield return new WaitForSeconds(2);
        }

        while (isEndlessMode)
        {
            Debug.Log("Endless Mode tamamlanmadı.");
            break;
        }
    }

    IEnumerator ExecuteMainWaves(WaveConfig wave)
    {
        float timer = 0f;
        float spawnRate = 1f;

        while (timer < wave.waveDurationSec)
        {
            if(wave.enemiesToSpawn.Count > 0)
            {
                foreach(var enemyPattern in wave.enemiesToSpawn)
                {
                    foreach(var enemySequence in enemyPattern.enemySequence)
                    {
                        for(int i = 0; i < enemySequence.quantity; i++)
                        {
                            GameObject newEnemy = EnemyPool.Instance.GetEnemyFromPool(enemySequence.enemyData.enemyPrefab);
                            Vector3 randomPoint = GetRandomPoint();
                            while(IsVisibleByCamera(randomPoint)) randomPoint = GetRandomPoint();

                            newEnemy.transform.position = randomPoint;
                        }
                    }
                }
            }

            yield return new WaitForSeconds(spawnRate);
            timer += spawnRate;
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
            Random.Range(-half.x, half.x),
            Random.Range(-half.y, half.y),
            -1
        );
    }

    bool IsVisibleByCamera(Vector2 point)
    {
        Vector2 view = Camera.main.WorldToViewportPoint(point);
        return view.x > 0 && view.x < 1 && view.y > 0 && view.y < 1;
    }
}