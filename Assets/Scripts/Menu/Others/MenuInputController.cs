using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInputController : MonoBehaviour
{
    [SerializeField] private Transform settingUI;
    [SerializeField] private Transform pauseUI;


    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (settingUI.gameObject.active == true) {
                settingUI.gameObject.SetActive(false);
                return;
            }

            if (pauseUI.gameObject.active == true) {
                pauseUI.gameObject.SetActive(false);
                return;
            }


            pauseUI.gameObject.SetActive(true);
        }
    }
}
