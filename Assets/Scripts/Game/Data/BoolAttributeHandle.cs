using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts.Game
{
    [Serializable]
    public class BoolAttributeHandle
    {
        [SerializeField, Header("基礎值")]
        private bool _value;
        public bool Value { get => _value; }
        //bool 改變事件
        public UnityEvent<bool> OnBoolChangedEvent = new UnityEvent<bool>();

        /// <summary>
        /// 改變 bool 值
        /// </summary>
        /// <returns></returns>
        public void ChangeValue(bool value)
        {
            _value = value;
            if (OnBoolChangedEvent != null)
            {
                OnBoolChangedEvent.Invoke(_value);
            }
        }

        public override string ToString()
        {
            return this.Value.ToString();
        }
    }
}