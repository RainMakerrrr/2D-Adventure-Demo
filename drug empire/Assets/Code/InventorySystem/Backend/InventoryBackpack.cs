using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    [CreateAssetMenu(fileName = "New Backpack", menuName = "Inventory System/Backpack")]
    public class InventoryBackpack : InventoryObject
    {
        public InventorySlot activeSlot;
        

    }
}