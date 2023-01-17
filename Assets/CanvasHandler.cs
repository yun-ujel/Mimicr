using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CanvasHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] windowsToOpen;
    //RectTransform canvasRectTransform;
    Canvas canvas;
    void Awake()
    {
        canvas = GetComponent<Canvas>();
        //canvasRectTransform = GetComponent<RectTransform>();
    }
    
    void Start()
    {

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
        Debug.Log("Number of Parents of " + rectTransform.gameObject.name + ": " + listOfParents.Length.ToString());

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
        Debug.Log("Number of Parents of " + rectTransform.gameObject.name + ": " + listOfParents.Length.ToString());

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
}
