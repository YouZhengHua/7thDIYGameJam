using Scripts.Game.Data;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Scripts.Game
{
    public class OptionController : MonoBehaviour, IOptionController
    {
        private Image _background;
        private Image _icon;
        private Button _button;
        private TextMeshProUGUI _depiction;
        private TextMeshProUGUI _selectCount;
        private IOptionsUIController _optionsUI;
        private OptionData _data;

        private void Awake()
        {
            _background = gameObject.GetComponent<Image>();
            foreach(Image image in gameObject.GetComponentsInChildren<Image>())
            {
                if (image.name == "Icon")
                    _icon = image;
            }
            _button = gameObject.GetComponent<Button>();
            foreach (TextMeshProUGUI text in gameObject.GetComponentsInChildren<TextMeshProUGUI>())
            {
                if (text.name == "Depiction")
                    _depiction = text;
                else if (text.name == "SelectCount")
                    _selectCount = text;
            }
            _button.onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            _optionsUI.OptionOnClick(_data);
        }

        public void SetOptionData(OptionData data)
        {
            _data = data;
            _icon.enabled = data.Image != null;
            if (data.Image != null)
                _icon.sprite = data.Image;

            _depiction.text = $"{data.Title}\n{data.Depiction}";
            if (data.OptionType == OptionType.Weapon)
            {
                if (data.SelectedCount > 0)
                {
                    _depiction.text = $"{data.Title}\n{((WeaponOptionData)data).WeaponUpdateAttributes[data.SelectedCount - 1].AttributeDepiction}";
                }
            }

            if (data.IsLoopOption)
            {
                _selectCount.text = $"{data.SelectedCount}/-";
            }
            else
            {
                _selectCount.text = $"{data.SelectedCount}/{data.MaxSelectedCount}";
            }
        }

        public IOptionsUIController SetOptionsUI { set => _optionsUI = value; }
    }
}
