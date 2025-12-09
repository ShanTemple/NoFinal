using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementCC : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 5f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    private CharacterController cc;
    private Vector3 velocity;
    private Vector2 moveInput;

    private PlayerInputActions inputActions;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        inputActions = new PlayerInputActions(); 
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();

        // Movement input
        inputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        // Jump input
        inputActions.Player.Jump.performed += ctx => Jump();
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }

    private void Update()
    {
        HandleMovement();
        ApplyGravity();
    }

    private void HandleMovement()
    {
        // Move player horizontally
        Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;
        cc.Move(move * speed * Time.deltaTime);
    }

    private void ApplyGravity()
    {
        if (cc.isGrounded && velocity.y < 0)
            velocity.y = -2f; // Downward force to stick to ground

        velocity.y += gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);
    }

    private void Jump()
    {
        if (cc.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }
}
