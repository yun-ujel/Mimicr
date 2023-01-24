using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CanvasHandler : MonoBehaviour
{
    public float currentScore;

    [SerializeField] private GameObject[] windowsToOpen;
    [SerializeField] private GameObject poemWindow;

    [SerializeField] int maxWindowsOpen = 10;
    [SerializeField] List<GameObject> windowsCurrentlyOpen = new List<GameObject>();

    [SerializeField] private GameObject cursor;
    private SpriteRenderer cursorRenderer;

    RectTransform canvasRectTransform;
    Canvas canvas;
    void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvasRectTransform = GetComponent<RectTransform>();

        if (cursor == null)
        {
            cursor = GameObject.FindGameObjectWithTag("IsCursorObject");
        }
        cursorRenderer = cursor.GetComponent<SpriteRenderer>();
    }
    

    void Start()
    {
        cursorRenderer.color = new Color
        (
            cursorRenderer.color.r,
            cursorRenderer.color.g,
            cursorRenderer.color.b,
            0f
        );
    }

    void Update()
    {
        if (windowsCurrentlyOpen.Count > maxWindowsOpen)
        {
            GameObject objectToDestroy = windowsCurrentlyOpen[0];

            windowsCurrentlyOpen.Remove(objectToDestroy);

            objectToDestroy.BroadcastMessage("OnWindowFail");

            currentScore -= 1;
        }

        if (currentScore > 10f)
        {
            
        }
    }

    public void InstantiateWindow()
    {
        // Randomly select and Instantiate window as a child of this object
        int selection = Random.Range(0, windowsToOpen.Length);
        GameObject windowToInstantiate = windowsToOpen[selection];

        GameObject newWindow = Instantiate(windowToInstantiate, transform);
        windowsCurrentlyOpen.Add(newWindow);

        // Randomize position of the object
        RectTransform rT = newWindow.GetComponent<RectTransform>();
        Vector2 randomizedPosition = ReanchorPosition(rT, new Vector2
        (
            Random.Range(0f, canvasRectTransform.sizeDelta.x - rT.sizeDelta.x),
            Random.Range(0f, canvasRectTransform.sizeDelta.y - rT.sizeDelta.y)
        ));

        rT.anchoredPosition = randomizedPosition;

        Debug.Log(rT.rect.size);
        Debug.Log(rT.sizeDelta);

        rT.gameObject.BroadcastMessage("OnWindowStart");
    }


    public void OnCursorEnter(int mode)
    {
        cursorRenderer.color = new Color
        (
            cursorRenderer.color.r,
            cursorRenderer.color.g,
            cursorRenderer.color.b,
            1f
        );
        if (mode == 3 || mode == 5) // Left or Right
        {
            cursor.transform.rotation = Quaternion.identity;
        }
        if (mode == 0 || mode == 8) // Top Left or Bottom Right
        {
            cursor.transform.rotation = Quaternion.Euler(0f, 0f, 135);
        }
        if (mode == 1 || mode == 7) // Top or Bottom
        {
            cursor.transform.rotation = Quaternion.Euler(0f, 0f, 90);
        }
        if (mode == 2 || mode == 6) // Top Right or Bottom Left
        {
            cursor.transform.rotation = Quaternion.Euler(0f, 0f, 45);
        }
    }
    public void OnCursorExit()
    {
        cursorRenderer.color = new Color
        (
            cursorRenderer.color.r,
            cursorRenderer.color.g,
            cursorRenderer.color.b,
            0f
        );
    }

    public void CompleteWindow()
    {
        currentScore += 1;
    }



    public Vector2 FindSize(RectTransform rectTransform)
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
        Vector2 transformSize = FindSize(rectTransform);
        RectTransform parentTransform = rectTransform.parent.GetComponent<RectTransform>();
        // Method will only work if direct parent has an unstretched size

        Vector2 basePivotPosition = new Vector2
            (
                rectTransform.anchoredPosition.x - (transformSize.x * rectTransform.pivot.x),
                rectTransform.anchoredPosition.y - (transformSize.y * rectTransform.pivot.y)
            );

        Vector2 baseAnchorPosition = new Vector2
        (
            basePivotPosition.x + (parentTransform.sizeDelta.x * ((rectTransform.anchorMax.x + rectTransform.anchorMin.x) / 2f)),
            basePivotPosition.y + (parentTransform.sizeDelta.y * ((rectTransform.anchorMax.y + rectTransform.anchorMin.y) / 2f))
        );

        return baseAnchorPosition;
    }
    public Vector2 ReanchorPosition(RectTransform rectTransform, Vector2 unanchoredPosition)
    {
        Vector2 transformSize = FindSize(rectTransform);
        RectTransform parentTransform = rectTransform.parent.GetComponent<RectTransform>();
        // Method will only work if direct parent has an unstretched size

        Vector2 reAnchorPosition = new Vector2
        (
            unanchoredPosition.x - (parentTransform.sizeDelta.x * ((rectTransform.anchorMax.x + rectTransform.anchorMin.x) / 2f)),
            unanchoredPosition.y - (parentTransform.sizeDelta.y * ((rectTransform.anchorMax.y + rectTransform.anchorMin.y) / 2f))
        );

        Vector2 rePivotPosition = new Vector2
        (
            reAnchorPosition.x + (transformSize.x * rectTransform.pivot.x),
            reAnchorPosition.y + (transformSize.y * rectTransform.pivot.y)
        );

        return rePivotPosition;
    }
    public Vector2 FullyUnanchorPosition(RectTransform rectTransform)
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
        Vector2[] listOfSizes = new Vector2[listOfParents.Length];

        Vector2 lastCalculatedSize = listOfParents[listOfParents.Length - 1].sizeDelta;

        for (int i = listOfSizes.Length - 1; i > 0; i--)
        {
            listOfSizes[i] = lastCalculatedSize;

            lastCalculatedSize = new Vector2
            (
                (lastCalculatedSize.x * Mathf.Abs(listOfParents[i - 1].anchorMax.x - listOfParents[i - 1].anchorMin.x)) + (listOfParents[i - 1].offsetMax.x - listOfParents[i - 1].offsetMin.x),
                (lastCalculatedSize.y * Mathf.Abs(listOfParents[i - 1].anchorMax.y - listOfParents[i - 1].anchorMin.y)) + (listOfParents[i - 1].offsetMax.y - listOfParents[i - 1].offsetMin.y)
            );
        }
        listOfSizes[0] = lastCalculatedSize;
        Vector2 relativePosition = new Vector2(0f, 0f);

        for (int i = 0; i < listOfParents.Length - 1; i++)
        {
            relativePosition = new Vector2
            (
                relativePosition.x + (listOfParents[i].anchoredPosition.x - (listOfSizes[i].x * listOfParents[i].pivot.x)) + (listOfSizes[i + 1].x * ((listOfParents[i].anchorMax.x + listOfParents[i].anchorMin.x) / 2f)),
                relativePosition.y + (listOfParents[i].anchoredPosition.y - (listOfSizes[i].y * listOfParents[i].pivot.y)) + (listOfSizes[i + 1].y * ((listOfParents[i].anchorMax.y + listOfParents[i].anchorMin.y) / 2f))
            );
        }

        return relativePosition;
    }
    public Vector2 FullyFindSize(RectTransform rectTransform)
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

        Vector2 lastCalculatedSize = listOfParents[listOfParents.Length - 1].sizeDelta;

        for (int i = listOfParents.Length - 1; i > 0; i--)
        {
            lastCalculatedSize = new Vector2
            (
                (lastCalculatedSize.x * Mathf.Abs(listOfParents[i - 1].anchorMax.x - listOfParents[i - 1].anchorMin.x)) + (listOfParents[i - 1].offsetMax.x - listOfParents[i - 1].offsetMin.x),
                (lastCalculatedSize.y * Mathf.Abs(listOfParents[i - 1].anchorMax.y - listOfParents[i - 1].anchorMin.y)) + (listOfParents[i - 1].offsetMax.y - listOfParents[i - 1].offsetMin.y)
            );
        }

        return lastCalculatedSize;
    }

    private float Remap(float inputValue, float inMin, float inMax, float outMin, float outMax)
    {
        return (inputValue - inMin) / (inMax - inMin) * (outMax - outMin) + outMin;
    }
}
