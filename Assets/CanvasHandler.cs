using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CanvasHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] windowsToOpen;
    RectTransform canvasRectTransform;
    Canvas canvas;
    void Awake()
    {
        canvas = GetComponent<Canvas>();
        canvasRectTransform = GetComponent<RectTransform>();
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


        Debug.Log(wRectTransform.anchorMax.Equals(wRectTransform.anchorMin));
        

        if (wRectTransform.anchorMax.Equals(wRectTransform.anchorMin))
        {
            wRectTransform.anchoredPosition = new Vector2
                (
                    canvasRectTransform.sizeDelta.x * ((wRectTransform.anchorMax.x * -1) + 1),
                    canvasRectTransform.sizeDelta.y * ((wRectTransform.anchorMax.y * -1) + 1)
                );
        }
    }

    private float Remap(float inputValue, float inMin, float inMax, float outMin, float outMax)
    {
        return (inputValue - inMin) / (inMax - inMin) * (outMax - outMin) + outMin;
    }
}
