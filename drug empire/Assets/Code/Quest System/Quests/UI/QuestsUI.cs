using System;
using UnityEngine;
using UnityEngine.UI;

namespace QuestingSystem.UI
{
    public class QuestsUI : MonoBehaviour
    {
        [SerializeField] private ScrollRect _phoneScrollRect;
        [SerializeField] private PhoneQuestPanel _questPanel;
        [SerializeField] private Image _tooltipPanel;

        [SerializeField] private Image _phoneButton;
        
        public ScrollRect PhoneScrollRect => _phoneScrollRect;

        public Image TooltipPanel => _tooltipPanel;

        private QuestsCollection _questsCollection;
        
        public static QuestsUI Instance { get; private set; }

        private void Awake() => Instance = this;

        
        
        private void Start()
        {
            _phoneScrollRect.gameObject.SetActive(false);
            _questsCollection = FindObjectOfType<QuestsCollection>();
            
            _questsCollection.OnAddingQuest += AddQuestOnUI;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                _phoneScrollRect.gameObject.SetActive(!_phoneScrollRect.gameObject.activeSelf);
                
                if(_phoneScrollRect.gameObject.activeSelf)
                    _phoneButton.rectTransform.anchoredPosition = new Vector2(-307f,17f);
                else _phoneButton.rectTransform.anchoredPosition = new Vector2(0f,17f);
            }
        }

        private void AddQuestOnUI(Quest quest, ItemQuestReceiver itemQuestReceiver)
        {
            var questPanel = Instantiate(_questPanel);
            questPanel.gameObject.SetActive(true);
            
            questPanel.GetComponent<RectTransform>().SetParent(_phoneScrollRect.content);
            questPanel.GetComponent<RectTransform>().localScale = Vector3.one;

            questPanel.Quest = quest;
            questPanel.QuestName.text = quest.Name;
            
            questPanel.Quest = quest;
            var itemQuest = (ItemQuest) questPanel.Quest;
            questPanel.ItemName.text = itemQuest.ItemObject.name;
            questPanel.StuffAmount.text = itemQuest.StuffAmount.ToString();

            questPanel.QuestIcon.sprite = itemQuest.QuestIcon;

            questPanel.DeadlineTime.text = "Deadline - " + quest.DeadlineTime;
            questPanel.ExperienceReward.text = "Reward - " + quest.ExperienceReward;
            questPanel.TargetPosition.text = "Position - " + (Vector2)itemQuestReceiver.transform.position;
        }
    }
}