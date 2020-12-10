using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DialogueSystem;
using InventorySystem;
using PlayerController;

namespace QuestingSystem
{
    public class HumanQuestReceiver : ItemQuestReceiver
    {
        [SerializeField] private string _questCompleteDialogue;
        [SerializeField] private RectTransform _dialogueBackground;
        [SerializeField] private DialogueBox _playerBox;
        
        private Player _player;
        
        public RectTransform DialogueBackground => _dialogueBackground;

        public override void Start()
        {
            base.Start();
            _player = FindObjectOfType<Player>();
        }
        
        public override void Update()
        {
            base.Update();

            if (_quest.IsCompleted) 
                CompleteQuestHandler();
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if(other.GetComponent<Player>())
                FixDialogueBox();
        }

        public override void Interact()
        {
           DeliveryPersonItem();
           _interactionButton.gameObject.SetActive(false);
        }
       

        private void DeliveryPersonItem()
        {
            if (QuestsCollection.Instance.QuestReceivers.Count > 0)
            {
                DialogueManager.Instance.DialogueStart(_questCompleteDialogue, _player, this);

                DialogueManager.Instance.OnDialogueEnded += DialogueEndHandler;
            }
        }

        private void DialogueEndHandler(string dialogueFile)
        {
            if (_questCompleteDialogue == dialogueFile)
            {
                DisplayInventory.Instance.isInventoryActive = !DisplayInventory.Instance.isInventoryActive;

                DisplayInventory.Instance.closestStash = this;
                DisplayInventory.Instance.OpenInventory();
            }
        }

        public void DeliveryItemHuman()
        {
            DisplayInventory.Instance.isInventoryActive = false;
            DisplayInventory.Instance.UpdateInventories();
                    
            DisplayInventory.Instance.closestStash = null;
            
            var deliveryPersonQuest = (DeliveryPersonItemQuest) _quest;
            
            foreach (var slot in inventory.slots)
            {
                if (slot.item == deliveryPersonQuest.ItemObject && slot.Amount >= deliveryPersonQuest.StuffAmount)
                {
                    deliveryPersonQuest.CompleteQuest(deliveryPersonQuest.ExperienceReward);
                    
                    _player.inventory.Money += slot.sellPrice * deliveryPersonQuest.StuffAmount;
                    inventory.Money += slot.sellPrice * deliveryPersonQuest.StuffAmount;

                    _player.inventory.Money += deliveryPersonQuest.ExperienceReward;
                }
                else
                {
                    deliveryPersonQuest.CompleteQuest(deliveryPersonQuest.ExperienceReward - 250);
                    
                    _player.inventory.Money += slot.sellPrice * deliveryPersonQuest.StuffAmount;
                    inventory.Money += slot.sellPrice * deliveryPersonQuest.StuffAmount;

                    _player.InDebt = true;

                    _player.inventory.Money += deliveryPersonQuest.ExperienceReward;
                }
            }

            if (inventory.slots.Count == 0)
            {
                deliveryPersonQuest.CompleteQuest(deliveryPersonQuest.ExperienceReward - 300);
                _player.inventory.Money += deliveryPersonQuest.ExperienceReward;
                _player.InDebt = true;
            }
            
            StartCoroutine(ClearStash());
        }

        private IEnumerator ClearStash()
        {
            yield return new WaitForSeconds(2f);
            inventory.slots = new List<InventorySlot>();
        }
        private void FixDialogueBox()
        {
            var distance = transform.position - _player.transform.position;
            
            if(Vector3.Distance(transform.position, _player.transform.position) <= 0.6f && distance.x > 0f)
                _playerBox.Offset = new Vector3(-0.55f,  _playerBox.Offset.y, 0f);
            else if(Vector3.Distance(transform.position, _player.transform.position) <= 0.6f && distance.x < 0f) 
                _playerBox.Offset = new Vector3(0.55f,  _playerBox.Offset.y, 0f);
            
            else _playerBox.Offset = new Vector3(0f,  _playerBox.Offset.y, 0f);
        }
    }
}