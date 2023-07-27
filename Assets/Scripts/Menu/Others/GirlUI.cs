using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using William.Tool.Event;

public class GirlUI : MonoBehaviour
{
    [SerializeField] private ImageNameAndSpriteSO imageDictSO;
    private Image girlImage;

    private void Awake()
    {
        girlImage = GetComponent<Image>();
    }

    private void Start()
    {
        SetGirlImage();
    }

    void OnEnable()
    {
        EventManager.StartListening(EGameEvents.UI_ChangeGirlImage.ToString(), OnSetGirlImage);
    }

    void OnDisable()
    {
        EventManager.StopListening(EGameEvents.UI_ChangeGirlImage.ToString(), OnSetGirlImage);
    }

    public void OnSetGirlImage(Dictionary<string, object> message)
    {
        if (message.ContainsKey("value"))
        {
            SetGirlImage((string)message["value"]);
        }
    }

    public void SetGirlImage(string imageName = "仿生人A")
    {
        foreach (ImageNameAndSpriteSO.name_Sprite name_Sprite in imageDictSO.nameSpriteDict)
        {
            if (imageName == name_Sprite.name)
            {
                girlImage.sprite = name_Sprite.sprite;
                girlImage.SetNativeSize();
                return;
            }
        }
    }
}
