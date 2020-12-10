using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PlayerController.UI
{
    public class CharacteristicsButton : MonoBehaviour
    {
        [SerializeField] private ScrollRect _characteristicsPanel;
        [SerializeField] private Sprite _activePanel;
        [SerializeField] private Sprite _nonActivePanel;
        private Image _image;

        private void Start() => _image = GetComponent<Image>();

        private void Update()
        {
            if (_characteristicsPanel.gameObject.activeSelf)
                _image.sprite = _activePanel;
            else _image.sprite = _nonActivePanel;
        }

        public void SetActiveCharPanel() =>
            _characteristicsPanel.gameObject.SetActive(!_characteristicsPanel.gameObject.activeSelf);
    }
}