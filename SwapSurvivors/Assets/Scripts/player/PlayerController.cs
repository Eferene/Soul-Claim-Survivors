using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    InputActions controls;

    [SerializeField] private float playerSpeed = 5.0f;
    Vector2 moveInput;

    private void OnEnable() => controls.Player.Enable();
    private void OnDisable() => controls.Player.Disable();

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        controls = new InputActions();

        controls.Player.Move.performed += ctx => { moveInput = ctx.ReadValue<Vector2>(); };
        controls.Player.Move.canceled += ctx => { moveInput = Vector2.zero; };
    }
    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(moveInput.x * playerSpeed, moveInput.y * playerSpeed);
    }
}
