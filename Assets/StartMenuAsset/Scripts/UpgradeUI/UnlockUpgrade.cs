using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnlockUpgrade : MonoBehaviour
{
    [SerializeField] private StageManager.stage critirionStage;
    [SerializeField] private StageManager stageManager;
    private Button button;

    private void Awake() {
        button = GetComponent<Button>();
    }

    private void Start() {
        if (critirionStage < stageManager.GetCurrentStage()) {
            button.enabled = true;
        } else {
            button.enabled = false;
        }
    }
}
