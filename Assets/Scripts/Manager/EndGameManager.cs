using Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameManager : MonoBehaviour
{
    [SerializeField] private StoryManager storyManager;
    [SerializeField] private FadeEffectUI fadeEffect;
    [SerializeField] private CreditManager credit;

    private void Start() {
        StartCoroutine(waitForSec(1));
        storyManager.StartStory(34, EndingAnimation);
    }

    private void EndingAnimation() {
        StartCoroutine(waitForSec(1));
        credit.gameObject.SetActive(true);
    }

    private IEnumerator waitForSec(float second) {
        yield return new WaitForSeconds(second);
    }

    private void FadeIn() {
        fadeEffect.FadeIn();
    }

    private void FadeOut() {
        fadeEffect.FadeOut();
    }

    public void LoadScene(string sceneName) {
        LoadingScreen.instance.LoadScene(sceneName, false);
    }
}
