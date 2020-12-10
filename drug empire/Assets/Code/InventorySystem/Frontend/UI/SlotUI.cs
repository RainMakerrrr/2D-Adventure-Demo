using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace InventorySystem
{
    public class SlotUI : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _amount;
        [SerializeField] private TextMeshProUGUI _itemName;
        [SerializeField] private TextMeshProUGUI _weight;
        [SerializeField] private TextMeshProUGUI _cost;
        [SerializeField] private GameObject _greyPanel;

        [SerializeField] private InventoryObject _parentInventory;
        [SerializeField] private InventorySlot _parentSlot;

        public Image Icon => _icon;
        public TextMeshProUGUI Amount =>_amount;
        public TextMeshProUGUI Name => _itemName;
        public TextMeshProUGUI Weight => _weight;
        public TextMeshProUGUI Cost => _cost;
        public GameObject GreyPanel => _greyPanel;

        public InventoryObject ParentInventory => _parentInventory;
        public InventorySlot ParentSlot => _parentSlot;

        public void Initialize(InventorySlot _slot, InventoryObject _inventory, int i)
        {
            Icon.sprite = _slot.item.icon;
            Amount.text = "x" + _inventory.slots[i].Amount.ToString();
            Name.text = _slot.item.name;
            Weight.text = _slot.item.weight.ToString() + " kg";
            if (DisplayInventory.Instance.inventoryState == DisplayInventory.InventoryState.Shop)
            {
                if (_inventory is InventoryBackpack)
                    Cost.text = "$" + _slot.sellPrice.ToString();
                else
                    Cost.text = "$" + _slot.buyPrice.ToString();
            }
            else
                Cost.text = "";
            _parentInventory = _inventory;
            _parentSlot = _slot;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (GetComponent<Button>().interactable)
            {
                if (eventData.button == PointerEventData.InputButton.Left)
                    DisplayInventory.Instance.LMBPressed(GetComponent<SlotUI>());

                else if (eventData.button == PointerEventData.InputButton.Right)
                    DisplayInventory.Instance.RMBPressed(GetComponent<SlotUI>());

                else if (eventData.button == PointerEventData.InputButton.Middle)
                    DisplayToolbar.Instance.MMBPressed(GetComponent<SlotUI>());
            }
        }

    }
}