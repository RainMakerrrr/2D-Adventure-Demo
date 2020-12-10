using System;
using System.Collections;
using System.Collections.Generic;
using QuestingSystem;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TimeSystem
{
    public class TimeManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _hoursAndMinuts;
        [SerializeField] private TextMeshProUGUI _dayAndWeekDay;
        [SerializeField] private TextMeshProUGUI _month;

        [SerializeField] private TextMeshProUGUI _deadlineHours;
        
        [SerializeField] private GameTime _gameTime;

        public GameTime GameTime => _gameTime;
        
        public static TimeManager Instance { get; private set; }

        private void Awake() => Instance = this;
        
        public IEnumerator SleepHandler(int countHour)
        {
            for (int i = 0; i < countHour; i++)
            {
                _gameTime.Hour += 1;
                yield return null;
            }
        }
        
        private void Update()
        {
            _gameTime.Minuts += 5 * Time.deltaTime;
            
            _hoursAndMinuts.text = _gameTime.Hour + ":" + Mathf.RoundToInt(_gameTime.Minuts);
            _dayAndWeekDay.text = _gameTime.CurrentWeekDay;
            _month.text = $"{_gameTime.CurrentMonthDay}.{_gameTime.CurrentMonth}.{_gameTime.CurrentYear}";

            if (QuestsCollection.Instance.CurrentReceiver != null)
            {
                _gameTime.DeadlineMinuts -= 5 * Time.deltaTime;
                _deadlineHours.text = "Deadline - " + "\n" + _gameTime.DeadlineHours + ":" + Mathf.RoundToInt(_gameTime.DeadlineMinuts);
            }
            else _deadlineHours.text = string.Empty;

        }
    }
}
