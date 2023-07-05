using Scripts.Base;

namespace Scripts.Game
{
    public interface IGameFiniteStateMachine : IBaseFiniteStateMachine<GameState>
    {
        public PlayerState PlayerState { get; }
        public void SetPlayerState(PlayerState value);
    }
}