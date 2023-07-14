using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Scripts.Game
{
    public class WeaponController : MonoBehaviour, IWeaponController
    {
        /// <summary>
        /// 遊戲狀態機
        /// </summary>
        private IGameFiniteStateMachine _gameFiniteStateMachine;

        /// <summary>
        /// 屬性處理器
        /// </summary>
        private IAttributeHandle _attributeHandle;

        /// <summary>
        /// 音效控制器
        /// </summary>
        private IAudioContoller _audio;

        /// <summary>
        /// 所有武器清單
        /// </summary>
        private IList<Weapon> canUseWeapons = new List<Weapon>();

        [SerializeField, Header("使用者裝載的武器"), Tooltip("測試用，將需要實作的武器放入清單內")]
        private List<WeaponIndex> TestPlayerWeapons;

        [SerializeField, Header("武器 Prefab 列表Data"), Tooltip("從這裡拿參考實例化武器")]
        private WeaponControllerData weaponControllerData;

        [SerializeField, Header("武器根節點"), Tooltip("武器實例化的根節點")]
        private Transform weaponRoot;

        private void Start()
        {
            foreach (Weapon weapon in this.GetComponentsInChildren<Weapon>())
            {
                weapon.SetGameFiniteStateMachine = _gameFiniteStateMachine;
                weapon.SetAudio = _audio;
                weapon.SetWeaponActive(false);
                canUseWeapons.Add(weapon);
            }

            if (TestPlayerWeapons != null)
            {
                foreach (Weapon weapon in canUseWeapons)
                {
                    weapon.SetWeaponActive(TestPlayerWeapons.Contains(weapon.GetWeaponIndex));
                }
            }
        }

        public IGameFiniteStateMachine SetGameFiniteStateMachine { set => _gameFiniteStateMachine = value; }
        public IAttributeHandle SetAttributeHandle { set => _attributeHandle = value; }
        public IAudioContoller SetAudio { set => _audio = value; }

        public void LoadWeapon(WeaponIndex weaponIndex, bool active = true)
        {
            GameObject weaponPrefab = weaponControllerData.weaponPrefabList.Find(x => x.name == weaponIndex.ToString());
            if (weaponPrefab != null)
            {
                Weapon weapon = Instantiate(weaponPrefab, this.transform).GetComponent<Weapon>();
                weapon.transform.SetParent(weaponRoot);
                weapon.SetGameFiniteStateMachine = _gameFiniteStateMachine;
                weapon.SetAudio = _audio;
                weapon.LoadWeapon(active);
                canUseWeapons.Add(weapon);
                //for test
                TestPlayerWeapons.Add(weaponIndex);
            }
        }

        public void SetWeaponActive(WeaponIndex weaponIndex, bool isActive)
        {
            foreach (Weapon weapon in canUseWeapons)
            {
                if (weapon.GetWeaponIndex == weaponIndex)
                    weapon.SetWeaponActive(isActive);
            }
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