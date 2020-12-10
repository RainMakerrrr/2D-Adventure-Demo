using System;
using System.Collections;
using System.Collections.Generic;
using InventorySystem;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerController.UI
{
    public class BackpackButton : MonoBehaviour
    {
        [SerializeField] private Sprite _activeButton;
        [SerializeField] private Sprite _nonActiveButton;
        private Image _image;

        private void Start() => _image = GetComponent<Image>();

        private void Update()
        {
            if (DisplayInventory.Instance.isInventoryActive)
                _image.sprite = _activeButton;
            else _image.sprite = _nonActiveButton;
        }

        public void SetActiveBackpack() => DisplayInventory.Instance.OpenPlayerInventory();
    }
}