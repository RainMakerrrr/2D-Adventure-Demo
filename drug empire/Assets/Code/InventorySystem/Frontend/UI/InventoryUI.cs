using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

namespace InventorySystem
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _status;
        [SerializeField] private TextMeshProUGUI _capacity;
        [SerializeField] private TextMeshProUGUI _money;
        [SerializeField] private Transform _content;

        public TextMeshProUGUI Status => _status;
        public TextMeshProUGUI Capacity => _capacity;
        public Transform Content => _content;
        

        public void Initialize(InventoryObject _inventory)
        {
            if (_inventory is InventoryShop)
            {
                _status.text = "Shop";
                Capacity.text = "";
                _money.text = _inventory.Money + "$";
            }
            else
            {
                if (_inventory is InventoryBackpack)
                    _status.text = "Player";
                else
                    _status.text = "Chest";
                Capacity.text = _inventory.backpackCapacity.CurrentWeight.ToString() + " / "
                + _inventory.backpackCapacity.MaxValue.ToString() + " kg";
                _money.text = _inventory.Money + "$";
            }
        }

      
    }
}