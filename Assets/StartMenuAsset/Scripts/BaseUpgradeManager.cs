using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUpgradeManager : MonoBehaviour
{

    [SerializeField] private BaseUpgradeInfo info;

    public float GetAttackRadius() {
        return info.GetEffect(BaseUpgradeInfo.Info.IncreaseAttackingRadius);
    }

    public float GetCoolDown() {
        return info.GetEffect(BaseUpgradeInfo.Info.Cooldown);
    }

    public float GetStrength() {
        return info.GetEffect(BaseUpgradeInfo.Info.Strength);
    }
        public float GetIncreaseMoneyQuantity() {
        return info.GetEffect(BaseUpgradeInfo.Info.IncreaseMoneyQuantity);
    }
        public float GetMoveSpeed() {
        return info.GetEffect(BaseUpgradeInfo.Info.MoveSpeed);
    }
        public int GetProjetileNumber() {
        return (int)info.GetEffect(BaseUpgradeInfo.Info.ProjetileNumber);
    }
        public int GetIncreaseSkillSlot() {
        return (int)info.GetEffect(BaseUpgradeInfo.Info.IncreaseSkillSlot);
    }
        public float GetHPGeneratePerSec() {
        return info.GetEffect(BaseUpgradeInfo.Info.HPGeneratePerSec);
    }
        public float GetIncreaseExpQuantity() {
        return info.GetEffect(BaseUpgradeInfo.Info.IncreaseExpQuantity);
    }
        public float GetProjectileSize() {
        return info.GetEffect(BaseUpgradeInfo.Info.ProjectileSize);
    }
        public float GetAttackPersistTime() {
        return info.GetEffect(BaseUpgradeInfo.Info.AttackPersistTime);
    }
        public int GetDefense() {
        return (int)info.GetEffect(BaseUpgradeInfo.Info.Defense);
    }
        public float GetMaxHP() {
        return info.GetEffect(BaseUpgradeInfo.Info.MaxHP);
    }
        public float GetProjectileSpeed() {
        return info.GetEffect(BaseUpgradeInfo.Info.ProjectileSpeed);
    }
        public float GetIncreasePickingArea() {
        return info.GetEffect(BaseUpgradeInfo.Info.IncreasePickingArea);
    }
        public int GetReviveTimes() {
        return (int)info.GetEffect(BaseUpgradeInfo.Info.ReviveTimes);
    }
}
