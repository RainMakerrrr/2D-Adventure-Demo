using UnityEngine;

namespace CharacterStatsSystem
{
    [CreateAssetMenu(fileName = "Endurance",menuName = "Character Stats/ Endurance")]
    public class Endurance : CharacterStats
    {
        public void DecreaseStamina()
        {
            _startValue -= 10f * Time.deltaTime;
        }
    }
}