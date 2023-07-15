using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Base;
using System;

namespace Scripts.Game
{
    public class PlayerStateMachine : BaseStateMachine<PlayerState>
    {
        #region 單例模式
        private static PlayerStateMachine _instance = null;
        public static PlayerStateMachine Instance 
        {
            get
            {
                if (_instance == null)
                    _instance = new PlayerStateMachine();
                return _instance;
            } 
        }
        #endregion

        protected PlayerStateMachine() : base()
        {

        }

        protected override void ChangeState(PlayerState value)
        {
            base.ChangeState(value);
        }
    }
}