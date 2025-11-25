using UnityEngine;

public abstract class BaseCharacterController : MonoBehaviour
{
    // --- Components ---
    protected Rigidbody2D rb;
    protected InputActions controls;

    // --- Movement ---
    protected Vector2 moveInput;
    protected float playerSpeed;

    // --- Combat ---
    protected float lastAttackTime = 0f;

    // --- Unity Methods ---
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controls = new InputActions();

        controls.Player.Move.performed += ctx => { moveInput = ctx.ReadValue<Vector2>(); };
        controls.Player.Move.canceled += ctx => { moveInput = Vector2.zero; };
    }

    protected virtual void OnEnable() => controls.Player.Enable();
    protected virtual void OnDisable() => controls.Player.Disable();

    protected virtual void Update()
    {
        ApplyAttack();
    }

    protected virtual void ApplyAttack()
    {
        // Saldırı cooldown kontrolü
        if (Time.time >= lastAttackTime + GetCooldown())
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

    protected abstract float GetCooldown(); // Saldırı uygulama yöntemi, türetilmiş sınıflarda uygulanacak
}
