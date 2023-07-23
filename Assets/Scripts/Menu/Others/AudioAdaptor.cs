using Scripts.Game.Data;
using Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioAdaptor : MonoBehaviour
{
    [SerializeField, Header("�ϥΪ̳]�w")]
    private UserSetting _userSetting;
    private void Awake() {
        AudioController.Instance.SetUserSetting(_userSetting);
    }
}
