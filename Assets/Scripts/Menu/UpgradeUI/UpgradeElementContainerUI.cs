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

    private List<Button> buttons;
    private UpgradeElementUI chosenElement;

    private void Awake()
    {
        buttons = new List<Button>();

        foreach (UpgradeElementUI element in elements)
        {
            buttons.Add(element.GetButton());
        }
    }

    private void Start()
    {
        foreach (UpgradeElementUI element in elements)
        {
            element.GetButton().onClick.AddListener(() => chosenElement = element);
        }


        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() =>
            {
                DisableAllButtonsOutline(buttons);
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
