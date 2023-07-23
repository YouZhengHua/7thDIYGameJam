using Scripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrentMoney : MonoBehaviour
{
    private TextMeshProUGUI moneyText;

    private void Awake() {
        moneyText = GetComponent<TextMeshProUGUI>();
    }

    private void Start() {
        StaticPrefs.OnScoreChange += UpdateVisual;

        UpdateVisual();
    }

    public void UpdateVisual(float money) {
        moneyText.text = money.ToString();
    }

    public void UpdateVisual() {
        moneyText.text = StaticPrefs.Score.ToString();
    }
}
