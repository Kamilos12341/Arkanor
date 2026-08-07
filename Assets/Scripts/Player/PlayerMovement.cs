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

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<CharacterStats>();

        input = new PlayerInputActions();
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    private void Update()
    {
        Vector2 rawInput = input.Player.Move.ReadValue<Vector2>();

        bool horizontalPressed = rawInput.x != 0;
        bool verticalPressed = rawInput.y != 0;

        if (Keyboard.current.aKey.wasPressedThisFrame ||
            Keyboard.current.dKey.wasPressedThisFrame)
        {
            lastPriority = AxisPriority.Horizontal;
        }

        if (Keyboard.current.wKey.wasPressedThisFrame ||
            Keyboard.current.sKey.wasPressedThisFrame)
        {
            lastPriority = AxisPriority.Vertical;
        }

        if (horizontalPressed && verticalPressed)
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
        rb.linearVelocity = movement * stats.MoveSpeed.Value;
    }
}