using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu]
public class ImageNameAndSpriteSO : ScriptableObject
{
    [Serializable]
    public struct name_Sprite {
        public string name;
        public Sprite sprite;
    }

    public List<name_Sprite> nameSpriteDict;
}
