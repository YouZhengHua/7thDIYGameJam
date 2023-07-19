using System;
using UnityEngine;

namespace Scripts.Game
{
    [Serializable]
    public class IntAttributeHandle
    {
        [SerializeField, Header("基礎值")]
        private int _value;
        [SerializeField, Header("基礎倍率")]
        private float _multiple = 1f;
        private int _extendPoint = 0;
        private float _extendMultiple = 0f;
        public int Value { get => Mathf.RoundToInt((_value + _extendPoint) * (_multiple + _extendMultiple)); }
        /// <summary>
        /// 增加固定值
        /// </summary>
        /// <param name="point"></param>
        public void AddValuePoint(int point)
        {
            _extendPoint += point;
        }
        /// <summary>
        /// 增加倍率
        /// </summary>
        /// <param name="point"></param>
        public void AddValueMultiple(float multiple)
        {
            _extendMultiple += multiple;
        }
        public override string ToString()
        {
            return this.Value.ToString();
        }
    }
}