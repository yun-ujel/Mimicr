using UnityEngine.UI;
using UnityEngine;
using TMPro;
public enum ColourType
{
    tone0,
    tone1,
    tone2,
    tone3,
    tone4,
    bright,
    outline,
    wildCard,
    none
}

public class ColourController : MonoBehaviour
{
    public ColourType colourType;
    public ColourType outlineType;

    [HideInInspector] public Outline outline;
    [HideInInspector] public Image image;
    [HideInInspector] public RawImage rawImage;
    [HideInInspector] public TextMeshProUGUI text;

    private Colour8 lastPaletteUsed;

    private void Awake()
    {
        // Assign UI Graphic Reference
        if (outline == null)
        {
            TryGetComponent(out outline);
        }
        if (image == null && rawImage == null && text == null)
        {
            TryGetComponent(out image);
            TryGetComponent(out rawImage);
            TryGetComponent(out text);
        }
    }

    public void OnColourUpdate(Colour8 colour8)
    {
        if (colour8 != null)
        {
            lastPaletteUsed = colour8;
        }

        if (colourType != ColourType.none)
        {
            ChangeColour(lastPaletteUsed);
        }
        if (outline != null && outlineType != ColourType.none)
        {
            ChangeOutline(lastPaletteUsed);
        }
        
    }

    void ChangeGraphic(Color colour)
    {
        if (image != null)
        {
            image.color = colour;
        }
        else if (rawImage != null)
        {
            rawImage.color = colour;
        }
        else if (text != null)
        {
            text.color = colour;
        }
    }

    void ChangeColour(Colour8 colour8)
    { 
        if (colourType == ColourType.tone0)
        {
            ChangeGraphic(colour8.Tone0);
        }
        else if (colourType == ColourType.tone1)
        {
            ChangeGraphic(colour8.Tone1);
        }
        else if (colourType == ColourType.tone2)
        {
            ChangeGraphic(colour8.Tone2);
        }
        else if (colourType == ColourType.tone3)
        {
            ChangeGraphic(colour8.Tone3);
        }
        else if (colourType == ColourType.tone4)
        {
            ChangeGraphic(colour8.Tone4);
        }
        else if (colourType == ColourType.bright)
        {
            ChangeGraphic(colour8.Bright);
        }
        else if (colourType == ColourType.outline)
        {
            ChangeGraphic(colour8.Outline);
        }
        else if (colourType == ColourType.wildCard)
        {
            ChangeGraphic(colour8.WildCard);
        }
    }

    void ChangeOutline(Colour8 colour8)
    {
        if (outlineType == ColourType.tone0)
        {
            outline.effectColor = colour8.Tone0;
        }
        else if (outlineType == ColourType.tone1)
        {
            outline.effectColor = colour8.Tone1;
        }
        else if (outlineType == ColourType.tone2)
        {
            outline.effectColor = colour8.Tone2;
        }
        else if (outlineType == ColourType.tone3)
        {
            outline.effectColor = colour8.Tone3;
        }
        else if (outlineType == ColourType.tone4)
        {
            outline.effectColor = colour8.Tone4;
        }
        else if (outlineType == ColourType.bright)
        {
            outline.effectColor = colour8.Bright;
        }
        else if (outlineType == ColourType.outline)
        {
            outline.effectColor = colour8.Outline;
        }
        else if (outlineType == ColourType.wildCard)
        {
            outline.effectColor = colour8.WildCard;
        }
    }
}
