﻿using Scripts.Base;

namespace Scripts.Game
{
    public interface IGameUIController : IBaseUIController
    {
        public void UpdateGameTime(float gameTime);

        public void UpdatePlayerHealth();

        public void UpdateExpGUI();
    }
}
