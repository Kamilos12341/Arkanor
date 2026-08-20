using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private CharacterStats stats;
    private PlayerInputHandler input;

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
        input = GetComponent<PlayerInputHandler>();
    }

    private void OnEnable()
    {
        if (input == null)
            return;

        input.MoveUp.started += OnMoveUp;
        input.MoveDown.started += OnMoveDown;
        input.MoveLeft.started += OnMoveLeft;
        input.MoveRight.started += OnMoveRight;

        input.Move.performed += OnGamepadMove;
    }

    private void OnDisable()
    {
        if (input == null)
            return;

        input.MoveUp.started -= OnMoveUp;
        input.MoveDown.started -= OnMoveDown;
        input.MoveLeft.started -= OnMoveLeft;
        input.MoveRight.started -= OnMoveRight;

        input.Move.performed -= OnGamepadMove;
    }

    private void Update()
    {
        Vector2 rawInput = input.Move.ReadValue<Vector2>();

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

        rb.linearVelocity =
            movement * stats.MoveSpeed.Value;
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