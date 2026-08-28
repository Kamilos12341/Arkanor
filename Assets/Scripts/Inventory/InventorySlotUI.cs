using UnityEngine;
using UnityEngine.EventSystems;

namespace Arkanor.Inventory
{
    public class InventorySlotUI : MonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler
    {
        private ItemTooltip tooltip;
        private ItemDefinition item;

        public void Initialize(ItemTooltip tooltip)
        {
            this.tooltip = tooltip;
        }

        public void SetItem(ItemDefinition item)
        {
            this.item = item;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (item != null && tooltip != null)
                tooltip.Show(item);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (tooltip != null)
                tooltip.Hide();
        }
    }
}