using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeConfirmButton : MonoBehaviour
{
   
    [SerializeField] private UpgradeElementSO chosenUpgradeElementSO;

    public void SetElement() {

    }

    public void UpgradeConfirm() {
        chosenUpgradeElementSO.IncreaseCurrentLevel();
    }
}
