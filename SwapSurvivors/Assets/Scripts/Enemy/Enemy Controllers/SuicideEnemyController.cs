using UnityEngine;

public class SuicideEnemyController : EnemyControllerBase<SuicideEnemyData>
{
    public bool isExploding;

    protected override bool CanMove()
    {
        return base.CanMove() && !isExploding;
    }
    
    protected override void Move() => KeepDistanceMovement(); 
}