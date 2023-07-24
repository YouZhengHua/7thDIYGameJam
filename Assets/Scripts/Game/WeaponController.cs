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

        private IList<WeaponColumnController> _weaponColumns = new List<WeaponColumnController>();
        private int _nextWeaponColumn = 0;
        [SerializeField, Header("武器欄位預置物")]
        private GameObject _weaponColumnPrefab;
        private Transform _weaponColumnContainer;

        private void Awake()
        {
            _weaponColumnContainer = GameObject.Find("WeaponUIContainer").GetComponent<Transform>();

            foreach (GameObject weaponPrefab in weaponControllerData.weaponPrefabList)
            {
                Weapon weapon = Instantiate(weaponPrefab, this.transform).GetComponent<Weapon>();
                weapon.transform.SetParent(weaponRoot);
                weapon.SetWeaponActive(false);
                canUseWeapons.Add(weapon.GetComponent<Weapon>());
            }
        }

        private void Start()
        {
            for (int i = 0; i < AttributeHandle.Instance.WeaponColumnMaxCount; i++)
            {
                GameObject weaponColumnPrefab = GameObject.Instantiate(_weaponColumnPrefab, _weaponColumnContainer);
                weaponColumnPrefab.transform.localPosition = new Vector3(i * -128f, 64f, 0f);
                WeaponColumnController weaponColumn = weaponColumnPrefab.GetComponent<WeaponColumnController>();
                weaponColumn.SetActive(i >= (AttributeHandle.Instance.WeaponColumnMaxCount - AttributeHandle.Instance.WeaponColumnActiveCount));
                _weaponColumns.Add(weaponColumn);
            }
            _nextWeaponColumn = AttributeHandle.Instance.WeaponColumnMaxCount - 1;
        }

        public void LoadWeapon(WeaponIndex weaponIndex, Sprite weaponIcon, bool active = true)
        {
            Weapon weapon = this.GetWeapon(weaponIndex);
            Debug.Log(weapon);
            weapon.LoadWeapon(active);
            _weaponColumns[_nextWeaponColumn].AddWeaponIcon(weaponIcon);
            _nextWeaponColumn--;
            _nextWeaponColumn = Mathf.Max(AttributeHandle.Instance.WeaponColumnMaxCount - AttributeHandle.Instance.WeaponColumnActiveCount, _nextWeaponColumn);
            _nextWeaponColumn = Mathf.Min(AttributeHandle.Instance.WeaponColumnMaxCount - 1, _nextWeaponColumn);
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