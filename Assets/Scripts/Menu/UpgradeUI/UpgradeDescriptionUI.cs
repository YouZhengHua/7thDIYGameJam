using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeDescriptionUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI context;
    [SerializeField] private TextMeshProUGUI cost;
    [SerializeField] private ConfirmCheckBoxUI confirmCheckBox;
    [SerializeField] private UpgradeElementContainerUI containerUI;

    private UpgradeElementSO chosenSO;

    public void SetChosenSO(UpgradeElementSO value) {
        chosenSO = value;
    }

    public void UpdateDescription() {
        SetChosenSO(containerUI.GetElementSO());
        UpdateCheckBoxVisual();
        UpdateTitle();
        UpdateCost();
        UpdateContext();
    }

    public void UpdateTitle() {
        title.text = chosenSO.elementName;
    }

    public void UpdateCost() {
        cost.text = "花費："　+ ((int)chosenSO.CalcCost()).ToString();

        if (chosenSO.currentLevel == chosenSO.maxLevel) {
            cost.text = "Level Max";
        }
    }
    public void UpdateContext() {
        context.text = chosenSO.DescriptionOnUI;
    }

    public void UpdateCheckBoxVisual() {
        if (chosenSO.currentLevel == chosenSO.maxLevel) {
            confirmCheckBox.gameObject.SetActive(false);
        } else {
            confirmCheckBox.gameObject.SetActive(true);
        }

        if (chosenSO.IsUpgradeAvailable()) {
            confirmCheckBox.ActiveCheckBox();
        } else {
            confirmCheckBox.DeActiveChechBox();
        }
    }
}
