using Scripts.Game.Data;
using UnityEngine;
using UnityEngine.Audio;

namespace Scripts
{
    public class AudioContoller : IAudioContoller
    {
        private AudioSource _musicAudio;
        private AudioSource _soundAudio;
        private UserSetting _userSetting;
        private AudioMixer _audioMixer;
        private readonly float maxDb = 0f;
        private readonly float minDb = -65f;

        public AudioContoller(UserSetting userSetting)
        {
            _userSetting = userSetting;
            _audioMixer = Resources.Load<AudioMixer>("Audio/AudioMixer");
            _musicAudio = GameObject.Find("MusicAudioSource").GetComponent<AudioSource>();
            _soundAudio = GameObject.Find("SoundAudioSource").GetComponent<AudioSource>();
        }

        public void UpdateAudioVolume()
        {
            _audioMixer.SetFloat("Music", ToVolumeDb(_userSetting.musicVolume));
            _audioMixer.SetFloat("Sound", ToVolumeDb(_userSetting.soundVolume));
        }

        public void PlayEffect(AudioClip audioClip, float extendVolume)
        {
            _soundAudio.PlayOneShot(audioClip, (1f + extendVolume));
        }

        private float ToVolumeDb(float volume)
        {
            return minDb + ((maxDb - minDb) * volume / 10f);
        }
    }
}