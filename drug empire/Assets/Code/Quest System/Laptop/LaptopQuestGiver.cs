using System;
using System.Collections;
using System.Collections.Generic;
using PlayerController;
using QuestingSystem.UI;
using TimeSystem;
using UnityEngine;
using UnityEngine.UI;

namespace QuestingSystem
{
    public class LaptopQuestGiver : MonoBehaviour, IInteractable
    {
        [SerializeField] private InteractionButton _interactionButton;
        
        [SerializeField] private ItemQuestReceiver _currentItemQuestReceiver;
        [SerializeField] private LaptopQuestPanel _laptopQuestPanel;

        [SerializeField] private Image _laptopPanel;
        [SerializeField] private Image _noQuestPanel;
        [SerializeField] private Image _tooltipPanel;
        
        private LaptopQuestUI _laptopQuestUI;
        private QuestGenerator _questGenerator;
        
        
        public static LaptopQuestGiver Instance { get; private set; }

        public ItemQuestReceiver CurrentItemQuestReceiver
        {
            get => _currentItemQuestReceiver;
            set => _currentItemQuestReceiver = value;
        }

        public LaptopQuestPanel LaptopQuestPanel
        {
            get => _laptopQuestPanel;
            set => _laptopQuestPanel = value;
        }

        public bool IsQuestAccepted { get; set; } = true;
        

        private void Awake() => Instance = this;
        

        private void Start()
        {

            _questGenerator = FindObjectOfType<QuestGenerator>();
            _laptopQuestUI = FindObjectOfType<LaptopQuestUI>();
            
            _laptopPanel.gameObject.SetActive(false);
            _noQuestPanel.gameObject.SetActive(false);
            
        }

        private void Update()
        {
            if (_laptopQuestPanel != null)
            {
                if (!_laptopPanel.gameObject.activeSelf)
                    Destroy(_laptopQuestPanel.gameObject);
            }
        }

        public void Interact()
        {
            if (GameStateHandler.Instance.State == GameState.JOB)
            {
                _tooltipPanel.gameObject.SetActive(false);
                
                if(QuestsUI.Instance.TooltipPanel != null)
                    Destroy(QuestsUI.Instance.TooltipPanel.gameObject);
                
                if (!_laptopPanel.gameObject.activeSelf)
                {
                    _laptopPanel.gameObject.SetActive(true);

                    if (_laptopPanel.gameObject.activeSelf)
                        GenerateQuest();
                }
                _interactionButton.gameObject.SetActive(false);
            }

        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.GetComponent<Player>())
            {
                _interactionButton.Inventory = gameObject;
                _interactionButton.gameObject.SetActive(true);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.GetComponent<Player>())
            {
                _laptopPanel.gameObject.SetActive(false);
                
                if(_laptopPanel == null)
                    Destroy(_laptopQuestPanel.gameObject);
            }
        }

        private void GenerateQuest()
        {
            if (_questGenerator.HasReceivers())
            {
                _noQuestPanel.gameObject.SetActive(false);
                
                _currentItemQuestReceiver = _questGenerator.GetRandomQuestDealer();
                
                _currentItemQuestReceiver.GetRandomQuest();
                
                foreach (var receiver in _questGenerator.Receivers)
                { 
                    receiver.CreateLaptopCustomer();
                }
                
                _laptopQuestPanel = _laptopQuestUI.GetRandomQuestUI(_currentItemQuestReceiver.Quest, _currentItemQuestReceiver);

            }
            else _noQuestPanel.gameObject.SetActive(true);

        }

        public void DismissQuest()
        {
            if (!_questGenerator.HasReceivers()) return;
            
            Destroy(_laptopQuestPanel.gameObject);
            
            GenerateQuest();
            
            foreach (var receiver in _questGenerator.Receivers)
            { 
                receiver.CreateLaptopCustomer();
            }
        }

        public void AssignQuest()
        {
            if (QuestsCollection.Instance.QuestReceivers.Count > 0 || !_questGenerator.HasReceivers()) return;

            if (!IsQuestAccepted)
            {
                _currentItemQuestReceiver.Quest.IsAssigned = true;
                
                _currentItemQuestReceiver.Quest.DeadlineTime = TimeManager.Instance.GameTime.Hour + 4;
                TimeManager.Instance.GameTime.DeadlineHours = _currentItemQuestReceiver.Quest.DeadlineTime - TimeManager.Instance.GameTime.Hour;
                TimeManager.Instance.GameTime.DeadlineMinuts = 0;

                QuestsCollection.Instance.AddReceiver(_currentItemQuestReceiver.Quest, _currentItemQuestReceiver);
                _questGenerator.RemoveReceiver(_currentItemQuestReceiver);

                IsQuestAccepted = true;

                Destroy(_laptopQuestPanel.gameObject);
                GenerateQuest();
            }
            else _tooltipPanel.gameObject.SetActive(true);
            
        }

        public void CloseLaptop()
        {
            _laptopPanel.gameObject.SetActive(false);
        }
    }
}