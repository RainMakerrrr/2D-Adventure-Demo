using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace QuestingSystem.UI
{
    public class PhoneButton : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        [SerializeField] private ScrollRect _phoneScrollRect;
        [SerializeField] private Sprite _phoneActiveSprite;
        [SerializeField] private Sprite _phoneHideSprite;
        [SerializeField] private Sprite _phoneSelectedSprite;
        [SerializeField] private Sprite _phoneDeselectedSprite;
        
        private Image _phoneImage;

        private void Start()
        {
            _phoneImage = GetComponent<Image>();
        }


        public void OpenPhonePanel()
        {
            _phoneScrollRect.gameObject.SetActive(!_phoneScrollRect.gameObject.activeSelf);
            
            if(_phoneScrollRect.gameObject.activeSelf)
                GetComponent<RectTransform>().anchoredPosition = new Vector2(-307f,17f);
            else GetComponent<RectTransform>().anchoredPosition = new Vector2(0f,17f);
            
            _phoneImage.sprite = _phoneScrollRect.gameObject.activeSelf ? _phoneActiveSprite : _phoneHideSprite;
        }

        public void OnSelect(BaseEventData eventData)
        {
            _phoneImage.sprite = _phoneSelectedSprite;
        }

        public void OnDeselect(BaseEventData eventData)
        {
            _phoneImage.sprite = _phoneDeselectedSprite;
        }
    }
}