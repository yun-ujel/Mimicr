using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(UIMaster))]
public abstract class DragFunction : MonoBehaviour, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public abstract void GetInfo(UIMaster uIMaster);

    public abstract void OnDrag(PointerEventData eventData);
    public abstract void OnPointerEnter(PointerEventData eventData);
    public abstract void OnPointerExit(PointerEventData eventData);
}

[System.Serializable]
public class Padding
{
    public float left;
    public float right;
    public float top;
    public float bottom;
}
