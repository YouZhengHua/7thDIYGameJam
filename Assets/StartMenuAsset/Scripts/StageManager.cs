using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public enum stage {
        firstStartGame, // OpenScene, Prologue
        Level_1, // The next level is level_1 and etc.
        Level_2, 
        Level_3,
        Level_4,
        gameEnd, // Trigger end script
    }

    public bool isPlayerDefeated;
    private stage currentStage;

    public stage GetCurrentStage() {
        return currentStage;
    }

    public void SetCurrentStage(stage value) {
        currentStage = value;
    }
}
