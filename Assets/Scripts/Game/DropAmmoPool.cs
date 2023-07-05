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
    public class DropAmmoPool : BasePool, IDropAmmoPool
    {
        public DropAmmoPool(PoolData poolData, IPlayerController player, IAttributeHandle attributeHandle, IGameUIController gameUI, IGameFiniteStateMachine gameFiniteStateMachine): base(poolData.prefab, poolData.poolSize)
        {
            _prefabPool.ForEach(prefab =>
            {
                prefab.GetComponent<IDropAmmoController>().SetPlayerController = player;
                prefab.GetComponent<IDropAmmoController>().SetAttributeHandle = attributeHandle;
                prefab.GetComponent<IDropAmmoController>().SetGameUI = gameUI;
                prefab.GetComponent<IDropAmmoController>().SetGameFiniteStateMachine = gameFiniteStateMachine;
            });
        }
    }
}