using System;
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

    [Header("傷害值")]
    public FloatAttributeHandle Damage;

    [Header("傷害來源")]
    public DamageFrom DamageFrom = DamageFrom.Gun;
    /// <summary>
    /// 擊退力道
    /// </summary>
    [Header("擊退力道")]
    public FloatAttributeHandle Force;
    /// <summary>
    /// 擊退持續時間
    /// </summary>
    [Header("怪物擊退持續時間")]
    public FloatAttributeHandle DelayTime;

    /// <summary>
    /// 攻擊音效清單
    /// </summary>
    [SerializeField, Header("攻擊音效清單")]
    public AudioClip[] ShootAudios;
    /// <summary>
    /// 隨機取得攻擊音效清單中的一個音效
    /// </summary>
    public AudioClip ShootAudio { get => (ShootAudios.Length == 0 || ShootAudios == null) ? null : ShootAudios[UnityEngine.Random.Range(0, ShootAudios.Length)]; }
    #endregion

    #region 投射物屬性

    [Header("投射物武器射擊間隔")]
    public FloatAttributeHandle CoolDownTime;

    /// <summary>
    /// 投射物大小
    /// </summary>
    [Header("投射物大小比例")]
    public FloatAttributeHandle AmmoScale;
    /// <summary>
    /// 投射物每次射出數量
    /// </summary>
    [Header("投射物每次射出數量")]
    public IntAttributeHandle OneShootAmmoCount;
    /// <summary>
    /// 投射物飛行速度
    /// </summary>
    [Header("投射物飛行速度")]
    public FloatAttributeHandle AmmoFlySpeed;
    /// <summary>
    /// 投射物穿透次數
    /// </summary>
    [Header("投射物穿透次數")]
    public IntAttributeHandle AmmoPenetrationCount;
    /// <summary>
    /// 投射物是否具有穿透上限
    /// </summary>
    [Header("投射物是否具有穿透上限"), Tooltip("影響穿透物是否會無限穿透怪物")]
    public bool HavaPenetrationLimit = true;
    /// <summary>
    /// 投射物飛行距離上限
    /// </summary>
    [Header("投射物飛行距離上限")]
    public FloatAttributeHandle AmmoFlyRange;
    #endregion

    #region 範圍武器屬性
    /// <summary>
    /// 範圍武器觸發間隔
    /// </summary>
    [Header("範圍武器觸發間隔")]
    public FloatAttributeHandle SkillTriggerInterval;
    /// <summary>
    /// 範圍武器傷害半徑
    /// </summary>
    [Header("範圍武器傷害半徑")]
    public FloatAttributeHandle DamageRadius;
    /// <summary>
    /// 範圍武器生成半徑
    /// </summary>
    [Header("範圍武器生成半徑")]
    public FloatAttributeHandle CreateRadius;
    #endregion
}