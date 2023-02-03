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
    public int index; // Index of the colour palette for use of overrides + extras

    public Colour8(Color newTone0, Color newTone1, Color newTone2, Color newTone3, Color newTone4, Color newTone5, Color newOutline, Color newWildCard, int dex)
    {
        Tone0 = newTone0;
        Tone1 = newTone1;
        Tone2 = newTone2;
        Tone3 = newTone3;
        Tone4 = newTone4;
        Tone5 = newTone5;
        Outline = newOutline;
        WildCard = newWildCard;
        index = dex;
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
    [HideInInspector] public float currentScore;

    [Header("Windows")]
    [SerializeField] private GameObject[] windowsToOpen;
    [SerializeField] private GameObject poemWindow;
    [SerializeField] int maxWindowsOpen = 10;
    [SerializeField] private List<GameObject> windowsCurrentlyOpen = new List<GameObject>();

    [SerializeField] private GameObject[] priorityWindows;

    [Header("References")]
    private GameObject cursor;
    private SpriteRenderer cursorRenderer;
    RectTransform canvasRectTransform;

    [Header("Colour Themes")]
    public Colour8[] palettes;
    [SerializeField] public int currentPalette;


    [Header("Rules")]
    [SerializeField] private ScoreThreshold[] scoreThresholds;
    float timeSinceLastSpawn;
    float timeToNextSpawn;
    private int thresholdIndex;

    void Awake()
    {
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

        BroadcastMessage("OnColourUpdate", palettes[currentPalette]);
        CheckForNextThreshold();

        //Debug.Log("End Start() method");
    }
    void Update()
    {
        LogPositions();

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
        Vector2 randomizedPosition = rT.ReanchorPosition(new Vector2
        (
            Random.Range(0f, canvasRectTransform.sizeDelta.x - rT.sizeDelta.x),
            Random.Range(0f, canvasRectTransform.sizeDelta.y - rT.sizeDelta.y)
        ));

        rT.anchoredPosition = randomizedPosition;

        rT.gameObject.BroadcastMessage("OnWindowStart");
        BroadcastMessage("OnColourUpdate", palettes[currentPalette]);
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
    public void CompleteWindow(GameObject window) // Called when a window is completed successfully
    {
        windowsCurrentlyOpen.Remove(window);
        currentScore += 1;
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

        if (currentScore > scoreThresholds[thresholdIndex].requiredScore) // If you've passed a score threshold and it wasn't the last:
        {
            CheckForNextThreshold();
        }
    }

    void CheckForNextThreshold()
    {
        for (int i = 0; i < scoreThresholds.Length - 1; i++)
        {
            if (currentScore < scoreThresholds[i].requiredScore)
            {
                thresholdIndex = i;
                timeToNextSpawn = scoreThresholds[thresholdIndex].timeBetweenSpawns;
                break;
            }
        }
    }



    
    private float Remap(float inputValue, float inMin, float inMax, float outMin, float outMax)
    {
        return (inputValue - inMin) / (inMax - inMin) * (outMax - outMin) + outMin;
    }

    [SerializeField] private RectTransform[] windowsToLog;
    void LogPositions()
    {
        foreach (RectTransform window in windowsToLog)
        {
            Debug.Log(window.name + " position: " + window.rect.position);
        }
    }
}
