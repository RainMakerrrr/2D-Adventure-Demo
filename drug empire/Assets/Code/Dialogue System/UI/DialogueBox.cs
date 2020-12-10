using NPC.Friends;
using PlayerController;
using UnityEngine;

namespace DialogueSystem
{
    public class DialogueBox : MonoBehaviour
    {
        [SerializeField] private Transform _speaker;
        [SerializeField] private Vector3 _offset;

        public Vector3 Offset
        {
            get => _offset;
            set => _offset = value;
        }

        private void Update()    
        {
            transform.position = Camera.main.WorldToScreenPoint(_speaker.transform.position + _offset);
        }
    }
}