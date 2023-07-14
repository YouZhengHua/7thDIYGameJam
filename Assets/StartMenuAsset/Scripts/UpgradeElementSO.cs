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


    public void IncreaseCurrentLevel() {
        if (currentLevel >= maxLevel) {
            Debug.Log("This Upgrade has maxed, can't upgrade more");
            return;
        }

        currentLevel++;
        Debug.Log(name + " has been upgrade to level " + currentLevel);
    }

}
