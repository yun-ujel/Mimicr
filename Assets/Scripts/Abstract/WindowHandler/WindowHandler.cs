using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(RectTransform))]
// WindowHandler should be a newer form of MinigameHandler built as a base for all window types.
public abstract class WindowHandler : MonoBehaviour
{
    protected RectTransform rectTransform;
    protected CanvasGroup canvasGroup;

    protected Vector2 targetWindowSize;

    protected Vector2 minRandomWindowSize;                 // Randomized window size - minimum X and Y values
    [SerializeField] protected Vector2 maxRandomWindowSize;// Randomized window size - maximum X and Y values

    [SerializeField] protected bool randomizeSize;

    protected bool isOpeningAnim = false;
    protected bool isClosingAnim = false;

    protected float t;

    public virtual void Awake()
    {
        rectTransform = gameObject.GetComponent<RectTransform>();
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
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
        isOpeningAnim = true;
    }

    public virtual void FixedUpdate()
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
        t += 8 * Time.deltaTime;
    }

    public virtual void OnWindowComplete()
    {
        t = 0.0f;
        targetWindowSize = rectTransform.sizeDelta * 0.8f;
        isClosingAnim = true;
    }

    public virtual void OnWindowFail()
    {
        t = 0.0f;
        targetWindowSize = rectTransform.sizeDelta * 0.8f;
        isClosingAnim = true;
    }
}
