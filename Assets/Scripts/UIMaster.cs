using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class UIMaster : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
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

    [HideInInspector] public Canvas canvas;              // Auto Assigned, hidden always

    [HideInInspector] public GameObject windowToClose;
    // This is hidden by default and appears when functionOnClick is set to close
    // ClickFunction.close is used only for Window Completions (victories), not failures
    // Will Auto-Assign to the closest parent to the canvas if left empty

    [HideInInspector] public string messageToSend;
    // This is hidden by default and appears when functionOnClick is set to sendMessageToCanvas

    [Space]
    public Padding padding;

    [Header("Functions")]
    public FunctionOnClick functionOnClick = FunctionOnClick.sendToTop;
    public enum FunctionOnClick
    {
        none,
        sendToTop,
        close,
        sendMessageToCanvas
    }

    [HideInInspector] public DragFunction dragFunction;

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
                    if (windowToClose == null && functionOnClick == FunctionOnClick.close) // Auto Assign windowToClose
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
    }

    void Start()
    {
        if (rectTransform != GetComponent<RectTransform>() && dragFunction != null)
        {
            UIMaster parentUIMaster = rectTransform.GetComponent<UIMaster>();
            minWindowSize = parentUIMaster.minWindowSize; // Auto-assign the minimum size of the window to the size stated on the object to resize
            padding = parentUIMaster.padding;
        }

        if (dragFunction != null)
        {
            dragFunction.GetInfo(this);
        }
    }

    public void OnPointerDown(PointerEventData eventData) // When Object is Clicked
    {
        if (functionOnClick == FunctionOnClick.sendToTop)
        {
            rectTransform.SetAsLastSibling(); // Send to Top
        }
        else if (functionOnClick == FunctionOnClick.close) // ClickFunction.close is used only for Window Completions (victories), not failures
        {
            windowToClose.BroadcastMessage("OnWindowComplete");
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (functionOnClick == FunctionOnClick.sendMessageToCanvas)
        {
            canvas.SendMessage(messageToSend);
        }
    }

    public void OnPointerEnter(PointerEventData eventData) // When Object is hovered over
    {
        if (dragFunction != null)
        {
            canvas.SendMessage("OnCursorEnter", anchorIndex); // Tell Canvas to show resize indicator
        }
    }

    public void OnPointerExit(PointerEventData eventData) // When Object stops being hovered over
    {
        if (dragFunction != null)
        {
            canvas.SendMessage("OnCursorExit"); // Hide resize indicator
        }
    }
}