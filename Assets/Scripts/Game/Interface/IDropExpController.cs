using Scripts.Game.Data;

namespace Scripts.Game
{
    public interface IDropExpController : IDropController
    {
        public ExpData SetExpData { set; }
        public IGameUIController SetGameUI { set; }
    }
}