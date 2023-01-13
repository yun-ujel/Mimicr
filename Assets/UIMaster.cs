using UnityEngine;
using UnityEngine.EventSystems;

public class UIMaster : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    //[Header("Functions")]
    //public ClickFunction functionOnClick = ClickFunction.sendToTop;
    //public DragFunction functionOnDrag = DragFunction.none;
    //public enum DragFunction { none, move, resizeBoth, resizeWidth, resizeHeight }
    public enum ClickFunction { none, sendToTop, close, openOtherWindow }

    public int clickFunctionIndex = 1;
    public int dragFunctionIndex = 0;

    [HideInInspector] public GameObject otherWindow;

    [Header("References")]
    [HideInInspector] public RectTransform rectTransform;
    [HideInInspector] public Canvas canvas;
    [HideInInspector] public GameObject windowParent;

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
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragFunctionIndex == 1) // Move
        {
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
        else if (dragFunctionIndex == 2) // Resize Both Height and Width
        {
            rectTransform.sizeDelta = new Vector2
            (
                rectTransform.sizeDelta.x + eventData.delta.x * -Map(rectTransform.pivot.x, 0, 1, -1, 1) / canvas.scaleFactor,
                rectTransform.sizeDelta.y + eventData.delta.y * -Map(rectTransform.pivot.y, 0, 1, -1, 1) / canvas.scaleFactor
            );
        }
        else if (dragFunctionIndex == 3) // Resize Width
        {
            rectTransform.sizeDelta = new Vector2
               (
                   rectTransform.sizeDelta.x + eventData.delta.x * -Map(rectTransform.pivot.x, 0, 1, -1, 1) / canvas.scaleFactor,
                   rectTransform.sizeDelta.y
               );

        }
        else if (dragFunctionIndex == 4) // Resize Height
        {
            rectTransform.sizeDelta = new Vector2
            (
                rectTransform.sizeDelta.x,
                rectTransform.sizeDelta.y + eventData.delta.y * -Map(rectTransform.pivot.y, 0, 1, -1, 1) / canvas.scaleFactor
            );
        }
        else if (dragFunctionIndex == 0) // None
        {

        }
        else
        {
            Debug.Log("Drag Function not recognised");
        }
    }

    public void OnPointerDown(PointerEventData eventData) // On Click
    {
        if (clickFunctionIndex == 1) // Send to Top
        {
            rectTransform.SetAsLastSibling();
        }
        else if (clickFunctionIndex == 2) // Close
        {
            Destroy(windowParent);
        }
        else if (clickFunctionIndex == 3) // Open Other Window
        {
            Instantiate
                (
                otherWindow, 
                new Vector3
                    (
                        canvas.transform.position.x,
                        canvas.transform.position.y,
                        canvas.transform.position.z
                    ), 
                Quaternion.identity, 
                canvas.gameObject.transform
                );
        }
        else if (clickFunctionIndex == 0) // None
        {

        }
        else
        {
            Debug.Log("Click Function not recognised");
        }
    }


    private float Map(float input, float min1, float max1, float min2, float max2)
    {
        return min2 + (input - min1) * (max2 - min2) / (max1 - min1);
    }
}