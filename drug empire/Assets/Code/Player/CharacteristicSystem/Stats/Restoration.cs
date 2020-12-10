using UnityEngine;

namespace CharacterStatsSystem
{
    [CreateAssetMenu(fileName = "Restoration",menuName = "Character Stats/ Restoration")]
    public class Restoration : CharacterStats
    {
        [SerializeField] private Health _health;
        [SerializeField] private Endurance _endurance;

        public void Regeneration()
        {
            _health.StartValue += _startValue/10f * Time.deltaTime;
            _endurance.StartValue += _startValue / 10f * Time.deltaTime;
            // to do 
            // remove poison
        }
        
    }
}