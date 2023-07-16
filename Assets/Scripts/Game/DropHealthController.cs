using Scripts.Game.Base;
using Scripts.Game.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Game
{
    public class DropHealthController : DropController, IDropHealthController
    {
        protected override void GetDropItem()
        {
            AttributeHandle.Instance.HealPlayer(1f);
            base.GetDropItem();
        }
    }
}