using UnityEngine;

namespace CharacterStatsSystem
{    
    [CreateAssetMenu(fileName = "Health",menuName = "Character Stats/ Health")]
    public class Health : CharacterStats
    {
      

        public void IncreaseHealth(float health)
        {
            _startValue -= health;
            if(_startValue <= 0)
                Debug.Log("Die");
        }

        public void DecreaseHealth(float health)
        {
            _startValue += health;
        }
    }
}