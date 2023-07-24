using System;
using UnityEngine;

namespace Scripts.Game.Data
{
    [Serializable]
    public struct DropStruct
    {
        [SerializeField, Header("掉落類別")]
        private DropType _dropType;
        /// <summary>
        /// 掉落類別
        /// </summary>
        public DropType DropType { get => _dropType; }
        [SerializeField, Header("掉落率")]
        private FloatAttributeHandle _chance;
        /// <summary>
        /// 掉落率
        /// </summary>
        public FloatAttributeHandle Chance { get => _chance; }
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

    public enum DropType
    {
        Other,
        Enemy,
        HealItem,
        Exp
    }
}