using UnityEngine;
using UnityEngine.EventSystems;

public class MoveBasic : DragFunction
{
    private RectTransform rectTransform;
    private float canvasScaleFactor;

    public override void GetInfo(UIMaster uIMaster)
    {
        rectTransform = uIMaster.rectTransform;
        canvasScaleFactor = uIMaster.canvas.scaleFactor;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvasScaleFactor;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {

    }

    public override void OnPointerExit(PointerEventData eventData)
    {

    }
}
