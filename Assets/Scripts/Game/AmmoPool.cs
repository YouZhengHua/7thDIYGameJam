using System.Collections;
using System.Collections.Generic;
using Scripts.Game.Base;
using UnityEngine;
using Scripts.Base;
using Scripts.Game.Data;

namespace Scripts.Game
{
    /// <summary>
    /// 遊戲物件池控制器
    /// 初始化時會依照傳入的物件池資料，建立對應的物件池
    /// 並提供取得物件的方法
    /// </summary>
    public class AmmoPool : BasePool, IAmmoPool
    {
        public AmmoPool(IGameFiniteStateMachine gameFiniteStateMachine, PoolData poolData, IAttributeHandle attributeHandle, IPlayerController player, IEndUIController endUI): base(poolData.prefab, poolData.poolSize)
        {
            _prefabPool.ForEach(prefab =>
            {
                prefab.GetComponent<IAmmoController>().SetGameFiniteStateMachine = gameFiniteStateMachine;
                prefab.GetComponent<IAmmoController>().SetEndUI = endUI;
                prefab.GetComponent<IAmmoController>().SetPlayer = player;
                prefab.GetComponent<IAmmoController>().SetAttributeHandle = attributeHandle;
            });
        }
    }
}