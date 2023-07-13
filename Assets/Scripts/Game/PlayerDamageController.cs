using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Game
{
    /// <summary>
    /// 玩家受傷控制器
    /// </summary>
    public class PlayerDamageController : MonoBehaviour, IPlayerDamageController
    {
        /// <summary>
        /// 屬性處理器
        /// </summary>
        private IAttributeHandle _attributeHandle;
        /// <summary>
        /// 結算UI控制器
        /// </summary>
        private IEndUIController _endUI;
        /// <summary>
        /// 遊戲UI控制器
        /// </summary>
        private IGameUIController _gameUI;
        /// <summary>
        /// 音效控制器
        /// </summary>
        private IAudioContoller _audio;

        [SerializeField, Header("目標判定階層")]
        private LayerMask enemyLayer;
        [SerializeField, Header("動畫控制器")]
        private Animator playerAni;

        /// <summary>
        /// 全域空間動畫控制器
        /// 用於玩家受到傷害使鏡頭泛紅
        /// </summary>
        [SerializeField, Header("全域空間動畫控制器"), Tooltip("用於玩家受到傷害使鏡頭泛紅")]
        private Animator globalVolumeAni;
        [SerializeField, Header("玩家受傷時的動畫名稱")]
        private string playerGetHitTriggerName = "Hit";
        [SerializeField, Header("玩家死亡時的動畫名稱")]
        private string playerDeadBoolName = "Dead";

        /// <summary>
        /// 玩家無敵時間
        /// </summary>
        private float _invincibleTime;

        private void Update()
        {
            if(_invincibleTime > 0f)
            {
                _invincibleTime -= Time.deltaTime;
                if (_invincibleTime < 0f)
                    _invincibleTime = 0f;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if ((((1 << collision.gameObject.layer) & enemyLayer) != 0) && _invincibleTime <= 0)
            {
                IEnemyController collisionEnemy = collision.gameObject.GetComponent<IEnemyController>();
                if (collisionEnemy != null && !collisionEnemy.IsDead)
                {
                    GetDamage((int)collisionEnemy.EnemyDamage);
                    playerAni.SetTrigger(playerGetHitTriggerName);
                    globalVolumeAni.SetTrigger(playerGetHitTriggerName);
                    collision.gameObject.GetComponent<IEnemyController>().PlayAttackAnimation();
                    foreach (var enemy in Physics2D.OverlapCircleAll(this.transform.position, _attributeHandle.PlayerRepelRadius, enemyLayer))
                    {
                        Vector2 distance = enemy.transform.position - this.transform.position;
                        if (distance.magnitude < _attributeHandle.PlayerRepelRadius)
                        {
                            enemy.GetComponent<Rigidbody2D>().AddForce(distance.normalized * (_attributeHandle.PlayerRepelRadius - distance.magnitude) * _attributeHandle.PlayerRepelForce);
                            enemy.GetComponent<IEnemyController>().AddVelocityTime(_attributeHandle.PlayerRepelTime);
                        }
                    }
                    _endUI.AddGetHitTimes();
                    _gameUI.UpdatePlayerHealth();
                }
            }
        }

        /// <summary>
        /// 受到傷害
        /// </summary>
        /// <param name="damage"></param>
        public void GetDamage(int damage)
        {
            float calDamage = CalTool.CalDamage(damage, _attributeHandle.PlayerDEF, 1f);
            _attributeHandle.PlayerHealthPoint -= calDamage;
            if (_attributeHandle.PlayerHealthPoint <= 0)
            {
                _attributeHandle.PlayerHealthPoint = 0;
                Dead();
            }
            else
            {
                _audio.PlayEffect(_attributeHandle.GetHitAudio);
            }
            _invincibleTime = _attributeHandle.InvincibleTime;
        }

        /// <summary>
        /// 死亡處理
        /// </summary>
        public void Dead()
        {
            playerAni.SetBool(playerDeadBoolName, true);
            gameObject.SetActive(false);
            GameStateMachine.Instance.SetNextState(GameState.GameEnd);
        }

        public IAttributeHandle SetAttributeHandle { set => _attributeHandle = value; }
        public IGameUIController SetGameUI { set => _gameUI = value; }
        public IEndUIController SetEndUI { set => _endUI = value; }
        public IAudioContoller SetAudio { set => _audio = value; }
    }
}
