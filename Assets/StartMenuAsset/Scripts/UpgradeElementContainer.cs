using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeElementContainer : MonoBehaviour
{
    [SerializeField] private Transform[] buttons;

    private void Start() {
        foreach (Transform button in buttons) {
            button.GetComponent<Button>().onClick.AddListener(() => {
                DisableAllButtonsOutline(buttons);
                EnableOutline(button);
            });
        }
    }

    public void DisableAllButtonsOutline(Transform[] buttons) {
        foreach(Transform button in buttons) {
            DisableOutline(button);
        }
    }

    private void EnableOutline(Transform transform) {
        transform.GetComponent<Outline>().enabled = true;
    }

    private void DisableOutline(Transform transform) {
        transform.GetComponent<Outline>().enabled = false;
    }
}
