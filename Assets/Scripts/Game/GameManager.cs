using Scripts.Game.Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scripts.Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField, Header("地圖資料")]
        private MapData _mapData;

        [SerializeField, Header("怪物池資料")]
        private PoolData _enemyPoolData;

        [SerializeField, Header("子彈池資料")]
        private PoolData _bulletPoolData;

        [SerializeField, Header("掉落彈藥池資料")]
        private PoolData _dropAmmoPoolData;

        [SerializeField, Header("傷害文字資料")]
        private PoolData _damagePoolData;

        [SerializeField, Header("補血包")]
        private PoolData _dropHealthPoolData;

        [SerializeField, Header("經驗值1")]
        private ExpData _exp1;

        [SerializeField, Header("經驗值2")]
        private ExpData _exp2;

        [SerializeField, Header("經驗值3")]
        private ExpData _exp3;

        [SerializeField, Header("玩家資料")]
        private PlayerData defaultPlayerData;
        private PlayerData _playerData;

        [SerializeField, Header("關卡資料")]
        private LevelData[] Levels;

        [SerializeField, Header("槍械陣列")]
        private GunData[] GunDatas;
        private int targetGunIndex;
        private GunData _gunData;

        [SerializeField, Header("近戰武器資料")]
        private MeleeData defaultMeleeData;
        private MeleeData _meleeData;

        [SerializeField, Header("玩家血量")]
        private GameObject _playerHealth;

        [SerializeField, Header("健康血量貼圖")]
        private Sprite _healthSprite;

        [SerializeField, Header("不健康血量貼圖")]
        private Sprite _unhealthSprite;

        [SerializeField, Header("升級選項預置物")]
        private GameObject _optionPrefab;

        private IList<OptionData> _optionDatas;

        [SerializeField, Header("使用者設定")]
        private UserSetting _userSetting;

        [SerializeField, Header("預設使用者設定")]
        private UserSetting _defaultSetting;

        [SerializeField, Header("遊戲總時間(秒)"), Range(1, 1800)]
        private float _gameTime = 1200f;

        [SerializeField, Header("勝利音效")]
        private AudioClip WinAudio;

        [SerializeField, Header("失敗音效")]
        private AudioClip LoseAudio;

        [SerializeField, Header("遊戲介面 Canvas")]
        private Canvas _gameUICanvas;

        [SerializeField, Header("暫停介面 Canvas")]
        private Canvas _pauseUICanvas;

        [SerializeField, Header("升級選項 Canvas")]
        private Canvas _gameOptionsUICanvas;

        [SerializeField, Header("結束介面 Canvas")]
        private Canvas _endUICanvas;

        [SerializeField, Header("設定介面 Canvas")]
        private Canvas _settingUICanvas;

        private float _nowTime;
        private GameObject _playerContainer;

        private IGameFiniteStateMachine _gameFiniteStateMachine;
        private ICameraController _cameraController;
        private IMapController _mapController;
        private IGameUIController _gameUI;
        private IOptionsUIController _optionsUI;
        private IPauseUIController _pauseUI;
        private IEndUIController _endUI;
        private ISettingUIController _settingUI;
        private IMeleeController _meleeController;
        private IAttributeHandle _attributeHandle;
        private IDropHealthPool _dropHealthPool;
        private IAmmoPool _ammoPool;
        private IEnemyPool _enemyPool;
        private IExpPool _expPool;
        private IDamagePool _damagePool;
        private IAudioContoller _audioContoller;
        private SpriteRenderer _gunImage;
        private IMoveController playerMoveController;
        private IPlayerDamageController playerDamageController;
        private IWeaponController weaponController;

        private void Awake()
        {
            Debug.Log("GameManager Awake() Start");
            targetGunIndex = StaticPrefs.GunIndex;
            foreach(GunData gunData in GunDatas)
            {
                if((int)gunData.GunIndex == targetGunIndex)
                {
                    _gunData = Object.Instantiate(gunData);
                }
            }
            if(_gunData == null)
            {
                _gunData = Object.Instantiate(GunDatas[0]);
            }
            _dropAmmoPoolData.prefab.GetComponent<SpriteRenderer>().sprite = _gunData.DropAmmoSprite;
            _playerData = Object.Instantiate(defaultPlayerData);
            _meleeData = Object.Instantiate(defaultMeleeData);
            _optionDatas = new List<OptionData>();
            _gunImage = GameObject.Find("GunHand").GetComponent<SpriteRenderer>();
            foreach (OptionData optionData in _gunData.Options)
            {
                _optionDatas.Add(Object.Instantiate(optionData));
            }
            foreach (OptionData optionData in _playerData.Options)
            {
                _optionDatas.Add(Object.Instantiate(optionData));
            }
            foreach (OptionData optionData in _meleeData.Options)
            {
                _optionDatas.Add(Object.Instantiate(optionData));
            }
            _bulletPoolData.prefab = _gunData.AmmoPrefab;
            _audioContoller = new AudioContoller(_userSetting);
            _playerContainer = GameObject.Find("PlayerContainer");
            _meleeController = GameObject.Find("MeleeWeapon").GetComponent<IMeleeController>();
            _gameFiniteStateMachine = new GameFiniteStateMachine(GameState.Loading, () => { Debug.Log("Loading 階段結束"); });
            _cameraController = new CameraController();
            _damagePool = new DamagePool(_damagePoolData);
            _mapController = new MapController(_mapData);
            _settingUI = new SettingUIController(_audioContoller, _settingUICanvas, _defaultSetting, _userSetting);
            _pauseUI = new PauseUIController(_gameFiniteStateMachine, _settingUI);
            _attributeHandle = new AttributeHandle(_gameFiniteStateMachine, _playerData, _gunData, _meleeData);
            _endUI = new EndUIController(_gameFiniteStateMachine, _attributeHandle);
            _ammoPool = new AmmoPool(_gameFiniteStateMachine, _bulletPoolData, _attributeHandle, _endUI, _playerContainer.transform);
            _optionsUI = new OptionsUIController(_gameFiniteStateMachine, _attributeHandle, _optionPrefab, _optionDatas);
            _gameUI = new GameUIController(_playerHealth, _attributeHandle, _optionsUI, _audioContoller, _healthSprite, _unhealthSprite, _gameUICanvas);
            _dropHealthPool = new DropHealthPool(_dropHealthPoolData, _attributeHandle, _gameUI, _gameFiniteStateMachine, _playerContainer.transform);
            _expPool = new ExpPool(_exp1, _exp2, _exp3, _attributeHandle, _gameUI, _gameFiniteStateMachine, _playerContainer.transform);
            _enemyPool = new EnemyPool(_gameFiniteStateMachine, _endUI, Levels, _attributeHandle, _expPool, _damagePool, _dropHealthPool, _playerContainer.transform);
            playerMoveController = _playerContainer.GetComponent<IMoveController>();
            playerDamageController = _playerContainer.GetComponent<IPlayerDamageController>();
            weaponController = _playerContainer.GetComponent<IWeaponController>();

            _meleeController.SetGameFiniteStateMachine = _gameFiniteStateMachine;
            _meleeController.SetAttributeHandle = _attributeHandle;
            _meleeController.SetGunImage = _gunImage;
            _meleeController.SetAudio = _audioContoller;

            playerMoveController.SetGameFiniteStateMachine = _gameFiniteStateMachine;
            playerMoveController.SetAttributeHandle = _attributeHandle;

            playerDamageController.SetGameFiniteStateMachine = _gameFiniteStateMachine;
            playerDamageController.SetAttributeHandle = _attributeHandle;
            playerDamageController.SetEndUI = _endUI;
            playerDamageController.SetGameUI = _gameUI;
            playerDamageController.SetAudio = _audioContoller;

            weaponController.SetAmmoPool = _ammoPool;
            weaponController.SetAttributeHandle = _attributeHandle;
            weaponController.SetAudio = _audioContoller;
            weaponController.SetGameFiniteStateMachine = _gameFiniteStateMachine;

            _attributeHandle.SetGameUI = _gameUI;

            Debug.Log("GameManager Awake() End");
        }
        private void Start()
        {
            Debug.Log("GameManager Start() Start");
            _nowTime = 0;
            _pauseUI.HideCanvas();
            _optionsUI.HideCanvas();
            _endUI.HideCanvas();
            _settingUI.HideCanvas();
            _gunImage.sprite = _gunData.GunSprite;
            _gameFiniteStateMachine.SetNextState(GameState.InGame);
            _gameFiniteStateMachine.SetPlayerState(PlayerState.Idle);
            foreach (LevelData level in Levels)
            {
                foreach (LevelEnemyData enemyData in level.EnemyDatas)
                {
                        enemyData.NextTime = 0;
                }
            }
            _audioContoller.UpdateAudioVolume();
            Debug.Log("GameManager Start() End");
        }

        private void Update()
        {
            if(_gameFiniteStateMachine.CurrectState == GameState.Loading)
            {
                _gameFiniteStateMachine.SetNextState(GameState.GameStart, () => { Debug.Log("GameStart 階段結束"); });
            }
            else if (_gameFiniteStateMachine.CurrectState == GameState.InGame)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    _pauseUI.ShowCanvas();
                    _gameFiniteStateMachine.SetNextState(GameState.GamePause);
                }
                UpdateGameTime();
                EnemyHandel();
                _cameraController.MoveMainCameraTo(_playerContainer.transform.position);
            }
            else if (_gameFiniteStateMachine.CurrectState == GameState.GamePause)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    _pauseUI.HideCanvas();
                    _gameFiniteStateMachine.SetNextState(GameState.InGame);
                }
            }
            else if (_gameFiniteStateMachine.CurrectState == GameState.BackToMenu)
            {
                _gameFiniteStateMachine.SetNextState(GameState.BackToMenued);
                LoadingScreen.instance.LoadScene("01_MenuScene", true);
            }
            else if (_gameFiniteStateMachine.CurrectState == GameState.GameEnd)
            {
                bool isWin = IsTimeWin;
                _endUI.ShowCanvas(isWin);
                _audioContoller.PlayEffect(isWin ? WinAudio : LoseAudio, isWin ? 0.5f : 1.5f);
                _gameFiniteStateMachine.SetNextState(GameState.GameEnded);
            }
            else if(_gameFiniteStateMachine.CurrectState == GameState.Restart)
            {
                _gameFiniteStateMachine.SetNextState(GameState.Restarted);
                LoadingScreen.instance.LoadScene(SceneManager.GetActiveScene().name, false);
            }
        }

        private void EnemyHandel()
        {
            foreach(LevelData level in Levels)
            {
                if(_nowTime >= level.LevelStartTime && _nowTime <= level.LevelEndTime)
                {
                    foreach(LevelEnemyData enemyData in level.EnemyDatas)
                    {
                        if(_nowTime >= enemyData.NextTime)
                        {
                            if (enemyData.IsGroup)
                            {
                                Vector3 nextPosition = _playerContainer.transform.position + GetRandomPosition(enemyData.Distance);
                                foreach (GameObject enemy in _enemyPool.GetEnemies(enemyData))
                                {
                                    enemy.transform.position = nextPosition + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
                                    nextPosition = _playerContainer.transform.position + GetRandomPosition(enemyData.Distance);
                                }
                            }
                            else if (enemyData.IsRound)
                            {
                                float angle = Random.Range(0f, 360f);
                                IList<GameObject> enemies = _enemyPool.GetEnemies(enemyData);
                                for (int i = 0; i < enemies.Count; i++)
                                {
                                    enemies[i].transform.position = _playerContainer.transform.position + GetAnglePosition(enemyData.Distance, angle);
                                    angle += 360f / enemies.Count;
                                }
                            }
                            else
                            {
                                foreach (GameObject enemy in _enemyPool.GetEnemies(enemyData))
                                {
                                    enemy.transform.position = _playerContainer.transform.position + GetRandomPosition(enemyData.Distance);
                                }
                            }
                            if (enemyData.WarmingAudio != null)
                            {
                                _audioContoller.PlayEffect(enemyData.WarmingAudio, enemyData.ExtendVolume);
                            }
                            enemyData.NextTime = _nowTime + enemyData.Intervals;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 取得指定距離、隨機角度的座標
        /// </summary>
        /// <param name="distance"></param>
        /// <returns></returns>
        private Vector3 GetRandomPosition(float distance)
        {
            return GetAnglePosition(distance, Random.value * 360f);
        }

        /// <summary>
        /// 取得指定距離、指定角度的座標
        /// </summary>
        /// <param name="distance"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        private Vector3 GetAnglePosition(float distance, float angle)
        {
            float radians = angle * Mathf.Deg2Rad;
            float x = distance * Mathf.Cos(radians);
            float y = distance * Mathf.Sin(radians);
            return new Vector3(x, y);
        }

        private void UpdateGameTime()
        {
            _nowTime += Time.deltaTime;
            _gameUI.UpdateGameTime(_gameTime - _nowTime);
            if(IsTimeWin)
            {
                _gameFiniteStateMachine.SetNextState(GameState.GameEnd);
            }
        }

        private bool IsTimeWin { get => _gameTime - _nowTime <= 0; }
    }
}