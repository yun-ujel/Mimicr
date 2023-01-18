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
            Vector2 unanchored = FullyUnanchorPosition(gameObject.GetComponent<RectTransform>());

            Debug.Log("Size On Canvas of " + gameObject.name + ": " + FindSizeOnCanvas(gameObject.GetComponent<RectTransform>()));
            Debug.Log("Unanchored Position Of " + gameObject.name + ": " + unanchored);
            //Debug.Log("Reanchored Position Of " + gameObject.name + ": " + FullyReanchorPosition(gameObject.GetComponent<RectTransform>(), unanchored));
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
        Vector2 randomizedPosition = FullyReanchorPosition(rT, new Vector2
        (
            Random.Range(0f, canvasRectTransform.sizeDelta.x - rT.sizeDelta.x),
            Random.Range(0f, canvasRectTransform.sizeDelta.y - rT.sizeDelta.y)
        ));

        rT.anchoredPosition = randomizedPosition;
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
    public Vector2 FullyUnanchorPosition(RectTransform rectTransform)
    {
        List<RectTransform> testListOfParents = new List<RectTransform>();
        RectTransform testTransform = rectTransform;
        while (testTransform.parent != null)
        {
            testListOfParents.Add(testTransform);
            testTransform = testTransform.parent.gameObject.GetComponent<RectTransform>();
        }

        RectTransform[] listOfParents = testListOfParents.ToArray();

        Vector2 transformSize = FindSizeOnCanvas(rectTransform);
        RectTransform childTransform = rectTransform;
        RectTransform parentTransform = childTransform.parent.GetComponent<RectTransform>();
        Vector2 relativePosition = new Vector2(0f, 0f);

        for (int i = 0; i < listOfParents.Length; i++)
        {
            parentTransform = childTransform.parent.GetComponent<RectTransform>();
            transformSize = FindSizeOnCanvas(childTransform);

            relativePosition = new Vector2
            (
                relativePosition.x + (childTransform.anchoredPosition.x - (transformSize.x * childTransform.pivot.x)) + (parentTransform.sizeDelta.x * ((childTransform.anchorMax.x + childTransform.anchorMin.x) / 2f)),
                relativePosition.y + (childTransform.anchoredPosition.y - (transformSize.y * childTransform.pivot.y)) + (parentTransform.sizeDelta.y * ((childTransform.anchorMax.y + childTransform.anchorMin.y) / 2f))
            );
            childTransform = parentTransform;
        }
        return relativePosition;
    }

    public void FindSizeOnCanvas2(RectTransform rectTransform)
    {
        List<RectTransform> testListOfParents = new List<RectTransform>();
        RectTransform testTransform = rectTransform;
        testListOfParents.Add(testTransform);
        while (testTransform.parent != null)
        {
            testTransform = testTransform.parent.gameObject.GetComponent<RectTransform>();
            testListOfParents.Add(testTransform);
        }

        RectTransform[] listOfParents = testListOfParents.ToArray();

        foreach (RectTransform joe in listOfParents)
        {
            Debug.Log(joe.gameObject.name + "is Parent of " + rectTransform.gameObject.name);
        }

        Vector2 returnedSizeDelta = new Vector2(0f, 0f);
        
        //return returnedSizeDelta;
    }



    public Vector2 FullyReanchorPosition(RectTransform rectTransform, Vector2 unanchoredPosition)
    {
        List<RectTransform> testListOfParents = new List<RectTransform>();
        RectTransform testTransform = rectTransform;
        while (testTransform.parent != null)
        {
            testListOfParents.Add(testTransform);
            testTransform = testTransform.parent.gameObject.GetComponent<RectTransform>();
        }

        RectTransform[] listOfParents = testListOfParents.ToArray();

        Vector2 transformSize = FindSizeOnCanvas(rectTransform);
        RectTransform childRectTransform = rectTransform;
        RectTransform parentRectTransform = childRectTransform.parent.GetComponent<RectTransform>();

        for (int i = 0; i < listOfParents.Length; i++)
        {
            parentRectTransform = childRectTransform.parent.GetComponent<RectTransform>();
            transformSize = FindSizeOnCanvas(childRectTransform);

            unanchoredPosition = new Vector2
            (
                (unanchoredPosition.x - (transformSize.x * childRectTransform.pivot.x)) + (parentRectTransform.sizeDelta.x * ((childRectTransform.anchorMax.x + childRectTransform.anchorMin.x) / 2f)),
                (unanchoredPosition.y - (transformSize.y * childRectTransform.pivot.y)) + (parentRectTransform.sizeDelta.y * ((childRectTransform.anchorMax.y + childRectTransform.anchorMin.y) / 2f))
            );
            childRectTransform = parentRectTransform;
        }


        return unanchoredPosition;
    }
}
