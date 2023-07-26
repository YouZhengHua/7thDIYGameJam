using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeElementUI : MonoBehaviour
{
    [SerializeField] private Transform progressUIContainer;
    [SerializeField] private UpgradeElementSO upgradeElementSO;
    [SerializeField] private Button button;
    [SerializeField] private Sprite upgradedSprite;
    [SerializeField] private Sprite notUpgradedSprite;

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
        upgradeElementSO.Load();

        int currentLevel = upgradeElementSO.currentLevel;
        for (int i = 0; i < currentLevel; i++)
        {
            ActivateProgressUI(progressUIs[i]);
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
        progressUI.GetComponent<Image>().sprite = upgradedSprite;
    }

    public void DeactivateProgressUI(Transform progressUI)
    {
        progressUI.GetComponent<Image>().sprite = notUpgradedSprite;
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
