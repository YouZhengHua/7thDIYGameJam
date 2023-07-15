using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Base
{
    public class BaseStateMachine<T>
    {
        protected T _currectState;
        protected IList<Action> _exitStateLamdbas;
        protected Action _exitStateLamdba;

        protected BaseStateMachine()
        {
            _exitStateLamdbas = new List<Action>();
        }

        public T CurrectState { get => _currectState; }

        public virtual void SetNextState(T value) {
            ChangeState(value);
        }

        public virtual void SetNextState(T value, Action nextExitStateLamdba)
        {
            ChangeState(value);
            _exitStateLamdbas.Add(nextExitStateLamdba);
        }

        protected virtual void ChangeState(T value)
        {
            Debug.Log($"Change {typeof(T).Name} : {_currectState} -> {value}.");
            _currectState = value;
            foreach(Action action in _exitStateLamdbas)
            {
                action.Invoke();
            }
            _exitStateLamdbas.Clear();
        }
    }
}