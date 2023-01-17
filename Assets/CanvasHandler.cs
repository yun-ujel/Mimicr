using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CanvasHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] windowsToOpen;
    RectTransform canvasRectTransform;
    Canvas canvas;
    void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvasRectTransform = GetComponent<RectTransform>();
    }
    
    void Start()
    {
        foreach (GameObject window in windowsToOpen)
        {
            RectTransform rT = window.GetComponent<RectTransform>();
            Vector2 screenPosition = AnchoredToScreenPosition(rT);
            Debug.Log("Screen Position of " + window.name + ": " + screenPosition.ToString());
            Debug.Log("Anchored Position of " + window.name + ": " + ScreenToAnchoredPosition(rT, screenPosition).ToString());
        }
    }

    public void InstantiateWindow(int windowIndex)
    {
        // Randomly select and Instantiate window as a child of this object
        int selection = Random.Range(0, windowsToOpen.Length);
        GameObject windowToInstantiate = windowsToOpen[selection];

        GameObject newWindow = Instantiate(windowToInstantiate, transform);

        // Randomize position of the object
        RectTransform rT = newWindow.GetComponent<RectTransform>();
    }

    public Vector2 AnchoredToScreenPosition(RectTransform rectTransform)
    {
        Vector2 basePivotPosition = new Vector2
            (
                rectTransform.anchoredPosition.x - (rectTransform.sizeDelta.x * rectTransform.pivot.x),
                rectTransform.anchoredPosition.y - (rectTransform.sizeDelta.y * rectTransform.pivot.y)
            );

        Vector2 returnedPosition = new Vector2
        (
            basePivotPosition.x + (canvasRectTransform.sizeDelta.x * rectTransform.anchorMax.x),
            basePivotPosition.y + (canvasRectTransform.sizeDelta.y * rectTransform.anchorMax.y)
        );

        return returnedPosition;
    }

    public Vector2 ScreenToAnchoredPosition(RectTransform rectTransform, Vector2 screenPosition)
    {
        Vector2 reanchoredPosition = new Vector2
        (
            screenPosition.x - (canvasRectTransform.sizeDelta.x * rectTransform.anchorMax.x),
            screenPosition.y - (canvasRectTransform.sizeDelta.y * rectTransform.anchorMax.y)
        );
        Vector2 returnedPosition = new Vector2
        (
            reanchoredPosition.x + (rectTransform.sizeDelta.x * rectTransform.pivot.x),
            reanchoredPosition.y + (rectTransform.sizeDelta.y * rectTransform.pivot.y)
        );
        return returnedPosition;
    }

    private float Remap(float inputValue, float inMin, float inMax, float outMin, float outMax)
    {
        return (inputValue - inMin) / (inMax - inMin) * (outMax - outMin) + outMin;
    }
}
