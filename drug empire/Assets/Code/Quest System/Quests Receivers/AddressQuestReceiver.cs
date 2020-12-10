using System.Collections;
using System.Collections.Generic;
using InventorySystem;
using PlayerController;
using QuestingSystem.UI;
using TimeSystem;
using UnityEngine;
using UnityEngine.UI;

namespace QuestingSystem
{
    public class AddressQuestReceiver : ItemQuestReceiver
    {
        private Player _player;

        public override void Start()
        {
            base.Start();
            _player = FindObjectOfType<Player>();

        }
        
        private void Awake()
        {
            Initialize();
        }

        public override void Update()
        {
            base.Update();
            if (_quest.IsCompleted)
            {
                if(_quest is TakeItemQuest)
                    CompleteTakeItemQuest();
                else
                    CompleteQuestHandler();
                
                
            }
                
        }
        
        public IEnumerator DeliveryAddressItem()
        {
            yield return _player.StartLooting();
            var addressQuest = (DeliveryAddressItemQuest)_quest;
            
            foreach (var slot in inventory.slots)
            {
                if (slot.item == addressQuest.ItemObject && slot.Amount >= addressQuest.StuffAmount && TimeManager.Instance.GameTime.Hour < _quest.DeadlineTime)
                {
                    addressQuest.CompleteQuest(addressQuest.ExperienceReward);
                    _player.inventory.Money += slot.sellPrice * addressQuest.StuffAmount;
                    inventory.Money -= slot.sellPrice * addressQuest.StuffAmount;

                    _player.inventory.Money += addressQuest.ExperienceReward;
                }
                else
                {
                    addressQuest.CompleteQuest(addressQuest.ExperienceReward - 250);
                    _player.inventory.Money += slot.sellPrice * addressQuest.StuffAmount;
                    inventory.Money -= slot.sellPrice * addressQuest.StuffAmount;
                    
                    _player.InDebt = true;

                    _player.inventory.Money += addressQuest.ExperienceReward;
                }
            }

            if (inventory.slots.Count == 0)
            {
                addressQuest.CompleteQuest(addressQuest.ExperienceReward - 300);
                _player.inventory.Money += addressQuest.ExperienceReward;
                _player.InDebt = true;
              
            }
            
            StartCoroutine(ClearStash());
        }

        public IEnumerator TakeAddressItem()
        {
            yield return _player.StartLooting();
            var takeItemQuest = (TakeItemQuest)_quest;
            
            if(inventory.slots.Count == 0)
                takeItemQuest.CompleteQuest(takeItemQuest.ExperienceReward);
            
            foreach (var slot in _player.inventory.slots)
            {
                if (slot.item == takeItemQuest.ItemObject && slot.Amount >= takeItemQuest.StuffAmount)
                {
                    if (inventory.Money >= slot.buyPrice * takeItemQuest.StuffAmount)
                    {
                        takeItemQuest.CompleteQuest(takeItemQuest.ExperienceReward);
                        Debug.Log("Succefully take");

                        inventory.Money += slot.buyPrice * takeItemQuest.StuffAmount;
                        _player.inventory.Money -= slot.buyPrice * takeItemQuest.StuffAmount;
                    }
                }
            }
            
        }
        
        private IEnumerator ClearStash()
        {
            yield return new WaitForSeconds(1f);
            inventory.slots = new List<InventorySlot>();
        }

        private void CompleteTakeItemQuest()
        {
            Debug.Log("quest complete"); 
            
            QuestsCollection.Instance.RemoveStash(this);
            QuestsCollection.Instance.AddStash(this);

            QuestsCollection.Instance.CurrentReceiver = QuestsCollection.Instance.QuestReceivers[0];
            
            _quest.SetDefaultValues();
        }
        
        private void OnApplicationQuit()
        {
            inventory.Clear();
        }
    }
}