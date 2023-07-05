using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Base
{
    public interface IBasePool
    {
        public GameObject GetPrefab();

        public List<GameObject> Prefabs { get; }
    }
}