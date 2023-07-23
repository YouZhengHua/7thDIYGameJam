using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Scripts.Game
{
    public class WeaponController : MonoBehaviour, IWeaponController
    {
        /// <summary>
        /// 所有武器清單
        /// </summary>
        private IList<Weapon> canUseWeapons = new List<Weapon>();

        [SerializeField, Header("武器 Prefab 列表Data"), Tooltip("從這裡拿參考實例化武器")]
        private WeaponControllerData weaponControllerData;

        [SerializeField, Header("武器根節點"), Tooltip("武器實例化的根節點")]
        private Transform weaponRoot;

        [SerializeField, Header("武器貼圖")]
        private WeaponColumnController[] _weaponColumns;
        private int _nextWeaponColumn = 0;

        private void Awake()
        {
            foreach (GameObject weaponPrefab in weaponControllerData.weaponPrefabList)
            {
                Weapon weapon = Instantiate(weaponPrefab, this.transform).GetComponent<Weapon>();
                weapon.transform.SetParent(weaponRoot);
                weapon.SetWeaponActive(false);
                canUseWeapons.Add(weapon.GetComponent<Weapon>());
            }
        }

        public void LoadWeapon(WeaponIndex weaponIndex, Sprite weaponIcon, bool active = true)
        {

            Weapon weapon = this.GetWeapon(weaponIndex);
            weapon.LoadWeapon(active);
            _weaponColumns[_nextWeaponColumn].AddWeaponIcon(weaponIcon);
            _nextWeaponColumn++;
        }

        public Weapon GetWeapon(WeaponIndex weaponIndex)
        {
            Weapon result = null;
            foreach (Weapon weapon in canUseWeapons)
            {
                if (weapon.GetWeaponIndex == weaponIndex)
                {
                    result = weapon;
                    continue;
                }
            }
            return result;
        }

        public IList<Weapon> GetWeapons()
        {
            return canUseWeapons;
        }
    }
}