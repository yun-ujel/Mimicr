using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderMinigame : MonoBehaviour
{
    private CanvasHandler canvasHandler;

    private int completedSliders = 0;
    private int requiredSliders = 1;

    [Header("References")]
    [SerializeField] private List<Slider> sliders = new List<Slider>();
    List<float> correctSliderValues = new List<float>();
    [SerializeField] private RectTransform fullWindow;

    [Header("Sizes")]
    [SerializeField] private Vector2 minWindowSize = new Vector2(400, 200);
    [SerializeField] private Vector2 maxWindowSize = new Vector2(600, 400);

    void Awake()
    {
        canvasHandler = GameObject.FindGameObjectWithTag("Canvas").GetComponent<CanvasHandler>();
    }
    void Update()
    {
        for (int i = 0; i < sliders.Count; i++)
        {
            if (sliders[i].value < correctSliderValues[i] + 0.05f &&
                sliders[i].value > correctSliderValues[i] - 0.05f)
            {
                Debug.Log("Hooray!");

                sliders[i].interactable = false;

                sliders.Remove(sliders[i]);
                completedSliders += 1;
            }
        }
        if (completedSliders == requiredSliders)
        {
            OnWindowComplete();
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

        fullWindow.sizeDelta = new Vector2
        (
            Random.Range(minWindowSize.x, maxWindowSize.x),
            Random.Range(minWindowSize.y, maxWindowSize.y)
        );
    }

    void OnWindowComplete()
    {
        canvasHandler.BroadcastMessage("WindowCompleted");
        Destroy(gameObject);
    }
}
