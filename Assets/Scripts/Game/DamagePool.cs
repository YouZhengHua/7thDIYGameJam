using System.Collections;
using System.Collections.Generic;
using Scripts.Game.Base;
using UnityEngine;
using Scripts.Base;
using Scripts.Game.Data;

namespace Scripts.Game
{
    public class DamagePool : BasePool, IDamagePool
    {
        public DamagePool(PoolData poolData): base(poolData.prefab, poolData.poolSize)
        {
        }
    }
}