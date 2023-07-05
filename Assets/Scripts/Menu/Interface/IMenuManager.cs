using Scripts.Game.Data;
using System.Collections.Generic;

namespace Scripts.Menu
{
    public interface IMenuManager
    {
        public void ShowMenu();
        public void ShowGameOptions();
        public void ShowGameOptionsWithReflash();
        public void ShowSetting();
    }
}
