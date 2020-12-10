using System;
using System.Collections;
using System.Collections.Generic;
using CharacterStatsSystem;
using UnityEngine;

namespace PlayerController
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private Speed _runSpeed;
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _idleTimer;
        
        private Rigidbody2D _rigidbody;
        private Animator _animator;
        private Vector2 _movement;

        private bool _canMove = true;

        public float IdleTimer
        {
            get => _idleTimer;
            set => _idleTimer = value;
        }

        public bool CanMove
        {
            get => _canMove;
            set => _canMove = value;
        }

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }

        private void FixedUpdate()
        {
            if (_canMove)
            {
                _movement.x = Input.GetAxis("Horizontal");
                _movement.y = Input.GetAxis("Vertical");
                _movement = Vector3.ClampMagnitude(_movement, 1f);

                _animator.SetFloat("Horizontal", _movement.x);
                _animator.SetFloat("Vertical", _movement.y);
                _animator.SetFloat("Speed", _movement.sqrMagnitude);

                _rigidbody.MovePosition(_rigidbody.position + _movement * _moveSpeed * Time.fixedDeltaTime);
               
            }

            MovementStateHandler();
        }

        private void MovementStateHandler()
        {
            if (_movement.magnitude != 0)
            {
                GetComponent<Player>().State = PlayerState.MOVE;
                _idleTimer = 0;
            }
            else
            {
                GetComponent<Player>().State = PlayerState.IDLE;
                _idleTimer += 1 * Time.deltaTime;
            }
        }
    }
}