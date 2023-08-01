using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeEffectUI : MonoBehaviour
{
    private Animator animator;
    private Image image;

    private void Awake() {
        animator= GetComponent<Animator>();
        image= GetComponent<Image>();
    }

    public void Black() {
        image.color = Color.black;
    }

    public void FadeIn() {
        animator.Play("FadeIn");
    }

    public void FadeOut() {
        animator.Play("FadeOut");
    }

    public void PrologueFadeIn() {
        animator.Play("PrologueFadeIn");
    }

    public void SlowFadeOut() {
        animator.Play("SlowFadeOut");
    }
}
