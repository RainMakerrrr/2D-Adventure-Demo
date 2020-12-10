using UnityEngine;

namespace CharacterStatsSystem
{
    [CreateAssetMenu(fileName = "Shot Accuracy",menuName = "Character Stats/ Shot Accuracy")]
    public class ShotAccuracy : CharacterStats
    {
        public float ScatterShootAngle => 100f / _startValue;
    }
}