using Scripts.Game.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Game
{
    public class AmmoController : MonoBehaviour, IAmmoController
    {
        private int _nowPenetrationCount = 0;
        private IGameFiniteStateMachine _gameFiniteStateMachine;
        private IAttributeHandle _attributeHandle;
        private IPlayerController _player;
        private IEndUIController _endUI;
        private bool isFirstHit = true;

        private void OnEnable()
        {
            _nowPenetrationCount = 0;
            isFirstHit = true;
        }

        private void Update()
        {
            if (_gameFiniteStateMachine.CurrectState == GameState.InGame)
            {
                AutoInactive();
            }
        }

        private void AutoInactive()
        {
            if ((this.transform.position - _player.GetTransform.position).magnitude > _attributeHandle.GunEffectiveRange)
            {
                gameObject.SetActive(false);
            }
        }

        public void HitEmeny()
        {
            _nowPenetrationCount += 1;
            if(_nowPenetrationCount >= _attributeHandle.AmmoPenetrationCount)
            {
                this.gameObject.SetActive(false);
            }
            if (isFirstHit)
            {
                _endUI.AddHitEnemyCount(AmmoGroup);
                isFirstHit = false;
            }
        }

        public bool IsActive { get => _nowPenetrationCount < _attributeHandle.AmmoPenetrationCount; }
        public IGameFiniteStateMachine SetGameFiniteStateMachine { set => _gameFiniteStateMachine = value; }
        public IAttributeHandle SetAttributeHandle { set => _attributeHandle = value; }
        public IPlayerController SetPlayer { set => _player = value; }
        public IEndUIController SetEndUI { set => _endUI = value; }
        public int AmmoGroup { get; set; }
    }
}
