using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scripts.Game
{
    public class PauseUIController : IPauseUIController
    {
        private IGameFiniteStateMachine _gameStateMachine;
        private ISettingUIController _settingUIController;
        private Button _backToGameButton;
        private Button _backToMenuButton;
        private Button _restateGameButton;
        private Button _settingGameButton;
        private Canvas _canvas;

        public PauseUIController(IGameFiniteStateMachine gameStateMachine, ISettingUIController settingUIController)
        {
            _gameStateMachine = gameStateMachine;
            _settingUIController = settingUIController;
            foreach (Canvas canvas in GameObject.FindObjectsOfType<Canvas>())
            {
                if (canvas.name == "PauseUICanvas")
                    _canvas = canvas;
            }

            foreach (Button button in _canvas.GetComponentsInChildren<Button>())
            {
                if (button.name == "PauseUIBackToGameButton")
                    _backToGameButton = button;
                else if (button.name == "PauseUIBackToMenuButton")
                    _backToMenuButton = button;
                else if (button.name == "PauseUIRestartGameButton")
                    _restateGameButton = button;
                else if (button.name == "PauseUISettingButton")
                    _settingGameButton = button;
            }

            _backToGameButton.onClick.AddListener(BackToGameButtonOnClick);
            _backToMenuButton.onClick.AddListener(BackToMenuButtonOnClick);
            _restateGameButton.onClick.AddListener(RestrartMenuButtonOnClick);
            _settingGameButton.onClick.AddListener(SettingButtonOnClick);
        }

        /// <summary>
        /// 離開事件觸發內容
        /// </summary>
        private void BackToMenuButtonOnClick()
        {
            _backToMenuButton.interactable = false;
            _gameStateMachine.SetNextState(GameState.BackToMenu);
        }

        private void BackToGameButtonOnClick()
        {
            HideCanvas();
            _gameStateMachine.SetNextState(GameState.InGame);
        }

        private void RestrartMenuButtonOnClick()
        {
            _restateGameButton.interactable = false;
            _gameStateMachine.SetNextState(GameState.Restart);
        }

        private void SettingButtonOnClick()
        {
            _settingUIController.ShowCanvas();
        }

        public void ShowCanvas()
        {
            _canvas.gameObject.SetActive(true);
            _backToGameButton.gameObject.SetActive(_gameStateMachine.CurrectState != GameState.GamePause);
        }

        public void HideCanvas()
        {
            _canvas.gameObject.SetActive(false);
        }
    }
}