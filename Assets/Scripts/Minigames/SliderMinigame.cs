using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderMinigame : MonoBehaviour
{
    [SerializeField] public Slider[] sliders;
    [SerializeField] List<float> correctSliderValues = new List<float>();

    void Start()
    {
        OnWindowStart();
    }
    void Update()
    {
        
    }

    void OnWindowStart()
    {
        for (int i = 0; i < sliders.Length; i++)
        {
            sliders[i].value = Random.Range(sliders[i].minValue, sliders[i].maxValue);

            correctSliderValues.Add(Random.Range(sliders[i].minValue, sliders[i].maxValue));
        }
    }
}
