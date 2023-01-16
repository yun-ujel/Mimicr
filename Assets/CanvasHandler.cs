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
        RectTransform rectTransform = newWindow.GetComponent<RectTransform>();
        Vector2 remappedAnchor = new Vector2
        (
            -((rectTransform.anchorMax.x * 2) - 1),
            -((rectTransform.anchorMax.y * 2) - 1)
        );

        rectTransform.anchoredPosition = new Vector2
        (
            Random.Range
            (
                0f, 
                canvasRectTransform.sizeDelta.x
            ) * remappedAnchor.x,
            Random.Range
            (
                0f, 
                canvasRectTransform.sizeDelta.y
            ) * remappedAnchor.y
        );
    }
    private float Remap(float inputValue, float inMin, float inMax, float outMin, float outMax)
    {
        return (inputValue - inMin) / (inMax - inMin) * (outMax - outMin) + outMin;
    }

    


    //                                                  Anchors: 1, 0
    // Bottom Right Min Range: -300, +300   Required Multiplier: -1, +1
    // Bottom Right Max Range: +0, +0       Required Multiplier: 0, 0

    //                                                  Anchors: 1, 1
    // Top Right Min Range: -300, +0        Required Multiplier: -1, 0
    // Top Right Max Range: +0, +300        Required Multiplier: 0, +1

    //                                                  Anchors: 0, 0
    // Bottom Left Min Range: +0, +300      Required Multiplier: 0, +1
    // Bottom Left Max Range: -300, +0      Required Multiplier: -1, 0

    //                                                  Anchors: 0, 1
    // Top Left Min Range: +0, +0           Required Multiplier: 0, 0
    // Top Left Max Range: -300, +300       Required Multiplier: -1, +1
}
