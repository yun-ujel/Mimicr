using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIMaster : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("References")]
    [HideInInspector] public RectTransform rectTransform; // Auto Assigned

    [HideInInspector] public int anchorIndex;
    [HideInInspector] public Vector2 minWindowSize;
    [HideInInspector] private Canvas canvas; // Auto Assigned

    [HideInInspector] public CanvasHandler canvasHandler;
    [HideInInspector] public GameObject windowToClose;

    [Header("Functions")]
    [SerializeField] public ClickFunction functionOnClick = ClickFunction.sendToTop;
    [SerializeField] public DragFunction functionOnDrag = DragFunction.none;
    public enum DragFunction
    {
        none,
        move,
        resize
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
        if (canvas == null) // Auto Assign Canvas
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
        if (rectTransform == null) // Auto Assign RectTransform as its own RectTransform
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
        else if (functionOnDrag == DragFunction.resize)
        {
            // Top Left
            if (anchorIndex == 0)
            {
                if (eventData.delta.x < 0 || rectTransform.sizeDelta.x > minWindowSize.x)
                {
                    rectTransform.offsetMin = new Vector2
                    (
                        rectTransform.offsetMin.x + eventData.delta.x / canvas.scaleFactor, // left
                        rectTransform.offsetMin.y
                    );
                }
                if (eventData.delta.y > 0 || rectTransform.sizeDelta.y > minWindowSize.y)
                {
                    rectTransform.offsetMax = new Vector2
                    (
                        rectTransform.offsetMax.x,
                        rectTransform.offsetMax.y + eventData.delta.y / canvas.scaleFactor  // top
                    );
                }
            }

            // Top
            else if (anchorIndex == 1)
            {
                if (eventData.delta.y > 0 || rectTransform.sizeDelta.y > minWindowSize.y)
                {
                    rectTransform.offsetMax = new Vector2
                    (
                        rectTransform.offsetMax.x,
                        rectTransform.offsetMax.y + eventData.delta.y / canvas.scaleFactor  // top
                    );
                }
            }

            // Top Right
            else if (anchorIndex == 2)
            {
                if (eventData.delta.y > 0 || rectTransform.sizeDelta.y > minWindowSize.y)
                {
                    rectTransform.offsetMax = new Vector2
                    (
                        rectTransform.offsetMax.x,
                        rectTransform.offsetMax.y + eventData.delta.y / canvas.scaleFactor  // top
                    );
                }
                if (eventData.delta.x > 0 || rectTransform.sizeDelta.x > minWindowSize.x)
                {
                    rectTransform.offsetMax = new Vector2
                    (
                        rectTransform.offsetMax.x + eventData.delta.x / canvas.scaleFactor, // right
                        rectTransform.offsetMax.y
                    );
                }
            }

            // Left
            else if (anchorIndex == 3)
            {
                if (eventData.delta.x < 0 || rectTransform.sizeDelta.x > minWindowSize.x)
                {
                    rectTransform.offsetMin = new Vector2
                    (
                        rectTransform.offsetMin.x + eventData.delta.x / canvas.scaleFactor, // left
                        rectTransform.offsetMin.y
                    );
                }
            }

            // Right
            else if (anchorIndex == 5)
            {
                if (eventData.delta.x > 0 || rectTransform.sizeDelta.x > minWindowSize.x)
                {
                    rectTransform.offsetMax = new Vector2
                    (
                        rectTransform.offsetMax.x + eventData.delta.x / canvas.scaleFactor, // right
                        rectTransform.offsetMax.y
                    );
                }
            }

            // Bottom Left
            else if (anchorIndex == 6)
            {
                if (eventData.delta.x < 0 || rectTransform.sizeDelta.x > minWindowSize.x)
                {
                    rectTransform.offsetMin = new Vector2
                    (
                        rectTransform.offsetMin.x + eventData.delta.x / canvas.scaleFactor, // left
                        rectTransform.offsetMin.y
                    );
                }
                if (eventData.delta.y < 0 || rectTransform.sizeDelta.y > minWindowSize.y)
                {
                    rectTransform.offsetMin = new Vector2
                    (
                        rectTransform.offsetMin.x,
                        rectTransform.offsetMin.y + eventData.delta.y / canvas.scaleFactor // bottom
                    );
                }
            }

            // Bottom
            else if (anchorIndex == 7)
            {
                if (eventData.delta.y < 0 || rectTransform.sizeDelta.y > minWindowSize.y)
                {
                    rectTransform.offsetMin = new Vector2
                    (
                        rectTransform.offsetMin.x,
                        rectTransform.offsetMin.y + eventData.delta.y / canvas.scaleFactor // bottom
                    );
                }
            }

            // Bottom Right
            else if (anchorIndex == 8)
            {
                if (eventData.delta.y < 0 || rectTransform.sizeDelta.y > minWindowSize.y)
                {
                    rectTransform.offsetMin = new Vector2
                    (
                        rectTransform.offsetMin.x,
                        rectTransform.offsetMin.y + eventData.delta.y / canvas.scaleFactor // bottom
                    );
                }
                if (eventData.delta.x > 0 || rectTransform.sizeDelta.x > minWindowSize.x)
                {
                    rectTransform.offsetMax = new Vector2
                    (
                        rectTransform.offsetMax.x + eventData.delta.x / canvas.scaleFactor, // right
                        rectTransform.offsetMax.y
                    );
                }
            }
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
            windowToClose.BroadcastMessage("OnWindowComplete");
        }
        else if (functionOnClick == ClickFunction.openNewWindow)
        {
            canvasHandler.InstantiateWindow();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (functionOnDrag == DragFunction.resize)
        {
            canvasHandler.OnCursorEnter(anchorIndex);
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