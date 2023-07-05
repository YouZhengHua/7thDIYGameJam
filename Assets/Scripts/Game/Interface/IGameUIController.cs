using Scripts.Base;

namespace Scripts.Game
{
    public interface IGameUIController : IBaseUIController
    {
        public void ShowReloadImage();

        public void HideReloadImage();

        public void UpdateReloadImage(float reloatReate);

        public void UpdateGameTime(float gameTime);

        public void UpdateAmmoCount();

        public void UpdatePlayerHealth();

        public void GetExp(ExpNumber exp);

        public void HealPlayer(int healPoint);

        public void AddPlayerHealthPointMax(int healPoint);
    }
}
