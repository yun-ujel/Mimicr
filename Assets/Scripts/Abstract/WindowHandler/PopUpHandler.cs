using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PopUpHandler should be a newer form of MinigameHandler built as a base for windows in the Stack.
public class PopUpHandler : WindowHandler
{
    [Header("References")]
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    [SerializeField] bool randomizeSizeOnSpawn;
    [Header("Sizes")]
    private Vector2 minWindowSize;                 // Randomized window size - minimum X and Y values
    [SerializeField] private Vector2 maxWindowSize;// Randomized window size - maximum X and Y values
    private Vector2 randWindowSize;                // Size for object to transition to when in closing or opening animation

    private bool isClosing = false;
    private bool isOpening = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
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

    public override void OnWindowComplete()
    {
        randWindowSize = new Vector2(rectTransform.sizeDelta.x * 0.8f, rectTransform.sizeDelta.y * 0.8f);

        isClosing = true; // Starts closing animation, will destroy object once finished

        canvas.SendMessage("CompleteWindow", gameObject); // Removes object from CanvasHandler.windowsCurrentlyOpen
    }

    public override void OnWindowFail()
    {
        randWindowSize = new Vector2(rectTransform.sizeDelta.x * 0.8f, rectTransform.sizeDelta.y * 0.8f);

        isClosing = true; // Starts closing animation, will destroy object once finished
    }

    public override void OnWindowStart()
    {
        if (randomizeSizeOnSpawn)
        {
            minWindowSize = GetComponent<UIMaster>().minWindowSize; // Auto-set the minimum window size to the size set on UIMaster

            canvasGroup.alpha = 0f;

            randWindowSize = new Vector2
            (
                Random.Range(minWindowSize.x, maxWindowSize.x),
                Random.Range(minWindowSize.y, maxWindowSize.y)
            );

            rectTransform.sizeDelta = new Vector2(randWindowSize.x * 0.8f, randWindowSize.y * 0.8f);
            isOpening = true;
        }
        else
        {
            canvasGroup.alpha = 0f;

            randWindowSize = rectTransform.sizeDelta;

            rectTransform.sizeDelta = new Vector2(randWindowSize.x * 0.8f, randWindowSize.y * 0.8f);
            isOpening = true;
        }
    }
}