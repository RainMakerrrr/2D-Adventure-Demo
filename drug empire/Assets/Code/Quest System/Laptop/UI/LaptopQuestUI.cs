using System;
using UnityEngine;
using UnityEngine.UI;

namespace QuestingSystem.UI
{
    public class LaptopQuestUI : MonoBehaviour
    {
        [SerializeField] private Button _confirmedButton;
        [SerializeField] private Button _dismissButton;
    
        [SerializeField] private LaptopQuestPanel _laptopQuestPanel;
        [SerializeField] private ScrollRect _laptopScrollRect;

        [SerializeField] private RectTransform _laptopCurrentQuest;

        [SerializeField] private LaptopCustomer _laptopCustomer;
        
        

        public LaptopQuestPanel GetRandomQuestUI(Quest quest, ItemQuestReceiver itemQuestReceiver)
        { 
            var questPanel = Instantiate(_laptopQuestPanel);
            
            questPanel.gameObject.SetActive(true);
            
            questPanel.Rect.SetParent(_laptopCurrentQuest);

            
             questPanel.Rect.localScale = Vector3.one;
             questPanel.Rect.anchorMin = new Vector2(0f, 0f);
             questPanel.Rect.anchorMax = new Vector2(1f, 1f);
             questPanel.Rect.pivot = new Vector2(0.5f,0.5f);
             questPanel.Rect.offsetMax = Vector2.zero;
             questPanel.Rect.offsetMin = Vector2.zero;
            
            questPanel.QuestName.text = quest.Name;
            questPanel.Description.text = quest.Description;
            questPanel.ExperienceReward.text = "Количество опыта - " + quest.ExperienceReward;

            questPanel.Quest = quest;
            var itemQuest = (ItemQuest) questPanel.Quest;
            questPanel.ItemName.text = "Товар - " + itemQuest.ItemObject.name;
            questPanel.StuffAmount.text = "Количество стаффа - " + itemQuest.StuffAmount;
            
            questPanel.DealerPosition.text = "Координаты - " + ((Vector2) itemQuestReceiver.transform.position);
            questPanel.DeadlineTime.text = "Дедлайн - " + quest.DeadlineTime;
           
            return questPanel;
        }

        public void UpdateQuestPanel(LaptopQuestPanel questPanel, ItemQuestReceiver itemQuestReceiver)
        {
            questPanel.QuestName.text = itemQuestReceiver.Quest.Name;
            questPanel.Description.text = itemQuestReceiver.Quest.Description;
            questPanel.ExperienceReward.text = "Количество опыта - " + itemQuestReceiver.Quest.ExperienceReward;
            
            questPanel.Quest = itemQuestReceiver.Quest;
            var itemQuest = (ItemQuest) questPanel.Quest;
            questPanel.ItemName.text = "Товар - " + itemQuest.ItemObject.name;
            questPanel.StuffAmount.text = "Количество стаффа - " + itemQuest.StuffAmount;
            
            questPanel.DealerPosition.text = "Координаты - " + ((Vector2) itemQuestReceiver.transform.position);
            questPanel.DeadlineTime.text = "Дедлайн - " + itemQuestReceiver.Quest.DeadlineTime;
        }
        
        public LaptopCustomer CreateLaptopCustomer(ItemQuestReceiver receiver)
        {
            var laptopCustomer = Instantiate(_laptopCustomer);
            
            laptopCustomer.gameObject.SetActive(true);
            
            laptopCustomer.Rect.SetParent(_laptopScrollRect.content);
            laptopCustomer.Rect.localScale = Vector3.one;
            
            laptopCustomer.NameQuest.text = receiver.Quest.Name;

            return laptopCustomer;
        }
    }
}