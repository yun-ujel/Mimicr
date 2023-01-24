using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameHandler : MonoBehaviour
{
    private UIMaster topUIMaster;
    private RectTransform rectTransform;
    private CanvasHandler canvasHandler;
    private CanvasGroup canvasGroup;

    [Header("Sizes")]
    private Vector2 minWindowSize;
    [SerializeField] private Vector2 maxWindowSize;
    private Vector2 randWindowSize;

    private bool isClosing = false;
    private bool isOpening = false;

    private float closingWindowCounter;

    void Awake()
    {
        topUIMaster = GetComponent<UIMaster>();
        rectTransform = GetComponent<RectTransform>();
        canvasHandler = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasHandler>();
        canvasGroup = GetComponent<CanvasGroup>();

        minWindowSize = topUIMaster.minWindowSize;
    }

    void Update()
    {
        if (isClosing)
        {
            if (canvasGroup.alpha > 0f)
            {
                rectTransform.sizeDelta = new Vector2
                (
                    Mathf.MoveTowards(rectTransform.sizeDelta.x, randWindowSize.x, 4f),
                    Mathf.MoveTowards(rectTransform.sizeDelta.y, randWindowSize.y, 4f)
                );

                canvasGroup.alpha -= Time.deltaTime * 10f;
            }
            else
            {
                isClosing = false;
                Destroy(gameObject);
            }
        }

        if (isOpening)
        {
            if (canvasGroup.alpha < 1f)
            {
                rectTransform.sizeDelta = new Vector2
                (
                    Mathf.MoveTowards(rectTransform.sizeDelta.x, randWindowSize.x, 4f),
                    Mathf.MoveTowards(rectTransform.sizeDelta.y, randWindowSize.y, 4f)
                );
                canvasGroup.alpha += Time.deltaTime * 10f;
            }
            else
            {
                isOpening = false;
            }
        }
    }

    void OnWindowStart()
    {
        canvasGroup.alpha = 0f;

        randWindowSize = new Vector2
        (
            Random.Range(minWindowSize.x, maxWindowSize.x),
            Random.Range(minWindowSize.y, maxWindowSize.y)
        );

        rectTransform.sizeDelta = new Vector2(randWindowSize.x * 0.8f, randWindowSize.y * 0.8f);
        isOpening = true;
    }

    void OnWindowComplete()
    {
        isClosing = true;

        canvasHandler.CompleteWindow();
    }

    void OnWindowFail()
    {
        isClosing = true;

        randWindowSize = new Vector2(rectTransform.sizeDelta.x * 0.8f, rectTransform.sizeDelta.y * 0.8f);
    }
}
