using Scripts.Game.Base;
using Scripts.Game.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Game
{
    public class DropController : MonoBehaviour, IDropController
    {
        protected IPlayerController _player;
        protected IAttributeHandle _attributeHandle;
        protected IGameFiniteStateMachine _gameFiniteStateMachine;
        protected DropItemData _dropItemData;
        private bool isGot = false;

        protected virtual void OnEnable()
        {
            isGot = false;
        }
        protected virtual void Update()
        {
            if (_gameFiniteStateMachine.CurrectState == GameState.InGame)
            {
                isGot = isGot || (_player.GetTransform.position - transform.position).magnitude < _attributeHandle.GetDropItemRadius;
                if (isGot)
                {
                    MoveDropItem();
                }
                if ((_player.GetTransform.position - transform.position).magnitude <= 0)
                {
                    GetDropItem();
                }
            }
        }

        protected virtual void MoveDropItem()
        {
            transform.position = Vector3.MoveTowards(transform.position, _player.GetTransform.position, this.ItemMoveSpeed * Time.deltaTime);
        }

        protected virtual void GetDropItem()
        {
            gameObject.SetActive(false);
        }

        private float ItemMoveSpeed { get => _dropItemData == null ? 10f : _dropItemData.speed; }

        public IGameFiniteStateMachine SetGameFiniteStateMachine { set => _gameFiniteStateMachine = value; }
        public IAttributeHandle SetAttributeHandle { set => _attributeHandle = value; }
        public IPlayerController SetPlayerController { set => _player = value; }
        protected DropItemData SetDropItemData { set => _dropItemData = value; }
    }
}