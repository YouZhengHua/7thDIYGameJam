using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeElementContainerUI : MonoBehaviour
{
    [SerializeField] private UpgradeElementUI[] elements;
    [SerializeField] private Button checkBox;

    private List<Button> buttons;
    private UpgradeElementUI chosenElement;

    private void Awake() {
        buttons = new List<Button>();

        foreach(UpgradeElementUI element in elements) {
            buttons.Add(element.GetButton());
        }
    }

    private void Start() {
        foreach(UpgradeElementUI element in elements) {
            element.GetButton().onClick.AddListener(() => chosenElement = element);
        }


        foreach (Button button in buttons) {
            button.onClick.AddListener(() => {
                DisableAllButtonsOutline(buttons);
                EnableOutline(button);
            });
        }

        checkBox.onClick.AddListener(() => { chosenElement.Upgrade(); });
    }

    public void DisableAllButtonsOutline(List<Button> buttons) {
        foreach(Button button in buttons) {
            DisableOutline(button);
        }
    }

    private void EnableOutline(Button button) {
        button.transform.GetComponent<Outline>().enabled = true;
    }

    private void DisableOutline(Button button) {
        button.transform.GetComponent<Outline>().enabled = false;
    }

}
