using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scripts.Game
{
    public class EndUIController : IEndUIController
    {
        private Button _backToMenuButton;
        private Button _restateGameButton;
        private Canvas _canvas;
        private TextMeshProUGUI _killEnemyCountText;
        private TextMeshProUGUI _scoreText;
        private Image _background;
        private Transform _activeWeaponContainer;
        private GameObject _activeWeaponIconPrefab;
        private readonly Color _winBackgroundColor = new (0.105f, 0.105f, 0.105f, 1f);
        private readonly Color _loseBackgroundColor = new (0.333f, 0.039f, 0.039f, 1f);

        public EndUIController(GameObject activeWeaponIconPrefab)
        {
            _activeWeaponIconPrefab = activeWeaponIconPrefab;
            foreach (Canvas canvas in GameObject.FindObjectsOfType<Canvas>())
            {
                if (canvas.name == "EndUICanvas")
                    _canvas = canvas;
            }

            foreach (Button button in GameObject.FindObjectsOfType<Button>())
            {
                if (button.name == "EndUIBackToMenuButton")
                    _backToMenuButton = button;
                if (button.name == "EndUIRestartGameButton")
                    _restateGameButton = button;
            }

            foreach(Image image in _canvas.gameObject.GetComponentsInChildren<Image>())
            {
                if (image.gameObject.name == "DarkBackground")
                    _background = image;
            }

            _killEnemyCountText = GameObject.Find("KillCount").GetComponentInChildren<TextMeshProUGUI>();
            _scoreText = GameObject.Find("Score").GetComponentInChildren<TextMeshProUGUI>();
            _activeWeaponContainer = GameObject.Find("ActiveWeaponContainer").GetComponent<Transform>();

            _backToMenuButton.onClick.AddListener(BackToMenuButtonOnClick);
            _restateGameButton.onClick.AddListener(RestrartMenuButtonOnClick);

        }

        /// <summary>
        /// 離開事件觸發內容
        /// </summary>
        private void BackToMenuButtonOnClick()
        {
            _backToMenuButton.interactable = false;
            GameStateMachine.Instance.SetNextState(GameState.BackToMenu);
        }

        private void RestrartMenuButtonOnClick()
        {
            _restateGameButton.interactable = false;
            GameStateMachine.Instance.SetNextState(GameState.Restart);
        }

        public void ShowCanvas(bool isWin)
        {
            _canvas.gameObject.SetActive(true);
            _background.color = isWin ? _winBackgroundColor : _loseBackgroundColor;
            _restateGameButton.gameObject.SetActive(!isWin);
            _backToMenuButton.transform.localPosition = new Vector3(isWin ? 0f : _backToMenuButton.transform.localPosition.x, _backToMenuButton.transform.localPosition.y, _backToMenuButton.transform.localPosition.z);
            UpdateScoreboard();
        }

        public void HideCanvas()
        {
            _canvas.gameObject.SetActive(false);
        }

        private void UpdateScoreboard()
        {
            _killEnemyCountText.text = AttributeHandle.Instance.TotalKillCount.ToString();
            _scoreText.text = AttributeHandle.Instance.TotalMoney.ToString();
            StaticPrefs.Score += AttributeHandle.Instance.TotalMoney;
            float startX = (AttributeHandle.Instance.ActiveWeapons.Count - 1) * -50f;
            for (int i = 0; i < AttributeHandle.Instance.ActiveWeapons.Count; i++)
            {
                GameObject activeWeaponIcon = GameObject.Instantiate(_activeWeaponIconPrefab, _activeWeaponContainer);
                activeWeaponIcon.transform.localPosition = new Vector3(startX + (i * 100f), 0f, 0f);
                activeWeaponIcon.GetComponent<Image>().sprite = AttributeHandle.Instance.ActiveWeapons[i].Image;
            }
        }
    }
}