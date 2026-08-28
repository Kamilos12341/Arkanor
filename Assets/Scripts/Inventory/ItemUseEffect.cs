using UnityEngine;

namespace Arkanor.Inventory
{
    public abstract class ItemUseEffect : ScriptableObject
    {
        public abstract bool Use(GameObject user);
    }
}