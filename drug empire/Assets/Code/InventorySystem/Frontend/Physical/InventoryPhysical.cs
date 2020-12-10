using PlayerController;
using UnityEngine;


namespace InventorySystem
{
    public class InventoryPhysical : MonoBehaviour, IInteractable
    {
        [SerializeField] protected InteractionButton _interactionButton;
        
        public InventoryObject inventory;
        private bool isOpen = false;

        

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<Player>())
            {
                _interactionButton.Inventory = gameObject;
                _interactionButton.gameObject.SetActive(true);
            }
        }


        protected void Initialize()
        {
            if (inventory.slots.Count == 0)
            {
                foreach (InventorySlot slot in inventory.defaultSlots)
                    inventory.AddItem(slot.item, slot.Amount);
            }

            if (inventory.backpackCapacity.CurrentWeight <= 0)
            {
                foreach (InventorySlot slot in inventory.slots)
                    for (int i = 0; i < slot.Amount; i++)
                        inventory.backpackCapacity.CurrentWeight += slot.item.weight;
            }
            if (inventory.defaultMoney > 0)
                inventory.Money = inventory.defaultMoney;
        }
        

        public virtual void OnTriggerExit2D(Collider2D other)
        {
            var player = other.GetComponent<Player>();
            
            if (player)
            {
                if (DisplayInventory.Instance.isInventoryActive)
                {
                    //DisplayInventory.Instance.CloseInventory();
                    DisplayInventory.Instance.inventoryPlayerUI.SetActive(false);
                    DisplayInventory.Instance.inventoryStashUI.SetActive(false);
                    DisplayInventory.Instance.closestStash = null;
                    DisplayInventory.Instance.isInventoryActive = false;
                    player.State = PlayerState.IDLE;
                }
            }
        }

        public virtual void Interact()
        {
            if (inventory.Money <= 40)
                inventory.Money = inventory.defaultMoney;
            
            DisplayInventory.Instance.isInventoryActive = !DisplayInventory.Instance.isInventoryActive;
            
            DisplayInventory.Instance.closestStash = this;
            DisplayInventory.Instance.OpenInventory();

            FindObjectOfType<Player>().State =
                DisplayInventory.Instance.isInventoryActive ? PlayerState.LOOTING : PlayerState.IDLE;
            
            _interactionButton.gameObject.SetActive(false);
            
        }
        
    }
}