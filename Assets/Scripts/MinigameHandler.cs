using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameHandler : MonoBehaviour
{
    private UIMaster topUIMaster;
    [SerializeField] private MonoBehaviour minigame;
    private RectTransform rectTransform;
    private CanvasHandler canvasHandler;


    [Header("Sizes")]
    private Vector2 minWindowSize;
    [SerializeField] private Vector2 maxWindowSize;

    private bool isClosing = false;
    private float closingWindowCounter;

    void Awake()
    {
        topUIMaster = GetComponent<UIMaster>();
        rectTransform = GetComponent<RectTransform>();
        canvasHandler = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasHandler>();

        minWindowSize = topUIMaster.minWindowSize;
    }

    void Update()
    {
        if (isClosing)
        {
            if (rectTransform.sizeDelta.y > 40f)
            {
                rectTransform.sizeDelta = new Vector2(Mathf.MoveTowards(rectTransform.sizeDelta.x, 0f, 20f), Mathf.MoveTowards(rectTransform.sizeDelta.y, 0f, 20f));
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    void OnWindowStart()
    {
        rectTransform.sizeDelta = new Vector2
        (
            Random.Range(minWindowSize.x, maxWindowSize.x),
            Random.Range(minWindowSize.y, maxWindowSize.y)
        );
    }

    void OnWindowComplete()
    {
        isClosing = true;

        canvasHandler.CompleteWindow();
    }
}
