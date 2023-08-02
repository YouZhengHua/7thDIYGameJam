using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Game
{
    /// <summary>
    /// 玩家受傷控制器
    /// </summary>
    public class PlayerDamageController : MonoBehaviour
    {
        private IWeaponController _weapon;

        [SerializeField, Header("怪物判定階層")]
        private LayerMask _enemyLayer;
        [SerializeField, Header("怪物子彈判定階層")]
        private LayerMask _bulletLayer;
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

        private void Awake()
        {
            _weapon = GetComponent<IWeaponController>();
        }

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
            if ((((1 << collision.gameObject.layer) | _enemyLayer) == _enemyLayer) && _invincibleTime <= 0)
            {
                Debug.Log("玩家遭受碰撞", collision.gameObject);
                BaseEnemyController collisionEnemy = collision.gameObject.GetComponent<BaseEnemyController>();
                if (collisionEnemy == null || collisionEnemy.IsDead)
                    return;
                GetDamage(collisionEnemy.EnemyData.Damage.Value);
                RepleEnemy(AttributeHandle.Instance.PlayerRepelRadius, AttributeHandle.Instance.PlayerRepelForce, AttributeHandle.Instance.PlayerRepelTime);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if ((((1 << collision.gameObject.layer) | _bulletLayer) == _bulletLayer) && _invincibleTime <= 0)
            {
                Debug.Log("玩家遭受碰撞", collision.gameObject);
                BulletController bullet = collision.gameObject.GetComponent<BulletController>();
                if (bullet == null)
                    return;
                GetDamage(bullet.Damage);
                RepleEnemy(AttributeHandle.Instance.PlayerRepelRadius, AttributeHandle.Instance.PlayerRepelForce, AttributeHandle.Instance.PlayerRepelTime);
            }
        }

        private void RepleEnemy(float radius, float repelForce, float repelTime)
        {
            foreach (var enemy in Physics2D.OverlapCircleAll(this.transform.position, radius, _enemyLayer))
            {
                enemy.GetComponent<BaseEnemyController>().AddForce(repelForce, repelTime);
            }
        }

        /// <summary>
        /// 受到傷害
        /// </summary>
        /// <param name="damage"></param>
        public void GetDamage(float damage)
        {
            if (_invincibleTime > 0f)
                return;
            AttributeHandle.Instance.PlayerGetDamage(damage);
            _playerAni.SetTrigger(_playerGetHitTriggerName);
            _globalVolumeAni.SetTrigger(_playerGetHitTriggerName);
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
            GetComponent<Collider2D>().enabled = false;
            foreach(Weapon weapon in _weapon.ActiveWeapons)
            {
                weapon.SetWeaponActive(false);
            }
            if(AttributeHandle.Instance.PlayerReviveTime.Value > 0)
            {
                PlayerStateMachine.Instance.SetNextState(PlayerState.Revive);
                StartCoroutine(Riveve());
            }
            else
            {
                PlayerStateMachine.Instance.SetNextState(PlayerState.Die);
                GameStateMachine.Instance.SetNextState(GameState.GameEnd);
            }
        }

        private IEnumerator Riveve()
        {
            yield return new WaitForSeconds(2f);
            AttributeHandle.Instance.HealPlayer(AttributeHandle.Instance.PlayerMaxHealthPoint);
            RepleEnemy(5f, 6f, 1.5f);
            ClearBuller(5f);
            PlayerStateMachine.Instance.SetNextState(PlayerState.Life);
            _playerAni.SetBool(_playerDeadBoolName, false);
            GetComponent<Collider2D>().enabled = true;
            AttributeHandle.Instance.PlayerReviveTime.AddValuePoint(-1);
            foreach (Weapon weapon in _weapon.ActiveWeapons)
            {
                weapon.SetWeaponActive(true);
            }
        }

        public void ClearBuller(float radius)
        {
            foreach (var bullet in Physics2D.OverlapCircleAll(this.transform.position, radius, _bulletLayer))
            {
                Destroy(bullet.gameObject);
            }
        }
    }
}
