using Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonButtonController : MonoBehaviour
{
    [Header("點擊時播放的音效")]
    public AudioClip ClickSound;
    public void PlayClickSound(float extendVolume = 0f)
    {
        if (ClickSound == null)
            return;
        AudioController.Instance.PlayEffect(ClickSound, extendVolume);
    }
}
