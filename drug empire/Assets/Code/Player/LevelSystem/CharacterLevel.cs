using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LevelSystem
{
    [CreateAssetMenu(fileName = "New Character Level", menuName = "Level System / Character Level")]
    public class CharacterLevel : ScriptableObject
    {
        public event UnityAction OnLevelIncreasing;
        
        [SerializeField] private int _currentLevel = 0;
        [SerializeField] private int _currentExperience;
        [SerializeField] private int _goalExperienceForNextLevel;
        [SerializeField] private int _skillPoints;
        [SerializeField] private int[] _amountExperienceForNextLevels = new int [10]
        {
            800,1200,1800,2700,3780,5292,7408,10372,14521,20329
        };
        [SerializeField] private int _maxLevel;

        public int CurrentLevel => _currentLevel;

        public int CurrentExperience
        {
            get => _currentExperience;
            set
            {
                if (value > 0)
                    _currentExperience = value;
            }
        }
        public int MaxLevel => _maxLevel;

        public int GoalExperienceForNextLevel => _goalExperienceForNextLevel;

        public int SkillPoints
        {
            get => _skillPoints;
            set
            {
                if (value >= 0) _skillPoints = value;
            }
        }

        private void OnEnable()
        {
            _maxLevel = _amountExperienceForNextLevels.Length - 1;
            _goalExperienceForNextLevel = _amountExperienceForNextLevels[_currentLevel];
        }

        public void GiveExperience(int experience)
        {
            CurrentExperience += experience;
            if (CurrentExperience >= _goalExperienceForNextLevel)
            {
                _currentLevel++;
                _skillPoints++;
                CurrentExperience = 0;
                _goalExperienceForNextLevel = _amountExperienceForNextLevels[_currentLevel];
                
            }
            OnLevelIncreasing?.Invoke();
        }

    }
}


