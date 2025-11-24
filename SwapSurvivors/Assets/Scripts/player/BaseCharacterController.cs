using UnityEngine;

public abstract class BaseCharacterController : MonoBehaviour
{
    // --- Components ---
    protected Rigidbody2D rb;
    protected InputActions controls;
    protected EnemyController enemyController;

    // --- Movement ---
    protected Vector2 moveInput;
    protected float playerSpeed;

    // --- Combat ---
    protected float attacCooldown;
    protected float attackRange;
    protected float lastAttackTime = 0f;
    protected float upgradesDamageMultiplier = 1.0f;

    // --- Unity Methods ---
    protected virtual void Awake()
    {
        playerSpeed = PlayerStats.Instance.PlayerSpeed;
        attacCooldown = PlayerStats.Instance.AttackCooldown;
        rb = GetComponent<Rigidbody2D>();
        controls = new InputActions();

        controls.Player.Move.performed += ctx => { moveInput = ctx.ReadValue<Vector2>(); };
        controls.Player.Move.canceled += ctx => { moveInput = Vector2.zero; };
    }

    protected virtual void OnEnable() => controls.Player.Enable();
    protected virtual void OnDisable() => controls.Player.Disable();

    protected virtual void Update()
    {
        // Saldırı cooldown kontrolü
        if (Time.time >= lastAttackTime + attacCooldown)
        {
            Attack();
            lastAttackTime = Time.time;
        }
    }

    protected virtual void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput.x * playerSpeed, moveInput.y * playerSpeed);
    }

    // --- Abstract Methods ---
    protected abstract void Attack(); // Saldırı yöntemi, türetilmiş sınıflarda uygulanacak
}
