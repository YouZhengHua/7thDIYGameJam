using Scripts.Game.Base;
using Scripts.Game.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Game
{
    public class DropController : MonoBehaviour, IDropController
    {
        protected Transform playerTransform;
        protected DropItemData _dropItemData;
        private bool isGot = false;

        protected virtual void OnEnable()
        {
            isGot = false;
        }
        protected virtual void Update()
        {
            if (GameStateMachine.Instance.CurrectState == GameState.InGame)
            {
                isGot = isGot || (playerTransform.position - transform.position).magnitude < AttributeHandle.Instance.GetDropItemRadius;
                if (isGot)
                {
                    MoveDropItem();
                }
                if ((playerTransform.position - transform.position).magnitude <= 0)
                {
                    GetDropItem();
                }
            }
        }

        protected virtual void MoveDropItem()
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, this.ItemMoveSpeed * Time.deltaTime);
        }

        protected virtual void GetDropItem()
        {
            gameObject.SetActive(false);
        }

        private float ItemMoveSpeed { get => _dropItemData == null ? 10f : _dropItemData.speed; }

        public Transform SetPlayerTransform { set => playerTransform = value; }
        protected DropItemData SetDropItemData { set => _dropItemData = value; }
    }
}