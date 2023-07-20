using Scripts.Game.Base;
using Scripts.Game.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Scripts.Game
{
    /// <summary>
    /// 遠程怪物控制器
    /// </summary>
    public class RangeEnemyController : BaseEnemyController
    {
        [SerializeField, Header("怪物子彈")]
        protected GameObject _bullet;
        [SerializeField, Header("子彈飛行速度")]
        protected float _flySpeed = 5f;
        [SerializeField, Header("遠程攻擊的間隔時間")]
        protected float _cooldownTime = 0f;
        protected float _nowAttackTime = 0f;

        /// <summary>
        /// <para>遠程怪物移動機制</para>
        /// <para>穩定朝向玩家移動至可攻擊範圍後，停下不移動並進行射擊</para>
        /// </summary>
        protected override void Move()
        {
            base.Move();
            if((_playerTransform.position - transform.position).magnitude <= _cloneEnemyData.AttackRange.Value)
            {
                _rigidbody2D.velocity = Vector2.zero;
            }
        }

        protected override void Attack()
        {
            if (_nowAttackTime <= 0)
            {
                _nowAttackTime = _cooldownTime;
                base.Attack();
                if (_bullet == null)
                {
                    Debug.LogWarning("遠程怪物未設定子彈預置物", gameObject);
                    return;
                }
                GameObject bullet = GameObject.Instantiate(_bullet);
                bullet.transform.position = this.transform.position;
                bullet.GetComponent<BulletController>().Init(this.transform.position, _playerTransform.position, _flySpeed, 1, EnemyData.Damage.Value);
            }
            else
            {
                _nowAttackTime -= Time.deltaTime;
            }
        }

        public override EnemyData EnemyData => base.EnemyData;
    }
}