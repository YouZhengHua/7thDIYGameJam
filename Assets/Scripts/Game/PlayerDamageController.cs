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
        /// 結算UI控制器
        /// </summary>
        private IEndUIController _endUI;

        [SerializeField, Header("傷害判定階層")]
        private LayerMask _damageLayer;
        [SerializeField, Header("怪物判定階層")]
        private LayerMask _enemyLayer;
        [SerializeField, Header("動畫控制器")]
        private Animator _playerAni;

        /// <summary>
        /// 全域空間動畫控制器
        /// 用於玩家受到傷害使鏡頭泛紅
        /// </summary>
        [SerializeField, Header("全域空間動畫控制器"), Tooltip("用於玩家受到傷害使鏡頭泛紅")]
        private Animator _globalVolumeAni;
        [SerializeField, Header("玩家受傷時的動畫名稱")]
        private string _playerGetHitTriggerName = "Hit";
        [SerializeField, Header("玩家死亡時的動畫名稱")]
        private string _playerDeadBoolName = "Dead";

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
            if ((((1 << collision.gameObject.layer) | _damageLayer) == _damageLayer) && _invincibleTime <= 0)
            {
                Debug.Log("玩家遭受碰撞", collision.gameObject);
                BaseEnemyController collisionEnemy = collision.gameObject.GetComponent<BaseEnemyController>();
                if (collisionEnemy == null || collisionEnemy.IsDead)
                    return;
                GetDamage(collisionEnemy.EnemyData.Damage.Value);
                RepleEnemy();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if ((((1 << collision.gameObject.layer) | _damageLayer) == _damageLayer) && _invincibleTime <= 0)
            {
                Debug.Log("玩家遭受碰撞", collision.gameObject);
                BulletController bullet = collision.gameObject.GetComponent<BulletController>();
                if (bullet == null)
                    return;
                GetDamage(bullet.Damage);
                RepleEnemy();
            }
        }

        private void RepleEnemy()
        {
            foreach (var enemy in Physics2D.OverlapCircleAll(this.transform.position, AttributeHandle.Instance.PlayerRepelRadius, _enemyLayer))
            {
                Vector2 distance = enemy.transform.position - this.transform.position;
                if (distance.magnitude < AttributeHandle.Instance.PlayerRepelRadius)
                {
                    enemy.GetComponent<BaseEnemyController>().AddForce(AttributeHandle.Instance.PlayerRepelForce, AttributeHandle.Instance.PlayerRepelTime);
                }
            }
        }

        /// <summary>
        /// 受到傷害
        /// </summary>
        /// <param name="damage"></param>
        public void GetDamage(float damage)
        {
            AttributeHandle.Instance.PlayerGetDamage(damage);
            _playerAni.SetTrigger(_playerGetHitTriggerName);
            _globalVolumeAni.SetTrigger(_playerGetHitTriggerName);
            _endUI.AddGetHitTimes();
            if (AttributeHandle.Instance.PlayerHealthPoint <= 0)
            {
                Dead();
            }
            else
            {
                AudioController.Instance.PlayEffect(AttributeHandle.Instance.GetHitAudio);
            }
            _invincibleTime = AttributeHandle.Instance.InvincibleTime;
        }

        /// <summary>
        /// 死亡處理
        /// </summary>
        public void Dead()
        {
            _playerAni.SetBool(_playerDeadBoolName, true);
            gameObject.SetActive(false);
            GameStateMachine.Instance.SetNextState(GameState.GameEnd);
        }
        public IEndUIController SetEndUI { set => _endUI = value; }
    }
}
