using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityWindow : MonoBehaviour
{
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private Vector2 lowWindowSize;
    private Vector2 highWindowSize;

    private bool isClosing = false;
    private bool isOpening = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        if (isClosing)
        {
            if (canvasGroup.alpha > 0f)
            {
                rectTransform.sizeDelta = new Vector2
                (
                    Mathf.MoveTowards(rectTransform.sizeDelta.x, lowWindowSize.x, 6f),
                    Mathf.MoveTowards(rectTransform.sizeDelta.y, lowWindowSize.y, 6f)
                );

                canvasGroup.alpha -= Time.deltaTime * 8f;
            }
            else
            {
                isClosing = false;
                rectTransform.SetAsFirstSibling();
            }
        }

        if (isOpening) // Opening Animation
        {
            if (canvasGroup.alpha < 1f)
            {
                rectTransform.sizeDelta = new Vector2
                (
                    Mathf.MoveTowards(rectTransform.sizeDelta.x, highWindowSize.x, 6f),
                    Mathf.MoveTowards(rectTransform.sizeDelta.y, highWindowSize.y, 6f)
                );
                canvasGroup.alpha += Time.deltaTime * 8f;
            }
            else
            {
                isOpening = false;
            }
        }
    }

    void OnWindowStart()
    {
        rectTransform.SetAsLastSibling();
        canvasGroup.alpha = 0f;
        isOpening = true;
    }

    void OnWindowComplete()
    {
        highWindowSize = rectTransform.sizeDelta;
        lowWindowSize = new Vector2
        (
            rectTransform.sizeDelta.x * 0.8f,
            rectTransform.sizeDelta.y * 0.8f
        );
        isClosing = true;
    }
}
