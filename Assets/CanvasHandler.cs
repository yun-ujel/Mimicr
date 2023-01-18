using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CanvasHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] windowsToOpen;
    [SerializeField] private GameObject[] windowsToLog;
    RectTransform canvasRectTransform;
    Canvas canvas;
    void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvasRectTransform = GetComponent<RectTransform>();
    }
    
    void Start()
    {
        foreach (GameObject gameObject in windowsToLog)
        {
            Debug.Log("Size On Canvas of " + gameObject.name + ": " + FindSizeOnCanvas(gameObject.GetComponent<RectTransform>()));
            Debug.Log("Unpivot Position Of " + gameObject.name + ": " + UnanchorPosition(gameObject.GetComponent<RectTransform>()));
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
        Vector2 randomizedPosition = CanvasToAnchoredPosition(rT, new Vector2
        (
            Random.Range(0f, canvasRectTransform.sizeDelta.x - rT.sizeDelta.x),
            Random.Range(0f, canvasRectTransform.sizeDelta.y - rT.sizeDelta.y)
        ));

        rT.anchoredPosition = randomizedPosition;
    }


    // Canvas Position methods have not yet been tested with anchors that have different Min and Max values
    public Vector2 AnchoredToCanvasPosition(RectTransform rectTransform)
    {
        List<RectTransform> testListOfParents = new List<RectTransform>();
        RectTransform testTransform = rectTransform;
        while (testTransform.parent != null)
        {
            testListOfParents.Add(testTransform);
            testTransform = testTransform.parent.gameObject.GetComponent<RectTransform>();
        }

        RectTransform[] listOfParents = testListOfParents.ToArray();

        RectTransform childRectTransform = rectTransform;
        RectTransform parentRectTransform = childRectTransform.parent.GetComponent<RectTransform>();
        Vector2 relativePosition = childRectTransform.anchoredPosition;

        for (int i = 0; i < listOfParents.Length; i++)
        {
            parentRectTransform = childRectTransform.parent.GetComponent<RectTransform>();
            relativePosition = new Vector2
                (
                    (relativePosition.x - (childRectTransform.sizeDelta.x * childRectTransform.pivot.x))
                    + (parentRectTransform.sizeDelta.x * childRectTransform.anchorMax.x),

                    (relativePosition.y - (childRectTransform.sizeDelta.y * childRectTransform.pivot.y))
                    + (parentRectTransform.sizeDelta.y * childRectTransform.anchorMax.y)
                );
            childRectTransform = parentRectTransform;
        }
        return relativePosition;
    }
    public Vector2 CanvasToAnchoredPosition(RectTransform rectTransform, Vector2 screenPosition)
    {
        List<RectTransform> testListOfParents = new List<RectTransform>();
        RectTransform testTransform = rectTransform;
        while (testTransform.parent != null)
        {
            testListOfParents.Add(testTransform);
            testTransform = testTransform.parent.gameObject.GetComponent<RectTransform>();
        }

        RectTransform[] listOfParents = testListOfParents.ToArray();

        RectTransform childRectTransform = rectTransform;
        RectTransform parentRectTransform = childRectTransform.parent.GetComponent<RectTransform>();
        Vector2 universalPosition = screenPosition;

        for (int i = 0; i < listOfParents.Length; i++)
        {
            parentRectTransform = childRectTransform.parent.GetComponent<RectTransform>();
            universalPosition = new Vector2
                (
                    (universalPosition.x - (parentRectTransform.sizeDelta.x * childRectTransform.anchorMax.x))
                    + (childRectTransform.sizeDelta.x * childRectTransform.pivot.x),

                    (universalPosition.y - (parentRectTransform.sizeDelta.y * childRectTransform.anchorMax.y))
                    + (childRectTransform.sizeDelta.y * childRectTransform.pivot.y)
                );
            childRectTransform = parentRectTransform;
        }
        return universalPosition;
    }

    private float Remap(float inputValue, float inMin, float inMax, float outMin, float outMax)
    {
        return (inputValue - inMin) / (inMax - inMin) * (outMax - outMin) + outMin;
    }


    public Vector2 FindSizeOnCanvas(RectTransform rectTransform)
    {
        RectTransform parentTransform = rectTransform.parent.GetComponent<RectTransform>();

        Vector2 returnedSizeDelta = new Vector2
        (
            (parentTransform.sizeDelta.x * Mathf.Abs(rectTransform.anchorMax.x - rectTransform.anchorMin.x)) + (rectTransform.offsetMax.x - rectTransform.offsetMin.x),
            (parentTransform.sizeDelta.y * Mathf.Abs(rectTransform.anchorMax.y - rectTransform.anchorMin.y)) + (rectTransform.offsetMax.y - rectTransform.offsetMin.y)
        );

        return returnedSizeDelta;
    }

    public Vector2 UnanchorPosition(RectTransform rectTransform)
    {
        Vector2 transformSize = FindSizeOnCanvas(rectTransform);
        RectTransform parentTransform = rectTransform.parent.GetComponent<RectTransform>();

        Vector2 basePivotPosition = new Vector2
            (
                rectTransform.anchoredPosition.x - (transformSize.x * rectTransform.pivot.x),
                rectTransform.anchoredPosition.y - (transformSize.y * rectTransform.pivot.y)
            );

        Vector2 returnedPosition = new Vector2
        (
            basePivotPosition.x + (parentTransform.sizeDelta.x * ((rectTransform.anchorMax.x + rectTransform.anchorMin.x) / 2f)),
            basePivotPosition.y + (parentTransform.sizeDelta.y * ((rectTransform.anchorMax.y + rectTransform.anchorMin.y) / 2f))
        );

        return returnedPosition;
    }


}
