using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Game/WeaponControllerData")]
public class WeaponControllerData : ScriptableObject
{
    [SerializeField, Header("武器 Prefab 列表")]
    public List<GameObject> weaponPrefabList = new List<GameObject>();
}
