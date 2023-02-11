using UnityEngine;
using UnityEngine.EventSystems;

public class PinButton : MonoBehaviour, IPointerClickHandler
{
    [Header("References")]
    [SerializeField] DisplayPin displayPin;
    [Header("Options")]
    [SerializeField] private bool clearPinOnClick = false;
    [SerializeField] private int intToAdd;


    public void OnPointerClick(PointerEventData eventData)
    {
        if (!clearPinOnClick)
        {
            displayPin.AddToType(intToAdd);
        }
        else
        {
            displayPin.ClearType();
        }
    }

    public void GetDisplay(DisplayPin display, int buttonType)
    {
        displayPin = display;
        intToAdd = buttonType;
    }
}
