using UnityEngine;
using UnityEngine.UI;
using TMPro;

// StackHandler should be a newer form of MinigameHandler built as a base for windows in the Stack.
public class StackHandler : WindowHandler
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI header;
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private AccountInfo accountInfo;


    [Header("Sizes")]
    private Vector2 minRandomWindowSize;                 // Randomized window size - minimum X and Y values
    [SerializeField] private Vector2 maxRandomWindowSize;// Randomized window size - maximum X and Y values
    private Vector2 randWindowSize;                      // Size for object to transition to when in closing or opening animation

    private bool isClosing = false;
    private bool isOpening = false;

    void Awake()
    {
        minRandomWindowSize = GetComponent<UIMaster>().minWindowSize; // Auto-set the minimum window size to the size set on UIMaster
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

        canvas.SendMessage("CompleteStackWindow", accountInfo);
    }

    public override void OnWindowFail()
    {
        randWindowSize = new Vector2(rectTransform.sizeDelta.x * 0.8f, rectTransform.sizeDelta.y * 0.8f);

        isClosing = true; // Starts closing animation, will destroy object once finished
    }

    public override void OnWindowStart()
    {
        if (maxRandomWindowSize.x + maxRandomWindowSize.y < minRandomWindowSize.x + minRandomWindowSize.y)
        {
            Debug.Log("Max Random Size of " + gameObject.name + " is below Min Random size, spawn size will be fixed at minimum");
            maxRandomWindowSize = minRandomWindowSize;
        }

        canvasGroup.alpha = 0f;

        randWindowSize = new Vector2
        (
            Random.Range(minRandomWindowSize.x, maxRandomWindowSize.x),
            Random.Range(minRandomWindowSize.y, maxRandomWindowSize.y)
        );

        rectTransform.sizeDelta = new Vector2(randWindowSize.x, randWindowSize.y);

        randWindowSize *= 1.2f;

        isOpening = true;
    }

    void OnStackStart(AccountInfo inAccountInfo)
    {
        inAccountInfo.windows[inAccountInfo.CurrentWindowIndex] = gameObject;
        accountInfo = inAccountInfo;
        header.text = gameObject.name + " - " + "Account #" + (accountInfo.accountIndex + 1).ToString();
    }
}
