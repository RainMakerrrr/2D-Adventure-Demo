using System;
using System.Collections.Generic;
using System.Linq;
using DialogueSystem;
using LevelSystem;
using TimeSystem;
using UnityEngine;
using UnityEngine.Events;

namespace QuestingSystem
{
    public abstract class Quest : ScriptableObject
    {
        public UnityEvent OnQuestCompleted;
        
        public List<Goal> Goals = new List<Goal>();
        
        
        [SerializeField] private string _name;
        [SerializeField] private string _description;
        [SerializeField] private int _experienceReward;
        [SerializeField] private bool _isCompleted;
        [SerializeField] private bool _isAssigned;
        [SerializeField] private CharacterLevel _level;
        [SerializeField] private int _deadlineTime;



        public bool IsAssigned
        {
            get => _isAssigned;
            set => _isAssigned = value;
            
        }
        
        
        public string Name => _name;

        public string Description => _description;

        public int ExperienceReward => _experienceReward;

        public bool IsCompleted => _isCompleted;

        public int DeadlineTime
        {
            get => _deadlineTime;
            set => _deadlineTime = value;
        }

        public void CheckGoals() =>  _isCompleted = Goals.All(goal => goal.IsCompleted);
        
        private void GiveReward(int experience)
        {
            _level.GiveExperience(experience);
            OnQuestCompleted?.Invoke();
        }

        public void CompleteQuest(int experience)
        {
            //Goals.ForEach(goal => goal.PerformGoal());
            //CheckGoals();
            _isCompleted = true;
            GiveReward(experience);
        }

        public virtual void SetDefaultValues()
        {
            _isCompleted = false;
            _isAssigned = false;
            Goals.ForEach(goal =>
            {
                goal.CurrentAmount = 0;
                goal.IsCompleted = false;
            });
        }

    }
}