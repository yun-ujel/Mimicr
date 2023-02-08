using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveCollision : DragFunction
{
    private RectTransform rectTransform;
    private RectTransform boundsRectTransform;

    private Vector2 minPosition; // The position of the object from the bottom left
    private Vector2 maxPosition; // The position of the object from the top right

    private Vector2 minDistance; // The distance between the object and the bottom left of the bounds (negative)
    private Vector2 maxDistance; // The distance between the object and the top right of the bounds   (positive)

    private float canvasScaleFactor;

    Padding padding; // Padding for the parent bounds to slightly adjust their size

    public override void GetInfo(UIMaster uIMaster)
    {
        rectTransform = uIMaster.rectTransform;
        canvasScaleFactor = uIMaster.canvas.scaleFactor;

        boundsRectTransform = rectTransform.parent.GetComponent<RectTransform>();
        padding = uIMaster.padding;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        CalculateBounds();
        CalculateDistance();

        float anchoredPosX = rectTransform.anchoredPosition.x;
        float anchoredPosY = rectTransform.anchoredPosition.y;

        // Upward Movement
        if (eventData.delta.y > 0 && maxPosition.y < boundsRectTransform.rect.height)
        {   // if mouse is moving upwards && object isn't touching the top of the Bounds

            anchoredPosY += Mathf.Min((eventData.delta.y / canvasScaleFactor), maxDistance.y); // Positive value, Mathf.Min is used

            // Move the object upwards, choosing the minimum distance between: 
            // the distance the mouse has travelled, and the distance between the object and the top of the bounds
            // This way the object can't move out of bounds
        }
        // Downward Movement
        else if (eventData.delta.y < 0 && minPosition.y > 0)
        {   // if mouse is moving downwards && object isn't touching the bottom of the bounds

            anchoredPosY += Mathf.Max((eventData.delta.y / canvasScaleFactor), minDistance.y); // Negative value, Mathf.Max is used

            // Move the object downwards, choosing the minimum distance between: 
            // the distance the mouse has travelled, and the distance between the object and the bottom of the bounds
            // This way the object can't move out of bounds
        }

        // Right Movement
        if (eventData.delta.x > 0 && maxPosition.x < boundsRectTransform.rect.width)
        {   // if the mouse is moving right && object isn't touching the right side of the bounds

            anchoredPosX += Mathf.Min((eventData.delta.x / canvasScaleFactor), maxDistance.x);

            // Move the object right, choosing the minimum distance between: 
            // the distance the mouse has travelled, and the distance between the object and the right side of the bounds
            // This way the object can't move out of bounds
        }
        // Left Movement
        else if (eventData.delta.x < 0 && minPosition.x > 0)
        {   // if the mouse is moving left && object isn't touching the left side of the bounds

            anchoredPosX += Mathf.Max((eventData.delta.x / canvasScaleFactor), minDistance.x);

            // Move the object left, choosing the minimum distance between: 
            // the distance the mouse has travelled, and the distance between the object and the left side of the bounds
            // This way the object can't move out of bounds
        }

        rectTransform.anchoredPosition = new Vector2(anchoredPosX, anchoredPosY);
    }

    private void CalculateBounds()
    {
        Vector2 unanchoredPosition = rectTransform.UnanchorPosition();

        minPosition = new Vector2
        (
            unanchoredPosition.x - padding.left,
            unanchoredPosition.y - padding.bottom
        );

        maxPosition = new Vector2
        (
            unanchoredPosition.x + rectTransform.rect.width + padding.right,
            unanchoredPosition.y + rectTransform.rect.height + padding.top
        );
    }

    private void CalculateDistance()
    {
        minDistance = -minPosition;
        maxDistance = boundsRectTransform.rect.size - maxPosition;
    }


    public override void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        
    }
}
