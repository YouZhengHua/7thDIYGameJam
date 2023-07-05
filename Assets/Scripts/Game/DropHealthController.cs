using Scripts.Game.Base;
using Scripts.Game.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Game
{
    public class DropHealthController : DropController, IDropHealthController
    {
        private IGameUIController _gameUI;

        protected override void GetDropItem()
        {
            _gameUI.HealPlayer(1);
            base.GetDropItem();
        }

        public IGameUIController SetGameUI { set => _gameUI = value; }
    }
}