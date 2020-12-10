using System;
using System.Collections.Generic;
using InventorySystem;
using QuestingSystem.UI;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace QuestingSystem
{
    public class QuestGenerator : MonoBehaviour
    {
        [SerializeField] private List<ItemQuestReceiver> _receivers = new List<ItemQuestReceiver>();
        public List<ItemQuestReceiver> Receivers => _receivers;
        

        public static QuestGenerator Instance;
        
        
        private void Awake()
        {
           if(!Instance) Instance = this;
           else Destroy(Instance.gameObject);
           
           DontDestroyOnLoad(this);
        }



        public ItemQuestReceiver GetRandomQuestDealer()
        {
            var randomIndex = Random.Range(0, _receivers.Count);
            var itemQuestReceiver = _receivers[randomIndex];

            return itemQuestReceiver;
        }

        public void AddReceiver(ItemQuestReceiver receiver)
        {
            _receivers.Add(receiver);
        }

        public void RemoveReceiver(ItemQuestReceiver receiver)
        {
            _receivers.Remove(receiver);
            Destroy(receiver.Customer.gameObject);
        }
        
        

        public bool HasReceivers() => _receivers.Count > 0;
    }
}