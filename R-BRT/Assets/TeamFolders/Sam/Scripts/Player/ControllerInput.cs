using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerInput : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float lookSpeed = 10f;
    public Transform playerCamera;

    private DefaultInputActions inputActions;
    private Vector2 moveInput;
    private Vector2 lookInput;

    private void Awake()
    {
        inputActions = new DefaultInputActions();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.Look.performed += OnLook;
        inputActions.Player.Look.canceled += OnLook;
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Look.performed -= OnLook;
        inputActions.Player.Look.canceled -= OnLook;
    }

    private void Update()
    {
        Move();
        Look();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    private void Move()
    {
        Vector3 move = new Vector3(moveInput.x, 0, moveInput.y) * movementSpeed * Time.deltaTime;
        transform.Translate(move, Space.Self);
    }

    private void Look()
    {
        Vector2 look = lookInput * lookSpeed * Time.deltaTime;
        playerCamera.Rotate(Vector3.up, look.x);
        playerCamera.Rotate(Vector3.left, look.y);
    }
}
