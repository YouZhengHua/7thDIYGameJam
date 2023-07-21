using Scripts.Game.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Start
{
    public class StartManager : MonoBehaviour, IStartManager
    {
        [SerializeField]
        private Canvas _menuCanvas;
        [SerializeField]
        private Canvas _settingCanvas;
        [SerializeField]
        private UserSetting _defaultSetting;
        [SerializeField]
        private UserSetting _userSetting;
        ISettingUIController _settingUIController;
        IMenuUIController _menuUIController;
        private float DebugTime = 0f;

        private IList<KeyCode> inputKeycodes;
        private KeyCode[] ReasetKeyCode = new KeyCode[] { KeyCode.R, KeyCode.E, KeyCode.S, KeyCode.E, KeyCode.T };
        private KeyCode[] AddMoneyCode = new KeyCode[] { KeyCode.M, KeyCode.O, KeyCode.N, KeyCode.E, KeyCode.Y };

        private void Awake()
        {
            AudioController.Instance.SetUserSetting(_userSetting);
            _menuUIController = new MenuUIController(this, _menuCanvas);
            _settingUIController = new SettingUIController(this, _settingCanvas, _defaultSetting, _userSetting);
            inputKeycodes = new List<KeyCode>();
            DataSystem.Instance.OnLoadData();
        }

        private void Start()
        {
            ShowMenu();
            AudioController.Instance.UpdateAudioVolume();
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

                if (string.Join(',', inputKeycodes) == string.Join(',', AddMoneyCode))
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
            if (DebugTime < 0)
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
            _settingUIController.HideCanvas();
        }

        public void ShowGameOptions()
        {
            LoadingScreen.instance.LoadScene("01_MenuScene", false, true);
        }

        public void ShowSetting()
        {
            _menuUIController.HideCanvas();
            _settingUIController.ShowCanvas();
        }
    }
}