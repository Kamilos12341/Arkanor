using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private CharacterStats stats;
    private PlayerInputActions input;

    private Vector2 movement;

    public Vector2 Movement => movement;

    private enum AxisPriority
    {
        Horizontal,
        Vertical
    }

    private AxisPriority lastPriority = AxisPriority.Vertical;

    public bool CanMove { get; set; } = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<CharacterStats>();

        input = new PlayerInputActions();
    }

    private void OnEnable()
    {
        input.Enable();

        input.Player.MoveUp.started += OnMoveUp;
        input.Player.MoveDown.started += OnMoveDown;
        input.Player.MoveLeft.started += OnMoveLeft;
        input.Player.MoveRight.started += OnMoveRight;

        input.Player.Move.performed += OnGamepadMove;
    }

    private void OnDisable()
    {
        input.Player.MoveUp.started -= OnMoveUp;
        input.Player.MoveDown.started -= OnMoveDown;
        input.Player.MoveLeft.started -= OnMoveLeft;
        input.Player.MoveRight.started -= OnMoveRight;

        input.Player.Move.performed -= OnGamepadMove;

        input.Disable();
    }

    private void Update()
    {
        Vector2 rawInput = input.Player.Move.ReadValue<Vector2>();

        if (rawInput.x != 0 && rawInput.y != 0)
        {
            movement = lastPriority == AxisPriority.Horizontal
                ? new Vector2(rawInput.x, 0)
                : new Vector2(0, rawInput.y);
        }
        else
        {
            movement = rawInput;
        }
    }

    private void FixedUpdate()
    {
        if (!CanMove)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        rb.linearVelocity = movement * stats.MoveSpeed.Value;
    }

    private void OnMoveUp(InputAction.CallbackContext context)
    {
        lastPriority = AxisPriority.Vertical;
    }

    private void OnMoveDown(InputAction.CallbackContext context)
    {
        lastPriority = AxisPriority.Vertical;
    }

    private void OnMoveLeft(InputAction.CallbackContext context)
    {
        lastPriority = AxisPriority.Horizontal;
    }

    private void OnMoveRight(InputAction.CallbackContext context)
    {
        lastPriority = AxisPriority.Horizontal;
    }

    private void OnGamepadMove(InputAction.CallbackContext context)
    {
        if (context.control.device is not Gamepad)
            return;

        Vector2 value = context.ReadValue<Vector2>();

        if (Mathf.Abs(value.x) > Mathf.Abs(value.y))
        {
            lastPriority = AxisPriority.Horizontal;
        }
        else if (Mathf.Abs(value.y) > 0)
        {
            lastPriority = AxisPriority.Vertical;
        }
    }
}