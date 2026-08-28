using UnityEngine;
using Arkanor.Player;

namespace Arkanor.Inventory
{
    public class InventoryWindowController : MonoBehaviour
    {
        [SerializeField]
        private GameObject inventoryWindow;

        private PlayerInputHandler input;

        private void Awake()
        {
            input = FindFirstObjectByType<PlayerInputHandler>();

            if (inventoryWindow == null)
            {
                Debug.LogError(
                    "InventoryWindowController nie ma przypisanego Inventory Window."
                );
            }
        }

        private void Start()
        {
            if (inventoryWindow != null)
                inventoryWindow.SetActive(false);
        }

        private void Update()
        {
            if (input == null || inventoryWindow == null)
                return;

            if (input.Inventory.WasPressedThisFrame())
            {
                inventoryWindow.SetActive(
                    !inventoryWindow.activeSelf
                );
            }
        }
    }
}