using System;
using System.Collections;
using UnityEngine;

namespace Scripts
{
    public interface IAudioContoller
    {
        public void PlayEffect(AudioClip audioClip, float extendVolume = 0f);
        public void UpdateAudioVolume();
    }
}