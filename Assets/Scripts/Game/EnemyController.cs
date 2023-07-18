﻿using Scripts.Game.Base;
using Scripts.Game.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Scripts.Game
{
    public class EnemyController : MonoBehaviour, IEnemyController
    {
        private Transform playerTransform;
        private IEndUIController _endUI;
        private IExpPool _expPool;
        private IDamagePool _damagePool;
        private IDropHealthPool _dropHealthPool;
        private EnemyData _baseEnemyData;
        private EnemyData _enemyData;
        private Animator _animator;
        private string _enemyGetHit = "Hit";
        private string _enemyDead = "Dead";
        private string _enemyAttack = "Attack";
        private float _velocityTime = 0;
        private Rigidbody2D _rigidbody2D;
        private Quaternion _lookLeft = Quaternion.Euler(0, 180, 0);
        private Quaternion _lookRight = Quaternion.Euler(0, 0, 0);
        private EnemyState _state;

        private void Awake()
        {
            _animator = gameObject.GetComponent<Animator>();
            _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            if (_baseEnemyData != null)
                _enemyData = Object.Instantiate(_baseEnemyData);
            gameObject.GetComponent<CapsuleCollider2D>().enabled = true;
            _state = EnemyState.Run;
        }

        private void Update()
        {
            if (_velocityTime > Time.deltaTime)
                _rigidbody2D.velocity -= _rigidbody2D.velocity * (Time.deltaTime / _velocityTime);
            else
                _rigidbody2D.velocity = Vector2.zero;

            if (GameStateMachine.Instance.CurrectState == GameState.InGame && _state != EnemyState.Dead)
            {
                if (_enemyData.AttackRange > 0 && (playerTransform.position - transform.position).magnitude <= _enemyData.AttackRange)
                {
                    PlayAttackAnimation();
                }

                if (_velocityTime > 0)
                {
                    _velocityTime -= Time.deltaTime;
                }
                else
                {
                    _velocityTime = 0;
                    this.LookAt(playerTransform.position);
                    this.MoveTo(playerTransform.position);
                }

            }
            if (_state == EnemyState.Attack && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            {
                _state = EnemyState.Run;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!IsDead)
            {
                if (collision.gameObject.layer == LayerMask.NameToLayer("玩家子彈"))
                {
                    string ammoName = collision.gameObject.name.Split('_')[1];
                    try
                    {
                        ammoName = ammoName.Substring(0, ammoName.IndexOf("(Clone)"));
                        Weapon weapon = null;
                        foreach(Weapon w in GameObject.Find("Weapons").GetComponentsInChildren<Weapon>())
                        {
                            if (w.gameObject.name.Contains(ammoName))
                            {
                                weapon = w;
                            }
                        }
                        if(weapon != null)
                        {
                            //子彈擊退力量
                            float repelForce = weapon.weaponData.Force;
                            //子彈擊退持續時間
                            float repelTime = weapon.weaponData.DelayTime;
                            //子彈傷害
                            float damage = weapon.weaponData.Damage;
                            GotHit(repelForce, repelTime);
                            GetDamage(damage, DamageFrom.Gun);
                            ShowDamageText(damage, collision.gameObject.transform.position);
                        }
                    }
                    catch
                    {
                        Debug.Log($"子彈名稱沒有包含 (clone), {ammoName}");
                    }
                }
            }
        }

        private void GotHit(float force, float delayTime)
        {
            if (_enemyData.IsBoss)
            {
                if (_rigidbody2D.velocity.magnitude < 200f)
                {
                    _rigidbody2D.AddForce((transform.position - playerTransform.position) * force);
                    _velocityTime += delayTime * 0.1f;
                }
            }
            else
            {
                _rigidbody2D.AddForce((transform.position - playerTransform.position) * force);
                _velocityTime += delayTime;
            }
            if (_state == EnemyState.Run)
                _animator.SetTrigger(_enemyGetHit);
        }

        private float MoveSpeed
        {
            get => _enemyData.MoveSpeedRate * _enemyData.BaseMoveSpeed;
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

        public void MoveTo(Vector3 target)
        {
            Vector3 newPosition = Vector3.MoveTowards(transform.position, target, MoveSpeed * Time.deltaTime);
            transform.position = newPosition;
        }

        public void TakeDamage(float damage, DamageFrom damageFrom, float force, float delayTime)
        {
            GotHit(force, delayTime);
            GetDamage(damage, damageFrom);
            ShowDamageText(damage, this.gameObject.transform.position);
        }
        private void GetDamage(float damage, DamageFrom damageFrom)
        {
            _endUI.AddDamage(damage);
            _enemyData.HealthPoint -= damage;
            if (_enemyData.HealthPoint <= 0)
            {
                _state = EnemyState.Dead;
                _enemyData.HealthPoint = 0;
                Dead(damageFrom);
            }
        }

        private void Dead(DamageFrom damageFrom)
        {
            _rigidbody2D.velocity = Vector2.zero;
            _animator.SetTrigger(_enemyDead);
            gameObject.GetComponent<CapsuleCollider2D>().enabled = false;
            foreach (GameObject exp in _expPool.GetExpPrefabs(_enemyData.exp))
            {
                exp.transform.position = transform.position + new Vector3(Random.Range(0, 0.3f), Random.Range(0, 0.3f));
            }
            if (AttributeHandle.Instance.NeedDropHealth)
            {
                _dropHealthPool.GetPrefab().transform.position = transform.position + new Vector3(Random.Range(0, 0.3f), Random.Range(0, 0.3f));
            }
            switch (damageFrom)
            {
                case DamageFrom.Gun:
                    _endUI.AddKillEnemyCountByGun();
                    break;
                case DamageFrom.Melee:
                    _endUI.AddKillEnemyCountByMelee();
                    break;
            }
            if (_enemyData.IsBoss)
            {
                _endUI.AddKillBossCount();
            }
            StartCoroutine(HideEnemy());
        }

        private IEnumerator HideEnemy()
        {
            yield return new WaitForSeconds(1f);
            gameObject.SetActive(false);
        }

        private void ShowDamageText(float damage, Vector3 postion)
        {
            GameObject damageText = _damagePool.GetPrefab();
            damageText.transform.position = postion + new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), 0f);
            damageText.GetComponentInChildren<TextMeshProUGUI>().text = Mathf.FloorToInt(damage).ToString();
            StartCoroutine(HideDamageText(damageText));
        }

        private IEnumerator HideDamageText(GameObject damageText)
        {
            yield return new WaitForSeconds(0.3f);
            damageText.SetActive(false);
        }

        public void AddVelocityTime(float dealyTime)
        {
            _velocityTime += dealyTime;
        }

        public void PlayAttackAnimation()
        {
            if (_enemyData.AttackRange > 0 && _state == EnemyState.Run)
            {
                _animator.SetTrigger(_enemyAttack);
                _state = EnemyState.Attack;
            }
        }

        public bool IsDead { get => _enemyData.HealthPoint <= 0; }

        public float EnemyDamage { get => _enemyData.Damage; }

        #region DI 設定
        public EnemyData SetEnemyData { set => _baseEnemyData = value; }
        public Transform SetPlayerTransform { set => playerTransform = value; }
        public IEndUIController SetEndUI { set => _endUI = value; }
        public IExpPool SetExpPool { set => _expPool = value; }
        public IDamagePool SetDamagePool { set => _damagePool = value; }
        public IDropHealthPool SetDropHealthPool { set => _dropHealthPool = value; }
        #endregion
    }
}