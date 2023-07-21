using System;
using UnityEngine;

namespace Scripts.Game.Data
{
    [Serializable]
    public struct DropStruct
    {
        [Header("掉落率")]
        public FloatAttributeHandle _chance;
        [SerializeField, Header("掉落預置物")]
        private GameObject _prefab;
        /// <summary>
        /// 是否掉落
        /// </summary>
        public bool IsDrop { get => UnityEngine.Random.value <= _chance.Value; }
        public void DropCheck(Vector3 postiton)
        {
            if (!IsDrop)
            {
                return;
            }
            float randomRange = 0.5f;
            GameObject gameObject = GameObject.Instantiate(_prefab);
            gameObject.transform.position = postiton + new Vector3(UnityEngine.Random.Range(randomRange * -1f, randomRange), UnityEngine.Random.Range(randomRange * -1f, randomRange), 0f);
        }
    }
}