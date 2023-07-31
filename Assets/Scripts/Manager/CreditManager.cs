using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditManager : MonoBehaviour
{
    [SerializeField] EndGameManager endGameManager;
    [SerializeField] RectTransform staffList;
    [SerializeField] Transform pressKeyToContinue;
    [SerializeField] StageManager stageManager;

    private bool isGoingToTitle;

    private void Start() {
        isGoingToTitle = false;
        StartCoroutine(RollingStaffList());
    }

    // Update is called once per frame
    void Update()
    {
        if (isGoingToTitle) {
            if (Input.anyKeyDown) {
                stageManager.SetCurrentStage(StageManager.stage.firstStartGame);
                endGameManager.LoadScene("00_StartScene");
            }
        }
    }

    private IEnumerator RollingStaffList() {
        staffList.GetComponent<Animator>().Play("RollingStaffList");
        yield return new WaitForSeconds(20);
        isGoingToTitle = true;
        pressKeyToContinue.gameObject.SetActive(true);
    }
}
