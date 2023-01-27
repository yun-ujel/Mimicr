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
    private int completedSliders = 0;
    private int requiredSliders = 1;

    [Header("References")]
    [SerializeField] private RawImage imageReference;
    [SerializeField] private CanvasGroup completionGroup; // This is the post button that appears after the filters are correctly adjusted
    [SerializeField] private Texture[] potentialTextures;
    [SerializeField] private GameSlider[] gSliders = new GameSlider[3];

    [Header("Rules")]
    [SerializeField] private float marginOfError = 0.02f;

    private float hValue;
    private float sValue;
    private float vValue;

    void Update()
    {
        for (int i = 0; i < gSliders.Length; i++) // check each GameSlider in gSliders
        {
            if (!gSliders[i].IsComplete)                                           // if Slider hasn't already been completed
            {
                if (gSliders[i].slider.value - marginOfError < gSliders[i].CorrectValue && // if Slider is in range of correct value
                gSliders[i].slider.value + marginOfError > gSliders[i].CorrectValue)
                {
                    gSliders[i].slider.interactable = false; // disable the slider
                    gSliders[i].IsComplete = true;

                    completedSliders += 1;
                }
            }
            else if (completedSliders == requiredSliders)
            {
                break;
            }

            if (gSliders[i].ColorMode == GameSlider.HSVMode.hue)
            {
                hValue = gSliders[i].slider.value;
            }
            else if (gSliders[i].ColorMode == GameSlider.HSVMode.saturation)
            {
                sValue = gSliders[i].slider.value;
            }
            else if (gSliders[i].ColorMode == GameSlider.HSVMode.value)
            {
                vValue = gSliders[i].slider.value;
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
        
    }

    void OnWindowStart()
    {
        int imageSelection = Random.Range(0, potentialTextures.Length); // Randomize displayed image
        imageReference.texture = potentialTextures[imageSelection];

        for (int i = 0; i < gSliders.Length; i++) // Randomize slider values
        {
            if (gSliders[i].ColorMode == GameSlider.HSVMode.hue)
            {
                gSliders[i].slider.value = Random.Range(gSliders[i].slider.minValue, gSliders[i].slider.maxValue);
                gSliders[i].CorrectValue = Random.Range(gSliders[i].slider.minValue, gSliders[i].slider.maxValue); // Hue value can be anything
            }
            else if (gSliders[i].ColorMode == GameSlider.HSVMode.value)
            {
                gSliders[i].slider.value = Random.Range(gSliders[i].slider.minValue, gSliders[i].slider.maxValue);
                gSliders[i].CorrectValue = Random.Range(gSliders[i].slider.maxValue * 0.75f, gSliders[i].slider.maxValue); // Value value can be anything above 0.75
            }
            else
            {
                gSliders[i].slider.value = Random.Range(gSliders[i].slider.minValue, gSliders[i].slider.maxValue);
                gSliders[i].CorrectValue = Random.Range(gSliders[i].slider.minValue, gSliders[i].slider.maxValue * 0.8f); // Sat value can be anything below 0.8
            }
        }

        completedSliders = 0;
        requiredSliders = gSliders.Length;

        completionGroup.alpha = 0f;
        completionGroup.interactable = false;
    }

    void GameComplete()
    {
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
    }
    // The completion group should include a button that closes the window
}
