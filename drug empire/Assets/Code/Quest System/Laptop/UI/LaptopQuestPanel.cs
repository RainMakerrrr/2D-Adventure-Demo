using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace QuestingSystem.UI
{
    public class LaptopQuestPanel : MonoBehaviour
    {
        [SerializeField] private RectTransform _rect;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private TextMeshProUGUI _questName;
        [SerializeField] private TextMeshProUGUI _experienceReward;
        [SerializeField] private TextMeshProUGUI _itemName;
        [SerializeField] private TextMeshProUGUI _dealerPosition;
        [SerializeField] private TextMeshProUGUI _deadlineTime;
        [SerializeField] private TextMeshProUGUI _stuffAmount;
        [SerializeField] private Quest _quest;
        

        public RectTransform Rect
        {
            get => _rect;
            set => _rect = value;
        }

        public TextMeshProUGUI Description => _description;

        public TextMeshProUGUI QuestName => _questName;

        public TextMeshProUGUI ExperienceReward => _experienceReward;

        public TextMeshProUGUI ItemName => _itemName;

        public TextMeshProUGUI DealerPosition => _dealerPosition;

        public TextMeshProUGUI DeadlineTime => _deadlineTime;

        public TextMeshProUGUI StuffAmount => _stuffAmount;

        public Quest Quest
        {
            get => _quest;
            set => _quest = value;
        }

        private void Start()
        {
            _quest.OnQuestCompleted.AddListener(() => Destroy(gameObject));

        }

       
    }
}