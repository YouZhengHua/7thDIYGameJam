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
        private TextMeshProUGUI _money;
        private Image _expRateImage;
        private Image _hpRateImage;
        private GameObject _shieldContainer;
        private GameObject _shieldIcon;
        private Sprite _shieldActive;
        private Sprite _shieldUnactive;
        private IOptionsUIController _optionsUI;
        private IList<GameObject> _shieldList;

        public GameUIController(IOptionsUIController optionsUI, Canvas canvas, GameObject shieldIcon, Sprite shieldActive, Sprite shieldUnactive) : base(canvas)
        {
            _optionsUI = optionsUI;
            _shieldIcon = shieldIcon;
            _shieldList = new List<GameObject>();
            _shieldActive = shieldActive;
            _shieldUnactive = shieldUnactive;
            foreach (Image image in GameObject.FindObjectsOfType<Image>())
            {
                if (image.gameObject.name == "ExpRate")
                    _expRateImage = image;
                else if (image.gameObject.name == "HpRate")
                    _hpRateImage = image;
            }

            foreach (TextMeshProUGUI text in GameObject.FindObjectsOfType<TextMeshProUGUI>())
            {
                if (text.gameObject.name == "GameTime")
                    _gameTime = text;
                else if (text.gameObject.name == "MoneyText")
                    _money = text;
            }

            _shieldContainer = GameObject.Find("ShieldContainer");

            for(int i = 0; i < AttributeHandle.Instance.PlayerMaxShield; i++)
            {
                GameObject shield = GameObject.Instantiate(_shieldIcon, _shieldContainer.transform);
                shield.transform.localPosition = new Vector3(-400f + i * 80f, -64f, 0f);
                _shieldList.Add(shield);
            }
        }

        /// <summary>
        /// 更新 UI 的血量資訊
        /// </summary>
        public void UpdatePlayerHealth()
        {
            _hpRateImage.fillAmount = AttributeHandle.Instance.PlayerHealthPoint / AttributeHandle.Instance.PlayerMaxHealthPoint;

            for(int i = 0; i < Mathf.Max(AttributeHandle.Instance.PlayerShield, _shieldList.Count); i++)
            {
                if(_shieldList.Count > i)
                {
                    GameObject shield = _shieldList[i];
                    shield.GetComponent<Image>().sprite = i < AttributeHandle.Instance.PlayerShield ? _shieldActive : _shieldUnactive;
                }
                else
                {
                    GameObject shield = GameObject.Instantiate(_shieldIcon, _shieldContainer.transform);
                    shield.transform.localPosition = new Vector3(-400f + i * 80f, -64f, 0f);
                    _shieldList.Add(shield);
                }
            }
        }

        /// <summary>
        /// 更新 UI 的遊戲時間
        /// </summary>
        /// <param name="gameTime"></param>
        public void UpdateGameTime(float gameTime, int enemyCount = 0)
        {
            int showTime = Mathf.Max(Mathf.FloorToInt(gameTime), 0);
            int min = Mathf.FloorToInt(showTime / 60);
            int sec = showTime % 60;
            if(min > 0 || sec > 0)
                _gameTime.text = string.Format("{0}:{1:00}", min, sec);
            else
                _gameTime.text = string.Format("剩餘怪物數量: {0}", enemyCount);
        }

        /// <summary>
        /// 更新 UI 的經驗條長度
        /// </summary>
        public void UpdateExpGUI()
        {
            if (AttributeHandle.Instance.IsLevelUp)
            {
                AttributeHandle.Instance.NowExp -= AttributeHandle.Instance.NextLevelExp;
                AttributeHandle.Instance.Level += 1;
                AudioController.Instance.PlayEffect(AttributeHandle.Instance.LevelUpAudio, 0.5f);
                _optionsUI.ShowCanvas();
            }
            _expRateImage.fillAmount = AttributeHandle.Instance.ExpPercentage;
        }

        public void UpdateMoneyGUI()
        {
            _money.text = CalTool.Round(AttributeHandle.Instance.TotalMoney).ToString();
        }
    }
}
