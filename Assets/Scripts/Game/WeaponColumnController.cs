using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponColumnController : MonoBehaviour
{
    [SerializeField, Header("有效武器欄位框")]
    private Sprite _activeBackground;
    [SerializeField, Header("無效武器欄位框")]
    private Sprite _unactiveBackground;
    private Image _background;
    private Image _weaponIcon;

    private void Awake()
    {
        _background = GetComponent<Image>();
        foreach(Image image in GetComponentsInChildren<Image>())
        {
            if (image.name == "WeaponIcon")
                _weaponIcon = image;
        }

        _weaponIcon.enabled = false;
    }

    public void SetActive(bool active)
    {
        _background.sprite = active ? _activeBackground : _unactiveBackground;
    }

    public void AddWeaponIcon(Sprite weaponIcon)
    {
        _weaponIcon.sprite = weaponIcon;
        _weaponIcon.enabled = true;
        _background.sprite = _activeBackground;
    }
}
