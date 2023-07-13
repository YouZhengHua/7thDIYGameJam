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
        private IAttributeHandle _attributeHandle;
        private Button _backToMenuButton;
        private Button _restateGameButton;
        private Canvas _canvas;
        private TextMeshProUGUI _levelText;
        private TextMeshProUGUI _shootCountText;
        private TextMeshProUGUI _getHitTimesText;
        private TextMeshProUGUI _hitEnemyCountText;
        private TextMeshProUGUI _killEnemyCountText;
        private TextMeshProUGUI _scoreText;
        private TextMeshProUGUI _gameResultText;
        private int _shootCount = 0;
        private int _getHitTimes = 0;
        private int _hitEnemyCount = 0;
        private int _killEnemyCountByGun = 0;
        private int _killEnemyCountByMelee = 0;
        private int _killBossCount = 0;
        private float _damage = 0;
        private Image _background;
        private readonly Color _winBackgroundColor = new (0.105f, 0.105f, 0.105f, 0.627f);
        private readonly Color _loseBackgroundColor = new (0.333f, 0.039f, 0.039f, 0.627f);
        private IList<int> _ammoGroups;

        public EndUIController(IAttributeHandle attributeHandle)
        {
            _attributeHandle = attributeHandle;
            _ammoGroups = new List<int>();

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

            _backToMenuButton.onClick.AddListener(BackToMenuButtonOnClick);
            _restateGameButton.onClick.AddListener(RestrartMenuButtonOnClick);

            foreach (TextMeshProUGUI text in GameObject.FindObjectsOfType<TextMeshProUGUI>())
            {
                if (text.gameObject.name == "Level")
                    _levelText = text;
                if (text.gameObject.name == "ShootCount")
                    _shootCountText = text;
                if (text.gameObject.name == "GetHitTimes")
                    _getHitTimesText = text;
                if (text.gameObject.name == "HitEnemyCount")
                    _hitEnemyCountText = text;
                if (text.gameObject.name == "KillEnemyCount")
                    _killEnemyCountText = text;
                if (text.gameObject.name == "GameResult")
                    _gameResultText = text;
                if (text.gameObject.name == "Score")
                    _scoreText = text;
            }

            foreach(Image image in _canvas.gameObject.GetComponentsInChildren<Image>())
            {
                if (image.gameObject.name == "DarkBackground")
                    _background = image;
            }

            _shootCount = 0;
            _getHitTimes = 0;
            _hitEnemyCount = 0;
            _killEnemyCountByGun = 0;
            _killEnemyCountByMelee = 0;
            _damage = 0f;
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
            _gameResultText.text = isWin ? "你活下來了" : "你已經死了";
            _background.color = isWin ? _winBackgroundColor : _loseBackgroundColor;
            UpdateScoreboard();
        }

        public void HideCanvas()
        {
            _canvas.gameObject.SetActive(false);
        }

        private void UpdateScoreboard()
        {
            _levelText.text = _attributeHandle.Level.ToString();
            _shootCountText.text = string.Format(_shootCount == 0 ? "未開槍" : "{0}%", HitRate);
            _getHitTimesText.text = string.Format("{0}", _killEnemyCountByMelee);
            _hitEnemyCountText.text = string.Format("{0}", KillEnemyCount);
            _killEnemyCountText.text = string.Format("{0}", _damage);
            _scoreText.text = Score.ToString();
            StaticPrefs.Score += Score;
        }

        private float HitRate { get => _shootCount == 0 ? 0 : Mathf.Round((float)_hitEnemyCount / (float)_shootCount * 10000f) / 100f; }

        private int KillEnemyCount { get => _killEnemyCountByGun + _killEnemyCountByMelee; }

        public void AddShootCount()
        {
            _shootCount += 1;
        }

        public void AddGetHitTimes()
        {
            _getHitTimes += 1;
        }

        public void AddHitEnemyCount(int ammoGroup)
        {
            if (!_ammoGroups.Contains(ammoGroup))
            {
                _ammoGroups.Add(ammoGroup);
                _hitEnemyCount += 1;
                if (_ammoGroups.Count > 10)
                    _ammoGroups.RemoveAt(0);
            }
        }

        public void AddKillEnemyCountByGun()
        {
            _killEnemyCountByGun += 1;
        }

        public void AddKillEnemyCountByMelee()
        {
            _killEnemyCountByMelee += 1;
        }

        public void AddKillBossCount()
        {
            _killBossCount += 1;
        }

        public void AddDamage(float value)
        {
            _damage += value;
        }

        private int Score { get => _killBossCount * 99 + this.KillEnemyCount; }
    }
}