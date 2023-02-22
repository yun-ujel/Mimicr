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
    public Color Tone4; // Black / Darkest Colour
    public Color Bright; // White - should stay as such, or at least stay bright
    public Color Outline; // Lining of windows - should be pretty dark if not pitch black
    public Color WildCard; // Blue, etc - not the same tone or hue as the rest
    public int index; // Index of the colour palette for use of overrides + extras

    public Colour8(Color newTone0, Color newTone1, Color newTone2, Color newTone3, Color newTone4, Color newBright, Color newOutline, Color newWildCard, int dex)
    {
        Tone0 = newTone0;
        Tone1 = newTone1;
        Tone2 = newTone2;
        Tone3 = newTone3;
        Tone4 = newTone4;
        Bright = newBright;
        Outline = newOutline;
        WildCard = newWildCard;
        index = dex;
    }
}

public class CanvasHandler : MonoBehaviour
{
    [Header("References")]
    private GameObject cursor;
    private SpriteRenderer cursorRenderer;
    private AutoSpawning autoSpawning;
    private RectTransform canvasRectTransform;

    [Header("Colour Themes")]
    public Colour8[] palettes;
    [SerializeField] public int currentPalette;

    [Header("Other Visuals")]
    [SerializeField] private RawImage stage1Wallpaper;
    [SerializeField] private RawImage stage2Wallpaper;
    [SerializeField] private Image notificationBackground;

    [Header("Sequencing")]
    [SerializeField] private GameObject softwarePolicyWindow;
    [SerializeField] private GameObject mainMimicrWindow;
    [SerializeField] private NotificationHandler notifications;
    private bool[] storyTriggers = new bool[]
    {
        false, // Player Agrees to Software Policy
        false, // Opens Mimicr Window and sends in notifications
        false, // Adds an Incomplete Account to AutoSpawning
        false
    };
    private int messagesSent;

    void Awake()
    {
        if (cursor == null)
        {
            cursor = GameObject.FindGameObjectWithTag("IsCursorObject");
        }
        cursorRenderer = cursor.GetComponent<SpriteRenderer>();
        autoSpawning = GetComponent<AutoSpawning>();
        canvasRectTransform = GetComponent<RectTransform>();
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

        BroadcastMessage("OnColourUpdate", palettes[currentPalette], SendMessageOptions.DontRequireReceiver);
        BroadcastMessage("SetDrag", 6f);
    }
    private GameObject InstantiateWindow(GameObject window)
    {
        GameObject newWindow = Instantiate(window, transform);

        newWindow.name = window.name;

        RectTransform rT = newWindow.GetComponent<RectTransform>();
        rT.anchoredPosition = rT.ReanchorPosition
        (new Vector2(
            Random.Range(0f, canvasRectTransform.sizeDelta.x - rT.sizeDelta.x),
            Random.Range(0f, canvasRectTransform.sizeDelta.y - rT.sizeDelta.y)
        ));


        newWindow.BroadcastMessage("OnWindowStart");
        newWindow.BroadcastMessage("OnColourUpdate", palettes[currentPalette]);
        newWindow.BroadcastMessage("SetDrag", 6f);

        return newWindow;
    }
    public void OnCursorEnter(int mode) // Adds a cursor indicator, called when hovering over Resize bars
    {
        if (mode != 4) // If Anchor Index isn't set to None/No effect
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
        else
        {
            cursorRenderer.color = new Color
            (
                cursorRenderer.color.r,
                cursorRenderer.color.g,
                cursorRenderer.color.b,
                0f
            );
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

    // Sequencing
    void SpawnMainWindow()
    {
        if (!storyTriggers[0] && transform.Find(softwarePolicyWindow.name) == null)
        {
            InstantiateWindow(softwarePolicyWindow);
        }
    }

    private void Update()
    {
        if (!storyTriggers[0])
        {
            // Do nothing until player has accepted software policy
        }
        else if (!storyTriggers[1])
        {
            mainMimicrWindow.transform.SetAsLastSibling();
            mainMimicrWindow.SetActive(true);
            mainMimicrWindow.SendMessage("OnWindowStart");

            Invoke("SendAssistantNotification", 1f);
            Invoke("SendAssistantNotification", 3f);
            Invoke("SendAssistantNotification", 7f);
            Invoke("SendAssistantNotification", 10f);

            UpdateStoryTrigger();
        }
        else if (!storyTriggers[2] && messagesSent >= 4)
        {
            autoSpawning.incompleteAccounts.Add(autoSpawning.CreateNewAccount());
            UpdateStoryTrigger();
        }
    }

    void AcceptSoftwarePolicy()
    {
        storyTriggers[0] = true;
    }

    void SendAssistantNotification()
    {
        if (messagesSent < 1)
        {
            notifications.ReceiveAssistantNotification("Hi! I'm the MIMICR Assistant.", 3);
        }
        else if (messagesSent < 2)
        {
            notifications.ReceiveAssistantNotification("In order to reach a larger audience, you'll have multiple accounts for MIMICR.", 3);
        }
        else if (messagesSent < 3)
        {
            notifications.ReceiveAssistantNotification("I'll manage all of them, just refer to the Account Manager tab for any info.", 3);
        }
        else if (messagesSent < 4)
        {
            notifications.ReceiveAssistantNotification("Creating your first account now...", 3);
        }

        messagesSent += 1;
    }

    void UpdateStoryTrigger()
    {
        for (int i = 0; i < storyTriggers.Length; i++)
        {
            if (storyTriggers[i])
            {
                continue;
            }
            else
            {
                storyTriggers[i] = true;
                break;
            }
        }
    }
}
