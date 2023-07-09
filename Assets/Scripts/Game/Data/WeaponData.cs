using System.Collections;
using System.Collections.Generic;
using Scripts.Game;
using Scripts.Game.Data;
using UnityEngine;
[CreateAssetMenu(menuName = "Game/Weapon")]
public class WeaponData : BaseItemData
{
    [Header("子彈預製物")]
    public GameObject AmmoPrefab;
    public float Damage = 1;
    public DamageFrom DamageFrom = DamageFrom.Gun;
    public float Force = 100f;
    public float DelayTime = 0.1f;
}
