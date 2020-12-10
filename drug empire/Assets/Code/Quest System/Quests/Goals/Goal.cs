using System.Collections;
using System.Collections.Generic;
using InventorySystem;
using UnityEngine;

namespace QuestingSystem
{    
    [System.Serializable]
    public class Goal
    {
        [SerializeField][TextArea(3,10)] private string _description;
        [SerializeField] protected Quest _quest;
        [SerializeField] private bool _isCompleted;
        [SerializeField] private int _currentAmount;
        [SerializeField] private int _requiredAmount;

        public int CurrentAmount
        {
            get => _currentAmount;
            set { if(_currentAmount < _requiredAmount || value == 0) _currentAmount = value; }
        }

        public bool IsCompleted
        {
            get => _isCompleted;
            set => _isCompleted = value;
        }


        public void Evaluate()
        {
            if (_currentAmount >= _requiredAmount)
                GoalComplete();
        }

        private void GoalComplete()
        {    
            _quest.CheckGoals();
            _isCompleted = true;
        }
        
        public void Talk(string name)
        {
            var dialogueQuest = (DialogueQuest) _quest;
            
            for (int i = 0; i < dialogueQuest.QuestDialogueName.Length; i++)
            {
                if (name == dialogueQuest.QuestDialogueName[i].DialogueFileName)
                {
                    CurrentAmount++;
                    Evaluate();
                    dialogueQuest.QuestDialogueName[i].IsDialogueDone = true;
                    Debug.Log("Goal is done");
                }
            }
           
        }

        public void PerformGoal()
        {
            CurrentAmount++;
            Evaluate();
        }
        
    }
}