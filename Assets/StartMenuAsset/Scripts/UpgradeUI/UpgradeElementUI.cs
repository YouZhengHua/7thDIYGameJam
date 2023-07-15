using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeElementUI : MonoBehaviour
{
    [SerializeField] private Transform progressUIContainer;
    [SerializeField] private UpgradeElementSO upgradeElementSO;
    [SerializeField] private TextMeshProUGUI ButtonName;
    [SerializeField] private Button button;

    private List<Transform> progressUIs;

    private void Awake()
    {
        progressUIs = new List<Transform>();
        foreach (Transform child in progressUIContainer)
        {
            progressUIs.Add(child);
        }
    }

    private void Start()
    {
        ButtonName.text = upgradeElementSO.elementName;

        if (progressUIs.Count != upgradeElementSO.maxLevel)
        {
            Debug.LogError("The length of UI checkbox of " + upgradeElementSO.name + " does not match");
        }

        int currentLevel = upgradeElementSO.currentLevel;
        for (int i = 0; i < currentLevel; i++)
        {
            ActivateProgressUI(progressUIs[i]);
        }
    }

    public void Set(int currentLevel)
    {
        upgradeElementSO.currentLevel = currentLevel;
        for (int i = 0; i < currentLevel; i++)
        {
            EnableNextCheckBox(i);
        }
    }

    public void Upgrade()
    {
        upgradeElementSO.IncreaseCurrentLevel();
        EnableNextCheckBox(upgradeElementSO.currentLevel - 1);
    }

    public void EnableNextCheckBox(int nextIndex)
    {
        if (nextIndex == upgradeElementSO.maxLevel)
        {
            Debug.Log("This ability has maxouted!");
            return;
        }

        ActivateProgressUI(progressUIs[nextIndex]);
    }

    public void ActivateProgressUI(Transform progressUI)
    {
        progressUI.GetComponent<Outline>().enabled = true;
    }

    public void DeactivateProgressUI(Transform progressUI)
    {
        progressUI.GetComponent<Outline>().enabled = false;
    }

    public Button GetButton()
    {
        return button;
    }

    public UpgradeElementSO GetUpgradeElementSO()
    {
        return upgradeElementSO;
    }
}
