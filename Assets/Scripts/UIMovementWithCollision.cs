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

    private Vector2 minDistance; // The distance between the object and the bottom left of the bounds (negative)
    private Vector2 maxDistance; // The distance between the object and the top right of the bounds   (positive)

    float min; // The negative value representing the difference between current and minimum size

    public Vector2 minSize;

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
            float anchoredPosX = rectTransform.anchoredPosition.x;
            float anchoredPosY = rectTransform.anchoredPosition.y;

            // Upward Movement
            if (eventData.delta.y > 0 && maxPosition.y < boundsRectTransform.rect.height)
            {   // if mouse is moving upwards && object isn't touching the top of the Bounds

                anchoredPosY += Mathf.Min((eventData.delta.y / canvas.scaleFactor), maxDistance.y);

                // Move the object upwards, choosing the minimum distance between: 
                // the distance the mouse has travelled, and the distance between the object and the top of the bounds
                // This way the object can't move out of bounds
            }
            // Downward Movement
            else if (eventData.delta.y < 0 && minPosition.y > 0)
            {   // if mouse is moving downwards && object isn't touching the bottom of the bounds

                anchoredPosY += Mathf.Max((eventData.delta.y / canvas.scaleFactor), minDistance.y);

                // Move the object downwards, choosing the minimum distance between: 
                // the distance the mouse has travelled, and the distance between the object and the bottom of the bounds
                // This way the object can't move out of bounds
            }

            // Right Movement
            if (eventData.delta.x > 0 && maxPosition.x < boundsRectTransform.rect.width)
            {   // if the mouse is moving right && object isn't touching the right side of the bounds

                anchoredPosX += Mathf.Min((eventData.delta.x / canvas.scaleFactor), maxDistance.x);

                // Move the object right, choosing the minimum distance between: 
                // the distance the mouse has travelled, and the distance between the object and the right side of the bounds
                // This way the object can't move out of bounds
            }
            // Left Movement
            else if (eventData.delta.x < 0 && minPosition.x > 0)
            {   // if the mouse is moving left && object isn't touching the left side of the bounds

                anchoredPosX += Mathf.Max((eventData.delta.x / canvas.scaleFactor), minDistance.x);

                // Move the object left, choosing the minimum distance between: 
                // the distance the mouse has travelled, and the distance between the object and the left side of the bounds
                // This way the object can't move out of bounds
            }

            rectTransform.anchoredPosition = new Vector2(anchoredPosX, anchoredPosY);
        }
        else if (functionOnDrag == DragFunction.resize)
        {
            float offsetMinX = rectTransform.offsetMin.x;
            float offsetMinY = rectTransform.offsetMin.y;

            float offsetMaxX = rectTransform.offsetMax.x;
            float offsetMaxY = rectTransform.offsetMax.y;

            // Top Left
            if (anchorIndex == 0)
            {
                // Upward Upsizing
                if (eventData.delta.y > 0 && maxPosition.y < boundsRectTransform.rect.height)
                {   // if mouse is moving upwards && object isn't touching the top of the Bounds

                    offsetMaxY += Mathf.Min((eventData.delta.y / canvas.scaleFactor), maxDistance.y);

                    // Upsize the object upwards, adding the minimum height between: 
                    // the distance the mouse has travelled, and the distance between the object and the top of the bounds
                    // This way the object can't stretch out of bounds
                }
                // Downward Downsizing
                else if (eventData.delta.y < 0 && rectTransform.rect.height > minSize.y)
                {   // if mouse is moving downwards && object is above the minimum height

                    min = minSize.y - rectTransform.rect.height;
                    // The negative value representing the difference between current and minimum height

                    offsetMaxY += Mathf.Max((eventData.delta.y / canvas.scaleFactor), min);
                    // Downsize the object down, subtracting the minimum height between:
                    // The distance the mouse has travelled, and the min
                    // This way the object can't shrink below min size
                }

                // Left Upsizing
                if (eventData.delta.x < 0 && minPosition.x > 0)
                {   // if the mouse is moving left && object isn't touching the left side of the bounds

                    offsetMinX += Mathf.Max((eventData.delta.x / canvas.scaleFactor), minDistance.x);

                    // Upsize the object left, adding the minimum length between: 
                    // the distance the mouse has travelled, and the distance between the object and the left side of the bounds
                    // This way the object can't stretch out of bounds
                }
                // Right Downsizing
                else if (eventData.delta.x > 0 && rectTransform.rect.width > minSize.y)
                {   // if mouse is moving right && object is above the minimum width

                    min = rectTransform.rect.width - minSize.x;
                    // The positive value representing the difference between current and minimum width

                    offsetMinX += Mathf.Min((eventData.delta.x / canvas.scaleFactor), min);
                    // Downsize the object right, subtracting the minimum width between:
                    // The distance the mouse has travelled, and the min
                    // This way the object can't shrink below min size
                }
            }

            // Bottom Right
            if (anchorIndex == 8)
            {
                // Downward Upsizing
                if (eventData.delta.y < 0 && minPosition.y > 0)
                {   // If mouse is moving downwards && object isn't touching the bottom of the bounds

                    offsetMinY += Mathf.Max((eventData.delta.y / canvas.scaleFactor), minDistance.y);

                    

                }
            }

            rectTransform.offsetMin = new Vector2(offsetMinX, offsetMinY);
            rectTransform.offsetMax = new Vector2(offsetMaxX, offsetMaxY);
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
