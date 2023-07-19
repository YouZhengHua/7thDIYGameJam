using Scripts.Game.Data;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Scripts.Game
{
    public class OptionController : MonoBehaviour, IOptionController
    {
        private Image _background;
        private Button _button;
        private TextMeshProUGUI _text;
        private IOptionsUIController _optionsUI;
        private OptionData _data;

        private void Awake()
        {
            _background = gameObject.GetComponent<Image>();
            _button = gameObject.GetComponent<Button>();
            _text = gameObject.GetComponentInChildren<TextMeshProUGUI>();
            _button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            _optionsUI.OptionOnClick(_data);
        }

        public void SetOptionData(OptionData data)
        {
            _data = data;
            if(data.OptionType == OptionType.Weapon)
            {
                if(data.SelectedCount > 0)
                {
                    _text.text = $"{data.Depiction}\n{((WeaponOptionData)data).WeaponUpdateAttributes[data.SelectedCount - 1].AttributeDepiction}";
                }
                else
                {
                    _text.text = $"解鎖武器\n{data.Depiction}";
                }
            }
            else
            {
                _text.text = data.Depiction;
            }
        }

        public IOptionsUIController SetOptionsUI { set => _optionsUI = value; }
    }
}
