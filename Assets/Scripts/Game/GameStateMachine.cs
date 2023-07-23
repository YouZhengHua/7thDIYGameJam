using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Base;
using System;

namespace Scripts.Game
{
    public class GameStateMachine : BaseStateMachine<GameState>
    {
        #region 單例模式
        private static readonly object padlock = new object();
        private static GameStateMachine _instance = null;
        public static GameStateMachine Instance 
        {
            get
            {
                lock (padlock)
                {
                    if (_instance == null)
                        _instance = new GameStateMachine();
                    return _instance;
                }
            } 
        }
        #endregion

        protected GameStateMachine() : base()
        {

        }

        protected override void ChangeState(GameState value)
        {
            base.ChangeState(value);
            if(value == GameState.InGame)
            {
                Time.timeScale = 1f;
            }
            else
            {
                Time.timeScale = 0;
            }
        }
    }
}