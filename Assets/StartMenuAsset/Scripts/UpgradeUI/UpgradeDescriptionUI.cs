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
    [SerializeField] private Button confirmCheckBox;
    [SerializeField] private UpgradeElementContainerUI containerUI;

    private UpgradeElementSO chosenSO;

    public void SetChosenSO() {
        chosenSO = containerUI.GetElementSO();
    }

    public void UpdateDescription() {
        SetChosenSO();
        UpdateTitle();
        UpdateCost();
        UpdateContext();
    }

    public void UpdateTitle() {
        title.text = chosenSO.elementName;
    }

    public void UpdateCost() {
        cost.text = "ªá¶O¡G"¡@+ chosenSO.cost.ToString();
    }

    public void UpdateContext() {
        context.text = chosenSO.DescriptionOnUI;
    }

}
