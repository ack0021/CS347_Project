using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform background;
    public float scaleAmount = 1.08f;   // how much bigger it gets
    public float speed = 10f;           // animation speed

    private Vector3 originalScale;

    private void Start()
    {
        originalScale = background.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(ScaleTo(originalScale * scaleAmount));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(ScaleTo(originalScale));
    }

    private System.Collections.IEnumerator ScaleTo(Vector3 target)
    {
        while (Vector3.Distance(background.localScale, target) > 0.01f)
        {
            background.localScale = Vector3.Lerp(
                background.localScale,
                target,
                Time.unscaledDeltaTime * speed
            );
            yield return null;
        }

        background.localScale = target;
    }
}



