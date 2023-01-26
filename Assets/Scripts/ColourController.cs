using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ColourController : MonoBehaviour
{
    private ColourType colourType;

    private Outline outline;
    private Image image;
    private RawImage rawImage;
    private TextMeshProUGUI text;

    private void Awake()
    {
        TryGetComponent(out outline);
        TryGetComponent(out image);
        TryGetComponent(out rawImage);
        TryGetComponent(out text);
    }
    private enum ColourType
    {
        light,
        dark,
        text
    }

    void OnColourUpdate(Colour4 colour4)
    {
        if (outline != null)
        {
            outline.effectColor = colour4.outlineColour;
        }

        if (colourType == ColourType.light)
        {
            ChangeGraphic(colour4.lightColour);
        }
        else if (colourType == ColourType.dark)
        {
            ChangeGraphic(colour4.darkColour);
        }
        else if (colourType == ColourType.text)
        {
            ChangeGraphic(colour4.textColour);
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
}
