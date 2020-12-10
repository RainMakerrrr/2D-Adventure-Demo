using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerController;
using UnityEngine.SceneManagement;

namespace NPC.Enemies
{
    public class Gang : Enemy
    {
        protected override void EnemyStatesHandler()
        {
            switch (_state)
            {
                case EnemyState.IDLE:
                StartCoroutine(Idle());
                break;
                case EnemyState.PATROL:
                Patrol();
                break;
                case EnemyState.CHASING:
                Chasing();
                break;
                case EnemyState.ARREST:
                KillPlayer();
                break;
                    
            }
        }

        private void Patrol()
        {
            _mark.gameObject.SetActive(false);
            
            var movement = Vector3.MoveTowards(transform.position, _wayPoints[_currentIndex].position, _moveSpeed * Time.deltaTime);
            transform.position = movement;
            SetEnemyAnimation(_wayPoints[_currentIndex]);
            
            if (Vector3.Distance(transform.position, _wayPoints[_currentIndex].position) < 0.2f)
            {
                if (_waitTime <= 0)
                {
                    _currentIndex = (_currentIndex + 1) % _wayPoints.Length;
                    _waitTime = _startWaitTime;
                }
                else
                {
                    _waitTime -= Time.deltaTime;
                    _animator.SetBool("isWalking",false);
                }
            }
            
            var visibleDistance = Vector3.Distance(transform.position, _player.transform.position);
            if (visibleDistance <= 2f && _player.InDebt)
                _state = EnemyState.CHASING;
        }

        private void KillPlayer()
        {
            _mark.gameObject.SetActive(false);
            
            Debug.Log("Kill player");
            _animator.SetBool("isWalking", false);
            GameStateHandler.Instance.State = GameState.LOSE;
            SceneManager.LoadScene(5);
        }
    }
}