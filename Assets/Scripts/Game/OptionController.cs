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
            _text.text = data.depiction;
        }

        public IOptionsUIController SetOptionsUI { set => _optionsUI = value; }
    }
}
