using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Scripts.Game
{
    public interface IWeaponController
    {
        /// <summary>
        /// 載入並設定指定的武器編號是否激活
        /// </summary>
        /// <param name="weaponIndex"></param>
        /// <param name="weaponIcon"></param>
        /// <param name="active"></param>
        public void LoadWeapon(WeaponIndex weaponIndex, Sprite weaponIcon, bool active = true);
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
        /// <summary>
        /// 取得已生效的武器清單
        /// </summary>
        public IList<Weapon> ActiveWeapons { get; }
    }
}