using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    [CreateAssetMenu(fileName = "New Drug", menuName = "Inventory System/Items/Drug")]
    public class DrugObject : ItemObject
    {
        public int level;
        public float strength;
    }
}