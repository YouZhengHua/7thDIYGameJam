using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Base
{
    public class BasePool : IBasePool
    {
        private GameObject _prefab;
        private int _poolSize = 100;

        protected List<GameObject> _prefabPool;

        private int index = 0;

        public BasePool(GameObject prefab, int poolSize)
        {
            _prefab = prefab;
            _poolSize = poolSize;
            _prefabPool = new List<GameObject>();
            for (int i = 0; i < _poolSize; i++)
            {
                GameObject tmp = GameObject.Instantiate(_prefab);
                tmp.SetActive(false);
                _prefabPool.Add(tmp);
            }
        }

        public GameObject GetPrefab()
        {
            GameObject gameObject = _prefabPool.Find(col => col.activeInHierarchy == false);
            if (gameObject == null)
            {
                index++;
                index %= _prefabPool.Count;
                gameObject = _prefabPool[index];
            }
            else
            {
                index = _prefabPool.IndexOf(gameObject);
            }
            gameObject.SetActive(true);
            return gameObject;
        }

        public List<GameObject> Prefabs { get => _prefabPool; }
    }
}