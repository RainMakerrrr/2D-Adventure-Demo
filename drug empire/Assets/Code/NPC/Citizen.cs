using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC
{
    public class Citizen : MonoBehaviour
    {
        [SerializeField] private Transform _wayPoint;
        [SerializeField] private float _moveSpeed;
        private Animator _animator;
        private Vector3 _startPosition;
        
        private void Start()
        {
            _animator = GetComponent<Animator>();
            _startPosition = transform.position;
        }

        private void Update()
        {
            Walk();
        }

        private void Walk()
        {
            transform.position =
                Vector3.MoveTowards(transform.position, _wayPoint.position, _moveSpeed * Time.deltaTime);
            _animator.SetBool("isMoving",true);
            _animator.SetFloat("Horizontal", transform.position.x - _wayPoint.position.x);
            _animator.SetFloat("Vertical", transform.position.y - _wayPoint.position.y);

            if (Vector3.Distance(transform.position, _wayPoint.position) <= 1f)
                transform.position = _startPosition;

        }
    }
}
