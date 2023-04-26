using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class GameSlider
{
    public Slider slider;
    public float CorrectValue { get; set; }
    public bool IsComplete { get; set; }

    public HSVMode ColorMode;
    public enum HSVMode
    {
        hue,
        saturation,
        value
    }
}



public class SliderMinigame : MonoBehaviour
{
    [SerializeField] private int completedSliders = 0;
    private int requiredSliders = 1;

    [Header("References")]
    [SerializeField] private RawImage imageReference;
    [SerializeField] private CanvasGroup completionGroup; // This is the post button that appears after the filters are correctly adjusted
    [SerializeField] private GameSlider[] sliders = new GameSlider[3];
    private CanvasHandler cHandler;

    [Header("Rules")]
    [SerializeField] private float marginOfError = 0.08f;

    private float hValue;
    private float sValue;
    private float vValue;

    private void Awake()
    {
        cHandler = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasHandler>();
    }

    void Update()
    {
        for (int i = 0; i < sliders.Length; i++) // check each GameSlider in gSliders
        {
            if (completedSliders == requiredSliders)
            {
                break;
            }
            else if (sliders[i].slider.value - marginOfError < sliders[i].CorrectValue && // if Slider is in range of correct value
                sliders[i].slider.value + marginOfError > sliders[i].CorrectValue)
            {
                ChangeSliderColour(sliders[i].slider, ColourType.wildCard, ColourType.bright);
                sliders[i].IsComplete = true;

                completedSliders += 1;
            }
            else
            {
                sliders[i].IsComplete = false;
                ChangeSliderColour(sliders[i].slider, ColourType.bright, ColourType.outline);
            }

            if (sliders[i].ColorMode == GameSlider.HSVMode.hue)
            {
                hValue = sliders[i].slider.value;
            }
            else if (sliders[i].ColorMode == GameSlider.HSVMode.saturation)
            {
                sValue = sliders[i].slider.value;
            }
            else if (sliders[i].ColorMode == GameSlider.HSVMode.value)
            {
                vValue = sliders[i].slider.value;
            }
        }

        if (completedSliders == requiredSliders)
        {
            GameComplete();
        }
        else
        {
            imageReference.color = Color.HSVToRGB(hValue, sValue, vValue);
        }
        completedSliders = 0;
    }

    void OnStackStart(AccountInfo accountInfo)
    {
        if (accountInfo.CurrentPostIndex < accountInfo.Posts.Length)
        {
            imageReference.texture = accountInfo.Posts[accountInfo.CurrentPostIndex];
        }
        else
        {
            accountInfo.CurrentPostIndex = 0;
            imageReference.texture = accountInfo.Posts[accountInfo.CurrentPostIndex];
        }
        

        for (int i = 0; i < sliders.Length; i++) // Randomize slider values
        {
            if (sliders[i].ColorMode == GameSlider.HSVMode.hue)
            {
                sliders[i].CorrectValue = Random.Range(sliders[i].slider.minValue, sliders[i].slider.maxValue - (marginOfError * 2f)); // Correct Hue value can be anything
                sliders[i].slider.value = RandomNewValue(sliders[i].CorrectValue, sliders[i].slider.minValue, sliders[i].slider.maxValue);
            }
            else if (sliders[i].ColorMode == GameSlider.HSVMode.saturation)
            {
                sliders[i].CorrectValue = Random.Range(sliders[i].slider.minValue, sliders[i].slider.maxValue * 0.8f); // Correct Sat value can be anything below 0.8
                sliders[i].slider.value = RandomNewValue(sliders[i].CorrectValue, sliders[i].slider.minValue, sliders[i].slider.maxValue);
            }
            else // if ColorMode == HSVMode.value
            {
                sliders[i].CorrectValue = Random.Range(sliders[i].slider.maxValue * 0.75f, sliders[i].slider.maxValue); // Correct Value value can be anything above 0.75
                sliders[i].slider.value = RandomNewValue(sliders[i].CorrectValue, sliders[i].slider.maxValue * 0.4f, sliders[i].slider.maxValue);
                // Starting value can be anything above 0.4
            }

        }

        completedSliders = 0;
        requiredSliders = sliders.Length;

        completionGroup.alpha = 0f;
        completionGroup.interactable = false;
        completionGroup.blocksRaycasts = false;
    }

    void GameComplete()
    {
        for (int i = 0; i < sliders.Length; i++)
        {
            ChangeSliderColour(sliders[i].slider, ColourType.bright, ColourType.bright);
            sliders[i].slider.interactable = false;
        }

        if (completionGroup.alpha < 1f)
        {
            completionGroup.alpha += Time.deltaTime * 4f;
        }

        imageReference.color = new Color
        (
            Mathf.MoveTowards(imageReference.color.r, 1f, 0.005f),
            Mathf.MoveTowards(imageReference.color.g, 1f, 0.005f),
            Mathf.MoveTowards(imageReference.color.b, 1f, 0.005f)
        );

        completionGroup.interactable = true;
        completionGroup.blocksRaycasts = true;
    }
    // The completion group should include a button that closes the window,
    // A.K.A. triggering method "OnWindowComplete" on StackHandler


    float RandomNewValue(float correctValue, float minValue, float maxValue)
    { // Returns a random value that is significantly different from the correctValue
        float distanceFromMax = maxValue - correctValue;
        float distanceFromMin = correctValue - minValue;

        float coinFlip = Random.Range(minValue, maxValue);
        
        if (coinFlip >= correctValue)
        {
            return correctValue + Random.Range(marginOfError * 2f, distanceFromMax);
        }
        else
        {
            return minValue + Random.Range(0f, distanceFromMin - (marginOfError * 2f));
        }
    }

    void ChangeSliderColour(Slider slider, ColourType baseColour, ColourType outlineColour)
    {
        if (slider.targetGraphic.gameObject.TryGetComponent(out ColourController colourController))
        {
            colourController.colourType = baseColour;
            colourController.outlineType = outlineColour;
            colourController.SendMessage("OnColourUpdate", cHandler.palettes[cHandler.currentPalette]);
        }
    }
}
