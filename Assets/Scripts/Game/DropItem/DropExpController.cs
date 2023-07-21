using Scripts.Game.Base;
using Scripts.Game.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Game
{
    public class DropExpController : DropController
    {
        protected override void GetDropItem()
        {
            base.GetDropItem();
            AttributeHandle.Instance.AddExp(1f);
        }
    }
}