using UnityEngine;
using UnityEngine.EventSystems;

public class UIMaster : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    [Header("References")]
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Canvas canvas;
    private CanvasHandler canvasHandler;
    [SerializeField] private GameObject windowParent;

    [Header("Functions")]
    [SerializeField] private ClickFunction functionOnClick = ClickFunction.sendToTop;
    [SerializeField] private DragFunction functionOnDrag = DragFunction.none;
    private enum DragFunction
    {
        none,
        move,
        resizeBoth,
        resizeWidth,
        resizeHeight
    }
    private enum ClickFunction
    {
        none,
        sendToTop,
        close,
        openNewWindow
    }

    [HideInInspector] public GameObject otherWindow;

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
        if (otherWindow == null)
        {
            otherWindow = this.gameObject;
        }
        canvasHandler = canvas.gameObject.GetComponent<CanvasHandler>();
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
            //the pivot must be remapped to a range between -1 and 1 (instead of 0 and 1) in order to work consistently
            rectTransform.sizeDelta = new Vector2
            (
                rectTransform.sizeDelta.x + (eventData.delta.x * -RemapPivot(rectTransform.pivot.x) / canvas.scaleFactor),
                rectTransform.sizeDelta.y + (eventData.delta.y * -RemapPivot(rectTransform.pivot.y) / canvas.scaleFactor)
            );
        }
        else if (functionOnDrag == DragFunction.resizeWidth)
        {
            rectTransform.sizeDelta = new Vector2
               (
                   rectTransform.sizeDelta.x + (eventData.delta.x * -RemapPivot(rectTransform.pivot.x) / canvas.scaleFactor),
                   rectTransform.sizeDelta.y
               );

        }
        else if (functionOnDrag == DragFunction.resizeHeight)
        {
            rectTransform.sizeDelta = new Vector2
            (
                rectTransform.sizeDelta.x,
                rectTransform.sizeDelta.y + (eventData.delta.y * -RemapPivot(rectTransform.pivot.y) / canvas.scaleFactor)
            );
        }
    }

    private float RemapPivot(float input)
    {
        return (input * 2) - 1;
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
        else if (functionOnClick == ClickFunction.openNewWindow)
        {
            canvasHandler.SendMessage("InstantiateWindow", 1) ;
        }
    }
}