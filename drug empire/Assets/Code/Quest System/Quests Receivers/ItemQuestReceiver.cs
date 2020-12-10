using System;
using System.Collections.Generic;
using InventorySystem;
using PlayerController;
using QuestingSystem.UI;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


namespace QuestingSystem
{
    public class ItemQuestReceiver : InventoryPhysical
    {
        [SerializeField] protected Quest _quest;
        [SerializeField] private List<ItemQuest> _quests = new List<ItemQuest>();
        private LaptopQuestUI _laptopUI;
        private LaptopCustomer _customer;

        public LaptopCustomer Customer => _customer;

        public IReadOnlyList<ItemQuest> Quests => _quests;

        public virtual void Start()
        {
            GetRandomQuest();
            
            _laptopUI = FindObjectOfType<LaptopQuestUI>();
            
            if(QuestGenerator.Instance.Receivers.Contains(this))
                _customer =  _laptopUI.CreateLaptopCustomer(this);
            
        }

        public virtual void Update()
        {
            if(_customer != null)
                _customer.GetComponent<Button>().onClick.AddListener(LaptopCustomerClickHandler);
           
        }

        public void CreateLaptopCustomer()
        {
            Destroy(_customer.gameObject);
            _customer = _laptopUI.CreateLaptopCustomer(this);
        }

        public Quest Quest
        {
            get => _quest;
            set => _quest = value;
        }
        
        private ItemQuest GenerateRandomQuest()
        {
            var randomIndex = Random.Range(0, _quests.Count);
            var randomQuest = _quests[randomIndex];

            return randomQuest;
        }


        public void GetRandomQuest()
        {
            _quest = GenerateRandomQuest();
        }

        protected void CompleteQuestHandler()
        {
            Debug.Log("quest complete");
            _customer = _laptopUI.CreateLaptopCustomer(this);
            
            QuestsCollection.Instance.RemoveReceivers(this);
            QuestGenerator.Instance.AddReceiver(this);
            
            _quest.SetDefaultValues();
        }

        private void LaptopCustomerClickHandler()
        {
            _laptopUI.UpdateQuestPanel(LaptopQuestGiver.Instance.LaptopQuestPanel, this);
            LaptopQuestGiver.Instance.CurrentItemQuestReceiver = this;
        }
        
    }
}

