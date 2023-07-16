using Scripts.Game.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Scripts.Base;

namespace Scripts.Game
{
    public class SettingUIController : BaseUIController, ISettingUIController
    {
        private Button _defaultButton;
        private Button _backToMenuButton;
        private Button _saveButton;
        private UserSetting _defaultSetting;
        private UserSetting _userSetting;
        private UserSetting _tmpSetting;
        private SliderObject _music;
        private SliderObject _sound;

        public SettingUIController(Canvas canvas, UserSetting defaultSetting, UserSetting userSetting) : base(canvas)
        {
            _defaultSetting = defaultSetting;
            _userSetting = userSetting;
            _music = new SliderObject(GameObject.Find("Music"));
            _sound = new SliderObject(GameObject.Find("Sound"));

            foreach (Button obj in _canvas.GetComponentsInChildren<Button>())
            {
                if (obj.name == "DefaultButton")
                    _defaultButton = obj;
                else if (obj.name == "BackToMenuButton")
                    _backToMenuButton = obj;
                else if (obj.name == "SaveButton")
                    _saveButton = obj;
            }

            _defaultButton.onClick.AddListener(DefaultButtonOnClick);
            _backToMenuButton.onClick.AddListener(BackToMenuButtonOnClick);
            _saveButton.onClick.AddListener(SaveButtonOnClick);
            _music.SetListiner(OnMusicValueOnChange);
        }

        private void DefaultButtonOnClick()
        {
            ResetUserSetting();
            ReflashCanvas();
        }

        private void SaveButtonOnClick()
        {
            _userSetting.soundVolume = _sound.Value;
            _userSetting.musicVolume = _music.Value;

            AudioContoller.Instance.UpdateAudioVolume();
            this.HideCanvas();
        }

        private void BackToMenuButtonOnClick()
        {
            _userSetting.soundVolume = _tmpSetting.soundVolume;
            _userSetting.musicVolume = _tmpSetting.musicVolume;

            _userSetting.ChangeShootType = _tmpSetting.ChangeShootType;
            _userSetting.MeleeAttack = _tmpSetting.MeleeAttack;
            _userSetting.Reload = _tmpSetting.Reload;
            _userSetting.Shoot = _tmpSetting.Shoot;

            AudioContoller.Instance.UpdateAudioVolume();
            this.HideCanvas();
        }

        public override void ShowCanvas()
        {
            base.ShowCanvas();
            ReflashCanvas();

            _tmpSetting = Object.Instantiate(_userSetting);
        }

        private void ReflashCanvas()
        {
            _music.Value = _userSetting.musicVolume;
            _sound.Value = _userSetting.soundVolume;
        }

        private void OnMusicValueOnChange()
        {
            _userSetting.musicVolume = _music.Value;
            AudioContoller.Instance.UpdateAudioVolume();
        }

        public void ResetUserSetting()
        {
            _userSetting.soundVolume = _defaultSetting.soundVolume;
            _userSetting.musicVolume = _defaultSetting.musicVolume;

            _userSetting.ChangeShootType = _defaultSetting.ChangeShootType;
            _userSetting.MeleeAttack = _defaultSetting.MeleeAttack;
            _userSetting.Reload = _defaultSetting.Reload;
            _userSetting.Shoot = _defaultSetting.Shoot;
        }
    }
}
