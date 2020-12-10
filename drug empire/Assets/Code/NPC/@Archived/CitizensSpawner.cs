using System.Collections;
using System.Collections.Generic;
using NPC.ObjectPool;
using UnityEngine;


namespace NPC
{
    public class CitizensSpawner : MonoBehaviour
    {
        [SerializeField] private Transform[] _spawnPositions;
        [SerializeField] private float _spawnRate;

        private void Start()
        {
            Spawn();
        }

        private void Spawn()
        {
            var citizen = CitizensObjectPool.Instance.Get();
            citizen.transform.position = _spawnPositions[Random.Range(0, _spawnPositions.Length)].position;
            citizen.gameObject.SetActive(true);
        }
    }
}
