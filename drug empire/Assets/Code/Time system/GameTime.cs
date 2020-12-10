using System;
using System.Collections;
using System.Collections.Generic;
using PlayerController;
using QuestingSystem;
using UnityEngine;

namespace TimeSystem
{
    [CreateAssetMenu(fileName = "Game time")]
    public class GameTime : ScriptableObject
    {
        [SerializeField] private float _minuts = 0f;
        [SerializeField] private int _hour = 0;
        [SerializeField] private string[] _days = {"Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"};

        [SerializeField] private string[] _months =
        {
            "January",
            "February",
            "March",
            "April",
            "May",
            "June",
            "July",
            "August",
            "September",
            "October",
            "November",
            "December"
        };

        private int[] _mothsMaxDays = {31, 28, 31, 30, 31, 31, 30, 31, 30, 31, 30, 31};
        
        private int _currentWeekDayIndex = 0;
        private int _currentMonthIndex = 0;

       [SerializeField] private int _currentMonthDay = 1;
       [SerializeField] private int _currentMaxMonthDayIndex = 0;

       [SerializeField]private int _deadlineHours;
       [SerializeField]private float _deadlineMinuts = 60;
       
        // private void OnEnable()
        // {
        //     _currentMonthDay = 1;
        //     _currentMonthIndex = 0;
        //     _currentMaxMonthDayIndex = 0;
        //     _currentWeekDayIndex = 0;
        //     CurrentMonth = 1;
        //     CurrentYear = 2020;
        // }

        public float Minuts
        {
            get => _minuts;
            set
            {
                _minuts = value;
                if (_minuts >= 60)
                {
                    Hour++;
                    _minuts = 0f;
                }
            }
        }

        public int Hour
        {
            get => _hour;
            set
            {
                if (_hour < 24)
                    _hour = value;
                if (_hour >= 24)
                {
                    _hour = 0;
                    
                    _currentWeekDayIndex++;
                    if (_currentWeekDayIndex >= _days.Length)
                        _currentWeekDayIndex = 0;
                    
                    CurrentMonthDay++;
                    LaptopQuestGiver.Instance.IsQuestAccepted = false;
                }
            }
        }

        public int CurrentMonthDay
        {
            get => _currentMonthDay;
            private set
            {
                _currentMonthDay = value;
                if (_currentMonthDay > _mothsMaxDays[_currentMaxMonthDayIndex])
                {
                    _currentMonthDay = 1;
                    
                    _currentMaxMonthDayIndex++;
                    if (_currentMaxMonthDayIndex >= _mothsMaxDays.Length)
                        _currentMaxMonthDayIndex = 0;
                    
                    CurrentMonth++;
                    if (CurrentMonth > _months.Length)
                    {
                        CurrentMonth = 1;
                        CurrentYear++;
                    }
                }
            }
        }

        public int CurrentMonth { get; set; } = 1;
       

        public int CurrentYear { get; set; } = 2020;

        public string CurrentWeekDay
        {
            get => _days[_currentWeekDayIndex];
            private set => CurrentWeekDay = value;
        }

        public float DeadlineMinuts
        {
            get => _deadlineMinuts;
            set
            {
                if (DeadlineHours >= 0 && _deadlineMinuts + value >= 0) 
                    _deadlineMinuts = value;

                if (Mathf.RoundToInt(_deadlineMinuts) <= 0 && DeadlineHours > 0)
                {
                    _deadlineMinuts = 60;
                    DeadlineHours--;
                }
                
                if(Mathf.RoundToInt(_deadlineMinuts) == 0 && DeadlineHours == 0)
                    FindObjectOfType<Player>().InDebt = true;
            }
        }

        public int DeadlineHours
        {
            get => _deadlineHours;
            set
            {
                _deadlineHours = value;
                if (_deadlineHours <= 0)
                    _deadlineHours = 0;
            }
        }
    }
}
