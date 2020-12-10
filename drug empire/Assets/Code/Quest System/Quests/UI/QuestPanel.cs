using System;
using UnityEngine;
using UnityEngine.UI;

namespace QuestingSystem.UI
{
    public class QuestPanel : MonoBehaviour
    {
        [SerializeField] private RectTransform _rect;
        [SerializeField] private Text _questName;
        [SerializeField] private Text _description;
        [SerializeField] private Text _goalsCount;
        [SerializeField] private Text _experienceReward;
        [SerializeField] private Quest _quest;
        public RectTransform Rect => _rect;

        public Text QuestName => _questName;

        public Text Description => _description;

        public Text GoalsCount => _goalsCount;

        public Text ExperienceReward => _experienceReward;

        public Quest Quest
        {
            get => _quest;
            set => _quest = value;
        }

        private void Update()
        {
            if(_quest.IsCompleted) Destroy(gameObject);
        }
    }
}