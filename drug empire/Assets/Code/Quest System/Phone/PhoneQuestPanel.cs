using System;
using System.Collections;
using System.Collections.Generic;
using InventorySystem;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace QuestingSystem.UI
{
    public class PhoneQuestPanel : MonoBehaviour
    {
        [SerializeField] private AnimationClip _takeItem;
        [SerializeField] private AnimationClip _deliveryItem;
        [SerializeField] private AnimationClip _deliveryHumanItem;

        [SerializeField] private Quest _quest;
        [SerializeField] private Text _questName;
        [SerializeField] private Text _itemName;
        [SerializeField] private Text _stuffAmount;
        [SerializeField] private Image _questIcon;
        [SerializeField] private Button _questIconButton;

        [SerializeField] private TextMeshProUGUI _deadlineTime;
        [SerializeField] private TextMeshProUGUI _experienceReward;
        [SerializeField] private TextMeshProUGUI _targetPosition;

        public TextMeshProUGUI DeadlineTime => _deadlineTime;

        public TextMeshProUGUI ExperienceReward => _experienceReward;

        public TextMeshProUGUI TargetPosition => _targetPosition;


        private Animation _animation;
        
        public Quest Quest
        {
            get => _quest;
            set => _quest = value;
        }

        public Text QuestName => _questName;

        public Text ItemName => _itemName;

        public Text StuffAmount => _stuffAmount;

        public Image QuestIcon
        {
            get => _questIcon;
            set => _questIcon = value;
        }
        
        
        private void Start()
        {
            _animation = GetComponent<Animation>();
            
            _questIconButton.interactable = false;
            _quest.OnQuestCompleted.AddListener(() => Destroy(gameObject));

            ConfirmationButtonHandler();
            
        }

        private void Update()
        {
            if (DisplayInventory.Instance.isInventoryActive && DisplayInventory.Instance.inventoryStashUI.activeSelf &&
                _quest == QuestsCollection.Instance.CurrentReceiver.Quest)
            {
                _questIconButton.interactable = true;
                _questIcon.color = Color.green;
            }
            else
            {
                _questIconButton.interactable = false;
                _questIcon.color = Color.white;
            }
        }

        private void ConfirmationButtonHandler()
        {
            _questIconButton.onClick.AddListener(ConfirmQuest);
        }

        private void ConfirmQuest()
        {
            if (QuestsCollection.Instance.CurrentReceiver.Quest is DeliveryAddressItemQuest)
            {
                _animation.clip = _deliveryItem;
                _animation.Play();
                
                var addressReceiver = (AddressQuestReceiver)QuestsCollection.Instance.CurrentReceiver;
                StartCoroutine(addressReceiver.DeliveryAddressItem());
            }

            if (QuestsCollection.Instance.CurrentReceiver.Quest is DeliveryPersonItemQuest)
            {
                _animation.clip = _deliveryHumanItem;
                _animation.Play();
                
                var humanReceiver = (HumanQuestReceiver) QuestsCollection.Instance.CurrentReceiver;
                humanReceiver.DeliveryItemHuman();
            }

            if (QuestsCollection.Instance.CurrentReceiver.Quest is TakeItemQuest)
            {
                _animation.clip = _takeItem;
                _animation.Play();
                
                var addressReceiver = (AddressQuestReceiver) QuestsCollection.Instance.CurrentReceiver;
                StartCoroutine(addressReceiver.TakeAddressItem());
            }
            if(QuestsCollection.Instance.CurrentReceiver.Quest.IsCompleted)
                QuestsCollection.Instance.CurrentReceiver = null;
            
        }
    }
}