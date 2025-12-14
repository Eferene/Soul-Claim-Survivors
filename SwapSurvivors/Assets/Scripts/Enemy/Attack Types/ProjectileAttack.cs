using UnityEngine;
using System;

public class ProjectileAttack : EnemyAttack
{
    public override bool Attack(Transform enemyTransform, Transform targetTransform, float damage, float damagePercentage, float range)
    {
        if(Vector2.Distance(enemyTransform.position + enemyTransform.GetComponent<ProjectileEnemyController>().enemyData.attackOffset, targetTransform.position) <= range) // Menzil kontrolü
        {
            ProjectileEnemyController enemyController = enemyTransform.GetComponent<ProjectileEnemyController>();
            if (enemyController != null && enemyController.enemyData.projectilePrefab != null)
            {
                GameObject projectileGameObject = Instantiate(enemyController.enemyData.projectilePrefab, enemyTransform.position + enemyController.enemyData.attackOffset, Quaternion.identity);

                int dmg = Convert.ToInt32(UnityEngine.Random.Range(damage * (1 - damagePercentage / 100f), damage * (1 + damagePercentage / 100f))); // Hasar aralığını hesapla

                ProjectileStraight projectile = projectileGameObject.GetComponent<ProjectileStraight>();
                projectile.damage = dmg;
                projectile.speed = enemyController.enemyData.projectileSpeed;

                return true;
            }
        }
        return false;
    }
}
