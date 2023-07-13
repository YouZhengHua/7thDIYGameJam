using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Game
{
    /// <summary>
    /// 移動控制器
    /// 賦予此 Script 的物件可以被玩家透過上下左右操控移動
    /// </summary>
    public class MoveController : MonoBehaviour, IMoveController
    {
        /// <summary>
        /// 屬性處理器
        /// </summary>
        private IAttributeHandle _attributeHandle;
        [SerializeField, Header("動畫控制器")]
        private Animator animator;
        /// <summary>
        /// 物理碰撞物件
        /// </summary>
        private new Rigidbody2D rigidbody;

        #region 玩家轉向參數
        [SerializeField, Header("玩家面對方向的物件")]
        private Transform zRotation;
        /// <summary>
        /// 玩家目前面對的角度
        /// </summary>
        private Quaternion selfQuaternion = Quaternion.identity;
        [SerializeField, Range(0.01f, 1f), Header("玩家轉向速度"), Tooltip("數值越小玩家轉向越慢")]
        private float spinningSpeed = 0.2f;
        #endregion

        #region 移動參數
        private float targetAd = 0f;
        private float targetWs = 0f;
        [SerializeField, Range(1f, 20f), Header("玩家移動延遲倍率"), Tooltip("數值越小玩家移動延遲越高")]
        private float deltaRate = 12f;
        #endregion

        private void Start()
        {
            rigidbody = this.GetComponent<Rigidbody2D>();
            selfQuaternion = this.zRotation.rotation;
        }

        private void Update()
        {
            // 遊戲進行中才能進行移動
            if(GameStateMachine.Instance.CurrectState == GameState.InGame)
            {
                float inputAd = Input.GetAxisRaw("Horizontal");
                float inputWs = Input.GetAxisRaw("Vertical");

                targetAd = Mathf.MoveTowards(targetAd, inputAd, Time.deltaTime * deltaRate);
                targetWs = Mathf.MoveTowards(targetWs, inputWs, Time.deltaTime * deltaRate);

                Vector2 moveVelocity = Vector2.ClampMagnitude(new Vector2(targetAd, targetWs), 1f);
                moveVelocity *= moveSpeed;
                rigidbody.velocity = moveVelocity;
                if(animator != null)
                {
                    animator.SetFloat("ws", targetWs);
                    animator.SetFloat("ad", targetAd);
                }

                if (inputAd != 0f || inputWs != 0f)
                {
                    zRotation.up = new Vector3(targetAd, targetWs, 0f);
                    selfQuaternion = Quaternion.Lerp(selfQuaternion, zRotation.rotation, spinningSpeed);
                    zRotation.rotation = selfQuaternion;
                }
            }
            else
            {
                //不在遊戲中，移動速度歸零
                rigidbody.velocity = Vector2.zero;
                if (animator != null)
                {
                    animator.SetFloat("ws", 0f);
                    animator.SetFloat("ad", 0f);
                }
            }
        }

        private float moveSpeed { get => _attributeHandle.PlayerMoveSpeed; }

        public IAttributeHandle SetAttributeHandle { set => _attributeHandle = value; }
    }
}