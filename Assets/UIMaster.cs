using UnityEngine;
using UnityEngine.EventSystems;

public class UIMaster : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    [Header("References")]
    [HideInInspector] public RectTransform rectTransform;

    [SerializeField] private Canvas canvas;

    [HideInInspector] public CanvasHandler canvasHandler;
    [HideInInspector] public GameObject windowToClose;
    [HideInInspector] public RectTransform movementBoundaries;

    [Header("Functions")]
    [SerializeField] public ClickFunction functionOnClick = ClickFunction.sendToTop;
    [SerializeField] public DragFunction functionOnDrag = DragFunction.none;
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
        none,
        sendToTop,
        close,
        openNewWindow
    }

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
                    if (windowToClose == null)
                    {
                        windowToClose = testParentObject;
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
        if (movementBoundaries == null)
        {
            movementBoundaries = canvas.GetComponent<RectTransform>();
        }
        canvasHandler = canvas.gameObject.GetComponent<CanvasHandler>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (functionOnDrag == DragFunction.move)
        {
            if(withinParentBoundaries(rectTransform, movementBoundaries))
            {
                rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
            }
        }
        else if (functionOnDrag == DragFunction.resizeBoth)
        {
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

        Debug.Log("Dragging");
    }

    private bool withinParentBoundaries(RectTransform rT, RectTransform parent)
    {
        Vector2 unanchoredPosition = canvasHandler.AnchoredToParentPosition(rT, parent);

        if
        (
            unanchoredPosition.x <= parent.sizeDelta.x - rT.sizeDelta.x ||
            unanchoredPosition.x >= 0f ||
            unanchoredPosition.y <= parent.sizeDelta.y - rT.sizeDelta.y ||
            unanchoredPosition.y >= 0f
        )
        {
            Debug.Log("true");
            return true;
        }
        else
        {
            Debug.Log("false");
            return false;
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
            Destroy(windowToClose);
        }
        else if (functionOnClick == ClickFunction.openNewWindow)
        {
            canvasHandler.SendMessage("InstantiateWindow", 1) ;
        }
    }
}