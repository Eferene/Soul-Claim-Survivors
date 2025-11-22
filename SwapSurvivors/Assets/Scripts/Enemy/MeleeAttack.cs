using UnityEngine;

public class MeleeAttack : BaseAttackType
{
    public override void Attack(Transform enemyTransform, Transform targetTransform, float damage, float range)
    {
        if(Vector3.Distance(enemyTransform.position, targetTransform.position) <= range)
        {
            PlayerStats.Instance.DecreaseHealth(damage);
            Debug.Log("Player hit! Current Health: " + PlayerStats.Instance.PlayerHealth);
        }
    }
}