using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NPC.ObjectPool
{
    public abstract class GenericObjectPool<T> : MonoBehaviour where T : Component
    {
        [SerializeField] private T _objectPrefab;
        [SerializeField] private T[] _objectPrefabs;
        private Queue<T> _objects = new Queue<T>();

        public static GenericObjectPool<T> Instance { get; private set; }

        private void Awake() => Instance = this;

        public T Get()
        {
            if (_objects.Count == 0)
            {
                _objectPrefab = _objectPrefabs[Random.Range(0, _objectPrefabs.Length)];
                AddObjects();
            }

            return _objects.Dequeue();
        }

        public void ReturnToPool(T objectToReturn)
        {
            objectToReturn.gameObject.SetActive(false);
            _objects.Enqueue(objectToReturn);
        }

        private void AddObjects()
        {
            var newObject = Instantiate(_objectPrefab);
            newObject.gameObject.SetActive(false);
            _objects.Enqueue(newObject);
        }
    }
}
