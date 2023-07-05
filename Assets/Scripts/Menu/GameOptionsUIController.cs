using Scripts.Game.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Scripts.Game;
using Scripts.Base;

namespace Scripts.Menu
{
    public class GameOptionsUIController : BaseUIController, IGameOptionsUIController
    {
        private IMenuManager _menuManager;
        private Button _gameStartButton;
        private Button _backToMenuButton;
        private Button _buyGunButton;
        private ToggleGroup _gunIndexGroup;
        private int _selectedGunIndex = 0;
        private GunData _gunData;
        private TextMeshProUGUI _score;
        private TextMeshProUGUI _gunDescription;
        private GameObject _gunOptionPrefab;
        private IList<GunData> _gunDatas;
        private string _gunDescriptionFormat;

        public GameOptionsUIController(IMenuManager menu, Canvas canvas, GameObject gunOptionPrefab, IList<GunData> gunDatas) : base(canvas)
        {
            _menuManager = menu;
            _gunOptionPrefab = gunOptionPrefab;
            _gunIndexGroup = _canvas.GetComponentInChildren<ToggleGroup>();
            _gunDatas = gunDatas;
            foreach (Button obj in _canvas.GetComponentsInChildren<Button>())
            {
                if (obj.name == "GameStartButton")
                    _gameStartButton = obj;
                else if (obj.name == "BackToMenuButton")
                    _backToMenuButton = obj;
                else if (obj.name == "BuyGunButton")
                    _buyGunButton = obj;
            }

            foreach (TextMeshProUGUI obj in _canvas.GetComponentsInChildren<TextMeshProUGUI>())
            {
                if (obj.name == "GunDescription")
                    _gunDescription = obj;
                else if (obj.name == "ScoreText")
                    _score = obj;
            }

            _gunDescriptionFormat = _gunDescription.text;

            foreach (GunData gunData in _gunDatas)
            {
                int index = _gunDatas.IndexOf(gunData);
                GameObject toggle = GameObject.Instantiate(_gunOptionPrefab, _gunIndexGroup.transform);
                toggle.transform.localPosition += new Vector3(index * 125, 0, 0);
                toggle.GetComponent<Toggle>().onValueChanged.AddListener(GunOptionOnChange);
                toggle.GetComponent<Toggle>().group = _gunIndexGroup;
                foreach (Image image in toggle.GetComponentsInChildren<Image>())
                {
                    if (image.name == "Gun")
                    {
                        image.sprite = gunData.GunSprite;
                        image.rectTransform.localScale = gunData.OptionScale;
                        image.rectTransform.localRotation = Quaternion.Euler(gunData.OptionRotation);
                    }
                    else if (image.name == "Coin")
                    {
                        image.enabled = !StaticPrefs.GetGunIsUnlocked(gunData.GunIndex, gunData.DefaultIsUnlocked);
                    }
                }
                toggle.name = $"GunOption {((int)gunData.GunIndex)}";
                toggle.GetComponent<Toggle>().isOn = index == 0;
            }

            _gameStartButton.onClick.AddListener(StartButtonOnClick);
            _backToMenuButton.onClick.AddListener(ExitButtonOnClick);
            _buyGunButton.onClick.AddListener(BuyGunButtonOnClick);
        }

        private void GunOptionOnChange(bool isSelected)
        {
            if (isSelected)
            {
                UpdateGunOptionDescription();
            }
        }

