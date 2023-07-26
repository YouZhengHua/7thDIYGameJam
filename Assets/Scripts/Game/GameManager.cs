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

        [SerializeField, Header("護盾值預置物")]
        private GameObject _shieldIcon;

        [SerializeField, Header("結算武器 ICON 預置物")]
        private GameObject _activeWeaponIconPrefab;

        [SerializeField, Header("有效護盾值")]
        private Sprite _shieldActive;

        [SerializeField, Header("失效護盾值")]
        private Sprite _shieldUnactive;

        [SerializeField, Header("進度管理")]
        private StageManager _stageManager;

        /// <summary>
        /// 攝影機控制器
        /// </summary>
        private ICameraController _cameraController;
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

        private void Awake()
        {
            CheckerUtility.SceneLoadChecker();
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
            AudioController.Instance.SetUserSetting(_userSetting);

            _endUI = new EndUIController(_activeWeaponIconPrefab);
            _cameraController = new CameraController();
            _settingUI = new SettingUIController(_settingUICanvas, _defaultSetting, _userSetting);
            _pauseUI = new PauseUIController(_settingUI);
            _optionsUI = new OptionsUIController(_optionPrefab, _optionDatas);
            _gameUI = new GameUIController(_optionsUI, _gameUICanvas, _shieldIcon, _shieldActive, _shieldUnactive);
            AttributeHandle.Instance.SetGameUIController(_gameUI);
            AttributeHandle.Instance.SetLobbyUpgrade(_upgradeManager);
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
            AudioController.Instance.UpdateAudioVolume();
            _gameUI.UpdatePlayerHealth();
            _gameUI.UpdateMoneyGUI();
            UpdateGameTime();
            PlayerStateMachine.Instance.SetNextState(PlayerState.Life);
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
                StaticPrefs.Score += CalTool.Round(AttributeHandle.Instance.TotalMoney);
                LoadingScreen.instance.LoadScene("01_MenuScene", _stageManager.LoadingImage, _stageManager.LoadingTip);
            }
            else if (GameStateMachine.Instance.CurrectState == GameState.GameEnd)
            {
                bool isWin = AttributeHandle.Instance.IsTimeWin && IsKillAllEnemy;
                _endUI.ShowCanvas(isWin);
                if (isWin)
                {
                    if (_stageManager) _stageManager.GoingToNextStage(_stageManager.GetCurrentStage() + 1);
                }
                AudioController.Instance.PlayEffect(isWin ? WinAudio : LoseAudio, isWin ? 0.5f : 1.5f);
                GameStateMachine.Instance.SetNextState(GameState.GameEnded);
            }
            else if (GameStateMachine.Instance.CurrectState == GameState.Restart)
            {
                GameStateMachine.Instance.SetNextState(GameState.Restarted);
                StaticPrefs.Score += CalTool.Round(AttributeHandle.Instance.TotalMoney);
                LoadingScreen.instance.LoadScene(SceneManager.GetActiveScene().name, _stageManager.LoadingImage, _stageManager.LoadingTip);
            }
        }

        private void UpdateGameTime()
        {
            AttributeHandle.Instance.GameTime += Time.deltaTime;
            _gameUI.UpdateGameTime(AttributeHandle.Instance.TotalGameTime - AttributeHandle.Instance.GameTime);
            if (AttributeHandle.Instance.IsTimeWin && IsKillAllEnemy)
            {
                GameStateMachine.Instance.SetNextState(GameState.GameEnd);
            }
        }

        /// <summary>
        /// 判斷是否殺光所有怪物
        /// </summary>
        private bool IsKillAllEnemy { get => _enemyContainer.GetComponent<Transform>().childCount == 0; }
    }
}