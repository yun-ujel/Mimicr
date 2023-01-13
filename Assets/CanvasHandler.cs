using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CanvasHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] windowsToOpen;

    Canvas canvas;
    void Awake()
    {
        canvas = GetComponent<Canvas>();
    }
    
    void Update()
    {
        
    }

    public void InstantiateWindow(int windowIndex)
    {
        int selection = Random.Range(0, windowsToOpen.Length);
        Instantiate
            (
            windowsToOpen[selection], 
            transform.position,
            Quaternion.identity,
            transform
            );
    }
}
