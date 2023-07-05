using System;

namespace Scripts.Base
{
    public interface IBaseFiniteStateMachine<T>
    {
        public T CurrectState { get; }

        public void SetNextState(T value);

        public void SetNextState(T value, Action nextExitStateLamdba);
    }
}