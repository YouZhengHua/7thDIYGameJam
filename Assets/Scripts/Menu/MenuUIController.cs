﻿using UnityEngine;
using UnityEngine.UI;
using Scripts.Base;

namespace Scripts.Menu
{
    public class MenuUIController : BaseUIController, IMenuUIController
    {
        private IMenuManager _menuManager;
        private Button _startButton;
        private Button _exitButton;
        private Button _settingButton;

        public MenuUIController(IMenuManager menu, Canvas canvas) : base(canvas)
        {
            _menuManager = menu;

            foreach (Button obj in _canvas.GetComponentsInChildren<Button>())
            {
                if (obj.name == "StartButton")
                    _startButton = obj;
                else if (obj.name == "ExitButton")
                    _exitButton = obj;
                else if (obj.name == "SettingButton")
                    _settingButton = obj;
            }

            _startButton.onClick.AddListener(StartButtonOnClick);
            _exitButton.onClick.AddListener(ExitButtonOnClick);
            _settingButton.onClick.AddListener(SettingButtonOnClick);
        }

        public void StartButtonOnClick()
        {
            _menuManager.ShowGameOptions();
        }

        public void ExitButtonOnClick()
        {
            Application.Quit();
        }

        public void SettingButtonOnClick()
        {
            _menuManager.ShowSetting();
        }
    }
}