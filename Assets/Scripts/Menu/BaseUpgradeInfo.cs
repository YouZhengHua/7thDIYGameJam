using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BaseUpgradeInfo : ScriptableObject
{
    [Serializable]
    public enum Info {
        Strength,
        Cooldown,
        IncreaseMoneyQuantity,
        MoveSpeed,
        ProjetileNumber,
        IncreaseSkillSlot,
        HPGeneratePerSec,
        IncreaseExpQuantity,
        ProjectileSize,
        AttackPersistTime,
        Defense,
        MaxHP,
        ProjectileSpeed,
        IncreasePickingArea,
        IncreaseAttackingRadius,
        ReviveTimes,
    }

    [Serializable]
    public struct Info_SO {
        public Info infoName;
        public UpgradeElementSO upgradeElementSO; 
    }

    public Info_SO[] infos;

    public int GetCurrentLevel(Info queryName) {
        foreach (Info_SO info_SO in infos) {
            if (queryName == info_SO.infoName) {
                return info_SO.upgradeElementSO.currentLevel;
            }
        }

        Debug.LogError("Can't find the corresponding query UpgradeSO");
        return -1;
    }

    public float GetEffect(Info queryName) {
        foreach (Info_SO info_SO in infos) {
            if (queryName == info_SO.infoName) {
                return info_SO.upgradeElementSO.GetEffect();
            }
        }

        Debug.LogError("Can't find the corresponding query UpgradeSO");
        return -1;
    }
}
