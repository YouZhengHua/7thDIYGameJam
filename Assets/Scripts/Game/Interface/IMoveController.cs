﻿namespace Scripts.Game
{
    public interface IMoveController
    {
        /// <summary>
        /// 設定遊戲狀態機
        /// </summary>
        public IGameFiniteStateMachine SetGameFiniteStateMachine { set; }
        /// <summary>
        /// 設定屬性處理器
        /// </summary>
        public IAttributeHandle SetAttributeHandle { set; }
    }
}