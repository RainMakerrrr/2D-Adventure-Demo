using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace LevelSystem
{
    public class LevelProgress : MonoBehaviour
    {
        [SerializeField] private CharacterLevel _level;
        [SerializeField] private TextMeshProUGUI _progressLevelText;

        [SerializeField] private TextMeshProUGUI _currentLevel;
        [SerializeField] private TextMeshProUGUI _nextLevel;
        
        private Slider _slider;

        private void Start()
        {
            _slider = GetComponent<Slider>();
            SetProgressBarValues();
            
            _level.OnLevelIncreasing += SetProgressBarValues;
        }

        private void Update()
        {
            _currentLevel.text = _level.CurrentLevel.ToString();
            _nextLevel.text = (_level.CurrentLevel + 1).ToString();
        }

        private void SetProgressBarValues()
        {
            _slider.value = _level.CurrentExperience;
            _slider.maxValue = _level.GoalExperienceForNextLevel;

            _progressLevelText.text = _level.CurrentExperience + " / " + _level.GoalExperienceForNextLevel;

            StartCoroutine(UpdateProgressBar());
        }
        

        public void TestAddExperience()
        {
            _level.GiveExperience(Random.Range(50,400));
            StartCoroutine(UpdateProgressBar());
            _progressLevelText.text = _level.CurrentExperience + " / " + _level.GoalExperienceForNextLevel;
        }
        
        private IEnumerator UpdateProgressBar()
        {
            while(_slider.value < _level.CurrentExperience)
            {
                _slider.value += 5;
                yield return null;
            }
            _slider.value = Mathf.RoundToInt(_level.CurrentExperience);
        }
    }
}

