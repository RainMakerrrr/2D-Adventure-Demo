using System.Collections;
using UnityEngine;
using InventorySystem;
using LevelSystem;
using UnityEngine.UI;

namespace PlayerController
{
    public class Player : MonoBehaviour
    {
        public InventoryBackpack inventory;

        [SerializeField] private RectTransform _dialogueBackground;
        [SerializeField] private Slider _slider;
        [SerializeField] private Vector3 _offset;
        
        [SerializeField] private PlayerState _state;
        [SerializeField] private CharacterLevel _level;

        public CharacterLevel Level => _level;

        private Camera _mainCamera;
        private PlayerMovement _movement;

       

        public RectTransform DialogueBackground => _dialogueBackground;
        public PlayerState State
        {
            get => _state;
            set => _state = value;
        }
        
        public bool InDebt { get; set; }
        public bool InHouse { get; set; }

        private void Start()
        {
            _movement = GetComponent<PlayerMovement>();
            _mainCamera = Camera.main;
            _slider.gameObject.SetActive(false);
        }

        private void Update()
        {
            _slider.transform.position = _mainCamera.WorldToScreenPoint(transform.position + _offset);
        }
        
        
        public IEnumerator StartLooting()
        {
            
            DisplayInventory.Instance.isInventoryActive = false;
            DisplayInventory.Instance.UpdateInventories();
                    
            DisplayInventory.Instance.closestStash = null;
            
            _state = PlayerState.LOOTING;
            
            _slider.gameObject.SetActive(true);
            _movement.CanMove = false;
            
            while (_slider.value < _slider.maxValue)
            {
                _slider.value += 40f * Time.deltaTime;
                yield return null;
            }
            _slider.gameObject.SetActive(false);
            _slider.value = 0f;
            _movement.CanMove = true;

            _state = PlayerState.IDLE;
        }
    }

    public enum PlayerState
    {
        IDLE, MOVE, LOOTING
    }
}