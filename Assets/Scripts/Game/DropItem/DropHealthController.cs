using Scripts.Game.Base;
using Scripts.Game.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Game
{
    public class DropHealthController : DropController
    {
        protected override void GetDropItem()
        {
            AttributeHandle.Instance.HealPlayer(AttributeHandle.Instance.PlayerMaxHealthPoint * 0.05f);
            base.GetDropItem();
        }
    }
}