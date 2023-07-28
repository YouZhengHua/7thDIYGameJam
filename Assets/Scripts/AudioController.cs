using Scripts.Game.Data;
using UnityEngine;
using UnityEngine.Audio;

namespace Scripts
{
    public class AudioController
    {
        private AudioSource _musicAudio;
        private AudioSource _soundAudio;
        private UserSetting _userSetting;
        private AudioMixer _audioMixer;
        private readonly float maxDb = 20f;
        private readonly float minDb = -65f;
        private static readonly object padlock = new object();
        public static AudioController _instance = null;
        public static AudioController Instance
        {
            get
            {
                lock (padlock)
                {
                    if (_instance == null)
                        _instance = new AudioController();
                    return _instance;
                }
            }
        }
        public AudioController()
        {
            _audioMixer = Resources.Load<AudioMixer>("Audio/AudioMixer");
        }

        private void Init()
        {
            _musicAudio = GameObject.Find("MusicAudioSource").GetComponent<AudioSource>();
            _soundAudio = GameObject.Find("SoundAudioSource").GetComponent<AudioSource>();
        }

        public void SetUserSetting(UserSetting userSetting)
        {
            _userSetting = userSetting;
            Init();
        }

        public void UpdateAudioVolume()
        {
            _audioMixer.SetFloat("Music", ToVolumeDb(_userSetting.musicVolume));
            _audioMixer.SetFloat("Sound", ToVolumeDb(_userSetting.soundVolume));
        }

        public void PlayEffect(AudioClip audioClip, float extendVolume = 0f)
        {
            _soundAudio.PlayOneShot(audioClip, (1f + extendVolume));
        }

        private float ToVolumeDb(float volume)
        {
            return minDb + ((maxDb - minDb) * volume / 10f);
        }

        public void SetBGM(AudioClip bgm, float? volumn = null)
        {
            _musicAudio.clip = bgm;
            if (volumn.HasValue)
                _musicAudio.volume = volumn.Value;
            _musicAudio.Play();
        }
    }
}