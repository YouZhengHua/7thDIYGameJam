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
        private IGameFiniteStateMachine _gameFiniteStateMachine;
        private IGameUIController _gameUI;
        private PlayerData _playerData;
        private GunData _gunData;

        private float _gunDamagePoint = 0f;
        private float _gunDamageMultiple = 0f;
        private float _gunShootCountPreSecondMultiple = 0f;
        private float _gunExtendEffectiveRangeMultiple = 0f;
        private int _gunExtendShootAmmoCount = 0;
        private int _gunExtendPenetrationCount = 0;
        private float _extendDropHealthMultiple = 0f;

        private float _playerSpeedMultiple = 0f;
        private float _totalExp = 0f;
        private float _expExtendMultiple = 0f;
        private float _extendGetItemRadius = 0f;

        private float _nowOffset;
        private float _nowRecovetTime;

        public AttributeHandle(IGameFiniteStateMachine gameFiniteStateMachine, PlayerData playerData, GunData gunData)
        {
            _gameFiniteStateMachine = gameFiniteStateMachine;
            _playerData = playerData;
            _gunData = gunData;
        }

        public void UpdateAttribute(OptionData data)
        {
            switch (data.optionAttribute)
            {
                case OptionAttribute.GunDamage:
                    _gunDamagePoint += data.Value;
                    break;
                case OptionAttribute.GunDamageMultiple:
                    _gunDamageMultiple += data.Value;
                    break;
                case OptionAttribute.GunShootCountPreSecond:
                    _gunShootCountPreSecondMultiple += data.Value;
                    break;
                case OptionAttribute.GunReloadTime:
                    break;
                case OptionAttribute.GunPenetrationCount:
                    _gunExtendPenetrationCount += Mathf.RoundToInt(data.Value);
                    break;
                case OptionAttribute.PlayerHeal:
                    _gameUI.HealPlayer(data.Value * _playerData.MaxHealthPoint);
                    break;
                case OptionAttribute.PlayerMaxHealth:
                    _gameUI.AddPlayerHealthPointMax(Mathf.RoundToInt(data.Value));
                    break;
                case OptionAttribute.PlayerSpeed:
                    _playerSpeedMultiple += data.Value;
                    break;
                case OptionAttribute.AmmoCount:
                    break;
                case OptionAttribute.MagazineSize:
                    break;
                case OptionAttribute.ExtendExp:
                    _expExtendMultiple += data.Value;
                    break;
                case OptionAttribute.GetDropItemRadius:
                    _extendGetItemRadius += data.Value;
                    break;
                case OptionAttribute.AddScore:
                    _totalExp += data.Value;
                    break;
                case OptionAttribute.AddShootAmmoCount:
                    _gunExtendShootAmmoCount += (int)data.Value;
                    break;
            }
        }

        public IGameUIController SetGameUI { set => _gameUI = value; }

        #region 槍械
        public int OneShootAmmoCount { get => _gunData.OneShootAmmoCount + _gunExtendShootAmmoCount; }
        public float AmmoFlySpeed { get => _gunData.AmmoFlySpeed; }
        public float ShootCooldownTime { get => 1f / (_gunData.ShootCountPreSecond * (1f + _gunShootCountPreSecondMultiple)); }
        public float GunDamage { get => (_gunData.Damage + _gunDamagePoint) * (1f + _gunDamageMultiple); }
        public float GunRepelForce { get => _gunData.Force; }
        public float GunRepelTime { get => _gunData.EnemyDelayTime; }
        public int AmmoPenetrationCount { get => _gunData.AmmoPenetrationCount + _gunExtendPenetrationCount; }
        public float GunEffectiveRange { get => _gunData.EffectiveRange * (1 + _gunExtendEffectiveRangeMultiple); }
        public AudioClip ShootAudio { get => _gunData.ShootAudio; }
        public AudioClip ReloadAudio { get => _gunData.ReloadAudio; }
        public AudioClip LoadedAudio { get => _gunData.LoadedAudio; }
        public AudioClip NoAmmoAudio { get => _gunData.NoAmmoAudio; }
        public AudioClip ChangeShootTypeAudio { get => _gunData.ChangeShootType; }
        public bool NeedDropHealth { get => (_playerData.DropHealthRate + _extendDropHealthMultiple) >= Random.value; }
        public string FirePointName { get => _gunData.FirePointName; }
        public Quaternion ShootOffset { get => Quaternion.Euler(0, 0, this.OffsetAngle); }
        public void RecoverOffset(float recoverTime)
        {
            
            if (_nowRecovetTime > 0f)
            {
                _nowOffset = Mathf.Min(_nowOffset - (_nowOffset * Mathf.Min(recoverTime / _nowRecovetTime, 1f)), 0f);
                _nowRecovetTime -= recoverTime;

                _nowOffset = 0f;
                _nowRecovetTime = 0f;
            }
        }
        public void AddOffset()
        {
            _nowOffset = Mathf.Min(_nowOffset + _gunData.OffsetAddCount, _gunData.MaxOffset);
            _nowRecovetTime = _gunData.OffsetRecoverTime;
        }
        private float OffsetAngle
        {
            get
            {
                if(_gunData.OneShootAmmoCount > 1)
                    return Random.Range(_gunData.MaxOffset * -1, _gunData.MaxOffset);
                if (_nowOffset <= 0f)
                    return 0;
                return Random.Range(_nowOffset * -1, _nowOffset);
            }
        }
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
        public float PlayerHealthPoint { get => _playerData.HealthPoint; set => _playerData.HealthPoint = value; }
        public float PlayerMaxHealthPoint { get => _playerData.MaxHealthPoint; set => _playerData.MaxHealthPoint = value; }
        public float PlayerMoveSpeed
        {
            get => _playerData.BaseMoveSpeed * (_playerData.MoveSpeedRate + _playerSpeedMultiple) * _gunData.NormalSpeedRate 
                        * (_gameFiniteStateMachine.PlayerState == PlayerState.Shoot ? _gunData.ShootSpeedRate
                        : _gameFiniteStateMachine.PlayerState == PlayerState.Reload ? _gunData.ReloadSpeedRate
                        : 1f);
        }
        public float InvincibleTime { get => _playerData.InvincibleTime; }
        #endregion
    }
}
