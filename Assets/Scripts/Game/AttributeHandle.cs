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

        /// <summary>
        /// 額外最大生命
        /// </summary>
        private float _extendMaxHealthPoint = 0f;

        /// <summary>
        /// 額外分數比率
        /// </summary>
        private float _extendMoneyMuliple = 0f;

        /// <summary>
        /// 額外補品掉落率
        /// </summary>
        private float _extendHealItemRate = 0f;

        private float _totalMoney = 0f;
        private int _totalKill = 0;

        private float _gameTime = 0f;

        /// <summary>
        /// 生效的武器清單
        /// </summary>
        private IList<WeaponOptionData> _activeWeapons = new List<WeaponOptionData>();

        /// <summary>
        /// 生效的武器清單
        /// </summary>
        private IList<WeaponIndex> _activeWeaponIndexs = new List<WeaponIndex>();

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

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            _totalMoney = 0f;
            _totalKill = 0;
            _gameTime = 0f;
            _extendHealItemRate = 0f;
            _weapon = GameObject.Find("PlayerContainer").GetComponent<IWeaponController>();
            _activeWeapons.Clear();
            _activeWeaponIndexs.Clear();
        }

        public void SetPlayerData(PlayerData playerData)
        {
            _playerData = playerData;
        }

        public void SetGameUIController(IGameUIController gameUIController)
        {
            _gameUI = gameUIController;
        }

        public void UpdateAttribute(AttributeOptionData data)
        {
            switch (data.AttributeType)
            {
                case AttributeType.Score:
                    this.AddTotalMoney(data.Value);
                    break;
                case AttributeType.DamageMultiple:
                    foreach (Weapon weapon in _weapon.GetWeapons())
                    {
                        weapon.weaponData.Damage.AddValueMultiple(data.Value);
                    }
                    break;
                case AttributeType.PlayerMaxHealth:
                    this.AddPlayerMaxHP(CalTool.Round(data.Value, 1));
                    break;
                case AttributeType.ExtendExp:
                    _playerData.ExpRate.AddValueMultiple(data.Value);
                    break;
                case AttributeType.PlayerSpeed:
                    _playerData.MoveSpeed.AddValueMultiple(data.Value);
                    break;
                case AttributeType.PlayerHeal:
                    this.HealPlayer(CalTool.Round(data.Value * this.PlayerMaxHealthPoint, 1));
                    break;
                case AttributeType.GetDropItemRadius:
                    _playerData.DropItemRadius.AddValuePoint(data.Value);
                    break;
                case AttributeType.RecoverShield:
                    this.RecoverShield((int)data.Value);
                    break;
                case AttributeType.ShootCount:
                    foreach (Weapon weapon in _weapon.GetWeapons())
                    {
                        weapon.weaponData.OneShootAmmoCount.AddValuePoint((int)data.Value);
                        weapon.ReloadWeapon();
                    }
                    break;
                case AttributeType.HealItemRate:
                    _extendHealItemRate += data.Value;
                    break;
            }
        }

        public void UpdateWeapon(WeaponOptionData data)
        {
            if (data.SelectedCount == 0)
            {
                _weapon.LoadWeapon(data.WeaponIndex, data.Image, true);
                this.ActiveWeapons.Add(data);
            }
            else
            {
                Weapon weapon = _weapon.GetWeapon(data.WeaponIndex);
                if (weapon == null)
                {
                    Debug.LogError("查無待升級的武器資料");
                    return;
                }
                int attuributeIndex = data.SelectedCount - 1;
                if (attuributeIndex > data.WeaponUpdateAttributes.Length)
                {
                    Debug.LogError("武器資料沒有足夠的升級選項", weapon.gameObject);
                    return;
                }
                WeaponAttribute weaponAttribute = data.WeaponUpdateAttributes[attuributeIndex];
                switch (weaponAttribute.AttributeType)
                {
                    case AttributeType.DamageMultiple:
                        Debug.Log($"調整武器傷害(Damage)");
                        weapon.weaponData.Damage.AddValueMultiple(weaponAttribute.Value);
                        break;
                    case AttributeType.DamageRadius:
                        Debug.Log($"調整武器傷害範圍(DamageRadius)");
                        weapon.weaponData.DamageRadius.AddValueMultiple(weaponAttribute.Value);
                        break;
                    case AttributeType.ShootCountPreSecond:
                        Debug.Log($"調整武器攻擊頻率(SkillTriggerInterval)");
                        Debug.Log($"調整武器攻擊頻率(CoolDownTime)");
                        weapon.weaponData.SkillTriggerInterval.AddValueMultiple(weaponAttribute.Value);
                        weapon.weaponData.CoolDownTime.AddValueMultiple(weaponAttribute.Value);
                        break;
                    case AttributeType.AmmoFlySpeed:
                        Debug.Log($"調整投射物的飛行速度(AmmoFlySpeed)");
                        weapon.weaponData.AmmoFlySpeed.AddValueMultiple(weaponAttribute.Value);
                        break;
                    case AttributeType.ShootCount:
                        Debug.Log($"調整投射物數量(OneShootAmmoCount)");
                        weapon.weaponData.OneShootAmmoCount.AddValuePoint((int)weaponAttribute.Value);
                        weapon.ReloadWeapon();
                        break;
                    case AttributeType.AmmoScale:
                        Debug.Log($"調整投射物大小(AmmoScale)");
                        weapon.weaponData.AmmoScale.AddValueMultiple(weaponAttribute.Value);
                        break;
                    case AttributeType.PenetrationCount:
                        Debug.Log($"調整投射物穿透數目(PenetrationCount)");
                        weapon.weaponData.AmmoPenetrationCount.AddValuePoint((int)weaponAttribute.Value);
                        break;
                    case AttributeType.SpecialOption:
                        Debug.Log($"武器特殊升級選項: {weapon.weaponData.WeaponIndex}");
                        if (weapon.weaponData.WeaponIndex == WeaponIndex.DroneA)
                        {
                            weapon.weaponData.BuffSpawnActive.ChangeValue(true);
                        }
                        else if(weapon.weaponData.WeaponIndex == WeaponIndex.TempGunAmmo)
                        {
                            weapon.weaponData.BuffSpawnActive.ChangeValue(true);
                        }
                        break;
                    case AttributeType.PushForce:
                        weapon.weaponData.Force.AddValueMultiple(weaponAttribute.Value);
                        break;
                    case AttributeType.BuffTime:
                        weapon.weaponData.BuffLifeTime.AddValuePoint(weaponAttribute.Value);
                        break;
                    case AttributeType.AmmoFlyTime:
                        weapon.weaponData.AmmoFlyTime.AddValuePoint(weaponAttribute.Value);
                        break;
                }
                weapon.ReloadWeapon();
            }
        }

        /// <summary>
        /// 取得大廳的升級內容並整併至遊戲內
        /// </summary>
        /// <param name="upgradeManager"></param>
        public void SetLobbyUpgrade(BaseUpgradeManager upgradeManager)
        {
            #region 人物數值調整
            // 分數獲取率
            _extendMoneyMuliple += upgradeManager.GetIncreaseMoneyQuantity();

            // 移動速度
            _playerData.MoveSpeed.AddValueMultiple(upgradeManager.GetMoveSpeed());

            // 自動回復
            _playerData.AutoRecoverPoint.AddValuePoint(upgradeManager.GetHPGeneratePerSec());

            // 經驗值獲取率
            _playerData.ExpRate.AddValueMultiple(upgradeManager.GetIncreaseExpQuantity());

            // 防禦力
            _playerData.DEF.AddValuePoint(upgradeManager.GetDefense());

            // 武器欄位
            _playerData.AddWeaponColumn(upgradeManager.GetIncreaseSkillSlot());

            // 增加拾取範圍
            _playerData.DropItemRadius.AddValueMultiple(upgradeManager.GetIncreasePickingArea());

            // 最大生命
            this.AddPlayerMaxHP(CalTool.Round(_playerData.MaxHealthPoint * upgradeManager.GetMaxHP(), 1));

            // 復活次數
            _playerData.ReviveTimes.AddValuePoint(upgradeManager.GetReviveTimes());
            #endregion

            #region 武器素質調整
            foreach (Weapon weapon in _weapon.GetWeapons())
            {
                // 力量
                weapon.weaponData.Damage.AddValueMultiple(upgradeManager.GetStrength());

                // 投射物大小
                weapon.weaponData.AmmoScale.AddValueMultiple(upgradeManager.GetProjectileSize());

                // 攻擊持續時間
                weapon.weaponData.AmmoFlyTime.AddValueMultiple(upgradeManager.GetAttackPersistTime());
                weapon.weaponData.BuffLifeTime.AddValueMultiple(upgradeManager.GetAttackPersistTime());

                // 投射物數量
                weapon.weaponData.OneShootAmmoCount.AddValuePoint(upgradeManager.GetProjetileNumber());

                // 攻擊範圍
                weapon.weaponData.DamageRadius.AddValueMultiple(upgradeManager.GetAttackRadius());

                // 冷卻時間
                weapon.weaponData.SkillTriggerInterval.AddValuePoint(upgradeManager.GetCoolDown() * -1f);

                // 投射物速度
                weapon.weaponData.AmmoFlySpeed.AddValueMultiple(upgradeManager.GetProjectileSpeed());
            }
            #endregion

        }

        #region 槍械
        /// <summary>
        /// 判斷是否需要掉落補品
        /// </summary>
        public bool NeedDropHealth { get => (_playerData.DropHealthRate) >= Random.value; }
        #endregion

        #region 經驗值
        /// <summary>
        /// 增加玩家的經驗直
        /// </summary>
        /// <param name="exp"></param>
        public void AddExp(float exp)
        {
            _playerData.NowExp += exp * ExpMultiple;
            _gameUI.UpdateExpGUI();
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
        private float ExpMultiple { get => _playerData.ExpRate.Value; }
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
        public float GetDropItemRadius { get => _playerData.DropItemRadius.Value; }
        /// <summary>
        /// 取得玩家受創時的擊退範圍
        /// </summary>
        public float PlayerRepelRadius { get => _playerData.Radius.Value; }
        /// <summary>
        /// 取得玩家受創時的擊退力道
        /// </summary>
        public float PlayerRepelForce { get => _playerData.Force.Value; }
        /// <summary>
        /// 取得玩家受創時的擊退時間
        /// </summary>
        public float PlayerRepelTime { get => _playerData.EnemyDelayTime.Value; }
        /// <summary>
        /// 取得或設定玩家的血量
        /// </summary>
        public float PlayerHealthPoint { get => CalTool.Round(_playerData.HealthPoint, 1); }
        /// <summary>
        /// 取得或設定玩家的最大血量
        /// </summary>
        public float PlayerMaxHealthPoint { get => CalTool.Round(_playerData.MaxHealthPoint + _extendMaxHealthPoint, 1); }

        /// <summary>
        /// 取得玩家的護盾值
        /// </summary>
        public int PlayerShield { get => (int)CalTool.Round(_playerData.Shield); }
        /// <summary>
        /// 恢復玩家的護盾值
        /// </summary>
        public void RecoverShield(int value)
        {
            _playerData.Shield += value;
            if (_playerData.Shield > this.PlayerMaxShield)
                _playerData.Shield = this.PlayerMaxShield;
            _gameUI.UpdatePlayerHealth();
        }
        /// <summary>
        /// 取得玩家的最大護盾值
        /// </summary>
        public int PlayerMaxShield { get => (int)CalTool.Round(_playerData.MaxShield); }
        /// <summary>
        /// 取得玩家的移動速度
        /// </summary>
        public float PlayerMoveSpeed
        {
            get => _playerData.MoveSpeed.Value;
        }
        /// <summary>
        /// 取得玩家的無敵時間
        /// </summary>
        public float InvincibleTime { get => _playerData.InvincibleTime; }
        /// <summary>
        /// 取得玩家的防禦力
        /// </summary>
        private float PlayerDEF { get => _playerData.DEF.Value; }
        /// <summary>
        /// 玩家受到傷害
        /// 優先扣除護盾
        /// </summary>
        /// <param name="damage"></param>
        public void PlayerGetDamage(float damage)
        {
            // 扣除防禦值
            damage = CalTool.CalDamage(damage, this.PlayerDEF);
            // 扣除
            if (_playerData.Shield > 0)
            {
                // 剩餘護盾值
                _playerData.Shield--;
                damage = 0f;
            }

            // 扣除血量
            if (damage > 0f)
                _playerData.HealthPoint -= damage;
            _gameUI.UpdatePlayerHealth();
        }

        /// <summary>
        /// 玩家接受治癒
        /// </summary>
        /// <param name="value">治癒量</param>
        public void HealPlayer(float value)
        {
            _playerData.HealthPoint += value;
            if (_playerData.HealthPoint > this.PlayerMaxHealthPoint)
                _playerData.HealthPoint = this.PlayerMaxHealthPoint;
            _gameUI.UpdatePlayerHealth();
        }

        /// <summary>
        /// 增加玩家最大血量
        /// </summary>
        /// <param name="value">增加量</param>
        public void AddPlayerMaxHP(float value)
        {
            _extendMaxHealthPoint += value;
            this.HealPlayer(value);
        }
        /// <summary>
        /// 玩家自動回復間隔
        /// </summary>
        public float PlayerAutoRecoverTime { get => _playerData.AutoRecoverTime.Value; }
        /// <summary>
        /// 玩家自動回復點數
        /// </summary>
        public float PlayerAutoRecoverPoint { get => CalTool.Round(_playerData.AutoRecoverPoint.Value, 1); }

        public void AddTotalMoney(float money)
        {
            _totalMoney += money * (1f + _extendMoneyMuliple);
            _gameUI.UpdateMoneyGUI();
        }

        public float TotalMoney { get => _totalMoney; }

        public void AddTotalKill(int kill = 1)
        {
            _totalKill += kill;
            this.AddTotalMoney(kill);
        }

        public int TotalKillCount { get => _totalKill; }

        /// <summary>
        /// 取得/設定遊戲時間
        /// </summary>
        public float GameTime { get => _gameTime; set => _gameTime = value; }
        /// <summary>
        /// 取得/設定遊戲總時間
        /// </summary>
        public float TotalGameTime { get; set; }

        public bool IsTimeWin { get => this.TotalGameTime - this.GameTime <= 0f; }

        public float GetExtendHealItemRate { get => _extendHealItemRate; }
        /// <summary>
        /// 有效武器欄位數量
        /// </summary>
        public int WeaponColumnActiveCount { get => _playerData.WeaponColumnActiveCount; }
        /// <summary>
        /// 最大武器欄位數量
        /// </summary>
        public int WeaponColumnMaxCount { get => _playerData.WeaponColumnMaxCount; }
        /// <summary>
        /// 取得生效的武器清單
        /// </summary>
        public IList<WeaponOptionData> ActiveWeapons { get => _activeWeapons; }
        public IList<WeaponIndex> ActiveWeaponIndexs 
        { 
            get
            {
                if(_activeWeaponIndexs.Count != _activeWeapons.Count)
                {
                    _activeWeaponIndexs.Clear();
                    foreach(WeaponOptionData weaponOption in _activeWeapons)
                    {
                        _activeWeaponIndexs.Add(weaponOption.WeaponIndex);
                    }
                }
                return _activeWeaponIndexs;
            } 
        }

        public IntAttributeHandle PlayerReviveTime { get => _playerData.ReviveTimes; }
        #endregion
    }
}
