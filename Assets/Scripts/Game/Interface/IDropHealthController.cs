using Scripts.Game.Data;

namespace Scripts.Game
{
    public interface IDropHealthController : IDropController
    {
        public IGameUIController SetGameUI { set; }
    }
}