using UnityEngine.EventSystems;
using UnityEngine;

public abstract class ResizeType : ScriptableObject
{ 
    public abstract RectTransform ResizeAll(RectTransform inputRectTransform, int anchorIndex, PointerEventData eventData, float canvasScaleFactor, Vector2 boundsSize, Vector2 minSize);
}
