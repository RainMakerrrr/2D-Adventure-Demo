using UnityEngine;

namespace CharacterStatsSystem
{
    [CreateAssetMenu(fileName = "Perception",menuName = "Character Stats/ Perception")]
    public class Perception : CharacterStats
    {
        [SerializeField] private float _timeToFindStash;

        public float ReduceTimeToFindStash()
        {
            _timeToFindStash -= _startValue;
            return _timeToFindStash;
        }

    }
}