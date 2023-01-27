using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


[System.Serializable]
public class Colour8
{
    public Color Tone0; // Lightest Colour
    public Color Tone1; // Second Lightest Colour, closer to tone 0
    public Color Tone2; // Darker Colour, closer to tone 3
    public Color Tone3; // Even Darker Colour, slight increase from tone 4
    public Color Tone4; // Even Darker Colour, slight decrease from tone 5
    public Color Tone5; // Black / Darkest Colour
    public Color Outline; // Lining of windows - should be pretty dark if not pitch black
    public Color WildCard; // Blue, etc - not the same tone or hue as the rest

    public Colour8(Color newTone0, Color newTone1, Color newTone2, Color newTone3, Color newTone4, Color newTone5, Color newOutline, Color newWildCard)
    {
        Tone0 = newTone0;
        Tone1 = newTone1;
        Tone2 = newTone2;
        Tone3 = newTone3;
        Tone4 = newTone4;
        Tone5 = newTone5;
        Outline = newOutline;
        WildCard = newWildCard;
    }
}

[System.Serializable]
public class ScoreThreshold
{
    public float requiredScore;
    public float timeBetweenSpawns;
}

public class CanvasHandler : MonoBehaviour
{
    public float currentScore;

    [Header("Windows")]
    [SerializeField] private GameObject[] windowsToOpen;
    [SerializeField] private GameObject poemWindow;
    [SerializeField] int maxWindowsOpen = 10;
    private List<GameObject> windowsCurrentlyOpen = new List<GameObject>();

    [SerializeField] private GameObject[] priorityWindows;

    [Header("References")]
    private GameObject cursor;
    private SpriteRenderer cursorRenderer;
    RectTransform canvasRectTransform;
    Canvas canvas;

    [Header("Colour Themes")]
    public Colour8[] palettes;
    [SerializeField] private int currentPalette;


    [Header("Rules")]
    float timeSinceLastSpawn;
    float timeToNextSpawn;
    [SerializeField] private ScoreThreshold[] scoreThresholds;
    private ScoreThreshold nextThreshold;

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

        BroadcastMessage("OnColourUpdate", palettes[0]);

        for (int i = 0; i < scoreThresholds.Length; i++)
        {
            if (currentScore < scoreThresholds[i].requiredScore)
            {
                nextThreshold = scoreThresholds[i];
                timeToNextSpawn = nextThreshold.timeBetweenSpawns;
            }
        }
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

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            OpenPriorityWindow(0);
        }

        HandleAutoSpawning();
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
        BroadcastMessage("OnColourUpdate", palettes[0]);
    }
    public void OpenPriorityWindow(int index)
    {
        priorityWindows[index].BroadcastMessage("OnWindowStart");
    }
    public void OnCursorEnter(int mode) // Adds a cursor indicator, called when hovering over Resize bars
    {
        cursorRenderer.color = new Color // Making the indicator visible
        (
            cursorRenderer.color.r,
            cursorRenderer.color.g,
            cursorRenderer.color.b,
            1f
        );
        if (mode == 3 || mode == 5)                                      // Left or Right (resizing width)
        {
            cursor.transform.rotation = Quaternion.identity;             // Indicator pointed upwards
        }
        if (mode == 0 || mode == 8)                                      // Top Left or Bottom Right (resizing width and height)
        {
            cursor.transform.rotation = Quaternion.Euler(0f, 0f, 135);   // Angled Indicator
        }
        if (mode == 1 || mode == 7)                                      // Top or Bottom (resizing height)
        {
            cursor.transform.rotation = Quaternion.Euler(0f, 0f, 90);    // Indicator pointed upwards
        }
        if (mode == 2 || mode == 6)                                      // Top Right or Bottom Left (resizing width and height)
        {
            cursor.transform.rotation = Quaternion.Euler(0f, 0f, 45);    // Angled Indicator
        }
    }
    public void OnCursorExit() // Removes the cursor indicator
    {
        cursorRenderer.color = new Color
        (
            cursorRenderer.color.r,
            cursorRenderer.color.g,
            cursorRenderer.color.b,
            0f
        );
    } 
    public void CompleteWindow() // Called when a window is completed successfully
    {
        currentScore += 1;
    } 
    public Vector2 UnanchorPosition(RectTransform rectTransform)
    {
        RectTransform parentTransform = rectTransform.parent.GetComponent<RectTransform>();
        Vector2 transformSize = rectTransform.rect.size;
        Vector2 parentTransformSize = parentTransform.rect.size;

        Vector2 basePivotPosition = new Vector2
            (
                rectTransform.anchoredPosition.x - (transformSize.x * rectTransform.pivot.x),
                rectTransform.anchoredPosition.y - (transformSize.y * rectTransform.pivot.y)
            );

        Vector2 baseAnchorPosition = new Vector2
        (
            basePivotPosition.x + (parentTransformSize.x * ((rectTransform.anchorMax.x + rectTransform.anchorMin.x) / 2f)),
            basePivotPosition.y + (parentTransformSize.y * ((rectTransform.anchorMax.y + rectTransform.anchorMin.y) / 2f))
        );

        return baseAnchorPosition;
    }
    public Vector2 ReanchorPosition(RectTransform rectTransform, Vector2 unanchoredPosition)
    {
        Vector2 transformSize = rectTransform.rect.size;
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
    private float Remap(float inputValue, float inMin, float inMax, float outMin, float outMax)
    {
        return (inputValue - inMin) / (inMax - inMin) * (outMax - outMin) + outMin;
    }



    private void HandleAutoSpawning()
    {
        // Spawn windows once timer has run out
        if (timeSinceLastSpawn > timeToNextSpawn)
        {
            InstantiateWindow();
            timeSinceLastSpawn = 0f;
        }
        else
        {
            timeSinceLastSpawn += Time.deltaTime;
        }

        // Scale timeToNextSpawn with Score

        if (currentScore > nextThreshold.requiredScore &&
            nextThreshold != scoreThresholds[scoreThresholds.Length]) // If you've passed a score threshold and it wasn't the last:
        {
            for (int i = 0; i < scoreThresholds.Length; i++)
            {
                if (currentScore < scoreThresholds[i].requiredScore)
                {
                    nextThreshold = scoreThresholds[i];
                    timeToNextSpawn = nextThreshold.timeBetweenSpawns;
                }
            }
        }
        else if (nextThreshold != scoreThresholds[scoreThresholds.Length]) // if it is the last threshold:
        {
            Debug.Log("Last Threshold Reached");
        }
    }
}
