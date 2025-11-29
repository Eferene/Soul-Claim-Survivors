using UnityEngine;

public class ThrowObject : MonoBehaviour
{
    public EnemyData enemyData;
    private float startTime;
    private float finalDamage;
    private float radius;

    // OverTime
    private float overTimeDamageStartTime;

    void Start()
    {
        startTime = Time.time;
        overTimeDamageStartTime = Time.time;

        transform.localScale = new Vector3(enemyData.damageRadius, enemyData.damageRadius, enemyData.damageRadius);
        radius = GetComponent<SpriteRenderer>().bounds.extents.x;
        
        finalDamage = Random.Range(enemyData.attackDamage * (1 - enemyData.attackDamagePercentage / 100f), enemyData.attackDamage * (1 + enemyData.attackDamagePercentage / 100f)); // Hasar aralığını hesapla
        finalDamage = Mathf.Round(finalDamage * 10f) / 10f; // Ondalık hassasiyetini ayarlamak için
    }

    void Update()
    {
        switch (enemyData.throwDamageType)
        {
            case ThrowDamageType.Instant:
                InstantDamage();
                if (Time.time - startTime >= enemyData.attackCooldown - 0.25f)
                {
                    Destroy(gameObject);
                }
                break;

            case ThrowDamageType.OverTime:
                if (Time.time - overTimeDamageStartTime >= enemyData.overTimeDamageInterval)
                {
                    OverTimeDamage();
                    overTimeDamageStartTime = Time.time;
                }

                if (Time.time - startTime >=  enemyData.damageOverTimeDuration)
                {
                    Destroy(gameObject);
                }
                break;
            default:
                break;
        }   
    }

    void InstantDamage()
    {
        var hits = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (var h in hits)
        {
            if(h != null && h.CompareTag("Player"))
            {
                PlayerStats.Instance.DecreaseHealth(finalDamage);
                finalDamage = 0;
            }
        }
    }

    void OverTimeDamage()
    {
        var hits = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (var h in hits)
        {
            if(h != null && h.CompareTag("Player"))
            {
                finalDamage = Random.Range(enemyData.attackDamage * (1 - enemyData.attackDamagePercentage / 100f), enemyData.attackDamage * (1 + enemyData.attackDamagePercentage / 100f)); // Hasar aralığını hesapla
                finalDamage = Mathf.Round(finalDamage * 10f) / 10f;
                PlayerStats.Instance.DecreaseHealth(finalDamage);
            }
        }
    }
}
