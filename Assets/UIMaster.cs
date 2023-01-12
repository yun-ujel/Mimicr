using UnityEngine;
using UnityEngine.EventSystems;

public class UIMaster : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    [Header("Functions")]
    public DragFunction functionOnDrag = DragFunction.none;
    public ClickFunction functionOnClick = ClickFunction.sendToTop;

    public enum DragFunction
    {
        none,
        move,
        resizeBoth,
        resizeWidth,
        resizeHeight
    }
    public enum ClickFunction
    {
        sendToTop,
        close,
        openOtherWindow,
        none
    }

    public GameObject otherWindow;

    [Header("References")]
    [SerializeField]
    private RectTransform rectTransform;
    [SerializeField]
    private Canvas canvas;
    [SerializeField]
    private GameObject windowParent;

    private void Awake()
    {
        if (canvas == null)
        {
            Transform testCanvasTransform = transform.parent;
            GameObject testParentObject = this.gameObject;
            while (testCanvasTransform != null)
            {
                canvas = testCanvasTransform.GetComponent<Canvas>();
                if (canvas != null)
                {
                    if (windowParent == null)
                    {
                        windowParent = testParentObject;
                    }

                    break;
                }
                testParentObject = testCanvasTransform.gameObject;
                testCanvasTransform = testCanvasTransform.parent;
            }
        }
        if (rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (functionOnDrag == DragFunction.move)
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
        else if (functionOnDrag == DragFunction.resizeBoth)
        {
            //rectTransform.sizeDelta += eventData.delta / canvas.scaleFactor; <----- This line was removed because it worked inversely if the pivot value was above 0.5
            rectTransform.sizeDelta = new Vector2
            (
                rectTransform.sizeDelta.x + eventData.delta.x * -Map(rectTransform.pivot.x, 0, 1, -1, 1) / canvas.scaleFactor,
                rectTransform.sizeDelta.y + eventData.delta.y * -Map(rectTransform.pivot.y, 0, 1, -1, 1) / canvas.scaleFactor
            );
        }
        else if (functionOnDrag == DragFunction.resizeWidth)
        {
            rectTransform.sizeDelta = new Vector2
               (
                   rectTransform.sizeDelta.x + eventData.delta.x * -Map(rectTransform.pivot.x, 0, 1, -1, 1) / canvas.scaleFactor,
                   rectTransform.sizeDelta.y
               );

        }
        else if (functionOnDrag == DragFunction.resizeHeight)
        {
            rectTransform.sizeDelta = new Vector2
            (
                rectTransform.sizeDelta.x,
                rectTransform.sizeDelta.y + eventData.delta.y * -Map(rectTransform.pivot.y, 0, 1, -1, 1) / canvas.scaleFactor
            );
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (functionOnClick == ClickFunction.sendToTop)
        {
            rectTransform.SetAsLastSibling();
        }
        else if (functionOnClick == ClickFunction.close)
        {
            Destroy(windowParent);
        }
    }


    private float Map(float input, float min1, float max1, float min2, float max2)
    {
        return min2 + (input - min1) * (max2 - min2) / (max1 - min1);
    }
}