using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControllerFPS : MonoBehaviour
{
    public float sensitivity = 2f;

    private float xRotation = 0f;
    private Vector2 lookInput;

    // InputActions asset reference
    private PlayerInputActions inputActions;

    void Awake()
    {
        // Create Input Actions instance
        inputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        // Enable the input action map
        inputActions.Player.Enable();

        // Subscribe to Look input
        inputActions.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        inputActions.Player.Look.canceled += ctx => lookInput = Vector2.zero;
    }

    void OnDisable()
    {
        inputActions.Player.Disable();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        float mouseX = lookInput.x * sensitivity;
        float mouseY = lookInput.y * sensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Vertical rotation (camera)
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Horizontal rotation (player body)
        transform.parent.Rotate(Vector3.up * mouseX);
    }
}
