using System.Collections;
using PlayerController;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace NPC.Enemies
{
    public class Police : Enemy
    {
        [SerializeField] private Slider _slider;

        public override void Start()
        {
            base.Start();
            _slider.gameObject.SetActive(false);
        }

        protected override void EnemyStatesHandler()
        {
            _slider.transform.position = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0f,0.75f,0f));
            
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
                case EnemyState.SEARCH:
                    StartCoroutine(PlayerSearch());
                    break;
                case EnemyState.ARREST:
                    Arrest();
                    break;
            }
            
        }
        
        
        private void Patrol()
        {
            StopAllCoroutines();
            
            _moveSpeed = 1f;
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
            if (visibleDistance <= 3f && _player.State == PlayerState.LOOTING)
                _state = EnemyState.CHASING;
            if (visibleDistance <= 3f && _player.GetComponent<PlayerMovement>().IdleTimer >= 7f)
                _state = EnemyState.SEARCH;
        }
        

        private void Arrest()
        {
            _mark.gameObject.SetActive(false);
            
            Debug.Log("Player lose");
            _animator.SetBool("isWalking", false);
            GameStateHandler.Instance.State = GameState.LOSE;
            SceneManager.LoadScene(2);
        }

        private IEnumerator PlayerSearch()
        {
            transform.position = Vector3.MoveTowards(transform.position, _player.transform.position,
                _moveSpeed * Time.deltaTime);
            SetEnemyAnimation(_player.transform);

            if (Vector3.Distance(transform.position, _player.transform.position) <= 1f)
            {
                Debug.Log("search");
                _animator.SetBool("isWalking",false);
                _moveSpeed = 0f;
                
                yield return StartSearching();
                
                if (_player.inventory.slots.Count > 0)
                {
                    Debug.Log("has drugs");
                    _state = EnemyState.ARREST;
                }
                else
                {
                    Debug.Log("don't have drugs");
                    _state = EnemyState.PATROL;
                    _player.GetComponent<PlayerMovement>().IdleTimer = 0;
                    
                }

            }
        }

        private IEnumerator StartSearching()
        {

            _slider.gameObject.SetActive(true);
            _player.GetComponent<PlayerMovement>().CanMove = false;

            while (_slider.value < _slider.maxValue)
            {
                _slider.value += 20 * Time.deltaTime;
                yield return new WaitForSeconds(0.3f);
            }
            _slider.gameObject.SetActive(false);
            _slider.value = 0f;
            
            _player.GetComponent<PlayerMovement>().CanMove = true;

        }
    }
}