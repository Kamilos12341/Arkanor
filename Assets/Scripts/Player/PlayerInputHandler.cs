using UnityEngine;
using UnityEngine.InputSystem;

namespace Arkanor.Player
{
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
        public InputAction Inventory => input.Player.Inventory;

        private void Awake()
        {
            input = new PlayerInputActions();
        }

        private void OnEnable()
        {
            if (input == null)
                return;

            input.Enable();
        }

        private void OnDisable()
        {
            if (input == null)
                return;

            input.Disable();
        }
    }
}