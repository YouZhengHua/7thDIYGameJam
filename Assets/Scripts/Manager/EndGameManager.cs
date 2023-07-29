using Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameManager : MonoBehaviour
{
    [SerializeField] private StoryManager storyManager;
    [SerializeField] private FadeEffectUI fadeEffect;
    [SerializeField] private CreditManager credit;
    [SerializeField] private StageManager stageManager;

    private void Start() {
        FadeIn();
        storyManager.StartStory(34, EndingAnimation);
    }

    private void EndingAnimation() {
        fadeEffect.SlowFadeOut();
        StartCoroutine(Credit(5));
    }

    private IEnumerator Credit(float second) {
        yield return new WaitForSeconds(second);
        fadeEffect.gameObject.SetActive(false);
        credit.gameObject.SetActive(true);
    }

    private void FadeIn() {
        fadeEffect.FadeIn();
    }

    private void FadeOut() {
        fadeEffect.FadeOut();
    }

    public void LoadScene(string sceneName) {
        LoadingScreen.instance.LoadScene(sceneName, stageManager.LoadingImage, stageManager.LoadingTip);
    }
}
