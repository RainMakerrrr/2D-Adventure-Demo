using System.Collections.Generic;
using QuestingSystem;
using QuestingSystem.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace InventorySystem
{
    public class DisplayInventory : MonoBehaviour
    {
        [SerializeField] private Button _closeInventoryButton;
        [SerializeField] private ScrollRect _phoneScrollRect;
        
        public static DisplayInventory Instance { get; private set; }

        public enum InventoryState { Player, Chest, Shop, Closed }
        [HideInInspector] public InventoryState inventoryState = InventoryState.Closed;

        public InventoryBackpack playerInventory;

        [Header("Scene refs")]
        public GameObject inventoryPlayerUI;
        public GameObject inventoryStashUI;
        public GameObject exchangeWindowUI;

        [Header("Prefab refs")]
        public GameObject slotPrefabStash;
        public GameObject slotPrefabInventory;

        public bool isInventoryActive;

        public InventoryPhysical closestStash;
        //public AddressQuestReceiver closestQuestStash;

        private SlotUI selectedSlotUI;
        private InventorySlot selectedSlot;

        void Awake()
        {
            Instance = this;
        }

        void Update()
        {
            InventoryInput();
            
            _closeInventoryButton.onClick.AddListener(CloseInventory);

        }

        void OpenExchange(SlotUI _slotUI)
        {
            if (inventoryState != InventoryState.Player)
            {
                if (_slotUI != null)
                {
                    selectedSlot = _slotUI.ParentSlot;
                    selectedSlotUI = _slotUI;
                    if (_slotUI.ParentSlot.Amount > 1)
                    {
                        exchangeWindowUI.SetActive(true);
                        exchangeWindowUI.GetComponent<ExchangeUI>().amountSlider.maxValue = _slotUI.ParentSlot.Amount;
                    }
                    else
                    {
                        if (_slotUI.ParentInventory is InventoryBackpack)
                            Stash(1, closestStash.inventory, selectedSlot, selectedSlotUI.ParentInventory);
                        else
                            Stash(1, playerInventory, selectedSlot, selectedSlotUI.ParentInventory);
                    }
                }
                else
                {
                    selectedSlot = null;
                    selectedSlotUI = null;
                    exchangeWindowUI.SetActive(false);
                }
            }
        }

        public void LMBPressed(SlotUI _slotUI)
        {
            OpenExchange(_slotUI);
        }

        public void RMBPressed(SlotUI _slotUI)
        {
            for (int i = _slotUI.ParentInventory.slots.Count - 1; i >= 0; i--)
            {
                InventorySlot slot = _slotUI.ParentInventory.slots[i];
                if (_slotUI.ParentInventory is InventoryBackpack)
                    Stash(slot.Amount, closestStash.inventory, slot, _slotUI.ParentInventory);
                else
                    Stash(slot.Amount, playerInventory, slot, _slotUI.ParentInventory);
            }
        }

        void Stash(int _amount, InventoryObject _stash, InventorySlot _slot, InventoryObject _sender)
        {
            int countInStash = _stash.CheckWeight(_slot.item, _amount);
            _stash.AddItem(_slot.item, _amount);
            _sender.RemoveItem(_slot.item, _amount);

            if (inventoryState == InventoryState.Shop)
            {
                float price;
                if (_sender is InventoryBackpack)
                    price = _slot.sellPrice;
                else
                    price = _slot.buyPrice;
                _stash.Money -= price * countInStash;
                _sender.Money += price * countInStash;
            }

            UpdateInventories();
        }

        public void OpenInventory()
        {
            if (!exchangeWindowUI.activeSelf && closestStash != null)
            {
                if (closestStash.inventory is InventoryShop)
                    inventoryState = InventoryState.Shop;
                else if (closestStash.inventory is InventoryChest)
                    inventoryState = InventoryState.Chest;
                UpdateInventories();
                
                if(!_phoneScrollRect.gameObject.activeSelf)
                    _phoneScrollRect.gameObject.SetActive(!_phoneScrollRect.gameObject.activeSelf);
                
            }
        }

        public void CloseInventory()
        {
            isInventoryActive = false;

            UpdateInventories();

            closestStash = null;
            exchangeWindowUI.gameObject.SetActive(false);
        }

        void InventoryInput()
        {
            if (Input.GetButtonDown("Inventory"))
                OpenPlayerInventory();

            if (Input.GetButtonDown("Cancel"))
                OpenExchange(null);

            if (exchangeWindowUI.activeSelf && Input.GetButtonDown("Submit"))
            {
                Exchange();
            }

            if (Input.GetButtonDown("Fire3") && inventoryState == InventoryState.Chest && playerInventory.activeSlot != null)
            {
                InventorySlot _miniSlot = playerInventory.activeSlot;
                Stash(_miniSlot.Amount, closestStash.inventory, _miniSlot, _miniSlot._slotUI.ParentInventory);
                playerInventory.activeSlot = null;
                DisplayToolbar.Instance.miniSlots[DisplayToolbar.Instance.toolbarSlotID].slot = null;
                DisplayToolbar.Instance.miniSlots[DisplayToolbar.Instance.toolbarSlotID].icon.sprite = null;
            }
        }

        public void Exchange()
        {
            int _amount = (int)exchangeWindowUI.GetComponent<ExchangeUI>().amountSlider.value;
            exchangeWindowUI.SetActive(false);
            if (selectedSlotUI.ParentInventory is InventoryBackpack)
                Stash(_amount, closestStash.inventory, selectedSlot, selectedSlotUI.ParentInventory);
            else
                Stash(_amount, playerInventory, selectedSlot, selectedSlotUI.ParentInventory);
            
            if (playerInventory.activeSlot != null)
            {
                playerInventory.activeSlot = null;
                DisplayToolbar.Instance.miniSlots[DisplayToolbar.Instance.toolbarSlotID].slot = null;
                DisplayToolbar.Instance.miniSlots[DisplayToolbar.Instance.toolbarSlotID].icon.sprite = null;
            }
        }

        void MakeGrey(InventoryObject _inventoryFrom, InventoryObject _inventoryTo)
        {
            foreach (KeyValuePair<InventorySlot, GameObject> slotUI in _inventoryTo.itemsDisplayed)
            {
                if (_inventoryFrom.CheckWeight(slotUI.Key.item, 1) == 0 ||
                    (_inventoryFrom.Money - slotUI.Key.buyPrice < 0 && inventoryState == InventoryState.Shop))
                {
                    slotUI.Value.GetComponent<Button>().interactable = false;
                    slotUI.Value.GetComponent<SlotUI>().GreyPanel.SetActive(true);
                }
                else
                {
                    slotUI.Value.GetComponent<Button>().interactable = true;
                    slotUI.Value.GetComponent<SlotUI>().GreyPanel.SetActive(false);
                }
            }
        }

        public void OpenPlayerInventory()
        {
            if (inventoryState == InventoryState.Closed || inventoryState == InventoryState.Player)
            {
                isInventoryActive = !isInventoryActive;
                inventoryState = InventoryState.Player;
                UpdateInventory(inventoryPlayerUI, playerInventory);
                
                
            }
        }

        void UpdateInventory(GameObject _inventoryUI, InventoryObject _inventory)
        {
            _inventoryUI.SetActive(isInventoryActive);
            if (_inventoryUI != null)
            {
                if (isInventoryActive)
                {
                    _inventoryUI.GetComponent<InventoryUI>().Initialize(_inventory);
                    UpdateSlots(_inventory, _inventoryUI.GetComponent<InventoryUI>().Content);
                }
                else
                {
                    foreach (KeyValuePair<InventorySlot, GameObject> _slot in _inventory.itemsDisplayed)
                        Destroy(_inventory.itemsDisplayed[_slot.Key]);
                    _inventory.itemsDisplayed.Clear();
                    inventoryState = InventoryState.Closed;
                }
            }
        }

        public void UpdateInventories()
        {
            UpdateInventory(inventoryPlayerUI, playerInventory);
            UpdateInventory(inventoryStashUI, closestStash.inventory);
            MakeGrey(playerInventory, closestStash.inventory);
            MakeGrey(closestStash.inventory, playerInventory);
        }
        
        void UpdateSlots(InventoryObject _inventory, Transform _parent)
        {
            for (int i = 0; i < _inventory.slots.Count; i++)
            {
                if (!_inventory.itemsDisplayed.ContainsKey(_inventory.slots[i]))
                {
                    GameObject _slot = null;
                    if (_inventory is InventoryBackpack)
                        _slot = Instantiate(slotPrefabInventory, Vector3.zero, Quaternion.identity);
                    else
                        _slot = Instantiate(slotPrefabStash, Vector3.zero, Quaternion.identity);
                    _slot.transform.SetParent(_parent, false);
                    _slot.transform.SetAsLastSibling();
                    _slot.GetComponent<SlotUI>().Initialize(_inventory.slots[i], _inventory, i);
                    _inventory.itemsDisplayed.Add(_inventory.slots[i], _slot);
                    _inventory.slots[i]._slotUI = _slot.GetComponent<SlotUI>();
                }
            }
        }

    }
}
