using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using InventorySystem;
using PlayerController;
using TimeSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;


namespace QuestingSystem
{
    public class QuestsCollection : MonoBehaviour
    {
        
        [SerializeField] private ItemQuestReceiver _currentReceiver;

        private Player _player;
        
        public ItemQuestReceiver CurrentReceiver
        {
            get => _currentReceiver;
            set => _currentReceiver = value;
        }
        
        public event UnityAction<Quest,ItemQuestReceiver> OnAddingQuest;
        

        [SerializeField] private List<ItemQuestReceiver> _questReceivers = new List<ItemQuestReceiver>();

        [SerializeField] private List<AddressQuestReceiver> _stashes = new List<AddressQuestReceiver>();

        [SerializeField] private AddressQuestReceiver _currentStash;
        
        public static QuestsCollection Instance { get; private set; }

        public List<ItemQuestReceiver> QuestReceivers => _questReceivers;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _player = FindObjectOfType<Player>();
        }
        
        
        private void UpdateTakeItemQuest()
        {
            if (_currentReceiver != null)
            {
                var takeItemQuest = (ItemQuest) _currentReceiver.Quest;

                if (!_player.inventory.HasItem(takeItemQuest.ItemObject, takeItemQuest.StuffAmount))
                {
                    if (_stashes.Count > 0)
                    {
                        var randomStash = _stashes[Random.Range(0, _stashes.Count)];

                        foreach (var quest in randomStash.Quests)
                        {
                            if (quest.ItemObject == takeItemQuest.ItemObject)
                            {
                                quest.StuffAmount = takeItemQuest.StuffAmount;
                                randomStash.Quest = quest;
                                _currentStash = randomStash;
                                _stashes.Remove(randomStash);

                                _currentReceiver = _currentStash;
                                randomStash.Quest.DeadlineTime = TimeManager.Instance.GameTime.Hour + 4;
                                
                                OnAddingQuest?.Invoke(randomStash.Quest, randomStash);
                                break;
                            } 
                            Debug.Log("don't have item");
                        }
                    }
                }
            }
        }
        
        public void AddReceiver(Quest quest, ItemQuestReceiver itemQuestReceiver)
        {
            _questReceivers.Add(itemQuestReceiver);
            OnAddingQuest?.Invoke(quest,itemQuestReceiver);
            _currentReceiver = _questReceivers[0];
            
            UpdateTakeItemQuest();
        }

        public void RemoveReceivers(ItemQuestReceiver itemQuestReceiver)
        {
            _questReceivers.Remove(itemQuestReceiver);
            _currentReceiver = null;
        }

        public void RemoveStash(AddressQuestReceiver addressQuestReceiver) =>
            _currentStash = null;

        public void AddStash(AddressQuestReceiver addressQuestReceiver) => _stashes.Add(addressQuestReceiver);
    }
}