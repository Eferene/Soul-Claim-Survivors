using UnityEngine;

public class OrbitWeapon : BaseWeaponController
{
    [SerializeField] private GameObject orbitWeapon;
    protected override void Attack()
    {

    }

    protected override float GetCooldown() => playerManager.CurrentCooldown;
}
