using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditManager : MonoBehaviour
{
    [SerializeField] EndGameManager endGameManager;

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown) {
            endGameManager.LoadScene("00_StartScene");
        }
    }
}
