using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class UIMaster : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("References")]
    [HideInInspector] public RectTransform rectTransform; // RectTransform used to move, resize or send to top. Usually a direct child of the canvas/very high in the hierarchy
                                                          // Will Auto-Assign to itself if left empty
    [HideInInspector] public int anchorIndex;
    // The anchorIndex is used for DragFunction.resize, and tells where the object should be resized from (e.g. top left = 0, middle right = 5, bottom right = 8, etc.)
    // Goes from 0 to 8 (index 4 has no function)
    // This is hidden by default and appears as a 3x3 grid (with an empty center) when functionOnDrag is set to resize

    [HideInInspector] public Vector2 minWindowSize;
    // The minimum size the window can be resized to.
    // This is hidden by default and appears when the rectTransform variable is set to the object's own (or left empty)
    // (Essentially, it only appears if this object is the one that'll be moved or resized)

    [HideInInspector] private Canvas canvas;              // Auto Assigned, hidden always
    [HideInInspector] public CanvasHandler canvasHandler; // Auto Assigned, hidden always

    [HideInInspector] public GameObject windowToClose;
    // This is hidden by default and appears when functionOnClick is set to close
    // ClickFunction.close is used only for Window Completions (victories), not failures
    // Will Auto-Assign to the closest parent to the canvas if left empty

    [HideInInspector] public string messageToSend;
    // This is hidden by default and appears when functionOnClick is set to sendMessageToCanvas

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
        sendMessageToCanvas
    }

    private void Awake()
    {
        if (canvas == null) // Auto Assign Canvas
        {
            Transform testCanvasTransform = transform.parent;
            GameObject testParentObject = this.gameObject;

            // Repeatedly run through parents until there's either no parent above it, or if the Canvas is found
            while (testCanvasTransform != null)
            {
                canvas = testCanvasTransform.GetComponent<Canvas>();
                if (canvas != null)
                {
                    if (windowToClose == null && functionOnClick == ClickFunction.close) // Auto Assign windowToClose
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
        if (functionOnDrag == DragFunction.resize)
        {
                                                                                      
            if (rectTransform != GetComponent<RectTransform>())                       
            {
                minWindowSize = rectTransform.GetComponent<UIMaster>().minWindowSize; // Auto-assign the minimum size of the window to the size stated on the object to resize
            }
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
                if (eventData.delta.x < 0 || rectTransform.sizeDelta.x > minWindowSize.x) // If the mouse is moving left or if the width is above minimum
                {
                    rectTransform.offsetMin = new Vector2
                    (
                        rectTransform.offsetMin.x + eventData.delta.x / canvas.scaleFactor, // left
                        rectTransform.offsetMin.y
                    );
                }
                if (eventData.delta.y > 0 || rectTransform.sizeDelta.y > minWindowSize.y) // If the mouse is moving up or if the height is above minimum
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
                if (eventData.delta.y > 0 || rectTransform.sizeDelta.y > minWindowSize.y) // If the mouse is moving up or the height is above minimum
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
                if (eventData.delta.x > 0 || rectTransform.sizeDelta.x > minWindowSize.x) // If the mouse is moving right or if the width is above minimum
                {
                    rectTransform.offsetMax = new Vector2
                    (
                        rectTransform.offsetMax.x + eventData.delta.x / canvas.scaleFactor, // right
                        rectTransform.offsetMax.y
                    );
                }
                if (eventData.delta.y > 0 || rectTransform.sizeDelta.y > minWindowSize.y) // If the mouse is moving up or if the height is above minimum
                {
                    rectTransform.offsetMax = new Vector2
                    (
                        rectTransform.offsetMax.x,
                        rectTransform.offsetMax.y + eventData.delta.y / canvas.scaleFactor  // top
                    );
                }
            }

            // Left
            else if (anchorIndex == 3)
            {
                if (eventData.delta.x < 0 || rectTransform.sizeDelta.x > minWindowSize.x) // If the mouse is moving left or if the width is above minimum
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
                if (eventData.delta.x > 0 || rectTransform.sizeDelta.x > minWindowSize.x) // If the mouse is moving right or if the width is above minimum
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
                if (eventData.delta.x < 0 || rectTransform.sizeDelta.x > minWindowSize.x) // If the mouse is moving left or if the width is above minimum
                {
                    rectTransform.offsetMin = new Vector2
                    (
                        rectTransform.offsetMin.x + eventData.delta.x / canvas.scaleFactor, // left
                        rectTransform.offsetMin.y
                    );
                }
                if (eventData.delta.y < 0 || rectTransform.sizeDelta.y > minWindowSize.y) // If the mouse is moving down or if the height is above minimum
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
                if (eventData.delta.y < 0 || rectTransform.sizeDelta.y > minWindowSize.y) // If the mouse is moving down or if the height is above minimum
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
                if (eventData.delta.x > 0 || rectTransform.sizeDelta.x > minWindowSize.x) // If the mouse is moving right or if the width is above minimum
                {
                    rectTransform.offsetMax = new Vector2
                    (
                        rectTransform.offsetMax.x + eventData.delta.x / canvas.scaleFactor, // right
                        rectTransform.offsetMax.y
                    );
                }
                if (eventData.delta.y < 0 || rectTransform.sizeDelta.y > minWindowSize.y) // If the mouse is moving down or if the height is above minimum
                {
                    rectTransform.offsetMin = new Vector2
                    (
                        rectTransform.offsetMin.x,
                        rectTransform.offsetMin.y + eventData.delta.y / canvas.scaleFactor // bottom
                    );
                }
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData) // When Object is Clicked
    {
        if (functionOnClick == ClickFunction.sendToTop)
        {
            rectTransform.SetAsLastSibling(); // Send to Top
        }
        else if (functionOnClick == ClickFunction.close) // ClickFunction.close is used only for Window Completions (victories), not failures
        {
            windowToClose.BroadcastMessage("OnWindowComplete");
        }
        else if (functionOnClick == ClickFunction.sendMessageToCanvas)
        {
            canvasHandler.SendMessage(messageToSend);
        }
    }

    public void OnPointerEnter(PointerEventData eventData) // When Object is hovered over
    {
        if (functionOnDrag == DragFunction.resize) // Used for resize indicator
        {
            canvasHandler.OnCursorEnter(anchorIndex); // Tell Canvas to show resize indicator
        }
    }

    public void OnPointerExit(PointerEventData eventData) // When Object stops being hovered over
    {
        if (!eventData.dragging) 
        {
            canvasHandler.OnCursorExit(); // Hide resize indicator
        }
    }
}