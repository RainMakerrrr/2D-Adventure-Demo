using UnityEngine;
using System.Collections.Generic;
using CharacterStatsSystem;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using LevelSystem;

namespace InventorySystem
{

    public class InventoryObject : ScriptableObject, ISerializationCallbackReceiver
    {
        public string savePath;
        public int slotCapacity = 10;
        [SerializeField]private float money;
        public float defaultMoney = 100;
        public BackpackCapacity backpackCapacity;
        public CharacterLevel characterLevel;

        public List<InventorySlot> slots = new List<InventorySlot>();
        public List<InventorySlot> defaultSlots = new List<InventorySlot>();
        public Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();
        public ItemDatabase database;

        public float Money
        {
            get => money;
            set
            {
                if (money >= 0)
                    money = value;
            }
        }

        public bool HasItem(ItemObject itemObject, int amount)
        {
            foreach (var slot in slots)
            {
                if (slot.item == itemObject && slot.Amount >= amount)
                {
                    return true;
                }
                return false;
            }
            
            return false;
        }
        

        public int CheckWeight(ItemObject _item, int _amount)
        {
            float weight = (backpackCapacity.MaxValue - backpackCapacity.CurrentWeight) / (_amount * _item.weight);
            int count = Mathf.FloorToInt(weight);
            return count >= _amount ? _amount : count;
        }

        void AddSlot(ItemObject _item, int _amount)
        {
            slots.Add(new InventorySlot(database.GetId[_item], _item, _amount, characterLevel));
            backpackCapacity.CurrentWeight += _item.weight * _amount;
        }

        public void AddItem(ItemObject _item, int _amount)
        {
            int count = CheckWeight(_item, _amount);

            if (count > 0)
            {
                for (int i = 0; i < slots.Count; i++)
                {
                    if (!_item.isStackable)
                        break;
                    else if (slots[i].item == _item)
                    {
                        if (count + slots[i].Amount <= slotCapacity)
                        {
                            slots[i].Amount += count;
                            backpackCapacity.CurrentWeight += _item.weight * count;
                            return;
                        }
                        else
                        {
                            int _amountLeft = count + slots[i].Amount - slotCapacity;
                            slots[i].Amount = slotCapacity;
                            AddSlot(_item, _amountLeft);
                            return;
                        }
                    }
                }

                if (count <= slotCapacity)
                    AddSlot(_item, count);
                else
                {
                    for (int i = 0; i < Mathf.Floor(count / slotCapacity); i++)
                        AddSlot(_item, slotCapacity);
                    AddSlot(_item, count % slotCapacity);
                }

            }
        }

        public bool RemoveItem(ItemObject _item, int _amount)
        {
            int count = _amount;
            List<InventorySlot> slotsPending = new List<InventorySlot>();

            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].item == _item)
                {
                    if (count > slots[i].Amount)
                    {
                        count -= slots[i].Amount;
                        backpackCapacity.CurrentWeight -= _item.weight * slots[i].Amount;
                        slots[i].Amount = 0;
                        slotsPending.Add(slots[i]);
                    }
                    else
                    {
                        slots[i].Amount -= count;
                        backpackCapacity.CurrentWeight -= _item.weight * count;
                        if (slots[i].Amount == 0)
                            slotsPending.Add(slots[i]);
                        count = 0;
                        break;
                    }
                }
            }

            if (count <= 0)
            {
                for (int i = 0; i < slotsPending.Count; i++)
                {
                    Destroy(itemsDisplayed[slotsPending[i]]);
                    itemsDisplayed.Remove(slotsPending[i]);
                    slots.Remove(slotsPending[i]);
                }
                return true;
            }
            else
                return false;
        }

        #region Save System
        public void Save()
        {
            string saveData = JsonUtility.ToJson(this, true);
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + savePath);
            bf.Serialize(file, saveData);
            file.Close();
        }

        public void Load()
        {
            if (File.Exists(Application.persistentDataPath + savePath))
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + savePath, FileMode.Open);
                JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(), this);
                file.Close();
            }
        }

        public void Clear()
        {
            slots.Clear();
            backpackCapacity.CurrentWeight = 0;
            money = defaultMoney;
        }

        public void OnBeforeSerialize()
        {

        }

        public void OnAfterDeserialize()
        {
            for (int i = 0; i < slots.Count; i++)
            {
                if (database.GetItem.ContainsKey(slots[i].ID)) {
                    slots[i].item = database.GetItem[slots[i].ID];
                }
                backpackCapacity.CurrentWeight += slots[i].item.weight;
            }
        }
        #endregion
    }

    [System.Serializable]
    public class InventorySlot
    {
        public ItemObject item;
        [HideInInspector] public SlotUI _slotUI;

        public int ID;
        [SerializeField] private int amount;
        
        public float buyPrice;
        public float sellPrice;
        
        public int Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                if (_slotUI != null)
                    _slotUI.Amount.text = Amount.ToString();
            }
        }

        public InventorySlot(int _id, ItemObject _item, int _amount, CharacterLevel _level)
        {
            item = _item;
            Amount = _amount;
            ID = _id;
            float levelMulti = ((1f - (_level.CurrentLevel / _level.MaxLevel)) * 30f / 100f);
            buyPrice = Mathf.Round(_item.cost + _item.cost * levelMulti);
            sellPrice = Mathf.Round(_item.cost - _item.cost * levelMulti);
        }
    }
}