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

        [SerializeField, Header("武器選項")]
        private OptionData[] Weapons;

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

        /// <summary>
        /// 攝影機控制器
        /// </summary>
        private ICameraController _cameraController;
        /// <summary>
        /// 地圖控制器
        /// </summary>
        private IMapController _mapController;
        /// <summary>
        /// 遊戲UI
        /// </summary>
        private IGameUIController _gameUI;
        /// <summary>
        /// 升級選項UI
        /// </summary>
        private IOptionsUIController _optionsUI;
        /// <summary>
        /// 暫停UI
        /// </summary>
        private IPauseUIController _pauseUI;
        /// <summary>
        /// 結算畫面UI
        /// </summary>
        private IEndUIController _endUI;
        /// <summary>
        /// 遊戲設定UI
        /// </summary>
        private ISettingUIController _settingUI;
        /// <summary>
        /// 補包掉落物池
        /// </summary>
        private IDropHealthPool _dropHealthPool;
        /// <summary>
        /// 怪物池
        /// </summary>
        private IEnemyPool _enemyPool;
        /// <summary>
        /// 經驗值掉落物池
        /// </summary>
        private IExpPool _expPool;
        /// <summary>
        /// 傷害文字池
        /// </summary>
        private IDamagePool _damagePool;
        /// <summary>
        /// 音效控制器
        /// </summary>
        private IAudioContoller _audioContoller;
        /// <summary>
        /// 玩家受傷控制器
        /// </summary>
        private IPlayerDamageController _playerDamageController;
        /// <summary>
        /// 玩家武器控制器
        /// </summary>
        private IWeaponController _weaponController;

        private void Awake()
        {
            Debug.Log("GameManager Awake() Start");
            _playerData = Object.Instantiate(defaultPlayerData);
            _optionDatas = new List<OptionData>();
            foreach (OptionData optionData in _playerData.Options)
            {
                if(optionData.IsAcitve)
                    _optionDatas.Add(Object.Instantiate(optionData));
            }
            foreach (OptionData optionData in Weapons)
            {
                if(optionData.IsAcitve)
                    _optionDatas.Add(Object.Instantiate(optionData));
            }

            GameStateMachine.Instance.SetNextState(GameState.Loading);
            AttributeHandle.Instance.SetPlayerData(_playerData);
            _playerContainer = GameObject.Find("PlayerContainer");
            _playerDamageController = _playerContainer.GetComponent<IPlayerDamageController>();
            _weaponController = _playerContainer.GetComponent<IWeaponController>();
            _endUI = new EndUIController();
            _cameraController = new CameraController();
            _audioContoller = new AudioContoller(_userSetting);
            _damagePool = new DamagePool(_damagePoolData);
            _mapController = new MapController(_mapData);
            _settingUI = new SettingUIController(_audioContoller, _settingUICanvas, _defaultSetting, _userSetting);
            _pauseUI = new PauseUIController(_settingUI);
            AttributeHandle.Instance.SetWeaponController(_weaponController);
            _optionsUI = new OptionsUIController(_optionPrefab, _optionDatas);
            _gameUI = new GameUIController(_optionsUI, _audioContoller, _gameUICanvas);
            _dropHealthPool = new DropHealthPool(_dropHealthPoolData, _gameUI, _playerContainer.transform);
            _expPool = new ExpPool(_exp1, _exp2, _exp3, _gameUI, _playerContainer.transform);
            _enemyPool = new EnemyPool(_endUI, Levels, _expPool, _damagePool, _dropHealthPool, _playerContainer.transform);

            _playerDamageController.SetEndUI = _endUI;
            _playerDamageController.SetGameUI = _gameUI;
            _playerDamageController.SetAudio = _audioContoller;

            _weaponController.SetAudio = _audioContoller;

            AttributeHandle.Instance.SetGameUIController(_gameUI);

            Debug.Log("GameManager Awake() End");
        }
        private void Start()
        {
            Debug.Log("GameManager Start() Start");
            GameStateMachine.Instance.SetNextState(GameState.GameStart);
            _nowTime = 0;
            _pauseUI.HideCanvas();
            _optionsUI.HideCanvas();
            _endUI.HideCanvas();
            _settingUI.HideCanvas();
            PlayerStateMachine.Instance.SetNextState(PlayerState.Idle);
            foreach (LevelData level in Levels)
            {
                foreach (LevelEnemyData enemyData in level.EnemyDatas)
                {
                        enemyData.NextTime = 0;
                }
            }
            _audioContoller.UpdateAudioVolume();
            _gameUI.UpdatePlayerHealth();
            Debug.Log("GameManager Start() End");
        }

        private void Update()
        {
            if(GameStateMachine.Instance.CurrectState == GameState.GameStart)
            {
                _optionsUI.ShowWeaponOptions();
            }
            else if (GameStateMachine.Instance.CurrectState == GameState.InGame)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    _pauseUI.ShowCanvas();
                    GameStateMachine.Instance.SetNextState(GameState.GamePause);
                }
                UpdateGameTime();
                EnemyHandel();
                _cameraController.MoveMainCameraTo(_playerContainer.transform.position);
            }
            else if (GameStateMachine.Instance.CurrectState == GameState.GamePause)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    _pauseUI.HideCanvas();
                    GameStateMachine.Instance.SetNextState(GameState.InGame);
                }
            }
            else if (GameStateMachine.Instance.CurrectState == GameState.BackToMenu)
            {
                GameStateMachine.Instance.SetNextState(GameState.BackToMenued);
                LoadingScreen.instance.LoadScene("01_MenuScene", true);
            }
            else if (GameStateMachine.Instance.CurrectState == GameState.GameEnd)
            {
                bool isWin = IsTimeWin;
                _endUI.ShowCanvas(isWin);
                _audioContoller.PlayEffect(isWin ? WinAudio : LoseAudio, isWin ? 0.5f : 1.5f);
                GameStateMachine.Instance.SetNextState(GameState.GameEnded);
            }
            else if(GameStateMachine.Instance.CurrectState == GameState.Restart)
            {
                GameStateMachine.Instance.SetNextState(GameState.Restarted);
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
                GameStateMachine.Instance.SetNextState(GameState.GameEnd);
            }
        }

        private bool IsTimeWin { get => _gameTime - _nowTime <= 0; }
    }
}