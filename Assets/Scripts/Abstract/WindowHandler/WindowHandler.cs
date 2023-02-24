using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(RectTransform))]
// WindowHandler should be a newer form of MinigameHandler built as a base for all window types.
public abstract class WindowHandler : MonoBehaviour
{
    [Header("References")]
    protected RectTransform rectTransform;
    protected CanvasGroup canvasGroup;

    [Header("Random Sizing")]
    protected Vector2 targetWindowSize;
    protected Vector2 minRandomWindowSize;                 // Randomized window size - minimum X and Y values
    [SerializeField] protected Vector2 maxRandomWindowSize;// Randomized window size - maximum X and Y values
    [SerializeField] protected bool randomizeSize;

    [Header("Opening/Closing Animations")]
    protected bool isOpeningAnim = false;
    protected bool isClosingAnim = false;

    protected float t;

    [Header("Failing Timer")]
    protected bool isRunningTimer;
    protected bool isTimerLow;
    protected float failTimeCounter;

    // Blinking / Alert
    protected float blinkFrequency = 0.1f;
    protected float blinkTimeCounter;
    protected bool blinkFlip;

    protected ColourController[] topColourControllers;
    protected ColourType[] topColourTypes;

    public virtual void Awake()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
        canvasGroup = gameObject.GetComponent<CanvasGroup>();

        List<ColourController> colourControllers = new List<ColourController>();
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).TryGetComponent(out ColourController cC))
                colourControllers.Add(cC);
        }
        topColourControllers = colourControllers.ToArray();

        topColourTypes = new ColourType[topColourControllers.Length];
        for (int i = 0; i < topColourTypes.Length; i++)
        {
            topColourTypes[i] = topColourControllers[i].colourType;
        }

    }

    public virtual void OnWindowStart()
    {
        if (gameObject.TryGetComponent(out UIMaster uIMaster))
        {
            minRandomWindowSize = uIMaster.minWindowSize;
        }
        else
        {
            randomizeSize = false;
        }

        if (maxRandomWindowSize.x + maxRandomWindowSize.y < minRandomWindowSize.x + minRandomWindowSize.y && randomizeSize)
        {
            Debug.Log("Max Random Size of " + gameObject.name + " is below Min Random size, spawn size will be fixed at minimum");
            maxRandomWindowSize = minRandomWindowSize;
        }
        if (randomizeSize)
        {
            targetWindowSize = new Vector2
            (
                Random.Range(minRandomWindowSize.x, maxRandomWindowSize.x),
                Random.Range(minRandomWindowSize.y, maxRandomWindowSize.y)
            );
        }
        else
        {
            targetWindowSize = rectTransform.sizeDelta;
        }

        t = 0.0f;
        canvasGroup.alpha = 0f;
        rectTransform.sizeDelta = targetWindowSize * 0.8f;
        canvasGroup.interactable = true;
        isOpeningAnim = true;
    }

    public virtual void Update()
    {
        if (isOpeningAnim)
        {
            if (canvasGroup.alpha < 1f)
            {
                rectTransform.sizeDelta = new Vector2
                (
                    Mathf.Lerp(rectTransform.sizeDelta.x, targetWindowSize.x, t),
                    Mathf.Lerp(rectTransform.sizeDelta.y, targetWindowSize.y, t)
                );

                canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1f, t);
            }
            else
            {
                canvasGroup.alpha = 1f;
                isOpeningAnim = false;
                rectTransform.sizeDelta = targetWindowSize;
            }
        }
        else if (isClosingAnim)
        {
            if (canvasGroup.alpha > 0f)
            {
                rectTransform.sizeDelta = new Vector2
                (
                    Mathf.Lerp(rectTransform.sizeDelta.x, targetWindowSize.x, t),
                    Mathf.Lerp(rectTransform.sizeDelta.y, targetWindowSize.y, t)
                );

                canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0f, t);
            }
            else
            {
                canvasGroup.alpha = 0f;
                isClosingAnim = false;
                Destroy(gameObject);
            }
        }
        t += 2 * Time.deltaTime;


        
        if (isRunningTimer && failTimeCounter > 0f)
        {
            failTimeCounter -= Time.deltaTime;
        }
        else if (isRunningTimer && !isClosingAnim)
        {
            isRunningTimer = false;
            failTimeCounter = 0f;
            OnWindowFail();
        }

        if (failTimeCounter < 5f && isRunningTimer)
        {
            BroadcastMessage("FailTimerLow");
            if (!isTimerLow)
            {
                SendMessage("TriggerShake", new Vector2(0.25f, 10f), SendMessageOptions.DontRequireReceiver);
                isTimerLow = true;
            }
        }
    }

    public virtual void OnWindowComplete()
    {
        t = 0.0f;
        targetWindowSize = rectTransform.sizeDelta * 0.8f;
        canvasGroup.interactable = false;
        isClosingAnim = true;
    }

    public virtual void OnWindowFail()
    {
        //Debug.Log("Failed " + gameObject.name);
        t = 0.0f;
        targetWindowSize = rectTransform.sizeDelta * 0.8f;
        canvasGroup.interactable = false;
        isClosingAnim = true;
    }

    public virtual void StartFailTimer(float timeUntilFail)
    {
        if (timeUntilFail > 0f)
        {
            //Debug.Log("Started Fail Timer on " + gameObject.name);
            isRunningTimer = true;
            isTimerLow = false;
            failTimeCounter = timeUntilFail;
        }
    }

    public virtual void FailTimerLow()
    {
        if (blinkTimeCounter < blinkFrequency)
        {
            blinkTimeCounter += Time.deltaTime;
        }
        else
        {
            blinkTimeCounter = 0f;
            blinkFlip = !blinkFlip;
        }

        if (blinkFlip)
        {
            for (int i = 0; i < topColourControllers.Length; i++)
            {
                topColourControllers[i].colourType = ColourType.wildCard;

                topColourControllers[i].OnColourUpdate(null);
            }
        }
        else
        {
            for (int i = 0; i < topColourControllers.Length; i++)
            {
                topColourControllers[i].colourType = topColourTypes[i];

                topColourControllers[i].OnColourUpdate(null);
            }
        }
    }
}
