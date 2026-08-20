using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInputActions input;

    public InputAction Move => input.Player.Move;
    public InputAction MoveUp => input.Player.MoveUp;
    public InputAction MoveDown => input.Player.MoveDown;
    public InputAction MoveLeft => input.Player.MoveLeft;
    public InputAction MoveRight => input.Player.MoveRight;

    public InputAction Interact => input.Player.Interact;
    public InputAction Attack => input.Player.Attack;

    private void Awake()
    {
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
}