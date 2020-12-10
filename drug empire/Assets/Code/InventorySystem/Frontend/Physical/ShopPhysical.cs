using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    public class ShopPhysical : InventoryPhysical
    {
        private void Awake()
        {
            Initialize();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                inventory.Save();
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                inventory.Load();
            }

        }

        private void OnApplicationQuit()
        {
            inventory.Clear();
        }
    }
}