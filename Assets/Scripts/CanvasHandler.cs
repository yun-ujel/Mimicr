using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


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
    [SerializeField] private GameObject monetPolicyWindow;
    [SerializeField] private GameObject mainMimicrWindow;
    [SerializeField] private NotificationHandler notifications;
    [SerializeField] private List<GameObject> scaryAds = new List<GameObject>();
    private bool[] storyTriggers = new bool[]
    {
        false, // 0 - Player Agrees to Software Policy
        false, // 1 - Opens Mimicr Window and sends in notifications
        false, // 2 - Player has received briefing from Assistant, adds an Incomplete Account to AutoSpawning
        false, // 3 - Player has entered PIN for first account, Assistant mentions Sliders window
        false, // 4 - Player has completed first account
        false, // 5 - Assistant criticises player's speed and tells them to go again
        false, // 6 - Player has received briefing from Assistant, adds two Incomplete Accounts to AutoSpawning
        false, // 7 - Player has completed or failed AutoSpawning Windows
        false, // 8 - Assistant comments on player performance for 7
        false, // 9 - 5 accounts are spawned over the course of 2 minutes
        false, // 10 - All 8 accounts are completed
        false, // 11 - Run an extra round until the player fails (unless 3 or more were failed on storyTrigger 10)
        false, // 12 - Better luck next time!
        false, // 13 - Initialise Last Stand
        false, // 14 - Last Stand
        false, // 15 - Fade to black
        false // 16 - Scene Transition
    };
    private int messagesSent;
    private float sequencerCounter;
    private bool agreedMonetPolicy;
    private bool oneMoreRound;

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
        else
        {
            mainMimicrWindow.transform.SetAsLastSibling();
        }
    }
    void SpawnAdAgreement()
    {
        GameObject adWindow = InstantiateWindow(monetPolicyWindow);
        adWindow.SendMessage("StartFailTimer", 60f);
    }

    private void Update()
    {
        if (!storyTriggers[0])
        {
            // Do nothing until player has accepted software policy
        }
        else if (!storyTriggers[1])
        {   // Briefing from Assistant
            mainMimicrWindow.transform.SetAsLastSibling();
            mainMimicrWindow.SetActive(true);
            mainMimicrWindow.SendMessage("OnWindowStart");
            mainMimicrWindow.BroadcastMessage("OnColourUpdate", palettes[currentPalette], SendMessageOptions.DontRequireReceiver);

            notifications.maxNotifs = 0;
            notifications.notifFadeOutTime = 12f;

            Invoke(nameof(SendAssistantNotification), 1f); // 1
            Invoke(nameof(SendAssistantNotification), 4.5f); // 2
            Invoke(nameof(SendAssistantNotification), 8f); // 3
            Invoke(nameof(SendAssistantNotification), 12f); // 4

            UpdateStoryTrigger();
        }
        else if (!storyTriggers[2])
        {   // Add new Account
            if (messagesSent >= 4)
            {
                notifications.notifFadeOutTime = 8f;

                autoSpawning.Invoke("AddNewAccount", 2f);
                UpdateStoryTrigger();
            }
        }
        else if (!storyTriggers[3])
        {   // Sliders tips
            if (autoSpawning.allAccounts.Count > 0)
            {
                if (autoSpawning.allAccounts[0].CurrentWindowIndex > 0)
                {
                    notifications.maxNotifs = 1;

                    SendAssistantNotification(); // 5
                    Invoke(nameof(SendAssistantNotification), 12f); // 6
                    Invoke(nameof(SpawnAdAgreement), 20f);

                    UpdateStoryTrigger();
                }
            }
        }
        else if (!storyTriggers[4])
        {   // Wait Until First Account is Completed
            if (autoSpawning.incompleteAccounts[0].isFinished)
            {
                UpdateStoryTrigger();
            }
        }
        else if (!storyTriggers[5])
        {   // "Great Job!" from assistant, but go faster
            notifications.maxNotifs = 0;
            notifications.notifFadeOutTime = 8f;

            SendAssistantNotification(); // 7
            Invoke(nameof(SendAssistantNotification), 1f); // 8
            Invoke(nameof(SendAssistantNotification), 5f); // 9
            Invoke(nameof(SendAssistantNotification), 9.5f); // 10

            UpdateStoryTrigger();
        }
        else if (!storyTriggers[6])
        {   // Add 2 new accounts on 20 second timer
            if (messagesSent >= 10)
            {
                autoSpawning.stackWindowTimer = 24f;
                notifications.maxNotifs = 1;
                notifications.notifFadeOutTime = 12f;

                autoSpawning.Invoke("AddNewAccount", 2f);
                autoSpawning.Invoke("AddNewAccount", 4f);

                Invoke(nameof(SendAssistantNotification), 7f); // 11

                UpdateStoryTrigger();
            }
        }
        else if (!storyTriggers[7])
        {   // Wait until windows are complete
            if (autoSpawning.allAccounts.Count == 3 && autoSpawning.incompleteAccounts.Count < 1)
            {
                UpdateStoryTrigger();
            }
        }
        else if (!storyTriggers[8])
        {   // Assistant comments on player performance

            notifications.maxNotifs = 0;
            notifications.notifFadeOutTime = 12f;

            Invoke(nameof(SendAssistantNotification), 1f); // 12
            Invoke(nameof(SendAssistantNotification), 3f); // 13
            Invoke(nameof(SendAssistantNotification), 6f); // 14
            Invoke(nameof(SendAssistantNotification), 9f); // 15

            UpdateStoryTrigger();
        }
        else if (!storyTriggers[9])
        {   // Wait until messages are received, then spawn windows automatically
            if (messagesSent >= 15)
            {
                autoSpawning.stackWindowTimer = 28f;

                autoSpawning.completedAccounts = 0;
                autoSpawning.failedAccounts = 0;

                autoSpawning.Invoke("AddNewAccount", 4f);
                autoSpawning.Invoke("AddNewAccount", 8f);
                autoSpawning.Invoke("AddNewAccount", 44f);
                autoSpawning.Invoke("AddNewAccount", 82f);
                autoSpawning.Invoke("AddNewAccount", 120f);

                UpdateStoryTrigger();
            }
        }
        else if (!storyTriggers[10])
        {   // Wait until all accounts are finished
            if (autoSpawning.allAccounts.Count == 8 && autoSpawning.incompleteAccounts.Count < 1)
            {
                if (autoSpawning.failedAccounts <= 3)
                {
                    oneMoreRound = true;
                }
                autoSpawning.failedAccounts = 0;
                autoSpawning.completedAccounts = 0;
                sequencerCounter = 4f;
                autoSpawning.stackWindowTimer = 20f;

                Invoke(nameof(SendAssistantNotification), 1f); // 16
                UpdateStoryTrigger();
            }
        }
        else if (!storyTriggers[11])
        {   // Run an extra round until the player fails
            if (oneMoreRound && autoSpawning.failedAccounts < 1)
            {
                if (sequencerCounter < 0f)
                {
                    autoSpawning.AddNewAccount();
                    sequencerCounter = 28f;
                }
                else
                {
                    sequencerCounter -= Time.deltaTime;
                }
            }
            else if (oneMoreRound && autoSpawning.incompleteAccounts.Count < 1)
            {
                Invoke(nameof(SendAssistantNotification), 1f); // 17
            }
            else
            {
                UpdateStoryTrigger();
            }
        }
        else if (!storyTriggers[12])
        {   // Better luck next time!
            currentPalette = 1;

            BroadcastMessage("OnColourUpdate", palettes[currentPalette]);

            notificationBackground.color = new Color(0f, 0f, 0f, notificationBackground.color.a);
            stage1Wallpaper.color = new Color(0f, 0f, 0f, 0f);
            stage2Wallpaper.color = Color.white;

            autoSpawning.popUpWindows = scaryAds;

            BroadcastMessage("TriggerShake", new Vector2(0.25f, 10f), SendMessageOptions.DontRequireReceiver);
            Invoke(nameof(SendAssistantNotification), 2f); // 18
            Invoke(nameof(SendAssistantNotification), 4f); // 19
            Invoke(nameof(SendAssistantNotification), 6f); // 20
            Invoke(nameof(SendAssistantNotification), 8f); // 20
            UpdateStoryTrigger();
        }
        else if (!storyTriggers[13])
        {   // Initialise last stand
            if (messagesSent == 20)
            {
                autoSpawning.failedAccounts = 0;
                autoSpawning.completedAccounts = 0;
                autoSpawning.timeToNextPopUp = 5f;
                sequencerCounter = 4f;
                UpdateStoryTrigger();
            }
        }
        else if (!storyTriggers[14])
        {   // Last Stand
            if (autoSpawning.failedAccounts < 3)
            {
                if (sequencerCounter < 0f)
                {
                    autoSpawning.AddNewAccount();
                    sequencerCounter = 16f;
                }
                else
                {
                    sequencerCounter -= Time.deltaTime;
                }
            }
            else
            {
                sequencerCounter = 0f;
                stage1Wallpaper.transform.SetAsLastSibling();
                UpdateStoryTrigger();
            }
        }
        else if (!storyTriggers[15])
        {
            if (stage1Wallpaper.color.a < 1f)
            {
                stage1Wallpaper.color = new Color(0f, 0f, 0f, Mathf.Lerp(0f, 1f, sequencerCounter));
                sequencerCounter += Time.deltaTime;
            }
            else
            {
                UpdateStoryTrigger();
            }
        }
        else if (!storyTriggers[16])
        {
            SceneManager.LoadScene(2);
        }
    }

    void AcceptSoftwarePolicy()
    {
        storyTriggers[0] = true;
    }

    void AcceptMonetPolicy()
    {
        if (!agreedMonetPolicy)
        {
            autoSpawning.timeToNextPopUp = 16f;
            agreedMonetPolicy = true;
        }
    }

    void SendAssistantNotification()
    {
        if (messagesSent < 1)
        {
            notifications.ReceiveAssistantNotification
                ("Hi! I'm the Assistant. I manage your experience on MIMICR.", 3);
        }
        else if (messagesSent < 2)
        {
            notifications.ReceiveAssistantNotification
                ("In order to reach a larger audience, you'll have multiple accounts for MIMICR.", 3);
        }
        else if (messagesSent < 3)
        {
            notifications.ReceiveAssistantNotification
                ("I'll manage all of them, just refer to the Account Manager tab for any info.", 3);
        }
        else if (messagesSent < 4)
        {
            notifications.ReceiveAssistantNotification
                ("Creating your first account now...", 3);
        }
        else if (messagesSent < 5)
        {
            notifications.ReceiveAssistantNotification
                ("When Editing a Photo, just drag each slider until you see the handle change colour.", 3);
        }
        else if (messagesSent < 6)
        {
            notifications.ReceiveAssistantNotification
                ("Try to keep the Value high and the Saturation a little low.", 3);
        }
        else if (messagesSent < 7)
        {
            notifications.ReceiveAssistantNotification
                ("Great Job!", 3);
        }
        else if (messagesSent < 8)
        {
            notifications.ReceiveAssistantNotification
                ("Could be a little bit faster though...", 1);
        }
        else if (messagesSent < 9)
        {
            notifications.ReceiveAssistantNotification
                ("How about we do 2 accounts, and I'll close the windows if you go a little too slow.", 3);
        }
        else if (messagesSent < 10)
        {
            notifications.ReceiveAssistantNotification
                ("Just creating them now...", 1);
        }
        else if (messagesSent < 11)
        {
            notifications.ReceiveAssistantNotification
                ("Remember, you can always check the top of a window to see which account it is.", 3);
        }
        else if (messagesSent < 12)
        {
            if (autoSpawning.failedAccounts == 0)
            {
                notifications.ReceiveAssistantNotification
                    ("Wow, you're doing great! I'm impressed!", 3);
            }
            else if (autoSpawning.failedAccounts == 1)
            {
                notifications.ReceiveAssistantNotification
                    ("Well, you tried your best...", 1);
            }
            else if (autoSpawning.failedAccounts == 2)
            {
                notifications.ReceiveAssistantNotification
                    ("Seriously? Our top creators can run 6 accounts at once, and 2 is your limit?!", 0);
            }
        }
        else if (messagesSent < 13)
        {
            if (autoSpawning.failedAccounts == 0)
            {
                notifications.ReceiveAssistantNotification
                    ("Let's keep going.", 3);
            }
            else if (autoSpawning.failedAccounts > 0)
            {
                notifications.ReceiveAssistantNotification
                    ("Oh well...", 1);
            }
        }
        else if (messagesSent < 14)
        {
            notifications.ReceiveAssistantNotification
                ("I'll leave you on your own for a while next.", 3);
        }
        else if (messagesSent < 15)
        {
            notifications.ReceiveAssistantNotification
                ("I'll spawn in windows every once and a while, so try to keep up!", 3);
        }
        else if (messagesSent < 16)
        {
            if (autoSpawning.failedAccounts > 3)
            {
                notifications.ReceiveAssistantNotification
                    ("You're doing terrible. Improve on the next one.", 0);
            }
            else
            {
                notifications.ReceiveAssistantNotification
                    ("Wow, you're doing great! I'm impressed! Let's keep going!", 3);
            }
        }
        else if (messagesSent < 17)
        {
            notifications.ReceiveAssistantNotification
                ("I guess you failed...", 0);
        }
        else if (messagesSent < 18)
        {
            notifications.ReceiveAssistantNotification
                ("Better luck next time!", 2);
        }
        else if (messagesSent < 19)
        {
            notifications.ReceiveAssistantNotification
                ("All of your followers are leaving you!", 2);
        }
        else if (messagesSent < 20)
        {
            notifications.ReceiveAssistantNotification
                ("What a shame!", 2);
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
