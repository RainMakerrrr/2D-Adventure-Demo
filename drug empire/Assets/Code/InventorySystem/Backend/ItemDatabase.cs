﻿using UnityEngine;
using System.Collections.Generic;

namespace InventorySystem {

    [CreateAssetMenu(fileName = "New Database", menuName = "Inventory System/Database")]
    public class ItemDatabase : ScriptableObject, ISerializationCallbackReceiver
    {
        public ItemObject[] items;
        public Dictionary<ItemObject, int> GetId = new Dictionary<ItemObject, int>();
        public Dictionary<int, ItemObject> GetItem = new Dictionary<int, ItemObject>();

        public void OnAfterDeserialize()
        {
            GetId = new Dictionary<ItemObject, int>();
            GetItem = new Dictionary<int, ItemObject>();
            for (int i = 0; i < items.Length; i++)
            {
                GetId.Add(items[i], i);
                GetItem.Add(i, items[i]);
            }
        }

        public void OnBeforeSerialize()
        {

        }

    }
}