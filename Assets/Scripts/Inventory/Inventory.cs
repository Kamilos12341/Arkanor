using System.Collections.Generic;
using UnityEngine;

namespace Arkanor.Inventory
{
    public class Inventory : MonoBehaviour
    {
        [System.Serializable]
        public class InventorySlot
        {
            public ItemDefinition Item;
            public int Amount;

            public bool IsEmpty => Item == null || Amount <= 0;
        }

        [Header("Inventory")]
        [SerializeField, Min(1)] private int capacity = 20;

        [SerializeField]
        private List<InventorySlot> slots = new();

        public int Capacity => capacity;
        public IReadOnlyList<InventorySlot> Slots => slots;

        public event System.Action OnInventoryChanged;

        private void Awake()
        {
            InitializeSlots();
        }

        public void InitializeSlots()
        {
            while (slots.Count < capacity)
            {
                slots.Add(new InventorySlot());
            }

            if (slots.Count > capacity)
            {
                slots.RemoveRange(
                    capacity,
                    slots.Count - capacity
                );
            }
        }

        public bool AddItem(ItemDefinition item, int amount = 1)
        {
            if (item == null || amount <= 0)
                return false;

            if (GetFreeSpace(item) < amount)
                return false;


            int remaining = amount;

            for (int i = 0; i < slots.Count; i++)
            {
                InventorySlot slot = slots[i];

                if (slot.Item != item)
                    continue;

                int space = item.MaxStack - slot.Amount;

                if (space <= 0)
                    continue;

                int added = Mathf.Min(space, remaining);

                slot.Amount += added;
                remaining -= added;

                if (remaining <= 0)
                {
                    OnInventoryChanged?.Invoke();
                    return true;
                }
            }

            for (int i = 0; i < slots.Count; i++)
            {
                InventorySlot slot = slots[i];

                if (!slot.IsEmpty)
                    continue;

                int added = Mathf.Min(
                    item.MaxStack,
                    remaining
                );

                slot.Item = item;
                slot.Amount = added;

                remaining -= added;

                if (remaining <= 0)
                {
                    OnInventoryChanged?.Invoke();
                    return true;
                }
            }

            return false;
        }

        public bool RemoveItem(ItemDefinition item, int amount = 1)
        {
            if (item == null || amount <= 0)
                return false;

            int remaining = amount;

            for (int i = 0; i < slots.Count; i++)
            {
                InventorySlot slot = slots[i];

                if (slot.Item != item)
                    continue;

                int removed = Mathf.Min(
                    slot.Amount,
                    remaining
                );

                slot.Amount -= removed;
                remaining -= removed;

                if (slot.Amount <= 0)
                {
                    slot.Item = null;
                    slot.Amount = 0;
                }

                if (remaining <= 0)
                {
                    OnInventoryChanged?.Invoke();
                    return true;
                }
            }

            return false;
        }

        public bool HasItem(ItemDefinition item, int amount = 1)
        {
            if (item == null || amount <= 0)
                return false;

            int total = 0;

            foreach (InventorySlot slot in slots)
            {
                if (slot.Item == item)
                {
                    total += slot.Amount;

                    if (total >= amount)
                        return true;
                }
            }

            return false;
        }

        private int GetFreeSpace(ItemDefinition item)
        {
            int freeSpace = 0;

            foreach (InventorySlot slot in slots)
            {
                if (slot.IsEmpty)
                {
                    freeSpace += item.MaxStack;
                }
                else if (slot.Item == item)
                {
                    freeSpace += Mathf.Max(
                        0,
                        item.MaxStack - slot.Amount
                    );
                }
            }

            return freeSpace;
        }

        public bool UseItem(int slotIndex, GameObject user)
        {
            if (slotIndex < 0 || slotIndex >= slots.Count)
                return false;

            InventorySlot slot = slots[slotIndex];

            if (slot.IsEmpty)
                return false;

            if (slot.Item.UseEffect == null)
                return false;

            if (!slot.Item.UseEffect.Use(user))
                return false;

            slot.Amount--;

            if (slot.Amount <= 0)
            {
                slot.Item = null;
                slot.Amount = 0;
            }

            OnInventoryChanged?.Invoke();

            return true;
        }
    }
}