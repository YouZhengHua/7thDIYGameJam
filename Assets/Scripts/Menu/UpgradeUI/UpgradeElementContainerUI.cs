using Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeElementContainerUI : MonoBehaviour
{
    [SerializeField] private UpgradeElementUI[] elements;
    [SerializeField] private UpgradeDescriptionUI descriptionUI;
    [SerializeField] private AudioClip upgradeSE;

    private List<Button> elementButtons;
    private UpgradeElementUI chosenElement;

    private void Awake() {
        elementButtons = new List<Button>();
        InitElementButton();
    }

    private void InitElementButton() {
        foreach (UpgradeElementUI element in elements) {
            elementButtons.Add(element.GetButton());

            element.GetButton().onClick.AddListener(() => chosenElement = element);
        }
    }

    private void Start() {
        InitButtonsOnClick();
    }

    private void InitButtonsOnClick() {
        foreach (Button button in elementButtons) {
            button.onClick.AddListener(() => {
                DisableAllButtonsOutline(elementButtons);
                EnableOutline(button);
                descriptionUI.UpdateDescription();
            });
        }
    }

    public void UpgradeChosenElement() {
        chosenElement.Upgrade();
        descriptionUI.UpdateDescription();
        AudioController.Instance.PlayEffect(upgradeSE);
    }

    public void DisableAllButtonsOutline(List<Button> buttons)
    {
        foreach (Button button in buttons)
        {
            DisableOutline(button);
        }
    }

    public UpgradeElementSO GetElementSO()
    {
        return chosenElement.GetUpgradeElementSO();
    }

    private void EnableOutline(Button button)
    {
        button.transform.GetComponent<Outline>().enabled = true;
    }

    private void DisableOutline(Button button)
    {
        button.transform.GetComponent<Outline>().enabled = false;
    }

}
