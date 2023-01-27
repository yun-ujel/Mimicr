using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class GameSlider
{
    public Slider CSlider;
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
    [SerializeField] private List<Slider> sliders = new List<Slider>();
    List<float> correctSliderValues = new List<float>();

    [SerializeField] private RawImage imageReference;

    [SerializeField] private Texture[] potentialTextures;

    [SerializeField] private GameSlider[] gSliders;
    void Update()
    {
        for (int i = 0; i < sliders.Count; i++)
        {
            if (sliders[i].value - 0.05f < correctSliderValues[i] &&
                sliders[i].value + 0.05f > correctSliderValues[i])
            {
                sliders[i].interactable = false;

                sliders.Remove(sliders[i]);
                correctSliderValues.Remove(correctSliderValues[i]);
                completedSliders += 1;
            }
        }
        if (completedSliders == requiredSliders)
        {
            BroadcastMessage("OnWindowComplete");
            completedSliders = 0;
        }
    }

    void OnWindowStart()
    {
        int imageSelection = Random.Range(0, potentialTextures.Length);

        imageReference.texture = potentialTextures[imageSelection];

        for (int i = 0; i < sliders.Count; i++)
        {
            sliders[i].value = Random.Range(sliders[i].minValue, sliders[i].maxValue);

            correctSliderValues.Add(Random.Range(sliders[i].minValue, sliders[i].maxValue));
        }
        completedSliders = 0;
        requiredSliders = sliders.Count;
    }
}
