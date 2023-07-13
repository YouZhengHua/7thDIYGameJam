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



}
