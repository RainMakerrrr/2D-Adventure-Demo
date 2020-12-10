using System;
using UnityEngine;
using InventorySystem;
using NPC.Friends;
using UnityEngine.UI;

namespace PlayerController
{
    public class Interactor : MonoBehaviour
    {
        [SerializeField] private InteractionButton _interactionButton; 
        private IInteractable _interactable;

        private void Update()
        {
            CheckForInteraction();
        }

        private void CheckForInteraction()
        {
            if (_interactable == null) return;

            if (Input.GetKeyDown(KeyCode.E))
            {
                _interactable.Interact();
            }
            
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            var interactable = other.GetComponent<IInteractable>();
            if (interactable == null) return;
            
            _interactable = interactable;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var interactable = other.GetComponent<IInteractable>();
            if (interactable != _interactable) return;
            
            DisplayInventory.Instance.closestStash = null;
            
            _interactable = null;
            _interactionButton.gameObject.SetActive(false);
        }
    }
}