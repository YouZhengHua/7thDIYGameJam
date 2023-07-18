using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WeaponsDescriptionSO : ScriptableObject
{
    public string weaponName;

#if UNITY_EDITOR
    [Multiline]
    public string weaponDescription = "";
#endif

}
