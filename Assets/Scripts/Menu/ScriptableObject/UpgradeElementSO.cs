using Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "UI/UpgradeElementSO")]
public class UpgradeElementSO : ScriptableObject
{
    public string elementName;
    public int currentLevel;
    public int maxLevel;
    public Sprite Icon;
    public float cost;
    [Multiline]
    public string DescriptionOnUI = "";

    [SerializeField] private float EffectPerLevel;

    public float GetEffect() {
        return EffectPerLevel * currentLevel;
    }

    private void OnEnable() {
        //Load();
    }

    public void Load() {
        if (DataSystem.Instance != null) {
            if (DataSystem.Instance.gameValueData.elementLevelDic.ContainsKey(name)) {
                currentLevel = (DataSystem.Instance.gameValueData.elementLevelDic[name]);
            }
        }
    }

    public void Save() {
        DataSystem.Instance.SaveElementData(name, currentLevel);
    }

    public bool IsUpgradeAvailable() {
        if (currentLevel == maxLevel) {
            return false;
        } else {
            return StaticPrefs.IsAffordable(cost);
        }
    }

    public void IncreaseCurrentLevel() {
        if (currentLevel >= maxLevel) {
            Debug.Log("This Upgrade has maxed, can't upgrade more");
            return;
        }

        if (IsUpgradeAvailable()) {
            StaticPrefs.Cost(cost);
            currentLevel++;
            Save();
            Debug.Log(name + " has been upgrade to level " + currentLevel);
        }
    }
}
