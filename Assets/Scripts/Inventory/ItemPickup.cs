using UnityEngine;
using Arkanor.Player;

namespace Arkanor.Inventory
{
    public class ItemPickup : MonoBehaviour
    {
        [Header("Item")]
        [SerializeField] private ItemDefinition item;
        [SerializeField] private int amount = 1;

        private void OnTriggerEnter2D(Collider2D other)
        {
            PlayerReference playerReference =
                other.GetComponent<PlayerReference>();

            if (playerReference == null)
                return;

            Inventory inventory =
                other.GetComponent<Inventory>();

            if (inventory == null)
            {
                Debug.LogWarning(
                    "Player nie posiada komponentu Inventory."
                );

                return;
            }

            if (item == null)
            {
                Debug.LogWarning(
                    $"{gameObject.name} nie posiada ItemDefinition."
                );

                return;
            }

            bool added = inventory.AddItem(item, amount);

            if (!added)
            {
                Debug.Log(
                    $"Nie można dodać {item.ItemName} do inventory."
                );

                return;
            }

            Debug.Log(
                $"Dodano do inventory: " +
                $"{item.ItemName} x{amount}"
            );

            Destroy(gameObject);
        }
    }
}