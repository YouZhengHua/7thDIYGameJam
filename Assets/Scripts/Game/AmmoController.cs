using Scripts.Game.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Game
{
    public class AmmoController : MonoBehaviour, IAmmoController
    {
        private int _nowPenetrationCount = 0;
        private IAttributeHandle _attributeHandle;
        private Transform playerTransform;
        private IEndUIController _endUI;
        private bool isFirstHit = true;
        
        /// <summary>
        /// 槍械有效射程
        /// </summary>
        private float _gunEffectiveRange = 15f;

        /// <summary>
        /// 子彈可穿透數量
        /// </summary>
        private int ammoPenetrationCount = 1;

        private void OnEnable()
        {
            _nowPenetrationCount = 0;
            isFirstHit = true;
        }

        private void Update()
        {
            if (GameStateMachine.Instance.CurrectState == GameState.InGame)
            {
                AutoInactive();
            }
        }

        private void AutoInactive()
        {
            if ((this.transform.position - playerTransform.position).magnitude > _gunEffectiveRange)
            {
                gameObject.SetActive(false);
            }
        }

        public void HitEmeny()
        {
            _nowPenetrationCount += 1;
            if(_nowPenetrationCount >= ammoPenetrationCount)
            {
                this.gameObject.SetActive(false);
            }
            if (isFirstHit)
            {
                _endUI.AddHitEnemyCount(AmmoGroup);
                isFirstHit = false;
            }
        }

        public bool IsActive { get => _nowPenetrationCount < ammoPenetrationCount; }
        public IAttributeHandle SetAttributeHandle { set => _attributeHandle = value; }
        public Transform SetPlayerTransform { set => playerTransform = value; }
        public IEndUIController SetEndUI { set => _endUI = value; }
        public int AmmoGroup { get; set; }
    }
}
