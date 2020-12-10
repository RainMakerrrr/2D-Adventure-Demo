using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NonPlayerCharacter : MonoBehaviour, IInteractable
{
    [SerializeField] private RectTransform _dialogueBackground;

    public RectTransform DialogueBackground => _dialogueBackground;

    public abstract void Interact();

}
