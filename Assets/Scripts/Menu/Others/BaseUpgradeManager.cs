using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseUpgradeManager : MonoBehaviour
{

    [SerializeField] private BaseUpgradeInfo info;

    /// <summary>
    /// <para>光環範圍</para>
    /// <para>光環類武器增加一定範圍比率</para>
    /// </summary>
    /// <returns></returns>
    public float GetAttackRadius()
    {
        return info.GetEffect(BaseUpgradeInfo.Info.IncreaseAttackingRadius);
    }
    /// <summary>
    /// <para>冷卻時間</para>
    /// <para>冷卻降低固定秒數</para>
    /// </summary>
    /// <returns></returns>
    public float GetCoolDown()
    {
        return info.GetEffect(BaseUpgradeInfo.Info.Cooldown);
    }
    /// <summary>
    /// <para>力量</para>
    /// <para>所有武器傷害提升一定比率</para>
    /// </summary>
    /// <returns></returns>
    public float GetStrength()
    {
        return info.GetEffect(BaseUpgradeInfo.Info.Strength);
    }
    /// <summary>
    /// <para>金幣量增加</para>
    /// <para>金幣獲取量增加一定比率</para>
    /// </summary>
    /// <returns></returns>
    public float GetIncreaseMoneyQuantity()
    {
        return info.GetEffect(BaseUpgradeInfo.Info.IncreaseMoneyQuantity);
    }
    /// <summary>
    /// <para>移動速度</para>
    /// <para>移動速度提高一定比率</para>
    /// </summary>
    /// <returns></returns>
    public float GetMoveSpeed()
    {
        return info.GetEffect(BaseUpgradeInfo.Info.MoveSpeed);
    }
    /// <summary>
    /// <para>投射物數量</para>
    /// <para>投射物數量增加固定數量</para>
    /// </summary>
    /// <returns></returns>
    public int GetProjetileNumber()
    {
        return (int)info.GetEffect(BaseUpgradeInfo.Info.ProjetileNumber);
    }
    /// <summary>
    /// <para>增加技能欄位</para>
    /// <para>技能格欄位增加一定數量</para>
    /// </summary>
    /// <returns></returns>
    public int GetIncreaseSkillSlot()
    {
        return (int)info.GetEffect(BaseUpgradeInfo.Info.IncreaseSkillSlot);
    }
    /// <summary>
    /// <para>生命恢復</para>
    /// <para>生命回復增加固定數值</para>
    /// </summary>
    /// <returns></returns>
    public float GetHPGeneratePerSec()
    {
        return info.GetEffect(BaseUpgradeInfo.Info.HPGeneratePerSec);
    }
    /// <summary>
    /// <para>經驗值增加</para>
    /// <para>經驗值獲取率增加一定比率</para>
    /// </summary>
    /// <returns></returns>
    public float GetIncreaseExpQuantity()
    {
        return info.GetEffect(BaseUpgradeInfo.Info.IncreaseExpQuantity);
    }
    /// <summary>
    /// <para>投射物大小</para>
    /// <para>投射物的體積增加一定比率</para>
    /// </summary>
    /// <returns></returns>
    public float GetProjectileSize()
    {
        return info.GetEffect(BaseUpgradeInfo.Info.ProjectileSize);
    }
    /// <summary>
    /// <para>攻擊持續時間</para>
    /// <para>攻擊持續時間增加一定比率</para>
    /// </summary>
    /// <returns></returns>
    public float GetAttackPersistTime()
    {
        return info.GetEffect(BaseUpgradeInfo.Info.AttackPersistTime);
    }
    /// <summary>
    /// <para>防禦力</para>
    /// <para>防禦力增加固定點數</para>
    /// </summary>
    /// <returns></returns>
    public int GetDefense()
    {
        return (int)info.GetEffect(BaseUpgradeInfo.Info.Defense);
    }
    /// <summary>
    /// <para>生命上限</para>
    /// <para>生命值上限增加一定比率</para>
    /// </summary>
    /// <returns></returns>
    public float GetMaxHP()
    {
        return info.GetEffect(BaseUpgradeInfo.Info.MaxHP);
    }
    /// <summary>
    /// <para>投射物射擊速度</para>
    /// <para>投射物射擊速度增加一定比率</para>
    /// </summary>
    /// <returns></returns>
    public float GetProjectileSpeed()
    {
        return info.GetEffect(BaseUpgradeInfo.Info.ProjectileSpeed);
    }
    /// <summary>
    /// <para>拾取範圍</para>
    /// <para>拾取範圍增加一定比率</para>
    /// </summary>
    /// <returns></returns>
    public float GetIncreasePickingArea()
    {
        return info.GetEffect(BaseUpgradeInfo.Info.IncreasePickingArea);
    }
    /// <summary>
    /// <para>復活次數</para>
    /// <para>玩家增加固定的復活次數</para>
    /// </summary>
    /// <returns></returns>
    public int GetReviveTimes()
    {
        return (int)info.GetEffect(BaseUpgradeInfo.Info.ReviveTimes);
    }
}
