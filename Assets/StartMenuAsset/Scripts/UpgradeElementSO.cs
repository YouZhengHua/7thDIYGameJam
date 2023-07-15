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

    [SerializeField] private float EffectPerLevel;

    public float GetEffect() {
        return EffectPerLevel * currentLevel;
    }

    private void OnEnable() {
        /*
        if (DataSystem.Instance.gameValueData.elementLevelDic.ContainsKey(name))
        {
            currentLevel = (DataSystem.Instance.gameValueData.elementLevelDic[name]);
        }
        */
    }

    private void OnDisable() {
        // DataSystem.Instance.SaveElementData(name, currentLevel);
    }

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
