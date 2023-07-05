using Scripts.Game.Base;
using Scripts.Game.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Game
{
    public class DropExpController : DropController, IDropExpController
    {
        private IGameUIController _gameUI;
        private ExpData _data;

        protected override void GetDropItem()
        {
            _gameUI.GetExp(_data.expNumber);
            base.GetDropItem();
        }

        public ExpData SetExpData 
        { set
            {
                _data = value;
                SetDropItemData = value;
            } 
        }

        public IGameUIController SetGameUI { set => _gameUI = value; }
    }
}