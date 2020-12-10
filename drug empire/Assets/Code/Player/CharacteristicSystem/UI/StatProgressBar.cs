using System.Collections;
using LevelSystem;
using UnityEngine;
using UnityEngine.UI;

namespace CharacterStatsSystem
{
    public class StatProgressBar : MonoBehaviour
    {
        [SerializeField] private CharacterStats _characterStats;
        [SerializeField] private CharacterLevel _level;
        private Slider _slider;
        
        private void Start()
        {
            _slider = GetComponent<Slider>();
            _slider.value = _characterStats.StartValue;
            _slider.maxValue = _characterStats.MaxValue;
            
        }
        
        public void UpgrageCurrentStat()
        {
            if (_level.SkillPoints > 0)
            {
                _characterStats.UpgradeStat();
                StartCoroutine(UpdateProgressBar());
                _level.SkillPoints--;
            }
            else Debug.Log("Don't have skill points");
        }

        private IEnumerator UpdateProgressBar()
        {
            while(_slider.value < _characterStats.StartValue)
            {
                _slider.value += 20f * Time.deltaTime;
                yield return null;
            }
            _slider.value = Mathf.RoundToInt(_slider.value);
        }
    }
}