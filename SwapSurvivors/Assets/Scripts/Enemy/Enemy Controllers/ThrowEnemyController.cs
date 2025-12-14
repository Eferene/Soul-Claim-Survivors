using UnityEngine;

public class ThrowEnemyController : EnemyControllerBase<ThrowEnemyData>
{
    protected override void Move() => KeepDistanceMovement(); 
}
