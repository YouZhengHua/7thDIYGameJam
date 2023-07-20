﻿using Scripts.Game.Data;
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
        private float _totalGameTime = 1200f;

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

        private GameObject _playerContainer;
        private GameObject _enemyContainer;

        [SerializeField, Header("大廳升級管理工具")]
        private BaseUpgradeManager _upgradeManager;

        [SerializeField, Header("怪物階層")]
        private LayerMask _enemyLayer;

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
        /// 經驗值掉落物池
        /// </summary>
        private IExpPool _expPool;
        /// <summary>
        /// 玩家受傷控制器
        /// </summary>
        private IPlayerDamageController _playerDamageController;

        private void Awake()
        {
            Debug.Log("GameManager Awake() Start");
            _playerContainer = GameObject.Find("PlayerContainer");
            _enemyContainer = GameObject.Find("EnemyContainer");

            _playerData = Object.Instantiate(defaultPlayerData);
            _optionDatas = new List<OptionData>();
            foreach (OptionData optionData in _playerData.Options)
            {
                if (optionData.IsAcitve)
                    _optionDatas.Add(Object.Instantiate(optionData));
            }
            foreach (OptionData optionData in Weapons)
            {
                if (optionData.IsAcitve)
                    _optionDatas.Add(Object.Instantiate(optionData));
            }

            GameStateMachine.Instance.SetNextState(GameState.Loading);
            AttributeHandle.Instance.Init();
            AttributeHandle.Instance.SetPlayerData(_playerData);
            AudioContoller.Instance.SetUserSetting(_userSetting);

            _playerDamageController = _playerContainer.GetComponent<IPlayerDamageController>();
            _endUI = new EndUIController();
            _cameraController = new CameraController();
            _mapController = new MapController(_mapData);
            _settingUI = new SettingUIController(_settingUICanvas, _defaultSetting, _userSetting);
            _pauseUI = new PauseUIController(_settingUI);
            _dropHealthPool = new DropHealthPool(_dropHealthPoolData, _playerContainer.transform);
            _optionsUI = new OptionsUIController(_optionPrefab, _optionDatas);
            _gameUI = new GameUIController(_optionsUI, _gameUICanvas);
            _expPool = new ExpPool(_exp1, _exp2, _exp3, _gameUI, _playerContainer.transform);
            AttributeHandle.Instance.SetGameUIController(_gameUI);
            _playerDamageController.SetEndUI = _endUI;
            Debug.Log("GameManager Awake() End");
        }
        private void Start()
        {
            Debug.Log("GameManager Start() Start");
            GameStateMachine.Instance.SetNextState(GameState.GameStart);
            _pauseUI.HideCanvas();
            _optionsUI.HideCanvas();
            _endUI.HideCanvas();
            _settingUI.HideCanvas();
            PlayerStateMachine.Instance.SetNextState(PlayerState.Idle);
            AudioContoller.Instance.UpdateAudioVolume();
            _gameUI.UpdatePlayerHealth();
            AttributeHandle.Instance.SetLobbyUpgrade(_upgradeManager);
            Debug.Log("GameManager Start() End");
        }

        private void Update()
        {
            if (GameStateMachine.Instance.CurrectState == GameState.GameStart)
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
                bool isWin = IsTimeWin && IsKillAllEnemy;
                _endUI.ShowCanvas(isWin);
                AudioContoller.Instance.PlayEffect(isWin ? WinAudio : LoseAudio, isWin ? 0.5f : 1.5f);
                GameStateMachine.Instance.SetNextState(GameState.GameEnded);
            }
            else if (GameStateMachine.Instance.CurrectState == GameState.Restart)
            {
                GameStateMachine.Instance.SetNextState(GameState.Restarted);
                LoadingScreen.instance.LoadScene(SceneManager.GetActiveScene().name, false);
            }
        }

        private void UpdateGameTime()
        {
            AttributeHandle.Instance.GameTime += Time.deltaTime;
            _gameUI.UpdateGameTime(_totalGameTime - AttributeHandle.Instance.GameTime);
            if (IsTimeWin && IsKillAllEnemy)
            {
                GameStateMachine.Instance.SetNextState(GameState.GameEnd);
            }
        }

        /// <summary>
        /// 判斷是否時間勝利
        /// </summary>
        private bool IsTimeWin { get => _totalGameTime - AttributeHandle.Instance.GameTime <= 0; }
        /// <summary>
        /// 判斷是否殺光所有怪物
        /// </summary>
        private bool IsKillAllEnemy { get => _enemyContainer.GetComponent<Transform>().childCount == 0; }
    }
}