        private void UpdateGunOptionDescription()
        {
            SetSelectedGunData();
            if (_gunData == null)
                return;
            string gunDescription = _gunDescriptionFormat;
            gunDescription = gunDescription.Replace("{GunName}", _gunData.Name);
            if (_gunData.OneShootAmmoCount > 1)
            {
                gunDescription = gunDescription.Replace("{Dmage}", $"{_gunData.Damage} * {_gunData.OneShootAmmoCount}");
            }
            else
            {
                gunDescription = gunDescription.Replace("{Dmage}", _gunData.Damage.ToString());
            }
            gunDescription = gunDescription.Replace("{ShootCountPerSecond}", _gunData.ShootCountPreSecond.ToString());
            gunDescription = gunDescription.Replace("{MagazineSize}", _gunData.MagazineSize.ToString());
            gunDescription = gunDescription.Replace("{MagazineCount}", _gunData.MagazineCount.ToString());
            gunDescription = gunDescription.Replace("{ReloadTime}", _gunData.ReloadTimeLevel);
            gunDescription = gunDescription.Replace("{Repel}", _gunData.RepleLevel);
            IList<string> shootTypeCht = new List<string>();
            foreach(ShootType shootType in _gunData.ShootTypes)
            {
                switch (shootType)
                {
                    case ShootType.One:
                        shootTypeCht.Add("單發");
                        break;
                    case ShootType.Three:
                        shootTypeCht.Add("三連發");
                        break;
                    case ShootType.Auto:
                        shootTypeCht.Add("全自動射擊");
                        break;
                }
            }
            gunDescription = gunDescription.Replace("{ShootTypes}", string.Join('、', shootTypeCht));
            gunDescription = gunDescription.Replace("{AmmoDropRate}", (_gunData.DropAmmoRate * 100f).ToString());
            gunDescription = gunDescription.Replace("{AmmoBox}", _gunData.AmmoBoxRate.ToString());
            _gunDescription.text = gunDescription;

            _gameStartButton.gameObject.SetActive(StaticPrefs.GetGunIsUnlocked(_gunData.GunIndex, _gunData.DefaultIsUnlocked));
            _buyGunButton.gameObject.SetActive(!StaticPrefs.GetGunIsUnlocked(_gunData.GunIndex, _gunData.DefaultIsUnlocked));
            _buyGunButton.interactable = StaticPrefs.Score >= _gunData.Price;
            if (StaticPrefs.Score >= _gunData.Price)
                _buyGunButton.GetComponentInChildren<TextMeshProUGUI>().text = $"${_gunData.Price}\n購買槍枝";
            else
                _buyGunButton.GetComponentInChildren<TextMeshProUGUI>().text = $"${_gunData.Price}\n餘額不足";
        }

        private void StartButtonOnClick()
        {
            Debug.Log("Game Sence Loading...");
            if(_gunData != null)
                StaticPrefs.GunIndex = (int)_gunData.GunIndex;
            _gameStartButton.interactable = false;
            LoadingScreen.instance.LoadScene("GameScene", false);
        }

        public void ReflashToggleGroup()
        {
            foreach (Toggle toggle in _gunIndexGroup.GetComponentsInChildren<Toggle>())
            {
                int toggleIndex = int.Parse(toggle.name.Split(' ')[1]);
                foreach(GunData gunData in _gunDatas)
                {
                    if((int)gunData.GunIndex == toggleIndex)
                    {
                        foreach (Image image in toggle.GetComponentsInChildren<Image>())
                        {
                            if (image.name == "Coin")
                            {
                                image.enabled = !StaticPrefs.GetGunIsUnlocked(gunData.GunIndex, gunData.DefaultIsUnlocked);
                            }
                        }
                    }
                }
            }
        }

        private void BuyGunButtonOnClick()
        {
            StaticPrefs.Score -= _gunData.Price;
            StaticPrefs.SetGunIsUnlocked(_gunData.GunIndex, true);
            ReflashToggleGroup();
            UpdateSocre();
            UpdateGunOptionDescription();
        }

        public override void ShowCanvas()
        {
            base.ShowCanvas();
            UpdateGunOptionDescription();
            UpdateSocre();
            ReflashToggleGroup();
        }

        private void SetSelectedGunData()
        {
            foreach (Toggle toggle in _gunIndexGroup.ActiveToggles())
            {
                _selectedGunIndex = int.Parse(toggle.name.Split(' ')[1]);
                foreach (GunData data in _gunDatas)
                {
                    if ((int)data.GunIndex == _selectedGunIndex)
                    {
                        _gunData = data;
                    }
                }
            }
            if (_gunData == null && _gunDatas.Count > 0)
                _gunData = _gunDatas[0];
        }

        private void UpdateSocre()
        {
            _score.text = $"${Mathf.RoundToInt(StaticPrefs.Score)}";
        }

        private void ExitButtonOnClick()
        {
            _menuManager.ShowMenu();
        }
    }
}
