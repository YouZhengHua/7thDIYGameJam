using System;

namespace Scripts.Base
{
    public class BaseFiniteStateMachine<T> : IBaseFiniteStateMachine<T>
    {
        protected T _currectState;
        protected Action _exitStateLamdba;

        public BaseFiniteStateMachine(T currectState)
        {
            _currectState = currectState;
        }

        public BaseFiniteStateMachine(T currectState, Action exitStateLamdba)
        {
            _currectState = currectState;
            _exitStateLamdba = exitStateLamdba;
        }

        public T CurrectState { get => _currectState; }

        public virtual void SetNextState(T value) {
            ChangeState(value);
        }

        public virtual void SetNextState(T value, Action nextExitStateLamdba)
        {
            ChangeState(value);
            _exitStateLamdba = nextExitStateLamdba;
        }

        protected virtual void ChangeState(T value)
        {
            _currectState = value;
            _exitStateLamdba?.Invoke();
            _exitStateLamdba = null;
        }
    }
}