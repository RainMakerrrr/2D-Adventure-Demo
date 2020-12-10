using System;
using System.Collections;
using System.Collections.Generic;
using CharacterStatsSystem;
using LevelSystem;
using InventorySystem;
using UnityEngine;
using DialogueSystem;

public class PlayerTest : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private Speed _speed;
    [SerializeField] private CharacterLevel _level;
    [SerializeField] private BackpackCapacity _backpack;
    [SerializeField] private InventoryObject _inventory;

    [HideInInspector] public bool canStash;
    [HideInInspector] public bool canShop;

    public DisplayInventory displayInventory;

    private Rigidbody _rigidbody;
    private SpriteRenderer _spriteRenderer;

    bool isChestOpen;
    bool isInventoryOpen;
    bool isInventoryActive;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        Movement();

        if (Input.GetKeyDown(KeyCode.K))
        {
            _inventory.Save();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            _inventory.Load();
        }

    }

    private void OnApplicationQuit()
    {
        _inventory.Clear();
    }

    void Movement()
    {
        Vector2 playerInput;
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");
        playerInput = Vector2.ClampMagnitude(playerInput, 1f);
        Vector3 velocity = new Vector3(playerInput.x, 0f, playerInput.y);
        Vector3 displacement = velocity * Time.deltaTime * 5;
        transform.localPosition += displacement;
    }

    #region Inventory System
    // void OnTriggerEnter(Collider other)
    // {
    //     ItemPhysical physicalItem = other.GetComponent<ItemPhysical>();
    //     if (physicalItem
    //         && _inventory.backpackCapacity.CurrentWeight + physicalItem.item.weight <= _inventory.backpackCapacity.MaxValue)
    //     {
    //         _inventory.AddItem(physicalItem.item, 1);
    //         Destroy(physicalItem.gameObject);
    //     }
    //
    //     if (other.GetComponent<InventoryPhysical>()) {
    //         displayInventory.closestStash = other.GetComponent<InventoryPhysical>();
    //     }
    // }
    //
    // private void OnTriggerExit(Collider other)
    // {
    //     if (other.GetComponent<InventoryPhysical>())
    //         displayInventory.closestStash = null;
    // }
    #endregion

    #region Characteristics, Level System
    [ContextMenu("Upgrade backpack")]
    public void UpgradeBackpackCapacity()
    {
        _backpack.UpgradeStat();
        Debug.Log(_backpack.StartValue);
        Debug.Log(_speed.StartValue);
    }
    
    [ContextMenu("Give Respect")]
    public void GiveRespect()
    {
        Debug.Log(_level.CurrentExperience);
        _level.GiveExperience(UnityEngine.Random.Range(50,400));
        Debug.Log(_level.CurrentExperience + "  " + _level.CurrentLevel);
    }
    
    [ContextMenu("Upgrade speed stat")]
    public void UpgradeSpeedStat()
    {
        _speed.UpgradeStat();
     
        Debug.Log(_speed.StartValue);
    }
    
    [ContextMenu("Upgrade health stat")]
    public void UpgradeHealth()
    {
        _health.UpgradeStat();
        
        Debug.Log(_health.StartValue);
    }
    #endregion
}
