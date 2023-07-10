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

        private float _extendDropHealthMultiple = 0f;

        private float _playerSpeedMultiple = 0f;
        private float _extendDEF = 0f;
        private float _totalExp = 0f;
        private float _expExtendMultiple = 0f;
        private float _extendGetItemRadius = 0f;

        public AttributeHandle(IGameFiniteStateMachine gameFiniteStateMachine, PlayerData playerData)
        {
            _gameFiniteStateMachine = gameFiniteStateMachine;
            _playerData = playerData;
        }

        public void UpdateAttribute(OptionData data)
        {
            switch (data.optionAttribute)
            {
                case OptionAttribute.PlayerHeal:
                    _gameUI.HealPlayer(data.Value * _playerData.MaxHealthPoint);
                    break;
                case OptionAttribute.PlayerMaxHealth:
                    _gameUI.AddPlayerHealthPointMax(Mathf.RoundToInt(data.Value));
                    break;
                case OptionAttribute.PlayerSpeed:
                    _playerSpeedMultiple += data.Value;
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
        #endregion
    }
}
