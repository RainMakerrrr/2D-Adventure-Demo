using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

namespace QuestingSystem.UI
{
    public class LaptopCustomer : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        [SerializeField] private TextMeshProUGUI _nameQuest;
        [SerializeField] private Sprite _activeSprite;
        [SerializeField] private Sprite _nonActiveSprite;
        [SerializeField] private RectTransform _rect;
        
        private Image _customerImage;
        public RectTransform Rect => _rect;

        public TextMeshProUGUI NameQuest => _nameQuest;

        private void Start()
        {
            _customerImage = GetComponent<Image>();

        }

        public void OnSelect(BaseEventData eventData) => _customerImage.sprite = _activeSprite;
        
        public void OnDeselect(BaseEventData eventData) => _customerImage.sprite = _nonActiveSprite;

    }
}