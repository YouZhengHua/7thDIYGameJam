using Scripts.Game.Base;
using Scripts.Game.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Game
{
    public class DropAmmoController : DropController, IDropAmmoController
    {
        private IGameUIController _gameUI;

        protected override void GetDropItem()
        {
            _attributeHandle.TotalAmmoCount += _attributeHandle.AmmoBoxCount;
            _gameUI.UpdateAmmoCount();
            base.GetDropItem();
        }

        public IGameUIController SetGameUI { set => _gameUI = value; }
    }
}