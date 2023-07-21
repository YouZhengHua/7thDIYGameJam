using Scripts.Game.Base;
using Scripts.Game.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

namespace Scripts.Game
{
    /// <summary>
    /// 基礎怪物控制器
    /// </summary>
    public abstract class BaseEnemyController : MonoBehaviour
    {
        #region Component
        protected Transform _playerTransform;
        protected Transform _enemyContainer;
        protected Rigidbody2D _rigidbody2D;
        protected Collider2D _collider2D;
        protected Animator _animator;
        #endregion

        #region 怪物資料
        [SerializeField, Header("怪物資料")]
        protected EnemyData _enemyData;
        protected EnemyData _cloneEnemyData;
        #endregion

        #region 傷害文字相關
        [SerializeField, Header("傷害文字預置物")]
        protected GameObject _damageTextPrefab;
        [SerializeField, Header("傷害文字持續時間")]
        protected float _textShowTime = 0.3f;
        #endregion

        #region 動畫相關
        [SerializeField, Header("怪物受傷的動畫名稱")]
        protected string _enemyGetHit = "Hit";
        [SerializeField, Header("怪物死亡的動畫名稱")]
        protected string _enemyDead = "Dead";
        [SerializeField, Header("怪物攻擊的動畫名稱")]
        protected string _enemyAttack = "Attack";
        #endregion

        #region 移動與擊退相關參數
        protected float _velocityTime = 0f;
        protected float _force = 0f;
        protected float _t = 0.05f;
        protected float _targetX = 0f;
        protected float _targetY = 0f;
        #endregion

        protected Quaternion _lookLeft = Quaternion.Euler(0, 180, 0);
        protected Quaternion _lookRight = Quaternion.Euler(0, 0, 0);

        [SerializeField, Header("玩家子彈階層")]
        protected LayerMask _playerBulletLayer;

        protected virtual void Awake()
        {
            _animator = gameObject.GetComponent<Animator>();
            _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
            _collider2D = gameObject.GetComponent<Collider2D>();
            _playerTransform = GameObject.Find("PlayerContainer").GetComponent<Transform>();
            _enemyContainer = GameObject.Find("EnemyContainer").GetComponent<Transform>();
            if (_enemyData != null)
                _cloneEnemyData = Object.Instantiate(_enemyData);
            else
                Debug.LogError("怪物未設定 EnemyData", this.gameObject);
        }
        protected virtual void Update()
        {
            if (IsDead || GameStateMachine.Instance.CurrectState != GameState.InGame)
            {
                _rigidbody2D.velocity = Vector2.zero;
                return;
            }
            Move();

            // 判斷是否要達到攻擊距離，以播放攻擊動畫
            if((transform.position - _playerTransform.position).magnitude <= _cloneEnemyData.AttackRange.Value)
            {
                Attack();
            }
        }
        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (IsDead || GameStateMachine.Instance.CurrectState != GameState.InGame)
            {
                return;
            }

            if (((1 << collision.gameObject.layer) | _playerBulletLayer) == _playerBulletLayer)
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

        /// <summary>
        /// 移動
        /// </summary>
        protected virtual void Move()
        {
            _rigidbody2D.velocity = Vector2.ClampMagnitude(new Vector2(_targetX, _targetY), 1f) * (_velocityTime > 0 ? _force : _cloneEnemyData.MoveSpeed.Value);
            if (_velocityTime <= 0)
            {
                _targetX = Mathf.Lerp(_targetX, _playerTransform.position.x - transform.position.x, _t);
                _targetY = Mathf.Lerp(_targetY, _playerTransform.position.y - transform.position.y, _t);
            }
            else
            {
                _force -= Mathf.Clamp(_force, 0f, Time.deltaTime);
                _velocityTime -= Time.deltaTime;
                _rigidbody2D.velocity = Vector2.ClampMagnitude(new Vector2(_targetX, _targetY), 1f) * _force;
                if(_velocityTime <= 0)
                {
                    _targetX = this.transform.position.x;
                    _targetY = this.transform.position.y;
                    _velocityTime = 0f;
                    _force = 0f;
                }
            }

            this.LookAt(_playerTransform.position);
        }

        /// <summary>
        /// 使怪物看向指定方向
        /// </summary>
        /// <param name="targetPosition"></param>
        protected void LookAt(Vector3 targetPosition)
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
        /// 怪物死亡
        /// </summary>
        protected virtual void Dead()
        {
            AttributeHandle.Instance.AddTotalKill();
            _rigidbody2D.velocity = Vector2.zero;
            _collider2D.enabled = false;
            if (_animator != null && !string.IsNullOrEmpty(_enemyDead))
                _animator.SetTrigger(_enemyDead);
            foreach(DropStruct drop in EnemyData.Drops)
            {
                drop.DropCheck(transform.position);
            }
            StartCoroutine(DestroyEnemy());
        }

        /// <summary>
        /// 移除怪物物件
        /// </summary>
        /// <returns></returns>
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
            GameObject damageText = GameObject.Instantiate(_damageTextPrefab, _enemyContainer);
            damageText.transform.position = this.transform.position + new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), 0f);
            damageText.GetComponentInChildren<TextMeshProUGUI>().text = Mathf.FloorToInt(damage).ToString();
            StartCoroutine(DestroyDamageText(damageText));
        }

        /// <summary>
        /// 移除傷害文字物件
        /// </summary>
        /// <param name="damageText"></param>
        /// <returns></returns>
        protected IEnumerator DestroyDamageText(GameObject damageText)
        {
            yield return new WaitForSeconds(_textShowTime);
            Destroy(damageText);
        }

        /// <summary>
        /// 攻擊
        /// </summary>
        protected virtual void Attack()
        {
            if (_animator != null && !string.IsNullOrEmpty(_enemyAttack))
                _animator.SetTrigger(_enemyAttack);
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
        public virtual void AddForce(float force = 0f, float time = 0f)
        {
            if (!_cloneEnemyData.CanAddedForce)
                return;

            _targetX = transform.position.x - _playerTransform.position.x;
            _targetY = transform.position.y - _playerTransform.position.y;
            _rigidbody2D.velocity = Vector2.ClampMagnitude(new Vector2(_targetX, _targetY), 1f) * force;
            _velocityTime = time;
            _force = force;
        }

        /// <summary>
        /// 回傳怪物是否已死亡
        /// </summary>
        public bool IsDead { get => _cloneEnemyData.HealthPoint <= 0; }

        public virtual EnemyData EnemyData { get => _cloneEnemyData; }
    }
}