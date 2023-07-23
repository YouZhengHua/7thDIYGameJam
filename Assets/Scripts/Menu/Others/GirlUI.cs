using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GirlUI : MonoBehaviour
{
    [SerializeField] private ImageNameAndSpriteSO imageDictSO;
    private Image girlImage;

    private void Awake() {
        girlImage = GetComponent<Image>();
    }

    private void Start() {
        SetGirlImage();
    }

    public void SetGirlImage(string imageName = "仿生人A") {
        foreach(ImageNameAndSpriteSO.name_Sprite name_Sprite in imageDictSO.nameSpriteDict) {
            if (imageName == name_Sprite.name) {
                girlImage.sprite = name_Sprite.sprite;
                return;
            }
        }
    }
}
