using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmCheckBoxUI : MonoBehaviour
{
    [SerializeField] private Sprite activeCheckBox;
    [SerializeField] private Sprite deactiveCheckBox;

    private Button button;
    private Image image;

    private void Awake() {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
    }

    public void ActiveCheckBox() {
        image.sprite = activeCheckBox;
        button.enabled = true;
    }

    public void DeActiveChechBox() {
        image.sprite = deactiveCheckBox;
        button.enabled = false;
    }
}
