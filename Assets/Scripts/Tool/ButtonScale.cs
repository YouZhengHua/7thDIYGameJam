using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[AddComponentMenu("Custom UI/Button Scale")]
[DisallowMultipleComponent]
[RequireComponent(typeof(Selectable))]
public class ButtonScale : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] public Vector3 onPointerDown = Vector3.one * 1.2f;
    [SerializeField] public Vector3 onPointerUp = Vector3.one;
    [SerializeField] float duration = 0.2f;
    [SerializeField] AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    RectTransform rectTransform;
    Vector3 from;
    Vector3 to;
    bool needScaling;
    float r;
    float elapsedTime = 0f;


    Toggle toggle;
    Button button;
    bool interactable;

    void OnEnable()
    {
        toggle = GetComponent<Toggle>();
        button = GetComponent<Button>();
        rectTransform = GetComponent<RectTransform>();
        r = (curve[curve.length - 1].time - curve[0].time) / duration;
        //  reset scale to onPointerUp
        needScaling = false;
        rectTransform.transform.localScale = onPointerUp;
    }

    void Update()
    {
        if (toggle)
            interactable = toggle.interactable;
        else if (button)
            interactable = button.interactable;
    }
    
    void LateUpdate()
    {
        if (needScaling)
        {
            if (elapsedTime >= duration)
            {
                elapsedTime = 0f;
                needScaling = false;
                return;
            }
            elapsedTime += Time.deltaTime;
            var t = curve.Evaluate(elapsedTime * r);
            rectTransform.transform.localScale = Vector3.LerpUnclamped(from, to, t);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!interactable)
        {
            rectTransform.localScale = onPointerUp;
            needScaling = false;
            return;
        }

        if (rectTransform.localScale != onPointerDown)
        {
            needScaling = true;
            from = onPointerUp;
            to = onPointerDown;
            elapsedTime = InverseLerp(from, to, rectTransform.transform.localScale) * duration;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!interactable)
        {
            rectTransform.localScale = onPointerUp;
            needScaling = false;
            return;
        }

        if (rectTransform.localScale != onPointerUp)
        {
            needScaling = true;
            from = onPointerDown;
            to = onPointerUp;
            elapsedTime = InverseLerp(from, to, rectTransform.transform.localScale) * duration;
        }
    }

    float InverseLerp(Vector3 a, Vector3 b, Vector3 v)
    {
        Vector3 ab = b - a;
        Vector3 av = v - a;
        return Vector3.Dot(av, ab) / Vector3.Dot(ab, ab);
    }
}
