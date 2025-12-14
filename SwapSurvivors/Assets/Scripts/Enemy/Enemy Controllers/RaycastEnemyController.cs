using UnityEngine;

public class RaycastEnemyController : EnemyControllerBase<RaycastEnemyData>
{
    protected override void Move() => KeepDistanceMovement(); 
}
