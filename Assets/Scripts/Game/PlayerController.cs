using Scripts.Game.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Game
{
    /// <summary>
    /// 玩家控制器
    /// 1. 控制玩家面對方向
    /// 2. 控制玩家移動
    /// 3. 主武器射擊
    /// </summary>
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        private IGameFiniteStateMachine _gameFiniteStateMachine;
        private GameObject _container;

        /// <summary>
        /// 子彈發射位置
        /// </summary>
        private Transform _firePoint;
        private Vector3 _firePointVector3;
        /// <summary>
        /// 持槍座標
        /// </summary>
        private Transform _gunHand;

        /// <summary>
        /// 槍械動畫
        /// </summary>
        private Animator _gunEffect;
        private string playerShootFire =  "Fire";

        /// <summary>
        /// 玩家輸入鍵位
        /// </summary>
        private UserSetting _userSetting;

        /// <summary>
        /// 子彈池
        /// </summary>
        private IAmmoPool _ammoPool;

        private IGameUIController _gameUI;
        private IEndUIController _endUI;
        private IMeleeController _meleeController;
        private IAttributeHandle _attributeHandle;
        private IAudioContoller _audio;

        #region 主武器相關
        /// <summary>
        /// 主武器下次射擊的時間(秒)
        /// </summary>
        private float currentMainShootCooldownTime = 0.0f;

        private List<GameObject> _shotAmmo = new List<GameObject>();

        private ShootType _currentShootType;

        /// <summary>
        /// 玩家動畫控制器
        /// </summary>
        private Animator playerAni;
        private SpriteRenderer _fireEffect;

        #endregion

        #region 裝彈相關
        /// <summary>
        /// 主武器裝彈完成時間(秒)
        /// </summary>
        private float currentReloadTime = 0f;
        #endregion

        private void Awake()
        {
            playerAni = gameObject.GetComponent<Animator>();
            _gunHand = GameObject.Find("GunHand").transform;
            _fireEffect = GameObject.Find("FireEffect").GetComponent<SpriteRenderer>();
            _fireEffect.enabled = false;
        }

        private void Start()
        {
            _gameUI.UpdateAmmoCount();
            _firePoint = GameObject.Find(_attributeHandle.FirePointName).GetComponent<Transform>();
            _firePointVector3 = _firePoint.localPosition;
            _gunEffect = _firePoint.gameObject.GetComponentInChildren<Animator>();
        }

        private void Update()
        {
            if (_gameFiniteStateMachine.CurrectState == GameState.InGame)
            {
                if(_gameFiniteStateMachine.PlayerState != PlayerState.Shoot)
                {
                    _attributeHandle.RecoverOffset(Time.deltaTime);
                }
                if (_gameFiniteStateMachine.PlayerState == PlayerState.Idle || _gameFiniteStateMachine.PlayerState == PlayerState.Shoot)
                {
                    MainShootHandel();
                    if ((Input.GetKeyDown(_userSetting.Reload) && _attributeHandle.NowAmmoCount < _attributeHandle.MagazineSize) || (_attributeHandle.NowAmmoCount == 0 && _attributeHandle.TotalAmmoCount > 0))
                    {
                        Reload();
                    }
                    if (Input.GetKeyDown(_userSetting.ChangeShootType) && _attributeHandle.ShootTypes.Length > 1)
                    {
                        ChangeShootType();
                    }
                }
                if(_gameFiniteStateMachine.PlayerState == PlayerState.Idle || _gameFiniteStateMachine.PlayerState == PlayerState.Reload)
                {
                    MeleeAttackHandel();
                }
                if (_gameFiniteStateMachine.PlayerState == PlayerState.Reload)
                {
                    if (_attributeHandle.MagazineSize > 0)
                    {
                        MainShootHandel();
                    }
                    ReloadHandel();
                }
            }
        }

        #region Handel
        /// <summary>
        /// 槍械射擊處理
        /// 判斷是否處於射擊冷卻時間
        /// 使用何種射擊模式
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

            if(currentMainShootCooldownTime == 0.0f && _attributeHandle.NowAmmoCount > 0)
            {
                // 單發射擊
                if(_currentShootType == ShootType.One && Input.GetKeyDown(_userSetting.Shoot))
                {
                    MainShoot();
                }
                // 三連發
                else if(_currentShootType == ShootType.Three && Input.GetKeyDown(_userSetting.Shoot))
                {
                    StartCoroutine(MainShoot(3));
                }
                // 全自動射擊
                else if (_currentShootType == ShootType.Auto && (Input.GetKey(_userSetting.Shoot)))
                {
                    MainShoot();
                }
            }
        }
        
        /// <summary>
        /// 換彈處理
        /// 換彈倒數計時
        /// 換彈完成時更新UI介面
        /// </summary>
        private void ReloadHandel()
        {
            if (_gameFiniteStateMachine.PlayerState == PlayerState.Reload)
            {
                currentReloadTime -= Time.deltaTime;
                _gameUI.UpdateReloadImage(currentReloadTime / ReloadMagazineTime);
                if (currentReloadTime <= 0f)
                {
                    _gameUI.HideReloadImage();
                    currentReloadTime = 0f;
                    if (_attributeHandle.ReloadType == ReloadType.One)
                    {
                        int targetAmmoCount = _attributeHandle.NowAmmoCount + 1;
                        _attributeHandle.NowAmmoCount = targetAmmoCount;
                        if(_attributeHandle.NowAmmoCount < _attributeHandle.MagazineSize)
                        {
                            Reload(true);
                        }
                        else
                        {
                            _gameFiniteStateMachine.SetPlayerState(PlayerState.Idle);
                        }
                        if (_attributeHandle.NowAmmoCount == _attributeHandle.MagazineSize)
                        {
                            _audio.PlayEffect(_attributeHandle.LoadedAudio);
                        }
                    }
                    else
                    {
                        _gameFiniteStateMachine.SetPlayerState(PlayerState.Idle);
                        if (_attributeHandle.NowAmmoCount <= 0)
                        {
                            _audio.PlayEffect(_attributeHandle.LoadedAudio);
                        }
                        int targetAmmoCount = _attributeHandle.MagazineSize + (_attributeHandle.NowAmmoCount > 0 ? 1 : 0);
                        targetAmmoCount = _attributeHandle.TotalAmmoCount > targetAmmoCount ? targetAmmoCount : _attributeHandle.TotalAmmoCount;
                        _attributeHandle.NowAmmoCount = targetAmmoCount;
                    }
                    _gameUI.UpdateAmmoCount();
                }
            }
            else
            {
                _gameUI.HideReloadImage();
            }
        }

        /// <summary>
        /// 近戰武器處理
        /// </summary>
        private void MeleeAttackHandel()
        {
            if (Input.GetKeyDown(_userSetting.MeleeAttack))
            {
                _gameUI.HideReloadImage();
                _meleeController.MeleeAttack();
            }
        }
        #endregion

        /// <summary>
        /// 槍械射擊
        /// </summary>
        public void MainShoot()
        {
            if (_attributeHandle.NowAmmoCount == 0)
            {
                _audio.PlayEffect(_attributeHandle.NoAmmoAudio);
                return;
            }

            _gameFiniteStateMachine.SetPlayerState(PlayerState.Shoot);
            
            for (int i = 0; i < _attributeHandle.OneShootAmmoCount; i++)
            {
                _shotAmmo.Add(_ammoPool.GetPrefab());
            }
            _attributeHandle.NowAmmoCount -= 1;
            _attributeHandle.NowAmmoCount = _attributeHandle.NowAmmoCount < 0 ? 0 : _attributeHandle.NowAmmoCount;
            _attributeHandle.TotalAmmoCount -= 1;
            _attributeHandle.TotalAmmoCount = _attributeHandle.TotalAmmoCount < 0 ? 0 : _attributeHandle.TotalAmmoCount;

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
            _gameUI.UpdateAmmoCount();
            _endUI.AddShootCount();

            if (_attributeHandle.NowAmmoCount == 0 && _attributeHandle.TotalAmmoCount > 0)
            {
                _audio.PlayEffect(_attributeHandle.NoAmmoAudio);
                Reload();
            }
        }

        /// <summary>
        /// 連續射擊固定次數
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public IEnumerator MainShoot(int count)
        {
            for (int times = 0; times < count; times++)
            {
                MainShoot();
                yield return new WaitForSeconds(_attributeHandle.ShootCooldownTime);
            }
        }

        public IEnumerator FireEffectOff()
        {
            yield return new WaitForSeconds(0.1f);
            _fireEffect.enabled = false;
        }

        /// <summary>
        /// 換彈
        /// 顯示換彈圖片
        /// </summary>
        private void Reload(bool notNeedCheckStatus = false)
        {
            if (_gameFiniteStateMachine.PlayerState != PlayerState.Reload || notNeedCheckStatus)
            {
                _gameFiniteStateMachine.SetPlayerState(PlayerState.Reload);
                _audio.PlayEffect(_attributeHandle.ReloadAudio);
                _gameUI.ShowReloadImage();
                _gameUI.UpdateReloadImage(1);
                currentReloadTime = ReloadMagazineTime;
            }
        }

        /// <summary>
        /// 更換射擊模式
        /// </summary>
        private void ChangeShootType()
        {
            int index = -1;
            for(int i = 0; i < _attributeHandle.ShootTypes.Length; i++)
            {
                if(_attributeHandle.ShootTypes[i] == _currentShootType)
                {
                    index = i;
                    break;
                }
            }
            index++;
            index %= _attributeHandle.ShootTypes.Length;
            _currentShootType = _attributeHandle.ShootTypes[index];
            _audio.PlayEffect(_attributeHandle.ChangeShootTypeAudio);
            Debug.Log($"切換射擊模式為: {_currentShootType}");
        }

        /// <summary>
        /// 取得換彈冷卻時間
        /// 判斷槍械中是否仍有子彈 有子彈則為換彈時間+上膛時間 沒有則只有換彈時間
        /// </summary>
        private float ReloadMagazineTime
        {
            get
            {
                return _attributeHandle.GetGunReloadTime(_attributeHandle.NowAmmoCount > 0);
            }
        }

        #region DI 設定
        public IGameFiniteStateMachine SetGameFiniteStateMachine { set => _gameFiniteStateMachine = value; }
        public IAmmoPool SetAmmoPool { set => _ammoPool = value; }
        public IGameUIController SetGameUI { set => _gameUI = value; }
        public IEndUIController SetEndUI { set => _endUI = value; }
        public IMeleeController SetMeleeController { set => _meleeController = value; }
        public UserSetting SetUserSetting { set => _userSetting = value; }
        public GameObject SetContainer { set => _container = value; }
        public Transform GetTransform { get => _container.transform; }
        public IAttributeHandle SetAttributeHandle 
        {
            set
            {
                _attributeHandle = value;
                _currentShootType = value.ShootTypes[0];
            }
        }
        public IAudioContoller SetAudio { set => _audio = value; }
        #endregion
    }
}