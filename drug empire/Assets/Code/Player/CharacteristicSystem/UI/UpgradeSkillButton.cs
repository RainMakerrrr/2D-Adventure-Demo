using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace CharacterStatsSystem
{
    public class UpgradeSkillButton : MonoBehaviour, ISelectHandler, IDeselectHandler
    {

        [SerializeField] private Sprite _selectedSprite;
        [SerializeField] private Sprite _deselectedSprite;
        private Image _upgradeSkillImage;

        private void Start()
        {
            _upgradeSkillImage = GetComponent<Image>();
            
        }
        
        public void OnSelect(BaseEventData eventData)
        {
            _upgradeSkillImage.sprite = _selectedSprite;
        }

        public void OnDeselect(BaseEventData eventData)
        {
            _upgradeSkillImage.sprite = _deselectedSprite;
        }
    }
}