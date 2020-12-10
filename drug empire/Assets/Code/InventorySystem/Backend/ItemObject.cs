using UnityEngine;

namespace InventorySystem
{
    public abstract class ItemObject : ScriptableObject
    {
        public Sprite icon;
        public float weight;
        public float cost;
        public bool isStackable;
        [TextArea(5, 10)] public string description;
    }
}