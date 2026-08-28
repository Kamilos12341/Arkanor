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
        [SerializeField, Min(1)] private int maxStack = 1;

        [Header("Use Effect")]
        public ItemUseEffect UseEffect => useEffect;

        public string ItemId => itemId;
        public string ItemName => itemName;
        public string Description => description;
        public Sprite Icon => icon;
        public int MaxStack => maxStack;
        [SerializeField] private ItemUseEffect useEffect;

        
    }
}