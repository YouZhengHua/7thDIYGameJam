using System;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Game
{
    [Serializable]
    public class FloatAttributeHandle
    {
        [SerializeField, Header("基礎值")]
        private float _value;
        [SerializeField, Header("基礎倍率")]
        private float _multiple = 1f;
        private float _extendPoint = 0f;
        private float _extendMultiple = 0f;
        public float Value { get => (_value + _extendPoint) * (_multiple + _extendMultiple); }
        //倍率改變事件
        public UnityEvent<float> OnMultipleChangedEvent = new UnityEvent<float>();
        /// <summary>
        /// 增加固定值
        /// </summary>
        /// <param name="point"></param>
        public void AddValuePoint(float point)
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
            if (OnMultipleChangedEvent != null)
            {
                OnMultipleChangedEvent.Invoke(_extendMultiple);
            }
        }
        public override string ToString()
        {
            return this.Value.ToString();
        }
    }
}