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
        private IGameFiniteStateMachine _gameFiniteStateMachine;
        private IAttributeHandle _attributeHandle;
        private IList<OptionData> _optionDatas;
        private IList<OptionData> _canSelectedOptionDatas;
        private IList<OptionData> _showOptionDatas;
        private IOptionController[] _options;
        private Canvas _canvas;

        public OptionsUIController(IGameFiniteStateMachine gameFiniteStateMachine, IAttributeHandle attributeHandle , GameObject prefab, IList<OptionData> optionDatas)
        {
            _gameFiniteStateMachine = gameFiniteStateMachine;
            _attributeHandle = attributeHandle;
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
                _options[i].transform.position = _options[i].transform.position + new Vector3((-400f + i * 400f) * StaticPrefs.Scale, 0, 0);
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
            _gameFiniteStateMachine.SetNextState(GameState.SelectOption);
            _canvas.gameObject.SetActive(true);
        }

        public void ShowWeaponOptions()
        {
            IList<OptionData> _weaponOptions = new List<OptionData>();
            foreach(OptionData option in _optionDatas)
            {
                if(option.optionType == OptionType.Weapon)
                {
                    _weaponOptions.Add(option);
                }
            }
            for (int i = 0; i < _options.Length; i++)
            {
                OptionData data = _weaponOptions[Random.Range(0, _weaponOptions.Count)];
                _weaponOptions.Remove(data);
                _options[i].SetOptionData(data);
            }

            _gameFiniteStateMachine.SetNextState(GameState.SelectOption);
            _canvas.gameObject.SetActive(true);
        }

        private void UpdateShowOptions()
        {
            _showOptionDatas.Clear();
            UpdateSelectedOptions();
            for (int i = 0; i < _options.Length; i++)
            {
                OptionData data = _canSelectedOptionDatas[Random.Range(0, _canSelectedOptionDatas.Count)];
                _canSelectedOptionDatas.Remove(data);
                _showOptionDatas.Add(data);
            }
        }

        private void UpdateSelectedOptions()
        {
            _canSelectedOptionDatas.Clear();
            foreach(OptionData option in _optionDatas)
            {
                if (!option.IsSelectedMax && !option.IsEndOption
                        && (option.optionAttribute != OptionAttribute.PlayerHeal 
                            || (option.optionAttribute == OptionAttribute.PlayerHeal && _attributeHandle.PlayerHealthPoint < _attributeHandle.PlayerMaxHealthPoint)
                        )
                    )
                    _canSelectedOptionDatas.Add(option);
            }
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
            _attributeHandle.UpdateAttribute(data);
            data.selectedCount += 1;
            HideCanvas();
            _gameFiniteStateMachine.SetNextState(GameState.InGame);
        }
    }
}
