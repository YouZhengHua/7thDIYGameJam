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
        private GameObject _reloadUI;
        private Image _reloadImage;
        private TextMeshProUGUI _gameTime;
        private TextMeshProUGUI _ammoCount;
        private IAttributeHandle _attributeHandle;
        private GameObject _playerHealthObject;
        private IList<GameObject> _playerHealthImages = new List<GameObject>();
        private Image _ammoIcon;
        private Image _expRateImage;
        private IOptionsUIController _optionsUI;
        private IAudioContoller _audio;
        private Sprite _healthSprite;
        private Sprite _unhealthSprite;

        public GameUIController(GameObject playerHealth, IAttributeHandle attributeHandle, IOptionsUIController optionsUI, IAudioContoller audio, Sprite healthSprite, Sprite unhealthSprite, Canvas canvas) : base(canvas)
        {
            _reloadUI = GameObject.Find("ReloadUI");
            _attributeHandle = attributeHandle;
            _playerHealthObject = playerHealth;
            _optionsUI = optionsUI;
            _audio = audio;
            _healthSprite = healthSprite;
            _unhealthSprite = unhealthSprite;
            foreach (Image image in GameObject.FindObjectsOfType<Image>())
            {
                if(image.gameObject.name == "ReloadImage")
                    _reloadImage = image;
                if (image.gameObject.name == "AmmoIcon")
                    _ammoIcon = image;
                if (image.gameObject.name == "ExpRate")
                    _expRateImage = image;
            }

            foreach (TextMeshProUGUI text in GameObject.FindObjectsOfType<TextMeshProUGUI>())
            {
                if (text.gameObject.name == "GameTime")
                    _gameTime = text;
                if (text.gameObject.name == "AmmoCount")
                    _ammoCount = text;
            }

            HideReloadImage();

            for(int i = 0; i < _attributeHandle.PlayerMaxHealthPoint; i++)
            {
                CreateHealthImage(i);
            }

            _ammoIcon.sprite = _attributeHandle.AmmoSprite;
        }

        public void UpdatePlayerHealth()
        {
            for(int i = 0;  i < _attributeHandle.PlayerMaxHealthPoint; i++)
            {
                if (i < _attributeHandle.PlayerHealthPoint)
                {
                    GetPlayerHealthImage(i).sprite = _healthSprite;
                }
                else
                {
                    GetPlayerHealthImage(i).sprite = _unhealthSprite;
                }
            }
        }

        private Image GetPlayerHealthImage(int index)
        {
            if(index >= _playerHealthImages.Count)
            {
                CreateHealthImage(index);
            }
            return _playerHealthImages[index].GetComponent<Image>();
        }

        private void CreateHealthImage(int index)
        {
            GameObject healthObject = GameObject.Instantiate(_playerHealthObject, _canvas.transform);
            healthObject.transform.position = healthObject.transform.position + new Vector3((20f + index * 110f) * StaticPrefs.Scale, -20f * StaticPrefs.Scale, 0);
            _playerHealthImages.Add(healthObject);
        }

        public void ShowReloadImage()
        {
            _reloadImage.fillAmount = 1;
            _reloadUI.SetActive(true);
        }

        public void HideReloadImage()
        {
            _reloadUI.SetActive(false);
        }

        public void UpdateReloadImage(float reloatReate)
        {
            _reloadImage.fillAmount = reloatReate;
        }

        public void UpdateGameTime(float gameTime)
        {
            int showTime = Mathf.Max(Mathf.FloorToInt(gameTime), 0);
            int min = Mathf.FloorToInt(showTime / 60);
            int sec = showTime % 60;
            _gameTime.text = string.Format("{0}:{1:00}", min, sec);
        }

        public void UpdateAmmoCount()
        {
            _ammoCount.text = $"{_attributeHandle.NowAmmoCount} / {_attributeHandle.TotalAmmoCount}";
        }

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

        private void UpdateExpGUI()
        {
            _expRateImage.fillAmount = _attributeHandle.ExpPercentage;
        }

        public void HealPlayer(int healPoint)
        {
            if(_attributeHandle.PlayerHealthPoint < _attributeHandle.PlayerMaxHealthPoint)
            {
                _attributeHandle.PlayerHealthPoint += healPoint;
                UpdatePlayerHealth();
            }
        }

        public void AddPlayerHealthPointMax(int healthPoint)
        {
            _attributeHandle.PlayerMaxHealthPoint += healthPoint;
            _attributeHandle.PlayerHealthPoint += healthPoint;
            UpdatePlayerHealth();
        }
    }
}
