using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "UI/WeaponsDescriptionSO")]
public class WeaponsDescriptionSO : ScriptableObject
{
    public string weaponName;
    [Multiline]
    public string weaponDescription = "";

}
