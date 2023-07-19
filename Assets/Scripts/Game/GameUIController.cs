using Scripts.Base;
using Scripts.Game.Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scripts.Game
{
    public class GameUIController : BaseUIController, IGameUIController
    {
        private TextMeshProUGUI _gameTime;
        private TextMeshProUGUI _hpText;
        private TextMeshProUGUI _shieldText;
        private Image _expRateImage;
        private Image _hpRateImage;
        private Image _shieldRateImage;
        private GameObject _shieldContainer;
        private IOptionsUIController _optionsUI;

        public GameUIController(IOptionsUIController optionsUI, Canvas canvas) : base(canvas)
        {
            _optionsUI = optionsUI;
            foreach (Image image in GameObject.FindObjectsOfType<Image>())
            {
                if (image.gameObject.name == "ExpRate")
                    _expRateImage = image;
                else if (image.gameObject.name == "HpRate")
                    _hpRateImage = image;
                else if (image.gameObject.name == "ShieldRate")
                    _shieldRateImage = image;
            }

            foreach (TextMeshProUGUI text in GameObject.FindObjectsOfType<TextMeshProUGUI>())
            {
                if (text.gameObject.name == "GameTime")
                    _gameTime = text;
                else if (text.gameObject.name == "HpText")
                    _hpText = text;
                else if (text.gameObject.name == "ShieldText")
                    _shieldText = text;
            }

            _shieldContainer = GameObject.Find("ShieldContainer");
        }

        /// <summary>
        /// 更新 UI 的血量資訊
        /// </summary>
        public void UpdatePlayerHealth()
        {
            _hpText.text = $"{AttributeHandle.Instance.PlayerHealthPoint} / {AttributeHandle.Instance.PlayerMaxHealthPoint}";
            _hpRateImage.fillAmount = AttributeHandle.Instance.PlayerHealthPoint / AttributeHandle.Instance.PlayerMaxHealthPoint;

            _shieldContainer.SetActive(AttributeHandle.Instance.PlayerShield > 0);
            if (AttributeHandle.Instance.PlayerShield > 0)
            {
                _shieldText.text = $"{AttributeHandle.Instance.PlayerShield} / {AttributeHandle.Instance.PlayerMaxShield}";
                _shieldRateImage.fillAmount = AttributeHandle.Instance.PlayerShield / AttributeHandle.Instance.PlayerMaxShield;
            }
        }

        /// <summary>
        /// 更新 UI 的遊戲時間
        /// </summary>
        /// <param name="gameTime"></param>
        public void UpdateGameTime(float gameTime)
        {
            int showTime = Mathf.Max(Mathf.FloorToInt(gameTime), 0);
            int min = Mathf.FloorToInt(showTime / 60);
            int sec = showTime % 60;
            _gameTime.text = string.Format("{0}:{1:00}", min, sec);
        }

        /// <summary>
        /// 取得經驗值
        /// </summary>
        /// <param name="exp"></param>
        public void GetExp(ExpNumber exp)
        {
            AttributeHandle.Instance.AddExp((float)exp);
            if(AttributeHandle.Instance.IsLevelUp)
            {
                AttributeHandle.Instance.NowExp -= AttributeHandle.Instance.NextLevelExp;
                AttributeHandle.Instance.Level += 1;
                AudioContoller.Instance.PlayEffect(AttributeHandle.Instance.LevelUpAudio, 0.5f);
                _optionsUI.ShowCanvas();
            }
            UpdateExpGUI();
        }

        /// <summary>
        /// 更新 UI 的經驗條長度
        /// </summary>
        private void UpdateExpGUI()
        {
            _expRateImage.fillAmount = AttributeHandle.Instance.ExpPercentage;
        }
    }
}
