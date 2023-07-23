using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
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
    private bool isLevelCleared;

    public stage GetCurrentStage() {
        return currentStage;
    }

    public void SetCurrentStage(stage value) {
        currentStage = value;
        Debug.Log("Current Stage is set to be " + currentStage);
    }

    public void GoingToNextStage(stage value) {
        SetCurrentStage(value);
        isPlayerDefeated = false;
        isSecInTheSameLevel = false;
    }
}
