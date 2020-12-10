using System.Collections;
using System.Collections.Generic;
using DialogueSystem;
using PlayerController;
using UnityEngine;

namespace NPC.Enemies
{
    public abstract class Enemy : MonoBehaviour
    {
        [SerializeField] protected float _moveSpeed;
        [SerializeField] protected Transform[] _wayPoints;
        [SerializeField] protected float _startWaitTime;
        [SerializeField] protected DialogueBox _mark;
        protected float _waitTime = 3f;
        protected int _currentIndex = 0;
        
        
        protected enum EnemyState{IDLE, PATROL, CHASING, ARREST, SEARCH}
        
        protected Player _player;
        [SerializeField]protected EnemyState _state;
        protected Animator _animator;

        public virtual void Start()
        {
            _player = FindObjectOfType<Player>();
            _animator = GetComponent<Animator>();
            
            _waitTime = _startWaitTime;
            
            _mark.gameObject.SetActive(false);
        }

        private void Update()
        {
            EnemyStatesHandler();
        }

        protected abstract void EnemyStatesHandler();
        
        protected void SetEnemyAnimation(Transform target)
        {
            _animator.SetBool("isWalking",true);
            _animator.SetFloat("Horizontal",(target.position.x - transform.position.x));
            _animator.SetFloat("Vertical",(target.position.y - transform.position.y));
        }
        
        protected IEnumerator Idle()
        {
            var transitionProbability = Random.Range(0, 1000);
            if (transitionProbability > 700)
                _state = EnemyState.PATROL;
            
            yield return new WaitForSeconds(0.2f);
            
            var visibleDistance = Vector3.Distance(transform.position, _player.transform.position);
            if (visibleDistance <= 3f && _player.State == PlayerState.LOOTING)
                _state = EnemyState.CHASING;
        }
        
        protected void Chasing()
        {
            _mark.gameObject.SetActive(true);
            
            transform.position = Vector3.MoveTowards(transform.position, _player.transform.position,
                _moveSpeed * Time.deltaTime);
            SetEnemyAnimation(_player.transform);
            
            if (Vector3.Distance(transform.position, _player.transform.position) < 0.3f)
                _state = EnemyState.ARREST;
            else if (Vector3.Distance(transform.position, _player.transform.position) >= 2f)
                _state = EnemyState.PATROL;
        }

    }
}