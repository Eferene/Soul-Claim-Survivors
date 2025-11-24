using UnityEngine;

public class ShotgunCharacter : BaseCharacterController
{
    [Header("Shotgun Özellikleri")]
    [SerializeField] private float cooldownMultiplier = 1.0f; // Tırpan için saldırı hızı çarpanı
    [SerializeField] private float damageMultiplier = 1.0f;         // Tırpan için hasar çarpanı
    [SerializeField] private float speedMultiplier = 1.0f;          // Tırpan karakteri için hız çarpanı
    [SerializeField] private float rangeMultiplier = 5f;
    [SerializeField] private LayerMask enemyLayer;

    private float currentCooldown;
    private float currentRange;
    private float currentDamage;
    private float currentSpeed;

    protected override void Awake()
    {
        base.Awake();
        ApplyUpgrades();
    }

    void ApplyUpgrades()
    {
        currentCooldown = attacCooldown * cooldownMultiplier;  // Tırpan için saldırı hızı
        currentRange = attackRange * rangeMultiplier;           // Tırpan için saldırı menzili
        currentDamage = PlayerStats.Instance.PlayerDamage * damageMultiplier * upgradesDamageMultiplier; // Tırpan için hasar
        currentSpeed = playerSpeed * speedMultiplier;               // Tırpan karakteri için hız
    }

    protected override void Attack()
    {
        throw new System.NotImplementedException();
    }
}
