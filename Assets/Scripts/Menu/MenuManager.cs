using Scripts.Game.Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scripts.Menu
{
    public class MenuManager : MonoBehaviour, IMenuManager
    {
        [SerializeField]
        private Canvas _gameOptionCanvas;
        [SerializeField]
        private Canvas _menuCanvas;
        [SerializeField]
        private Canvas _settingCanvas;
        [SerializeField, Header("槍械資料")]
        private GunData[] _gunDatas;
        [SerializeField, Header("槍械選項預置物")]
        private GameObject _gunOptionPrefab;
        [SerializeField]
        private UserSetting _defaultSetting;
        [SerializeField]
        private UserSetting _userSetting;
        IGameOptionsUIController _gameOptionsUIController;
        IMenuUIController _menuUIController;
        ISettingUIController _settingUIController;
        IAudioContoller _audioContoller;
        private float DebugTime = 0f;

        private IList<KeyCode> inputKeycodes;
        private KeyCode[] ReasetKeyCode = new KeyCode[] { KeyCode.R, KeyCode.E, KeyCode.S, KeyCode.E, KeyCode.T };
        private KeyCode[] AddMoneyCode = new KeyCode[] { KeyCode.M, KeyCode.O, KeyCode.N, KeyCode.E, KeyCode.Y };

        private void Awake()
        {
            _gameOptionsUIController = new GameOptionsUIController(this, _gameOptionCanvas, _gunOptionPrefab, _gunDatas);
            _audioContoller = new AudioContoller(_userSetting);
            _menuUIController = new MenuUIController(this, _menuCanvas);
            _settingUIController = new SettingUIController(this, _audioContoller, _settingCanvas, _defaultSetting, _userSetting);
            inputKeycodes = new List<KeyCode>();

            if (StaticPrefs.IsFirstIn)
                FirstOpenGame();

        }

        private void Start()
        {
            ShowMenu();
            _audioContoller.UpdateAudioVolume();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F12) && DebugTime == 0)
            {
                DebugTime = 10f;
                inputKeycodes.Clear();
                Debug.Log("作弊指令 10秒輸入");
            }
            else if (DebugTime > 0)
            {
                if (Input.GetKeyDown(KeyCode.Backspace))
                {
                    inputKeycodes.Clear();
                    Debug.Log("清空輸入");
                }

                if (inputKeycodes.Count < ReasetKeyCode.Length && Input.GetKeyDown(ReasetKeyCode[inputKeycodes.Count]))
                {
                    inputKeycodes.Add(ReasetKeyCode[inputKeycodes.Count]);
                }
                else if (inputKeycodes.Count < AddMoneyCode.Length && Input.GetKeyDown(AddMoneyCode[inputKeycodes.Count]))
                {
                    inputKeycodes.Add(AddMoneyCode[inputKeycodes.Count]);
                }
                DebugTime -= Time.deltaTime;

                if(string.Join(',', inputKeycodes) == string.Join(',', AddMoneyCode))
                {
                    StaticPrefs.Score += 1000;
                    inputKeycodes.Clear();
                    DebugTime = 0;
                    Debug.Log("分數增加 1000");
                }
                else if (string.Join(',', inputKeycodes) == string.Join(',', ReasetKeyCode))
                {
                    StaticPrefs.Score = 0;
                    StaticPrefs.SetGunIsUnlocked(Game.GunIndex.Rifle, false);
                    StaticPrefs.SetGunIsUnlocked(Game.GunIndex.ShotGun, false);
                    StaticPrefs.IsFirstIn = true;
                    inputKeycodes.Clear();
                    DebugTime = 0;
                    Debug.Log("重置分數及槍械解鎖狀態");
                }
            }
            if(DebugTime < 0)
            {
                DebugTime = 0;
                inputKeycodes.Clear();
                Debug.Log("作弊指令輸入截止");
            }
            else if (DebugTime == 0 && inputKeycodes.Count > 0)
            {
                inputKeycodes.Clear();
                Debug.Log("作弊指令輸入截止");
            }
        }

        public void ShowMenu()
        {
            _menuUIController.ShowCanvas();
            _gameOptionsUIController.HideCanvas();
            _settingUIController.HideCanvas();
        }

        public void ShowGameOptions()
        {
            _menuUIController.HideCanvas();
            _gameOptionsUIController.ShowCanvas();
            _settingUIController.HideCanvas();
        }

        public void ShowGameOptionsWithReflash()
        {
            ShowGameOptions();
            _gameOptionsUIController.ReflashToggleGroup();
        }

        public void ShowSetting()
        {
            _menuUIController.HideCanvas();
            _gameOptionsUIController.HideCanvas();
            _settingUIController.ShowCanvas();
        }

        private void FirstOpenGame()
        {
            Debug.Log("FirstOpenGame");
            _settingUIController.ResetUserSetting();
            StaticPrefs.IsFirstIn = false;
        }
    }
}