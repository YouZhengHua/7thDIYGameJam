using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Scripts.Game
{
    public interface IWeaponController
    {
        /// <summary>
        /// 設定遊戲狀態機
        /// </summary>
        public IGameFiniteStateMachine SetGameFiniteStateMachine { set; }
        /// <summary>
        /// 設定屬性處理器
        /// </summary>
        public IAttributeHandle SetAttributeHandle { set; }
        /// <summary>
        /// 設定音效控制器
        /// </summary>
        public IAudioContoller SetAudio { set; }

        /// <summary>
        /// 設定玩家總共持有哪些武器
        /// </summary>
        /// <param name="weapons"></param>
        public void SetPlayerWeapons(IList<WeaponIndex> weapons);

    }
}