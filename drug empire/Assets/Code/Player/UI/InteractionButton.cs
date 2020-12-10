using System;
using System.Collections;
using System.Collections.Generic;
using InventorySystem;
using NPC.Friends;
using UnityEngine;
using UnityEngine.UI;

public class InteractionButton : MonoBehaviour
{
    [SerializeField] private GameObject _interactor;
    [SerializeField] private Button _interactorButton;

    public GameObject Inventory
    {
        get => _interactor;
        set => _interactor = value;
    }

    public Button InteractorButton => _interactorButton;

    private void Update()
    {
        transform.position = Camera.main.WorldToScreenPoint(_interactor.transform.position + new Vector3(0f, 0.5f, 0f));
        
    }
}
