using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CanvasHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] windowsToOpen;
    RectTransform rectTransform;
    Canvas canvas;
    void Awake()
    {
        canvas = GetComponent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
    }
    
    void Update()
    {
        
    }

    public void InstantiateWindow(int windowIndex)
    {
        // Randomly select and Instantiate window as a child of this object
        int selection = Random.Range(0, windowsToOpen.Length);
        GameObject windowToInstantiate = windowsToOpen[selection];

        GameObject newWindow = Instantiate(windowToInstantiate, transform);

        // Randomize position of the object
        RectTransform wRectTransform = newWindow.GetComponent<RectTransform>();

        wRectTransform.anchoredPosition = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.y);
    }

    private float Remap(float inputValue, float inMin, float inMax, float outMin, float outMax)
    {
        return (inputValue - inMin) / (inMax - inMin) * (outMax - outMin) + outMin;
    }
}
