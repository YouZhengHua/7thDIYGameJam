using Scripts.Base;

namespace Scripts.Game
{
    public interface IGameUIController : IBaseUIController
    {
        public void UpdateGameTime(float gameTime);

        public void UpdatePlayerHealth();

        public void GetExp(ExpNumber exp);

        public void HealPlayer(int healPoint);

        public void AddPlayerHealthPointMax(int healPoint);
    }
}
