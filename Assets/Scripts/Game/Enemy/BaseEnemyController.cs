using Scripts.Game.Base;
using Scripts.Game.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Scripts.Game
{
    public class BaseEnemyController : MonoBehaviour
    {
        protected Transform _playerTransform;
        protected Rigidbody2D _rigidbody2D;
        protected Collider2D _collider2D;
        protected Animator _animator;
        [SerializeField, Header("怪物資料")]
        private EnemyData _enemyData;
        protected EnemyData _cloneEnemyData;

        [SerializeField, Header("傷害文字預置物")]
        protected GameObject _damageTextPrefab;
        [SerializeField, Header("傷害文字持續時間")]
        protected float _textShowTime = 0.3f;

        protected string _enemyGetHit = "Hit";
        protected string _enemyDead = "Dead";
        protected string _enemyAttack = "Attack";
        protected float _velocityTime = 0f;
        protected float _force = 0f;
        protected float _t = 0.05f;
        protected Quaternion _lookLeft = Quaternion.Euler(0, 180, 0);
        protected Quaternion _lookRight = Quaternion.Euler(0, 0, 0);
        protected EnemyState _state;

        protected float targetX = 0f;
        protected float targetY = 0f;

        protected void Awake()
        {
            _animator = gameObject.GetComponent<Animator>();
            _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
            _collider2D = gameObject.GetComponent<Collider2D>();
            _playerTransform = GameObject.Find("PlayerContainer").GetComponent<Transform>();
            if (_enemyData != null)
                _cloneEnemyData = Object.Instantiate(_enemyData);
            else
                Debug.LogError("怪物未設定 EnemyData", this.gameObject);
        }

        protected virtual void Move()
        {
            _rigidbody2D.velocity = Vector2.ClampMagnitude(new Vector2(targetX, targetY), 1f) * (_velocityTime > 0 ? _force : _cloneEnemyData.MoveSpeed.Value);
            if (_velocityTime <= 0)
            {
                targetX = Mathf.Lerp(targetX, _playerTransform.position.x - transform.position.x, _t);
                targetY = Mathf.Lerp(targetY, _playerTransform.position.y - transform.position.y, _t);
            }
            else
            {
                _force -= Mathf.Clamp(_force, 0f, Time.deltaTime);
                _velocityTime -= Time.deltaTime;
                _rigidbody2D.velocity = Vector2.ClampMagnitude(new Vector2(targetX, targetY), 1f) * _force;
                if(_velocityTime <= 0)
                {
                    targetX = this.transform.position.x;
                    targetY = this.transform.position.y;
                    _velocityTime = 0f;
                    _force = 0f;
                }
            }

            this.LookAt(_playerTransform.position);
        }

        private void Update()
        {
            if (IsDead || GameStateMachine.Instance.CurrectState != GameState.InGame)
            {
                return;
            }
            Move();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (IsDead || GameStateMachine.Instance.CurrectState != GameState.InGame)
            {
                return;
            }

            if (collision.gameObject.layer == LayerMask.NameToLayer("玩家子彈"))
            {
                string ammoName = collision.gameObject.name.Split('_')[1];
                try
                {
                    ammoName = ammoName.Substring(0, ammoName.IndexOf("(Clone)"));
                    Weapon weapon = null;
                    foreach (Weapon w in GameObject.Find("Weapons").GetComponentsInChildren<Weapon>())
                    {
                        if (w.gameObject.name.Contains(ammoName))
                        {
                            weapon = w;
                        }
                    }
                    if (weapon != null)
                    {
                        //子彈擊退力量
                        float repelForce = weapon.weaponData.Force.Value;
                        //子彈擊退持續時間
                        float repelTime = weapon.weaponData.DelayTime.Value;
                        //子彈傷害
                        float damage = weapon.weaponData.Damage.Value;
                        this.TakeDamage(damage);
                        if(repelTime > 0f)
                            this.AddForce(repelForce, repelTime);
                    }
                }
                catch
                {
                    Debug.Log($"子彈名稱沒有包含 (clone), {ammoName}");
                }
            }
        }

        public void LookAt(Vector3 targetPosition)
        {
            if (transform.position.x > targetPosition.x)
            {
                transform.rotation = _lookLeft;
            }
            else
            {
                transform.rotation = _lookRight;
            }
        }

        /// <summary>
        /// 怪物受到傷害
        /// </summary>
        /// <param name="damage">傷害值</param>
        public virtual void TakeDamage(float damage)
        {
            AttributeHandle.Instance.AddTotalDamage(damage);

            _cloneEnemyData.HealthPoint -= damage;
            if (_cloneEnemyData.HealthPoint <= 0)
            {
                _state = EnemyState.Dead;
                _cloneEnemyData.HealthPoint = 0;
                Dead();
            }
            this.ShowDamageText(damage);
        }
        /// <summary>
        /// 施加擊退力量
        /// </summary>
        /// <param name="force"></param>
        /// <param name="time"></param>
        protected virtual void AddForce(float force = 0f, float time = 0f)
        {
            targetX = transform.position.x - _playerTransform.position.x;
            targetY = transform.position.y - _playerTransform.position.y;
            _rigidbody2D.velocity = Vector2.ClampMagnitude(new Vector2(targetX, targetY), 1f) * force;
            _velocityTime = time;
            _force = force;
        }

        /// <summary>
        /// 怪物死亡
        /// </summary>
        protected virtual void Dead()
        {
            AttributeHandle.Instance.AddTotalKill();
            _rigidbody2D.velocity = Vector2.zero;
            _animator.SetTrigger(_enemyDead);
            _collider2D.enabled = false;
            StartCoroutine(DestroyEnemy());
        }

        protected IEnumerator DestroyEnemy()
        {
            yield return new WaitForSeconds(1f);
            Destroy(gameObject);
        }

        /// <summary>
        /// 顯示傷害文字
        /// </summary>
        /// <param name="damage"></param>
        protected virtual void ShowDamageText(float damage)
        {
            GameObject damageText = GameObject.Instantiate(_damageTextPrefab);
            damageText.transform.position = this.transform.position + new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), 0f);
            damageText.GetComponentInChildren<TextMeshProUGUI>().text = Mathf.FloorToInt(damage).ToString();
            StartCoroutine(DestroyDamageText(damageText));
        }
        /// <summary>
        /// 移除傷害文字
        /// </summary>
        /// <param name="damageText"></param>
        /// <returns></returns>
        private IEnumerator DestroyDamageText(GameObject damageText)
        {
            yield return new WaitForSeconds(_textShowTime);
            Destroy(damageText);
        }

        public void AddVelocityTime(float dealyTime)
        {
            _velocityTime += dealyTime;
        }

        public void PlayAttackAnimation()
        {
            if (_cloneEnemyData.AttackRange.Value > 0 && _state == EnemyState.Run)
            {
                _animator.SetTrigger(_enemyAttack);
                _state = EnemyState.Attack;
            }
        }

        public bool IsDead { get => _cloneEnemyData.HealthPoint <= 0; }

        #region DI 設定
        public EnemyData EnemyData { get => _cloneEnemyData; }
        #endregion
    }
}