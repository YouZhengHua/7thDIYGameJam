using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLoading : MonoBehaviour
{

    [SerializeField] private List<UpgradeElementSO> baseUpgrades;

    private void Start() {
        foreach (UpgradeElementSO upgradeElementSO in baseUpgrades) {
            upgradeElementSO.Load();
        }
    }
}
