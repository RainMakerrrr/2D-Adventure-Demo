using System;
using UnityEngine;

namespace CharacterStatsSystem
{
    [CreateAssetMenu(fileName = "Speed",menuName = "Character Stats/ Speed")]
    public class Speed : CharacterStats
    {
        [SerializeField] private BackpackCapacity _backpack;
        


        public override float StartValue 
        {
            get
            {
                return Mathf.RoundToInt(_startValue *(1 - (_backpack.CurrentWeight/50f)));
            }
            set
            {
                _startValue = value;
            }
        }

        
    }
        
    }


