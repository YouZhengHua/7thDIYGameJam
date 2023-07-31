using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockFight : MonoBehaviour
{
    [SerializeField] private StageManager.stage condition;
    [SerializeField] private StageManager stageManager;

    private void Start() {
        Button button = GetComponent<Button>();

        if (stageManager.GetCurrentStage() == condition) {
            button.interactable = false;
        } else {
            button.interactable = true;
        }

    }
}
