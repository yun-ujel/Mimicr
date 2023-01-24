using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderMinigame : MonoBehaviour
{
    private int completedSliders = 0;
    private int requiredSliders = 1;

    [Header("References")]
    [SerializeField] private List<Slider> sliders = new List<Slider>();
    List<float> correctSliderValues = new List<float>();

    void Update()
    {
        for (int i = 0; i < sliders.Count; i++)
        {
            if (sliders[i].value - 0.05f < correctSliderValues[i] &&
                sliders[i].value + 0.05f > correctSliderValues[i])
            {
                //Debug.Log("Hooray!");

                sliders[i].interactable = false;

                sliders.Remove(sliders[i]);
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
        Debug.Log("Spawned Slider Window");
        for (int i = 0; i < sliders.Count; i++)
        {
            sliders[i].value = Random.Range(sliders[i].minValue, sliders[i].maxValue);

            correctSliderValues.Add(Random.Range(sliders[i].minValue, sliders[i].maxValue));
        }
        completedSliders = 0;
        requiredSliders = sliders.Count;
    }
}