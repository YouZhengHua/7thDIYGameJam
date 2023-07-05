using Scripts.Game.Data;

namespace Scripts.Game
{
    public interface IDropAmmoController : IDropController
    {
        public IGameUIController SetGameUI { set; }
    }
}