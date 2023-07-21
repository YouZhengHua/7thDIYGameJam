using Scripts.Game.Base;
using Scripts.Game.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Game
{
    public class DropController : MonoBehaviour
    {
        protected Transform _playerContainer;
        protected DropItemData _dropItemData;
        private bool isGot = false;
        protected virtual void Awake()
        {
            _playerContainer = GameObject.Find("PlayerContainer").GetComponent<Transform>();
        }
        protected virtual void OnEnable()
        {
            isGot = false;
        }
        protected virtual void Update()
        {
            if (GameStateMachine.Instance.CurrectState == GameState.InGame)
            {
                isGot = isGot || (_playerContainer.position - transform.position).magnitude < AttributeHandle.Instance.GetDropItemRadius;
                if (isGot)
                {
                    MoveDropItem();
                }
                if ((_playerContainer.position - transform.position).magnitude <= 0)
                {
                    GetDropItem();
                }
            }
        }

        protected virtual void MoveDropItem()
        {
            transform.position = Vector3.MoveTowards(transform.position, _playerContainer.position, this.FlySpeed * Time.deltaTime);
        }

        protected virtual void GetDropItem()
        {
            Destroy(this.gameObject);
        }

        private float FlySpeed { get => _dropItemData == null ? 10f : _dropItemData.FlySpeed; }
    }
}