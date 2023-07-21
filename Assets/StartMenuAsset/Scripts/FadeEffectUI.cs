using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeEffectUI : MonoBehaviour
{
    private Animator animator;

    private void Awake() {
        animator= GetComponent<Animator>();
    }

    public void FadeIn() {
        animator.Play("FadeIn");
        animator.Play("Idle");
    }

    public void FadeOut() {
        animator.Play("FadeOut");
        animator.Play("Idle");
    }
}
