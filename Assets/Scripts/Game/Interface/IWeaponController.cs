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
        /// 設定指定的武器編號是否處於激活狀態
        /// </summary>
        /// <param name="weaponIndex"></param>
        /// <param name="isActive"></param>
        public void SetWeaponActive(WeaponIndex weaponIndex, bool isActive);

        /// <summary>
        /// 取得武器編號對應的武器資料
        /// </summary>
        /// <param name="weaponIndex"></param>
        /// <returns></returns>
        public Weapon GetWeapon(WeaponIndex weaponIndex);
        /// <summary>
        /// 取得所有武器資料
        /// </summary>
        /// <returns></returns>
        public IList<Weapon> GetWeapons();
    }
}