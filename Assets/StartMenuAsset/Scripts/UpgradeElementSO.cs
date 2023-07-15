using Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class UpgradeElementSO : ScriptableObject
{
    public string elementName;
    public int currentLevel;
    public int maxLevel;
    public Sprite Icon;
    public float cost;

    public enum effectType {
        floatType,
        intType
    };

    [SerializeField] private float floatEffect;
    [SerializeField] private int intEffect;




    public void IncreaseCurrentLevel() {
        if (currentLevel >= maxLevel) {
            Debug.Log("This Upgrade has maxed, can't upgrade more");
            return;
        }

        if (StaticPrefs.IsAffordable(cost)) {
            StaticPrefs.Cost(cost);
            currentLevel++;
            Debug.Log(name + " has been upgrade to level " + currentLevel);
        }
    }
}
