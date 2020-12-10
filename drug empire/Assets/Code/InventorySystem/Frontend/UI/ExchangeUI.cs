using System;
using System.Collections;
using System.Collections.Generic;
using InventorySystem;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExchangeUI : MonoBehaviour
{
    [SerializeField] private Button _confirmationButton;
    public TextMeshProUGUI amountText;
    public Slider amountSlider;

    private void Start()
    {
        _confirmationButton.onClick.AddListener(() =>
        {
            if(DisplayInventory.Instance.exchangeWindowUI.activeSelf)
                DisplayInventory.Instance.Exchange();
        });
    }

    public void UpdateText()
    {
        amountText.text = "Amount: " + amountSlider.value;
    }
}