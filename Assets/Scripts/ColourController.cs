using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ColourController : MonoBehaviour
{
    private ColourType colourType;
    private ChosenGraphic chosenGraphic;

    private Outline outline;

    private Image image;
    private RawImage rawImage;
    private TextMeshProUGUI text;

    private void Awake()
    {
        TryGetComponent(out outline);
        
        if (TryGetComponent(out image))
        { chosenGraphic = ChosenGraphic.image; }

        else if (TryGetComponent(out rawImage))
        { chosenGraphic = ChosenGraphic.rawImage; }

        else if (TryGetComponent(out text))
        { chosenGraphic = ChosenGraphic.text; }
    }
    private enum ColourType
    {
        light,
        dark,
        text
    }
    private enum ChosenGraphic
    {
        image,
        rawImage,
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

        }
        else if (colourType == ColourType.dark)
        {

        }
        else if (colourType == ColourType.text)
        {

        }
    }
}
