using UnityEngine;

public abstract class EnemyAttack : MonoBehaviour
{ 
    public abstract bool Attack(Transform enemyTransform, Transform targetTransform, float damage, float damagePercentage, float range);
}