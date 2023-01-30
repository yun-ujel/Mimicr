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
    private Vector2 minWindowSize;                 // Randomized window size - minimum X and Y values
    [SerializeField] private Vector2 maxWindowSize;// Randomized window size - maximum X and Y values
    private Vector2 randWindowSize;                // Size for object to transition to when in closing or opening animation

    private bool isClosing = false;
    private bool isOpening = false;

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
        if (isClosing) // Closing Animation
        {
            if (canvasGroup.alpha > 0f)
            {
                rectTransform.sizeDelta = new Vector2
                (
                    Mathf.MoveTowards(rectTransform.sizeDelta.x, randWindowSize.x, 6f),
                    Mathf.MoveTowards(rectTransform.sizeDelta.y, randWindowSize.y, 6f)
                );

                canvasGroup.alpha -= Time.deltaTime * 8f;
            }
            else
            {
                isClosing = false;
                Destroy(gameObject);
            }
        }

        if (isOpening) // Opening Animation
        {
            if (canvasGroup.alpha < 1f)
            {
                rectTransform.sizeDelta = new Vector2
                (
                    Mathf.MoveTowards(rectTransform.sizeDelta.x, randWindowSize.x, 6f),
                    Mathf.MoveTowards(rectTransform.sizeDelta.y, randWindowSize.y, 6f)
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
        randWindowSize = new Vector2(rectTransform.sizeDelta.x * 0.8f, rectTransform.sizeDelta.y * 0.8f);

        isClosing = true; // Starts closing animation, will destroy object once finished

        canvasHandler.CompleteWindow(gameObject); // Removes object from CanvasHandler.windowsCurrentlyOpen
    }

    void OnWindowFail()
    {
        randWindowSize = new Vector2(rectTransform.sizeDelta.x * 0.8f, rectTransform.sizeDelta.y * 0.8f);

        isClosing = true; // Starts closing animation, will destroy object once finished
    }
}
