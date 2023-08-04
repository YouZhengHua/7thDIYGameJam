using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Stage/StageManager")]
public class StageManager : ScriptableObject
{
    [Serializable]
    public enum stage {
        firstStartGame, // OpenScene, Prologue
        Level_1, // The next level is level_1 and etc.
        Level_2, 
        Level_3,
        Level_4,
        gameEnd, // Trigger end script
    }

    public bool isPlayerDefeated;
    public bool isSecInTheSameLevel;
    [SerializeField] private stage currentStage;
    [SerializeField]
    private Sprite[] _loadImages;
    [SerializeField]
    private string[] _loadTips;
    [SerializeField]
    private Vector3[] _tipsLostion;
    private bool isLevelCleared;

    public stage GetCurrentStage() {
        return currentStage;
    }

    public void Load()
    {
        if (DataSystem.Instance != null)
        {
            currentStage = DataSystem.Instance.gameValueData.currentStage;
        }
    }

    public void SetCurrentStage(stage value)
    {
        currentStage = value;
        Debug.Log("Current Stage is set to be " + currentStage);
        DataSystem.Instance.SaveCurrentStage(currentStage);
    }

    public void GoingToNextStage(stage value) {
        SetCurrentStage(value);
        isPlayerDefeated = false;
        isSecInTheSameLevel = false;
    }

    public Sprite LoadingImage 
    {
        get
        {
            int index = Mathf.Min(Mathf.Max((int)currentStage-1, 0), _loadImages.Length-1);
            return _loadImages[index];
        }
    }

    public string LoadingTip
    {
        get
        {
            int index = Mathf.Min(Mathf.Max((int)currentStage-1, 0), _loadTips.Length-1);
            return _loadTips[index];
        }
    }

    public Vector3 TipsLostion
    {
        get
        {
            int index = Mathf.Min(Mathf.Max((int)currentStage - 1, 0), _tipsLostion.Length - 1);
            return _tipsLostion[index];
        }
    }
}
