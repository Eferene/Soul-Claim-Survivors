using UnityEngine;
using System.Collections;
using System;

public class SuicideAttack : EnemyAttack
{
    bool isTouched = false;
    public override bool Attack(Transform enemyTransform, Transform targetTransform, float damage, float damagePercentage, float range)
    {
        if (Vector2.Distance(enemyTransform.position + enemyTransform.GetComponent<EnemyController>().enemyData.attackOffset, targetTransform.position) <= range && !isTouched) // Menzil kontrolü
        {
            int dmg = Convert.ToInt32(UnityEngine.Random.Range(damage * (1 - damagePercentage / 100f), damage * (1 + damagePercentage / 100f))); // Hasar aralığını hesapla
            isTouched = true;
            if(isTouched) StartCoroutine(Explode(dmg));

            return true;
        }
        return false;
    }

    IEnumerator Explode(int dmg)
    {
        GetComponent<EnemyController>().isExploding = true;
        EnemyData enemyData = GetComponent<EnemyController>().enemyData;
        GameObject expAreaClone = Instantiate(enemyData.explodeArea, transform.position, Quaternion.identity);
        expAreaClone.transform.localScale = new Vector3(enemyData.explodeAreaRadius * 2, enemyData.explodeAreaRadius * 2, enemyData.explodeAreaRadius * 2);
        expAreaClone.transform.parent = transform;
        Color expAreaStartColor = expAreaClone.GetComponent<SpriteRenderer>().color;
        Color startColor = Color.white;
        Vector3 startScale = transform.localScale;
        float t = 0;

        while(t < enemyData.explodeDuration)
        {
            t += Time.deltaTime;
            GetComponent<SpriteRenderer>().color = Color.Lerp(startColor, Color.red, t / enemyData.explodeDuration);
            transform.localScale = Vector3.Lerp(startScale, startScale * 1.1f, t / enemyData.explodeDuration);
            float trans = Mathf.Lerp(0, 0.35f, t / enemyData.explodeDuration);
            expAreaClone.GetComponent<SpriteRenderer>().color = new Color(expAreaStartColor.r, expAreaStartColor.b, expAreaStartColor.b, trans); 
            yield return null;
        }
        
        GetComponent<SpriteRenderer>().color = Color.red;
        var hits = Physics2D.OverlapCircleAll(transform.position, enemyData.explodeAreaRadius, LayerMask.GetMask("Player"));

        foreach (var h in hits)
        {
            if (h != null && h.CompareTag("Player"))
            {
                if (h.TryGetComponent<PlayerManager>(out PlayerManager player))
                {
                    player.TakeDamageCharacter(dmg);
                }
                dmg = 0;
            }
        }

        GetComponent<EnemyController>().DieEffect();
        Destroy(gameObject);
    }
}