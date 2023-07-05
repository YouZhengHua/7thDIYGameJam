namespace Scripts.Game
{
    public interface IDropController
    {
        public IGameFiniteStateMachine SetGameFiniteStateMachine { set; }
        public IPlayerController SetPlayerController { set; }
        public IAttributeHandle SetAttributeHandle { set; }
    }
}