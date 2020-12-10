using UnityEngine;

namespace CharacterStatsSystem
{
    [CreateAssetMenu(fileName = "Backpack Capacity", menuName = "Character Stats/ Backpack Capacity")]

    public class BackpackCapacity : CharacterStats
    {
        [SerializeField] private float _currentWeight = 0;

        public float CurrentWeight
        {
            get => _currentWeight;
            set => _currentWeight = (float)System.Math.Round(value, 2);
        }

    }
}