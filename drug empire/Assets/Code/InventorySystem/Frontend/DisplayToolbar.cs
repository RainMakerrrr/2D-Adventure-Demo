using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace InventorySystem
{
    public class DisplayToolbar : MonoBehaviour
    {
        public static DisplayToolbar Instance { get; private set; }

        public InventoryBackpack playerInventory;

        [Header("Scene refs")]
        public GameObject toolbarUI;

        [Header("Prefab refs")]
        public GameObject slotMiniPrefab;

        [Header("Sprite refs")]
        public Sprite slotMiniInactive;
        public Sprite slotMiniActive;

        [Header("Settings")]
        public int toolbarCapacity;
        public Vector2 toolbarAddSize = new Vector2(30, 30);

        [HideInInspector] public int toolbarSlotID = 0;
        [HideInInspector] public List<SlotMiniUI> miniSlots = new List<SlotMiniUI>();

        Vector2 toolbarScaledSize;
        Vector2 toolbarOriginalSize;

        private void Awake()
        {
            Instance = this;
            playerInventory.activeSlot = null;
        }

        void Start()
        {
            CreateToolbar();
        }

        void Update()
        {
            ScrollToolbar();
        }

        public void MMBPressed(SlotUI _slotUI)
        {
            if (_slotUI.ParentInventory is InventoryBackpack)
                SetToolbarSlot(_slotUI);
        }

        void CreateToolbar()
        {
            for (int i = 0; i < toolbarCapacity; i++)
            {
                GameObject _slot = Instantiate(slotMiniPrefab, Vector3.zero, Quaternion.identity);
                _slot.transform.SetParent(toolbarUI.transform, false);
                _slot.transform.SetAsLastSibling();
                miniSlots.Add(_slot.GetComponent<SlotMiniUI>());
            }
            toolbarUI.SetActive(true);
            toolbarOriginalSize = slotMiniPrefab.GetComponent<RectTransform>().sizeDelta;
            toolbarScaledSize = toolbarOriginalSize + toolbarAddSize;
            miniSlots[toolbarSlotID].gameObject.GetComponent<RectTransform>().sizeDelta = toolbarScaledSize;
        }

        void ScrollToolbar()
        {
            float scrollAxis = Input.GetAxis("Mouse ScrollWheel");
            if (scrollAxis != 0)
            {
                if (scrollAxis > 0)
                    toolbarSlotID++;
                else if (scrollAxis < 0)
                    toolbarSlotID--;
                ResetToolbarID();
                SelectToolbarSlot();
            }
            ResetToolbarID();
        }

        void ResetToolbarID()
        {
            if (toolbarSlotID >= toolbarCapacity)
                toolbarSlotID = 0;
            if (toolbarSlotID < 0)
                toolbarSlotID = toolbarCapacity - 1;
        }

        void SelectToolbarSlot()
        {
            for (int i = 0; i < miniSlots.Count; i++)
            {
                if (toolbarSlotID == i)
                {
                    miniSlots[i].gameObject.GetComponent<RectTransform>().sizeDelta = toolbarScaledSize;
                    miniSlots[i].gameObject.GetComponent<Image>().sprite = slotMiniActive;
                    if (miniSlots[i].slot != null)
                        playerInventory.activeSlot = miniSlots[i].slot;
                    else
                        playerInventory.activeSlot = null;
                }
                else
                {
                    miniSlots[i].gameObject.GetComponent<RectTransform>().sizeDelta = toolbarOriginalSize;
                    miniSlots[i].gameObject.GetComponent<Image>().sprite = slotMiniInactive;
                }
            }
        }

        void SetToolbarSlot(SlotUI _slotUI)
        {
            for (int i = 0; i < miniSlots.Count; i++)
            {
                if (i != toolbarSlotID && miniSlots[i].slot == _slotUI.ParentSlot)
                {
                    miniSlots[i].slot = null;
                    miniSlots[i].icon.sprite = null;
                }
                else if (i == toolbarSlotID && miniSlots[i].slot != _slotUI.ParentSlot)
                {
                    miniSlots[i].slot = _slotUI.ParentSlot;
                    miniSlots[i].icon.sprite = _slotUI.ParentSlot.item.icon;
                    playerInventory.activeSlot = _slotUI.ParentSlot;
                }
            }
        }

    }
}