using UnityEngine;

public class ProjectileEnemyController : EnemyControllerBase<ProjectileEnemyData>
{
    protected override void Move() => KeepDistanceMovement(); 
}
