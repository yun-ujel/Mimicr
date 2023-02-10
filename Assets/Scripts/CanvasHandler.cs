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
    [HideInInspector] public float currentScore;

    [Header("References")]
    [SerializeField] private RawImage stage1Wallpaper;
    [SerializeField] private RawImage stage2Wallpaper;
    [SerializeField] private Image notificationBackground;

    private GameObject cursor;
    private SpriteRenderer cursorRenderer;

    [Header("Colour Themes")]
    public Colour8[] palettes;
    [SerializeField] public int currentPalette;

    

    void Awake()
    {
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

        //Debug.Log("End Start() method");
    }
    void Update()
    {
        LogPositions();
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
    
    


    [Header("Debug")]
    [SerializeField] private RectTransform[] windowsToLog;
    void LogPositions()
    {
        foreach (RectTransform window in windowsToLog)
        {
            Debug.Log(window.name + " position: " + window.rect.position);
        }
    }

}
