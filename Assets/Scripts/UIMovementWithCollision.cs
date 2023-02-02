using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIMovementWithCollision : MonoBehaviour, IDragHandler  
{
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private RectTransform boundsRectTransform;
    [SerializeField] private Canvas canvas;

    private Vector2 minPosition; // The position of the object from the bottom left
    private Vector2 maxPosition; // The position of the object from the top right

    private Vector2 minDistance; // The distance between the object and the bottom left of the bounds
    private Vector2 maxDistance; // The distance between the object and the top right of the bounds

    [SerializeField] public DragFunction functionOnDrag = DragFunction.none;
    public enum DragFunction
    {
        none,
        move,
        resize
    }

    [SerializeField] private int anchorIndex;

    private void Awake()
    {
        
    }
    public void OnDrag(PointerEventData eventData)
    {
        CalculateBounds();
        CalculateDistance();

        if (functionOnDrag == DragFunction.move)
        {
            Vector2 anchoredPos = rectTransform.anchoredPosition;

            // Upward Movement
            if (eventData.delta.y > 0 && maxPosition.y < boundsRectTransform.rect.height)
            {// if mouse is moving upwards && object isn't touching the top of the Bounds
                anchoredPos = new Vector2
                (
                    anchoredPos.x,
                    anchoredPos.y + Mathf.Min((eventData.delta.y / canvas.scaleFactor), maxDistance.y)
                ); // Move the object upwards, choosing the minimum distance between: 
                   // the distance the mouse has travelled, and the distance between the object and the top of the bounds
                   // This way the object can't move out of bounds
            }
            // Downward Movement
            else if (eventData.delta.y < 0 && minPosition.y > 0)
            {// if mouse is moving downwards && object isn't touching the bottom of the bounds
                anchoredPos = new Vector2
                (
                    anchoredPos.x,
                    anchoredPos.y + Mathf.Max((eventData.delta.y / canvas.scaleFactor), minDistance.y)
                ); // Move the object downwards, choosing the minimum distance between: 
                   // the distance the mouse has travelled, and the distance between the object and the bottom of the bounds
                   // This way the object can't move out of bounds
            }

            // Right Movement
            if (eventData.delta.x > 0 && maxPosition.x < boundsRectTransform.rect.width)
            {// if the mouse is moving right && object isn't touching the right side of the bounds
                anchoredPos = new Vector2
                (
                    anchoredPos.x + Mathf.Min((eventData.delta.x / canvas.scaleFactor), maxDistance.x),
                    anchoredPos.y
                ); // Move the object right, choosing the minimum distance between: 
                   // the distance the mouse has travelled, and the distance between the object and the right side of the bounds
                   // This way the object can't move out of bounds
            }
            // Left Movement
            else if (eventData.delta.x < 0 && minPosition.x > 0)
            {// if the mouse is moving left && object isn't touching the left side of the bounds
                anchoredPos = new Vector2
                (
                    anchoredPos.x + Mathf.Max((eventData.delta.x / canvas.scaleFactor), minDistance.x),
                    anchoredPos.y
                ); // Move the object left, choosing the minimum distance between: 
                   // the distance the mouse has travelled, and the distance between the object and the left side of the bounds
                   // This way the object can't move out of bounds
            }

            rectTransform.anchoredPosition = anchoredPos;
        }
        else if (functionOnDrag == DragFunction.resize)
        {
            Vector2 offsetMin = rectTransform.offsetMin;
            Vector2 offsetMax = rectTransform.offsetMax;

            // Upward Upsizing
            if (eventData.delta.y > 0 && maxPosition.y < boundsRectTransform.rect.height)
            {// if mouse is moving upwards && object isn't touching the top of the Bounds
                offsetMax = new Vector2
                (
                    offsetMax.x,
                    offsetMax.y + Mathf.Min((eventData.delta.y / canvas.scaleFactor), maxDistance.y)
                ); // Upsize the object upwards, adding the minimum height between: 
                   // the distance the mouse has travelled, and the distance between the object and the top of the bounds
                   // This way the object can't stretch out of bounds
            }


            // Left Upsizing
            if (eventData.delta.x < 0 && minPosition.x > 0)
            {// if the mouse is moving left && object isn't touching the left side of the bounds
                offsetMin = new Vector2
                (
                    offsetMin.x + Mathf.Max((eventData.delta.x / canvas.scaleFactor), minDistance.x),
                    offsetMin.y
                ); // Upsize the object left, adding the minimum length between: 
                   // the distance the mouse has travelled, and the distance between the object and the left side of the bounds
                   // This way the object can't stretch out of bounds
            }
            

            rectTransform.offsetMin = offsetMin;
            rectTransform.offsetMax = offsetMax;
        }
    }

    private void CalculateBounds()
    {
        minPosition = rectTransform.UnanchorPosition();
        maxPosition = new Vector2
        (
            minPosition.x + rectTransform.rect.width,
            minPosition.y + rectTransform.rect.height
        );
    }

    private void CalculateDistance()
    {
        minDistance = -minPosition;
        maxDistance = boundsRectTransform.rect.size - maxPosition;
    }
}
