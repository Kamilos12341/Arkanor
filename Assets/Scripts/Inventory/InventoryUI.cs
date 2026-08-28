using Arkanor.Player;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Arkanor.Inventory
{
    public class InventoryUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Inventory inventory;
        [SerializeField] private Transform slotContainer;
        [SerializeField] private GameObject slotPrefab;
        [SerializeField] private ItemTooltip tooltip;

        private PlayerInputHandler input;
        private readonly List<GameObject> slotObjects = new();

        private void Awake()
        {
            input = FindFirstObjectByType<PlayerInputHandler>();

            if (inventory == null)
                inventory = FindFirstObjectByType<Inventory>();

            if (inventory == null)
            {
                Debug.LogError(
                    "InventoryUI nie znalazł komponentu Inventory."
                );

                return;
            }

            CreateSlots();
            Refresh();
        }

        private void OnEnable()
        {
            if (inventory == null)
                inventory = FindFirstObjectByType<Inventory>();

            if (inventory == null)
                return;

            inventory.OnInventoryChanged += Refresh;

            Refresh();
        }

        private void OnDisable()
        {
            if (inventory != null)
            {
                inventory.OnInventoryChanged -= Refresh;
            }
        }

        private void CreateSlots()
        {
            if (slotContainer == null || slotPrefab == null)
            {
                Debug.LogError(
                    "InventoryUI wymaga SlotContainer oraz Slot Prefab."
                );

                return;
            }

            foreach (GameObject slot in slotObjects)
            {
                Destroy(slot);
            }

            slotObjects.Clear();

            for (int i = 0; i < inventory.Capacity; i++)
            {
                GameObject slot =
                    Instantiate(slotPrefab, slotContainer);

                slot.name = $"InventorySlot_{i}";

                Button button = slot.GetComponent<Button>();

                if (button != null)
                {
                    int slotIndex = i;
                    button.onClick.AddListener(() => UseSlot(slotIndex));
                }
                else
                {
                    Debug.LogWarning(
                        $"Slot {slot.name} nie posiada komponentu Button."
                    );
                }

                InventorySlotUI slotUI =
                   slot.GetComponent<InventorySlotUI>();

                if (slotUI != null)
                    slotUI.Initialize(tooltip);

                slotObjects.Add(slot);

               
            }
        }

        private void UseSlot(int slotIndex)
        {
            if (input == null)
                return;

            GameObject player = input.gameObject;

            inventory.UseItem(slotIndex, player);
        }

        public void Refresh()
        {
            if (inventory == null)
                return;

            for (int i = 0; i < slotObjects.Count; i++)
            {
                Inventory.InventorySlot inventorySlot =
                    inventory.Slots[i];

                InventorySlotUI slotUI =
                    slotObjects[i].GetComponent<InventorySlotUI>();

                Transform iconTransform =
                    slotObjects[i].transform.Find("Icon");

                Transform amountTransform =
                    slotObjects[i].transform.Find("AmountText");

                if (iconTransform == null ||
                    amountTransform == null)
                {
                    Debug.LogError(
                        "InventorySlot musi posiadać " +
                        "dzieci Icon oraz AmountText."
                    );

                    continue;
                }

                Image icon =
                    iconTransform.GetComponent<Image>();

                TMP_Text amountText =
                    amountTransform.GetComponent<TMP_Text>();
                if (inventorySlot.IsEmpty)
                {
                    icon.enabled = false;
                    amountText.text = "";

                    if (slotUI != null)
                        slotUI.SetItem(null);
                }
                else
                {
                    icon.enabled = true;
                    icon.sprite = inventorySlot.Item.Icon;

                    amountText.text =
                        inventorySlot.Amount > 1
                            ? inventorySlot.Amount.ToString()
                            : "";

                    if (slotUI != null)
                        slotUI.SetItem(inventorySlot.Item);
                }
            }
        }
    }
}