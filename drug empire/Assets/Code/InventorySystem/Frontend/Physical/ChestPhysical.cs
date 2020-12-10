using System.Collections;
using System.Collections.Generic;
using PlayerController;
using UnityEngine;

namespace InventorySystem
{
    public class ChestPhysical : InventoryPhysical
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

        // public override void Interact()
        // {
        //     base.Interact();
        //     gameObject.GetComponent<Animator>().SetBool("Open", DisplayInventory.Instance.isInventoryActive);
        // }
        //
        // public override void OnTriggerExit2D(Collider2D collision)
        // {
        //     base.OnTriggerExit2D(collision);
        //     
        //     if(collision.GetComponent<Player>())
        //         gameObject.GetComponent<Animator>().SetBool("Open", DisplayInventory.Instance.isInventoryActive);
        // }
    }
}