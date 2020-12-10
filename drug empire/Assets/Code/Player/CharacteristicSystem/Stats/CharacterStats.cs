using System;
using UnityEngine;


namespace CharacterStatsSystem
{

    public abstract class CharacterStats : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] protected float _startValue;
        [SerializeField] private float _maxValue;

       

        public virtual float StartValue
        {
            get => _startValue;
            set => _startValue = value;
        }

        public float MaxValue => _maxValue;
        
        public string Name => _name;
        

        public float UpgradeStat()
        {
            var increasePercentage = _maxValue/ 100f * 10f;
            
            if(_startValue < _maxValue) _startValue += increasePercentage;
            if (_startValue + increasePercentage > _maxValue) _startValue = _maxValue;

            return increasePercentage;
        }

       
    }
}



