using System.Collections;
using System.Collections.Generic;
using Scripts.Game;
using Scripts.Game.Data;
using UnityEngine;
[CreateAssetMenu(menuName = "Game/Weapon")]
public class WeaponData : BaseItemData
{
    #region 通用屬性
    /// <summary>
    /// 槍械編號
    /// </summary>
    [Header("槍械編號")]
    public WeaponIndex WeaponIndex;
    [Header("子彈預製物")]
    public GameObject AmmoPrefab;
    /// <summary>
    /// 傷害值
    /// </summary>
    [Header("傷害值")]
    public float Damage = 1;
    [Header("傷害來源")]
    public DamageFrom DamageFrom = DamageFrom.Gun;
    /// <summary>
    /// 擊退力道
    /// </summary>
    [Header("擊退力道")]
    public float Force = 100f;
    /// <summary>
    /// 擊退持續時間
    /// </summary>
    [Header("怪物擊退持續時間")]
    public float DelayTime = 0.1f;
    #endregion

    #region 投射物屬性
    /// <summary>
    /// 投射物武器射擊間隔
    /// </summary>
    [Header("投射物類武器射擊間隔")]
    public float CoolDownTime = 0.1f;
    /// <summary>
    /// 投射物大小
    /// </summary>
    [Header("投射物大小比例")]
    public float AmmoScale = 1f;
    /// <summary>
    /// 投射物每次射出數量
    /// </summary>
    [Header("投射物每次射出數量")]
    public int OneShootAmmoCount = 1;
    /// <summary>
    /// 投射物飛行速度
    /// </summary>
    [Header("投射物飛行速度")]
    public float AmmoFlySpeed = 500f;
    /// <summary>
    /// 投射物穿透次數
    /// </summary>
    [Header("投射物穿透次數"), Min(0)]
    public int AmmoPenetrationCount = 1;
    /// <summary>
    /// 投射物是否具有穿透上限
    /// </summary>
    [Header("投射物是否具有穿透上限"), Tooltip("影響穿透物是否會無限穿透怪物")]
    public bool HavaPenetrationLimit = true;
    /// <summary>
    /// 投射物飛行距離上限
    /// </summary>
    [Header("投射物飛行距離上限"), Range(0f, 100f)]
    public float AmmoFlyRange = 15f;
    #endregion

    #region 範圍武器屬性
    /// <summary>
    /// 範圍武器觸發間隔
    /// </summary>
    [Header("範圍武器觸發間隔")]
    public float SkillTriggerInterval = 0.1f;
    /// <summary>
    /// 範圍武器傷害半徑
    /// </summary>
    [Header("範圍武器傷害半徑")]
    public float DamageRadius = 2f;
    /// <summary>
    /// 範圍武器生成半徑
    /// </summary>
    [Header("範圍武器生成半徑")]
    public float CreateRadius = 2f;
    #endregion
}
