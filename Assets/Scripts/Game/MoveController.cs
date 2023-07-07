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
        /// 遊戲狀態機
        /// </summary>
        private IGameFiniteStateMachine _gameFiniteStateMachine;
        /// <summary>
        /// 屬性處理器
        /// </summary>
        private IAttributeHandle _attributeHandle;
        [SerializeField, Header("動畫控制器")]
        private Animator animator;
        private new Rigidbody2D rigidbody;

        private float targetAd = 0f;
        private float targetWs = 0f;
        private float deltaRate = 12f;

        private void Start()
        {
            rigidbody = this.GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            // 確認有狀態機才執行移動判定
            if(_gameFiniteStateMachine != null)
            {
                // 遊戲進行中才能進行移動
                if(_gameFiniteStateMachine.CurrectState == GameState.InGame)
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
        }

        private float moveSpeed { get => _attributeHandle.PlayerMoveSpeed; }

        /// <summary>
        /// 設定遊戲狀態機
        /// </summary>
        public IGameFiniteStateMachine SetGameFiniteStateMachine { set => _gameFiniteStateMachine = value; }
        public IAttributeHandle SetAttributeHandle { set => _attributeHandle = value; }
    }
}