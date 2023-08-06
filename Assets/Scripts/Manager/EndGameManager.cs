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
    [SerializeField] private Transform background2;

    private void Start() {
        FadeIn();
        storyManager.StartStory(34, () => {
            StartCoroutine(secPhase());
        });
    }

    private IEnumerator secPhase() {
        FadeOut();
        yield return new WaitForSeconds(2f);
        FadeIn();
        storyManager.StartStory(35, () => {
            StartCoroutine(thirdPhase());
        });
    }

    private IEnumerator thirdPhase() {
        FadeOut();
        yield return new WaitForSeconds(1f);
        FadeIn();
        background2.gameObject.SetActive(true);
        storyManager.StartStory(36, EndingAnimation);
    }

    private void EndingAnimation() {
        StartCoroutine(Credit(5));
    }

    private IEnumerator Credit(float second) {
        yield return new WaitForSeconds(second);
        fadeEffect.gameObject.SetActive(false);
        credit.gameObject.SetActive(true);
        background2.gameObject.SetActive(false); 
        StopAllCoroutines();
    }

    private void FadeIn() {
        fadeEffect.FadeIn();
    }

    private void FadeOut() {
        fadeEffect.FadeOut();
    }

    public void LoadScene(string sceneName) {
        LoadingScreen.instance.LoadScene(sceneName, stageManager.LoadingImage, stageManager.LoadingTip, stageManager.TipsLostion);
    }
}
