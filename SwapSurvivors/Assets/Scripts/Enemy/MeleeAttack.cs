using System;
using UnityEngine;

public class MeleeAttack : EnemyAttack
{
    [SerializeField] PlayerManager playerManager;

    public override bool Attack(Transform enemyTransform, Transform targetTransform, float damage, float damagePercentage, float range)
    {
        if (Vector2.Distance(enemyTransform.position + enemyTransform.GetComponent<EnemyController>().enemyData.attackOffset, targetTransform.position) <= range) // Menzil kontrolü
        {
            int dmg = Convert.ToInt32(UnityEngine.Random.Range(damage * (1 - damagePercentage / 100f), damage * (1 + damagePercentage / 100f))); // Hasar aralığını hesapla

            PlayerManager playerManager = targetTransform.GetComponent<PlayerManager>();
            playerManager.TakeDamageCharacter(dmg);
            Debug.Log("| " + dmg + " | Player hit! Current Health: " + playerManager.CurrentHealth);
            return true;
        }
        return false;
    }
}