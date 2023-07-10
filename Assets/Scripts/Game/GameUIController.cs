﻿using Scripts.Base;
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
        private IAttributeHandle _attributeHandle;
        private Image _expRateImage;
        private Image _hpRateImage;
        private IOptionsUIController _optionsUI;
        private IAudioContoller _audio;

        public GameUIController(IAttributeHandle attributeHandle, IOptionsUIController optionsUI, IAudioContoller audio, Canvas canvas) : base(canvas)
        {
            _attributeHandle = attributeHandle;
            _optionsUI = optionsUI;
            _audio = audio;
            foreach (Image image in GameObject.FindObjectsOfType<Image>())
            {
                if (image.gameObject.name == "ExpRate")
                    _expRateImage = image;
                if (image.gameObject.name == "HpRate")
                    _hpRateImage = image;
            }

            foreach (TextMeshProUGUI text in GameObject.FindObjectsOfType<TextMeshProUGUI>())
            {
                if (text.gameObject.name == "GameTime")
                    _gameTime = text;
                if (text.gameObject.name == "HpText")
                    _hpText = text;
            }
        }

        /// <summary>
        /// 更新 UI 的血量資訊
        /// </summary>
        public void UpdatePlayerHealth()
        {
            _hpText.text = $"{_attributeHandle.PlayerHealthPoint} / {_attributeHandle.PlayerMaxHealthPoint}";
            _hpRateImage.fillAmount = _attributeHandle.PlayerHealthPoint / _attributeHandle.PlayerMaxHealthPoint;
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
            _attributeHandle.AddExp((float)exp);
            if(_attributeHandle.IsLevelUp)
            {
                _attributeHandle.NowExp -= _attributeHandle.NextLevelExp;
                _attributeHandle.Level += 1;
                _audio.PlayEffect(_attributeHandle.LevelUpAudio, 0.5f);
                _optionsUI.ShowCanvas();
            }
            UpdateExpGUI();
        }

        /// <summary>
        /// 更新 UI 的經驗條長度
        /// </summary>
        private void UpdateExpGUI()
        {
            _expRateImage.fillAmount = _attributeHandle.ExpPercentage;
        }

        /// <summary>
        /// 玩家接收到治療
        /// </summary>
        /// <param name="healPoint"></param>
        public void HealPlayer(int healPoint)
        {
            if(_attributeHandle.PlayerHealthPoint < _attributeHandle.PlayerMaxHealthPoint)
            {
                _attributeHandle.PlayerHealthPoint = Mathf.Min(_attributeHandle.PlayerHealthPoint + healPoint, _attributeHandle.PlayerMaxHealthPoint);
                UpdatePlayerHealth();
            }
        }

        /// <summary>
        /// 玩家增加最大血量
        /// </summary>
        /// <param name="healthPoint"></param>
        public void AddPlayerHealthPointMax(int healthPoint)
        {
            _attributeHandle.PlayerMaxHealthPoint += healthPoint;
            _attributeHandle.PlayerHealthPoint += healthPoint;
            UpdatePlayerHealth();
        }
    }
}
