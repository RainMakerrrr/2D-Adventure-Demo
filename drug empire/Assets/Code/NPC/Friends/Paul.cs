using System;
using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
using InventorySystem;
using PlayerController;
using QuestingSystem;
using UnityEngine;

namespace NPC.Friends
{
    public class Paul : MonoBehaviour, IInteractable
    {
        [SerializeField] private InteractionButton _interactionButton;
        [SerializeField] private InventoryObject _inventory;
        [SerializeField] private string _dialogueFile;
        [SerializeField] private ItemQuestReceiver _receiver;
        [SerializeField] private ItemObject _item;
        [SerializeField] private RectTransform _dialogueBackground;
        [SerializeField] private DialogueBox _playerBox;
        
        public RectTransform DialogueBackground => _dialogueBackground;

        private Player _player;
        private bool _isTalked;
        
        private void Start()
        {
            _player = FindObjectOfType<Player>();
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_isTalked)
            {
                if (other.GetComponent<Player>())
                {
                    _interactionButton.Inventory = gameObject;
                    _interactionButton.gameObject.SetActive(true);
                }
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if(other.GetComponent<Player>())
                FixDialogueBox();
        }

        public void Interact()
        {
            if (!_isTalked)
            {
                _player.GetComponent<Animator>().SetFloat("Speed", 0f);
                
                DialogueManager.Instance.DialogueStart(_dialogueFile, _player, this);

                DialogueManager.Instance.OnAccept += AcceptDrugQuest;
                DialogueManager.Instance.OnDismiss += DismissDrugQuest;

                _isTalked = true;
                
                _interactionButton.gameObject.SetActive(false);
            }
        }
        
        private void AcceptDrugQuest(string fileName)
        {
            if (_dialogueFile == fileName)
            {
                Debug.Log("Accept quest");

                var itemQuest = (ItemQuest) _receiver.Quest;
                _player.inventory.AddItem(_item, itemQuest.StuffAmount);
                _inventory.RemoveItem(_item, itemQuest.StuffAmount);

                QuestsCollection.Instance.AddReceiver(_receiver.Quest, _receiver);

                GameStateHandler.Instance.State = GameState.JOB;
            }
        }

        private void DismissDrugQuest(string fileName)
        {
            if (_dialogueFile == fileName)
            {
                Debug.Log("Dismiss quest");
                _player.GetComponent<PlayerMovement>().CanMove = true;
            }
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