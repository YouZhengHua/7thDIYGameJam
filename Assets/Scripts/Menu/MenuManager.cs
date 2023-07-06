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
        ISettingUIController _settingUIController;
        IAudioContoller _audioContoller;

        private void Awake()
        {
            _gameOptionsUIController = new GameOptionsUIController(this, _gameOptionCanvas, _gunOptionPrefab, _gunDatas);
            _audioContoller = new AudioContoller(_userSetting);
            _settingUIController = new SettingUIController(this, _audioContoller, _settingCanvas, _defaultSetting, _userSetting);

            if (StaticPrefs.IsFirstIn)
                FirstOpenGame();

        }

        private void Start()
        {
            ShowGameOptions();
            _audioContoller.UpdateAudioVolume();
        }

        public void ShowGameOptions()
        {
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