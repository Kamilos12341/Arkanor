using UnityEngine;

namespace Arkanor.Inventory
{
    [CreateAssetMenu(
        fileName = "NewItem",
        menuName = "Arkanor/Inventory/Item"
    )]
    public class ItemDefinition : ScriptableObject
    {
        [Header("Item")]
        [SerializeField] private string itemId;
        [SerializeField] private string itemName;
        [TextArea]
        [SerializeField] private string description;

        [Header("Visual")]
        [SerializeField] private Sprite icon;

        [Header("Stack")]
        [SerializeField] private int maxStack = 1;

        public string ItemId => itemId;
        public string ItemName => itemName;
        public string Description => description;
        public Sprite Icon => icon;
        public int MaxStack => maxStack;
    }
}