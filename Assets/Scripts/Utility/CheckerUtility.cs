using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class CheckerUtility
{
    public static int startInt = 0;
    /// <summary>
    /// 避免進錯場景
    /// </summary>
    public static void SceneLoadChecker()
    {
        if (startInt != 0) return;
#if UNITY_EDITOR
        UnityEditor.EditorUtility.DisplayDialog("提醒", "請從 00_StartScene 開啟遊戲", "確定");
        UnityEditor.EditorApplication.isPlaying = false;
#else
				Application.Quit();
#endif
        return;
    }
}
