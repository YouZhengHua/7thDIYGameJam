using System.Collections;
using System.Collections.Generic;
using Scripts.Game;
using Scripts.Game.Data;
using UnityEngine;
[CreateAssetMenu(menuName = "Game/Weapon")]
public class WeaponData : BaseItemData
{
    [Header("槍械編號")]
    public WeaponIndex WeaponIndex;
    [Header("子彈預製物")]
    public GameObject AmmoPrefab;
    public float Damage = 1;
    public DamageFrom DamageFrom = DamageFrom.Gun;
    public float Force = 100f;
    public float DelayTime = 0.1f;
    //冷卻時間
    public float CoolDownTime = 0.1f;
    //技能觸發間隔時間
    public float SkillTriggerInterval = 0.1f;
}
