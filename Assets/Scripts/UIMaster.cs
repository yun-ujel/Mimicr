using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIMaster : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("References")]
    [HideInInspector] public RectTransform rectTransform;

    [HideInInspector] public Vector2 minWindowSize;
    [SerializeField] private Canvas canvas;

    [HideInInspector] public CanvasHandler canvasHandler;
    [HideInInspector] public GameObject windowToClose;

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
        canvasHandler = canvas.gameObject.GetComponent<CanvasHandler>();
    }

    void Start()
    {
        if (rectTransform != GetComponent<RectTransform>())
        {
            minWindowSize = rectTransform.GetComponent<UIMaster>().minWindowSize;
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
            if ((eventData.delta.x * -RemapPivot(rectTransform.pivot.x) / canvas.scaleFactor) > 0 || rectTransform.sizeDelta.x > minWindowSize.x)
            {
                rectTransform.sizeDelta = new Vector2
               (
                   rectTransform.sizeDelta.x + (eventData.delta.x * -RemapPivot(rectTransform.pivot.x) / canvas.scaleFactor),
                   rectTransform.sizeDelta.y
               );
            }

            if ((eventData.delta.y * -RemapPivot(rectTransform.pivot.y) / canvas.scaleFactor) > 0 || rectTransform.sizeDelta.y > minWindowSize.y)
            {
                rectTransform.sizeDelta = new Vector2
                (
                    rectTransform.sizeDelta.x,
                    rectTransform.sizeDelta.y + (eventData.delta.y * -RemapPivot(rectTransform.pivot.y) / canvas.scaleFactor)
                );
            }
        }
        else if (functionOnDrag == DragFunction.resizeWidth)
        {
            if ((eventData.delta.x * -RemapPivot(rectTransform.pivot.x) / canvas.scaleFactor) > 0 || rectTransform.sizeDelta.x > minWindowSize.x)
            {
                rectTransform.sizeDelta = new Vector2
               (
                   rectTransform.sizeDelta.x + (eventData.delta.x * -RemapPivot(rectTransform.pivot.x) / canvas.scaleFactor),
                   rectTransform.sizeDelta.y
               );
            }
        }
        else if (functionOnDrag == DragFunction.resizeHeight)
        {
            if ((eventData.delta.y * -RemapPivot(rectTransform.pivot.y) / canvas.scaleFactor) > 0 || rectTransform.sizeDelta.y > minWindowSize.y)
            {
                rectTransform.sizeDelta = new Vector2
                (
                    rectTransform.sizeDelta.x,
                    rectTransform.sizeDelta.y + (eventData.delta.y * -RemapPivot(rectTransform.pivot.y) / canvas.scaleFactor)
                );
            }
        }

        //Debug.Log(eventData.delta);
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
            canvasHandler.SendMessage("InstantiateWindow", 1);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (functionOnDrag == DragFunction.resizeWidth)
        {
            canvasHandler.OnCursorEnter(0);
        }
        else if (functionOnDrag == DragFunction.resizeBoth)
        {
            canvasHandler.OnCursorEnter(1);
        }
        else if (functionOnDrag == DragFunction.resizeHeight)
        {
            canvasHandler.OnCursorEnter(2);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!eventData.dragging)
        {
            canvasHandler.OnCursorExit();
        }
    }
}