using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using TMPro;
using System;

public class WaveController : MonoBehaviour
{
    public static WaveController Instance;
    private UIController uiController;

    [Header("Main Wave Settings")]
    public List<WaveConfig> waves = new List<WaveConfig>();

    [Header("General Settings")]
    public int radius;
    public TextMeshProUGUI waveText;
    private int currentWave = 0;
    private bool isEndlessMode = false;

    public List<EnemyData> enemies = new List<EnemyData>();

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            uiController = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIController>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        NewWave();
    }

    void NewWave()
    {
        WaveConfig cWave = waves[currentWave];
        waveText.text = "Wave " + (currentWave + 1);

        if(!isEndlessMode)
        {
            StartCoroutine(ExecuteMainWaves(cWave));
        }
        currentWave++;
    }


    IEnumerator ExecuteMainWaves(WaveConfig wave)
    {
        StartCoroutine(uiController.StartWaveTimer(wave.waveDurationSec));
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
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    Vector3 GetRandomPoint()
    {
        float r = Mathf.Sqrt(UnityEngine.Random.Range(0f, 1f)) * radius;
        float angle = UnityEngine.Random.Range(0, Mathf.PI * 2f);
        return new Vector3(
            Mathf.Cos(angle) * r,
            Mathf.Sin(angle) * r,
            -1
        );
    }

    bool IsVisibleByCamera(Vector2 point)
    {
        Vector2 view = Camera.main.WorldToViewportPoint(point);
        return view.x > 0 && view.x < 1 && view.y > 0 && view.y < 1;
    }
}