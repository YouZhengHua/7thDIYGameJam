using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeElementUI : MonoBehaviour
{
    [SerializeField] private Transform[] progressUIs;
    [SerializeField] private UpgradeElementSO upgradeElementSO;

    private void Start() {
        if (progressUIs.Length != upgradeElementSO.maxLevel) {
            Debug.LogError("The length of UI checkbox of " + upgradeElementSO.name + " does not match");
        }

        int currentLevel = upgradeElementSO.currentLevel;
        for(int i = 0; i < currentLevel; i++) {
            ActivateProgressUI(progressUIs[i]);
        }
    }

    public void Upgrade() {
        upgradeElementSO.IncreaseCurrentLevel();
        EnableNextCheckBox(upgradeElementSO.currentLevel - 1);
    }

    public void EnableNextCheckBox(int nextIndex) {
        if (nextIndex == upgradeElementSO.maxLevel) {
            Debug.Log("This ability has maxouted!");
            return;
        }

        ActivateProgressUI(progressUIs[nextIndex]);
    }

    public void ActivateProgressUI(Transform progressUI) {
        progressUI.GetComponent<Outline>().enabled = true;
    }

    public void DeactivateProgressUI(Transform progressUI) {
        progressUI.GetComponent<Outline>().enabled = false;
    }
}
