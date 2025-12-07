using UnityEngine;
using System;

public class ThrowObject : MonoBehaviour
{
    public EnemyData enemyData;
    private float startTime;
    private int finalDamage;
    private float radius;

    // OverTime
    private float overTimeDamageStartTime;

    void Start()
    {
        startTime = Time.time;
        overTimeDamageStartTime = Time.time;

        transform.localScale = new Vector3(enemyData.damageRadius, enemyData.damageRadius, enemyData.damageRadius);
        radius = GetComponent<SpriteRenderer>().bounds.extents.x;

        finalDamage = Convert.ToInt32(UnityEngine.Random.Range(enemyData.attackDamage * (1 - enemyData.attackDamagePercentage / 100f), enemyData.attackDamage * (1 + enemyData.attackDamagePercentage / 100f))); // Hasar aralığını hesapla

        if (enemyData.throwDamageType == ThrowDamageType.Instant)
        {
            InstantDamage();
        }
    }

    void Update()
    {
        switch (enemyData.throwDamageType)
        {
            case ThrowDamageType.Instant:
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

                if (Time.time - startTime >= enemyData.damageOverTimeDuration)
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
            if (h != null && h.CompareTag("Player"))
            {
                if (h.TryGetComponent<PlayerManager>(out PlayerManager player))
                {
                    player.TakeDamageCharacter(finalDamage);
                }
                finalDamage = 0;
            }
        }
    }

    void OverTimeDamage()
    {
        var hits = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (var h in hits)
        {
            if (h != null && h.CompareTag("Player"))
            {
                int finalDamage = Convert.ToInt32(UnityEngine.Random.Range(enemyData.attackDamage * (1 - enemyData.attackDamagePercentage / 100f), enemyData.attackDamage * (1 + enemyData.attackDamagePercentage / 100f))); // Hasar aralığını hesapla
                if (h.TryGetComponent<PlayerManager>(out PlayerManager player))
                {
                    player.TakeDamageCharacter(finalDamage);
                }
            }
        }
    }
}
