using Scripts.Game.Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scripts.Game
{
    public class AttributeHandle : IAttributeHandle
    {
        private IWeaponController _weapon;
        private IGameUIController _gameUI;
        private PlayerData _playerData;

        private float _extendDropHealthMultiple = 0f;

        private float _playerSpeedMultiple = 0f;
        private float _extendDEF = 0f;
        private float _totalExp = 0f;
        private float _expExtendMultiple = 0f;
        private float _extendGetItemRadius = 0f;

        public AttributeHandle(PlayerData playerData, IWeaponController weaponController)
        {
            _playerData = playerData;
            _weapon = weaponController;
        }

        public void UpdateAttribute(OptionData data)
        {
            if (data.OptionType == OptionType.Weapon)
            {
                if(data.SelectedCount == 0)
                {
                    _weapon.SetWeaponActive(data.WeaponIndex, true);
                }
                else
                {
                    Weapon weapon = _weapon.GetWeapon(data.WeaponIndex);
                    if (weapon == null)
                        Debug.Log("查無升級武器資料");
                    else
                    {
                        // TODO 調整武器素質
                        Debug.Log("調整武器素質");
                    }
                }
            }
            else
            {
                switch (data.AttributeType)
                {
                    case AttributeType.PlayerHeal:
                        _gameUI.HealPlayer(data.Value * _playerData.MaxHealthPoint);
                        break;
                    case AttributeType.PlayerMaxHealth:
                        _gameUI.AddPlayerHealthPointMax(Mathf.RoundToInt(data.Value));
                        break;
                    case AttributeType.PlayerSpeed:
                        _playerSpeedMultiple += data.Value;
                        break;
                    case AttributeType.ExtendExp:
                        _expExtendMultiple += data.Value;
                        break;
                    case AttributeType.GetDropItemRadius:
                        _extendGetItemRadius += data.Value;
                        break;
                    case AttributeType.Score:
                        _totalExp += data.Value;
                        break;
                    case AttributeType.PlayerDef:
                        _extendDEF += data.Value;
                        break;
                    case AttributeType.DamageMultiple:
                        foreach(Weapon weapon in _weapon.GetWeapons())
                        {
                            weapon.weaponData.Damage *= 1f + data.Value;
                        }
                        break;
                }
            }
        }

        public IGameUIController SetGameUI { set => _gameUI = value; }

        #region 槍械
        public bool NeedDropHealth { get => (_playerData.DropHealthRate + _extendDropHealthMultiple) >= Random.value; }
        #endregion

        #region 經驗值
        public void AddExp(float exp)
        {
            _playerData.NowExp += exp * ExpMultiple;
            _totalExp += exp * ExpMultiple;
        }
        public float NowExp { get => _playerData.NowExp; set => _playerData.NowExp = value; }
        public float NextLevelExp { get => _playerData.NextLevelExp; }
        private float ExpMultiple { get => _playerData.ExpRate + _expExtendMultiple; }
        public bool IsLevelUp { get => _playerData.NowExp >= _playerData.NextLevelExp; }
        public int Level { get => _playerData.Level; set => _playerData.Level = value; }
        public float ExpPercentage { get => _playerData.NowExp / _playerData.NextLevelExp; }
        public float TotalExp { get => _totalExp; }
        public AudioClip LevelUpAudio { get => _playerData.LevelUpAudio; }
        public AudioClip GetHitAudio { get => _playerData.GetHitAudio; }
        #endregion

        #region 玩家
        public float GetDropItemRadius { get => _playerData.DropItemRadius + _extendGetItemRadius; }
        public float PlayerRepelRadius { get => _playerData.Radius; }
        public float PlayerRepelForce { get => _playerData.Force; }
        public float PlayerRepelTime { get => _playerData.EnemyDelayTime; }
        public float PlayerHealthPoint { get => CalTool.Round(_playerData.HealthPoint, 1); set => _playerData.HealthPoint = value; }
        public float PlayerMaxHealthPoint { get => CalTool.Round(_playerData.MaxHealthPoint, 1); set => _playerData.MaxHealthPoint = value; }
        public float PlayerMoveSpeed
        {
            get => _playerData.BaseMoveSpeed * (_playerData.MoveSpeedRate + _playerSpeedMultiple);
        }
        public float InvincibleTime { get => _playerData.InvincibleTime; }
        public float PlayerDEF { get => _playerData.DEF + _extendDEF; }
        /// <summary>
        /// 玩家添加武器
        /// </summary>
        /// <param name="weaponIndex"></param>
        private void AddWeapon(WeaponIndex weaponIndex)
        {
            
        }
        #endregion
    }
}
