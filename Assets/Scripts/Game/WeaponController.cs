using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Scripts.Game
{
    public class WeaponController : MonoBehaviour, IWeaponController
    {
        /// <summary>
        /// 遊戲狀態機
        /// </summary>
        private IGameFiniteStateMachine _gameFiniteStateMachine;

        /// <summary>
        /// 屬性處理器
        /// </summary>
        private IAttributeHandle _attributeHandle;

        /// <summary>
        /// 子彈發射位置
        /// </summary>
        private Transform _firePoint;

        /// <summary>
        /// 槍械動畫
        /// </summary>
        private SpriteRenderer _fireEffect;

        private List<GameObject> _shotAmmo = new List<GameObject>();

        /// <summary>
        /// 主武器下次射擊的時間(秒)
        /// </summary>
        private float currentMainShootCooldownTime = 0.0f;

        /// <summary>
        /// 槍械動畫
        /// </summary>
        private Animator _gunEffect;
        private string playerShootFire = "Fire";

        /// <summary>
        /// 子彈池
        /// </summary>
        private IAmmoPool _ammoPool;

        /// <summary>
        /// 音效控制器
        /// </summary>
        private IAudioContoller _audio;

        private void Start()
        {
            _fireEffect = GameObject.Find("FireEffect").GetComponent<SpriteRenderer>();
            _fireEffect.enabled = false;

            _firePoint = GameObject.Find(_attributeHandle.FirePointName).GetComponent<Transform>();
            _gunEffect = _firePoint.gameObject.GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            if (_gameFiniteStateMachine.CurrectState == GameState.InGame)
            {
                MainShootHandel();
            }
        }

        /// <summary>
        /// 槍械射擊處理
        /// 判斷是否處於射擊冷卻時間
        /// </summary>
        private void MainShootHandel()
        {
            currentMainShootCooldownTime -= Time.deltaTime;
            if (currentMainShootCooldownTime <= 0.0f)
            {
                currentMainShootCooldownTime = 0f;
                if (_gameFiniteStateMachine.PlayerState == PlayerState.Shoot)
                    _gameFiniteStateMachine.SetPlayerState(PlayerState.Idle);
            }

            if (currentMainShootCooldownTime == 0.0f && _attributeHandle.NowAmmoCount > 0)
            {
                MainShoot();
            }
        }

        /// <summary>
        /// 槍械射擊
        /// </summary>
        public void MainShoot()
        {
            _gameFiniteStateMachine.SetPlayerState(PlayerState.Shoot);

            for (int i = 0; i < _attributeHandle.OneShootAmmoCount; i++)
            {
                _shotAmmo.Add(_ammoPool.GetPrefab());
            }

            foreach (GameObject bullet in _shotAmmo)
            {
                bullet.transform.position = _firePoint.position;
                bullet.transform.rotation = _firePoint.rotation * _attributeHandle.ShootOffset;
                Rigidbody2D bulletRigidbody = bullet.GetComponent<Rigidbody2D>();
                bulletRigidbody.velocity = Vector3.zero.normalized;
                bulletRigidbody.AddForce(_attributeHandle.ShootOffset * _firePoint.up * _attributeHandle.AmmoFlySpeed);
                bullet.GetComponent<IAmmoController>().AmmoGroup = Time.time.GetHashCode();
            }

            _gunEffect.SetTrigger(playerShootFire);

            _attributeHandle.AddOffset();
            _fireEffect.enabled = true;
            StartCoroutine(FireEffectOff());
            _audio.PlayEffect(_attributeHandle.ShootAudio);

            _shotAmmo.Clear();

            currentMainShootCooldownTime = _attributeHandle.ShootCooldownTime;

            if (_attributeHandle.NowAmmoCount == 0 && _attributeHandle.TotalAmmoCount > 0)
            {
                _audio.PlayEffect(_attributeHandle.NoAmmoAudio);
            }
        }

        public IEnumerator FireEffectOff()
        {
            yield return new WaitForSeconds(0.1f);
            _fireEffect.enabled = false;
        }

        /// <summary>
        /// 設定遊戲狀態機
        /// </summary>
        public IGameFiniteStateMachine SetGameFiniteStateMachine { set => _gameFiniteStateMachine = value; }
        public IAttributeHandle SetAttributeHandle { set => _attributeHandle = value; }
        public IAmmoPool SetAmmoPool { set => _ammoPool = value; }
        public IAudioContoller SetAudio { set => _audio = value; }
    }
}