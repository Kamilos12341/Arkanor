using TMPro;
using UnityEngine;

namespace Arkanor.Inventory
{
    public class ItemTooltip : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject tooltipObject;
        [SerializeField] private TMP_Text itemNameText;
        [SerializeField] private TMP_Text descriptionText;

        private void Awake()
        {
            Hide();
        }

        public void Show(ItemDefinition item)
        {
            if (item == null)
            {
                Hide();
                return;
            }

            if (itemNameText != null)
                itemNameText.text = item.ItemName;

            if (descriptionText != null)
                descriptionText.text = item.Description;

            if (tooltipObject != null)
                tooltipObject.SetActive(true);
        }

        public void Hide()
        {
            if (tooltipObject != null)
                tooltipObject.SetActive(false);
        }
    }
}