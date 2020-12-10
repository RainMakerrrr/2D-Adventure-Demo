using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace PlayerController.UI
{
    public class PlayerAvatar : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _playerMoney;
        [SerializeField] private TextMeshProUGUI _playerRepspect;
        private Player _player;

        private void Start() => _player = FindObjectOfType<Player>();
        

        private void Update()
        {
            _playerMoney.text = "$" + _player.inventory.Money;
            _playerRepspect.text = "RP " + _player.Level.CurrentExperience + "\n" + "LEVEL - " + _player.Level.CurrentLevel;
        }
    }
}
