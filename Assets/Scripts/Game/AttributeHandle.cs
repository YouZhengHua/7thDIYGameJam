using Scripts.Game.Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scripts.Game
{
    public class AttributeHandle
    {
        private IWeaponController _weapon;
        private IGameUIController _gameUI;
        private PlayerData _playerData;

        private float _extendDropHealthMultiple = 0f;

        private float _playerSpeedMultiple = 0f;
        private float _extendDEF = 0f;
        private float _expExtendMultiple = 0f;
        private float _extendGetItemRadius = 0f;

        private static readonly object padlock = new object();
        private static AttributeHandle _instance = null;
        public static AttributeHandle Instance
        {
            get
            {
                lock (padlock)
                {
                    if (_instance == null)
                        _instance = new AttributeHandle();
                    return _instance;
                }
            }
        }

        public AttributeHandle()
        {

        }

        public void SetPlayerData(PlayerData playerData)
        {
            _playerData = playerData;
        }

        public void SetWeaponController(IWeaponController weaponController)
        {
            _weapon = weaponController;
        }

        public void SetGameUIController(IGameUIController gameUIController)
        {
            _gameUI = gameUIController;
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
                        this.HealPlayer(data.Value * _playerData.MaxHealthPoint);
                        _gameUI.UpdatePlayerHealth();
                        break;
                    case AttributeType.PlayerMaxHealth:
                        this.AddPlayerMaxHP(Mathf.RoundToInt(data.Value));
                        _gameUI.UpdatePlayerHealth();
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
                        StaticPrefs.Score += data.Value;
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

        #region 槍械
        /// <summary>
        /// 判斷是否需要掉落補品
        /// </summary>
        public bool NeedDropHealth { get => (_playerData.DropHealthRate + _extendDropHealthMultiple) >= Random.value; }
        #endregion

        #region 經驗值
        /// <summary>
        /// 增加玩家的經驗直
        /// </summary>
        /// <param name="exp"></param>
        public void AddExp(float exp)
        {
            _playerData.NowExp += exp * ExpMultiple;
        }
        /// <summary>
        /// 取得或設定玩家的經驗值
        /// </summary>
        public float NowExp { get => _playerData.NowExp; set => _playerData.NowExp = value; }
        /// <summary>
        /// 取得玩家的下一階段升級經驗值
        /// </summary>
        public float NextLevelExp { get => _playerData.NextLevelExp; }
        /// <summary>
        /// 取得玩家的經驗值倍率
        /// </summary>
        private float ExpMultiple { get => _playerData.ExpRate + _expExtendMultiple; }
        /// <summary>
        /// 判斷玩家是否升級
        /// </summary>
        public bool IsLevelUp { get => _playerData.NowExp >= _playerData.NextLevelExp; }
        /// <summary>
        /// 取得或設定玩家的等級
        /// </summary>
        public int Level { get => _playerData.Level; set => _playerData.Level = value; }
        /// <summary>
        /// 取得玩家當前的經驗值比率
        /// </summary>
        public float ExpPercentage { get => _playerData.NowExp / _playerData.NextLevelExp; }
        /// <summary>
        /// 取得玩家的升級音效
        /// </summary>
        public AudioClip LevelUpAudio { get => _playerData.LevelUpAudio; }
        /// <summary>
        /// 取得玩家的受擊音效
        /// </summary>
        public AudioClip GetHitAudio { get => _playerData.GetHitAudio; }
        #endregion

        #region 玩家
        /// <summary>
        /// 取得拾取掉落物的範圍
        /// </summary>
        public float GetDropItemRadius { get => _playerData.DropItemRadius + _extendGetItemRadius; }
        /// <summary>
        /// 取得玩家受創時的擊退範圍
        /// </summary>
        public float PlayerRepelRadius { get => _playerData.Radius; }
        /// <summary>
        /// 取得玩家受創時的擊退力道
        /// </summary>
        public float PlayerRepelForce { get => _playerData.Force; }
        /// <summary>
        /// 取得玩家受創時的擊退時間
        /// </summary>
        public float PlayerRepelTime { get => _playerData.EnemyDelayTime; }
        /// <summary>
        /// 取得或設定玩家的血量
        /// </summary>
        public float PlayerHealthPoint { get => CalTool.Round(_playerData.HealthPoint, 1); }
        /// <summary>
        /// 取得或設定玩家的最大血量
        /// </summary>
        public float PlayerMaxHealthPoint { get => CalTool.Round(_playerData.MaxHealthPoint, 1); }
        /// <summary>
        /// 取得玩家的移動速度
        /// </summary>
        public float PlayerMoveSpeed
        {
            get => _playerData.BaseMoveSpeed * (_playerData.MoveSpeedRate + _playerSpeedMultiple);
        }
        /// <summary>
        /// 取得玩家的無敵時間
        /// </summary>
        public float InvincibleTime { get => _playerData.InvincibleTime; }
        /// <summary>
        /// 取得玩家的防禦力
        /// </summary>
        private float PlayerDEF { get => _playerData.DEF + _extendDEF; }
        /// <summary>
        /// 玩家受到傷害
        /// 優先扣除護盾
        /// </summary>
        /// <param name="damage"></param>
        public void PlayerGetDamage(float damage)
        {
            damage = CalTool.CalDamage(damage, this.PlayerDEF);
            damage -= _playerData.Shield;
            _gameUI.UpdatePlayerHealth();
        }

        /// <summary>
        /// 玩家接受治癒
        /// </summary>
        /// <param name="value">治癒量</param>
        public void HealPlayer(float value)
        {
            _playerData.HealthPoint += value;
            if (_playerData.HealthPoint > _playerData.MaxHealthPoint)
                _playerData.HealthPoint = _playerData.MaxHealthPoint;
        }

        /// <summary>
        /// 增加玩家最大血量
        /// </summary>
        /// <param name="value">增加量</param>
        public void AddPlayerMaxHP(float value)
        {
            _playerData.MaxHealthPoint += value;
            _playerData.HealthPoint += value;
        }
        #endregion
    }
}
