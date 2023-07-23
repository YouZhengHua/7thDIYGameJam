using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/StoryResource")]
public class StoryResourceData : ScriptableObject
{
    [Header("背景圖片")]
    public List<Sprite> backgroundList;
    [Header("角色圖片")]
    public List<Sprite> characterList;
    [Header("音樂音效")]
    public List<AudioClip> audioList;

    /// <summary>
    /// 用名稱取得背景圖片
    /// </summary>
    /// <param name="name">背景圖片名稱</param>
    /// <returns></returns>
    public Sprite GetBackground(string name)
    {
        for (int i = 0; i < backgroundList.Count; i++)
        {
            if (backgroundList[i].name == name)
            {
                return backgroundList[i];
            }
        }
        return null;
    }

    /// <summary>
    /// 用名稱取得角色圖片
    /// </summary>
    /// <param name="name">角色圖片名稱</param>
    /// <returns></returns>
    public Sprite GetCharacter(string name)
    {
        if (characterList.Count == 0) return null;
        for (int i = 0; i < characterList.Count; i++)
        {
            if (characterList[i].name == name)
            {
                return characterList[i];
            }
        }
        return null;
    }

    /// <summary>
    /// 用名稱取得音樂音效
    /// </summary>
    /// <param name="name">音樂音效名稱</param>
    /// <returns></returns>
    public AudioClip GetAudio(string name)
    {
        if (audioList.Count == 0) return null;
        for (int i = 0; i < audioList.Count; i++)
        {
            if (audioList[i].name == name)
            {
                return audioList[i];
            }
        }
        return null;
    }
}
