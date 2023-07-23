using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponsContainerUI : MonoBehaviour
{
    [SerializeField] private WeaponsDescriptionSO descriptionSO;
    [SerializeField] private TextMeshProUGUI weaponsName;
    [SerializeField] private TextMeshProUGUI weaponsDescription;


    public void DisplayInfo() {
        weaponsName.text = descriptionSO.weaponName;
#if UNITY_EDITOR
        weaponsDescription.text = descriptionSO.weaponDescription;
#endif
    }

}
