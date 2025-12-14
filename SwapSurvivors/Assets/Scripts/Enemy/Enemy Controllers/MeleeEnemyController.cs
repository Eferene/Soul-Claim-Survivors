using UnityEngine;

public class MeleeEnemyController : EnemyControllerBase<MeleeEnemyData>
{
    protected override void Move() => MoveTowardsPlayer();
}
