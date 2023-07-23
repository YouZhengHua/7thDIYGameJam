using Scripts.Game.Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scripts.Game
{
    public class OptionsUIController : IOptionsUIController
    {
        private IList<OptionData> _optionDatas;
        private IList<OptionData> _canSelectedOptionDatas;
        private IList<OptionData> _showOptionDatas;
        private IOptionController[] _options;
        private Canvas _canvas;

        public OptionsUIController(GameObject prefab, IList<OptionData> optionDatas)
        {
            _optionDatas = optionDatas;
            _canSelectedOptionDatas = new List<OptionData>();

            foreach (Canvas canvas in GameObject.FindObjectsOfType<Canvas>())
            {
                if (canvas.name == "GameOptionsUICanvas")
                    _canvas = canvas;
            }

            _options = new IOptionController[3];
            for(int i = 0; i < _options.Length; i++)
            {
                _options[i] = GameObject.Instantiate(prefab, _canvas.transform).GetComponent<IOptionController>();
                _options[i].transform.localPosition = new Vector3((-520f + i * 520f), 60f, 0);
                _options[i].SetOptionsUI = this;
            }

            _showOptionDatas = new List<OptionData>();
        }

        public void ShowCanvas()
        {
            UpdateShowOptions();
            for(int i = 0; i < _options.Length; i++)
            {
                _options[i].SetOptionData(_showOptionDatas[i]);
            }
            GameStateMachine.Instance.SetNextState(GameState.SelectOption);
            _canvas.gameObject.SetActive(true);
        }

        public void ShowWeaponOptions()
        {
            IList<OptionData> _weaponOptions = new List<OptionData>();
            foreach(OptionData option in _optionDatas)
            {
                if(option.OptionType == OptionType.Weapon && !option.IsEndOption)
                {
                    _weaponOptions.Add(option);
                }
            }
            for (int i = 0; i < _options.Length; i++)
            {
                if (_weaponOptions.Count > 0)
                {
                    OptionData data = _weaponOptions[Random.Range(0, _weaponOptions.Count)];
                    _weaponOptions.Remove(data);
                    _options[i].SetOptionData(data);
                }
            }

            GameStateMachine.Instance.SetNextState(GameState.SelectOption);
            _canvas.gameObject.SetActive(true);
        }

        /// <summary>
        /// 更新升級選項內容
        /// </summary>
        private void UpdateShowOptions()
        {
            _showOptionDatas.Clear();
            UpdateSelectedOptions();
            for (int i = 0; i < _options.Length; i++)
            {
                if (_canSelectedOptionDatas.Count > 0)
                {
                    OptionData data = _canSelectedOptionDatas[Random.Range(0, _canSelectedOptionDatas.Count)];
                    _canSelectedOptionDatas.Remove(data);
                    _showOptionDatas.Add(data);
                }
            }
        }

        /// <summary>
        /// 更新可選擇的升級選項清單
        /// </summary>
        private void UpdateSelectedOptions()
        {
            _canSelectedOptionDatas.Clear();
            // 將未達最大選取次數 及 不是最終選項的 升級選項加入選項清單中
            foreach(OptionData option in _optionDatas)
            {
                if (!option.IsSelectedMax && !option.IsEndOption)
                    _canSelectedOptionDatas.Add(option);
            }

            // 如果選項清單的數量少於畫面可選的數量，就再把 未達最大選取次數 及 是最終選項 的升級選項加入清單中
            if(_canSelectedOptionDatas.Count < _options.Length)
            {
                foreach (OptionData option in _optionDatas)
                {
                    if (!option.IsSelectedMax && option.IsEndOption)
                        _canSelectedOptionDatas.Add(option);
                }
            }
        }

        public void HideCanvas()
        {
            _canvas.gameObject.SetActive(false);
        }

        public void OptionOnClick(OptionData data)
        {
            if (data.OptionType == OptionType.Attribute)
            {
                AttributeHandle.Instance.UpdateAttribute((AttributeOptionData)data);
            }
            else if(data.OptionType == OptionType.Weapon)
            {
                AttributeHandle.Instance.UpdateWeapon((WeaponOptionData)data);
            }
            data.SelectedCount += 1;
            HideCanvas();
            GameStateMachine.Instance.SetNextState(GameState.InGame);
        }
    }
}
