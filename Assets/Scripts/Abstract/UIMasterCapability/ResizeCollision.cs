using UnityEngine;
using UnityEngine.EventSystems;

public class ResizeCollision : DragFunction
{
    private RectTransform rectTransform;
    private RectTransform boundsRectTransform;
    private int anchorIndex;
    private Vector2 minWindowSize;

    private float canvasScaleFactor;

    private Vector2 minPosition; // The position of the object from the bottom left
    private Vector2 maxPosition; // The position of the object from the top right

    private Vector2 minDistance; // The distance between the object and the bottom left of the bounds (negative)
    private Vector2 maxDistance; // The distance between the object and the top right of the bounds   (positive)

    float min; // The negative value representing the difference between current and minimum size

    Padding padding; // Padding for the parent bounds to slightly adjust their size
    public override void GetInfo(UIMaster uIMaster)
    {
        rectTransform = uIMaster.rectTransform;
        minWindowSize = uIMaster.minWindowSize;

        canvasScaleFactor = uIMaster.canvas.scaleFactor;
        anchorIndex = uIMaster.anchorIndex;

        padding = uIMaster.padding;

        boundsRectTransform = rectTransform.parent.GetComponent<RectTransform>();
    }

    public override void OnDrag(PointerEventData eventData)
    {
        CalculateBounds();
        CalculateDistance();

        float offsetMinX = rectTransform.offsetMin.x;
        float offsetMinY = rectTransform.offsetMin.y;

        float offsetMaxX = rectTransform.offsetMax.x;
        float offsetMaxY = rectTransform.offsetMax.y;

        // Top Left
        if (anchorIndex == 0)
        {
            offsetMaxY = TopResize(eventData.delta.y, offsetMaxY);
            offsetMinX = LeftResize(eventData.delta.x, offsetMinX);
        }

        // Top
        if (anchorIndex == 1)
        {
            offsetMaxY = TopResize(eventData.delta.y, offsetMaxY);
        }

        // Top Right
        if (anchorIndex == 2)
        {
            offsetMaxY = TopResize(eventData.delta.y, offsetMaxY);
            offsetMaxX = RightResize(eventData.delta.x, offsetMaxX);
        }

        // Left
        if (anchorIndex == 3)
        {
            offsetMinX = LeftResize(eventData.delta.x, offsetMinX);
        }

        // Right
        if (anchorIndex == 5)
        {
            offsetMaxX = RightResize(eventData.delta.x, offsetMaxX);
        }

        // Bottom Left
        if (anchorIndex == 6)
        {
            offsetMinY = BottomResize(eventData.delta.y, offsetMinY);
            offsetMinX = LeftResize(eventData.delta.x, offsetMinX);
        }

        // Bottom 
        if (anchorIndex == 7)
        {
            offsetMinY = BottomResize(eventData.delta.y, offsetMinY);
        }

        // Bottom Right
        if (anchorIndex == 8)
        {
            offsetMinY = BottomResize(eventData.delta.y, offsetMinY);
            offsetMaxX = RightResize(eventData.delta.x, offsetMaxX);
        }

        rectTransform.offsetMin = new Vector2(offsetMinX, offsetMinY);
        rectTransform.offsetMax = new Vector2(offsetMaxX, offsetMaxY);
    }



    public override void OnPointerEnter(PointerEventData eventData)
    {

    }

    public override void OnPointerExit(PointerEventData eventData)
    {

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


    private float TopResize(float deltaY, float offsetMaxY)
    {
        // Upward Upsizing (moving top of the object up)
        if (deltaY > 0 && maxPosition.y < boundsRectTransform.rect.height)
        {   // if mouse is moving upwards && object isn't touching the top of the Bounds

            offsetMaxY += Mathf.Min((deltaY / canvasScaleFactor), maxDistance.y);

            // Upsize the object upwards, adding the minimum height between: 
            // the distance the mouse has travelled, and the distance between the object and the top of the bounds
            // This way the object can't stretch out of bounds
        }
        // Downward Downsizing (moving top of the object down)
        else if (deltaY < 0 && rectTransform.rect.height > minWindowSize.y)
        {   // if mouse is moving downwards && object is above the minimum height

            min = minWindowSize.y - rectTransform.rect.height;
            // The negative value representing the difference between current and minimum height

            offsetMaxY += Mathf.Max((deltaY / canvasScaleFactor), min);
            // Downsize the object down, subtracting the minimum height between:
            // The distance the mouse has travelled, and the min
            // This way the object can't shrink below min height
        }

        return offsetMaxY;
    }

    private float BottomResize(float deltaY, float offsetMinY)
    {
        // Downward Upsizing (moving bottom of the object down)
        if (deltaY < 0 && minPosition.y > 0)
        {   // If mouse is moving downwards && object isn't touching the bottom of the bounds

            offsetMinY += Mathf.Max((deltaY / canvasScaleFactor), minDistance.y);

            // Upsize the object down, adding the minimum height between:
            // The distance the mouse has travelled, and the distance from the bottom of the bounds
            // This way the object can't stretch out of bounds
        }
        // Upward Downsizing (moving bottom of the object up)
        else if (deltaY > 0 && rectTransform.rect.height > minWindowSize.y)
        {   // If mouse is moving upwards && object is above minimum height

            min = rectTransform.rect.height - minWindowSize.y;
            // The positive value representing the difference between current and minimum height

            offsetMinY += Mathf.Min((deltaY / canvasScaleFactor), min);
            // Downsize the object up, subtracting the minimum height between:
            // The distance the mouse has travelled, and the min
            // This way the object can't shrink below min height
        }

        return offsetMinY;
    }

    private float RightResize(float deltaX, float offsetMaxX)
    {
        // Right Upsizing (moving right of the object right)
        if (deltaX > 0 && maxPosition.x < boundsRectTransform.rect.width)
        {   // If mouse is moving right && object isn't touching the right side of the bounds

            offsetMaxX += Mathf.Min((deltaX / canvasScaleFactor), maxDistance.x);

            // Upsize the object right, adding the minimum height between:
            // The distance the mouse has travelled, and the distance from the right side of the bounds
            // This way the object can't stretch out of bounds
        }
        // Left Downsizing (moving right of the object left)
        else if (deltaX < 0 && rectTransform.rect.width > minWindowSize.x)
        {   // If mouse is moving left && object is above minimum width

            min = minWindowSize.x - rectTransform.rect.width;
            // The negative value representing the difference between current and minimum width

            offsetMaxX += Mathf.Max((deltaX / canvasScaleFactor), min);

            // Downsize the object left, subtracting the minimum height between:
            // The distance the mouse has travelled, and the min
            // This way the object can't shrink below min width
        }

        return offsetMaxX;
    }

    private float LeftResize(float deltaX, float offsetMinX)
    {
        // Left Upsizing (moving left of the object left)
        if (deltaX < 0 && minPosition.x > 0)
        {   // if the mouse is moving left && object isn't touching the left side of the bounds

            offsetMinX += Mathf.Max((deltaX / canvasScaleFactor), minDistance.x);

            // Upsize the object left, adding the minimum length between: 
            // the distance the mouse has travelled, and the distance between the object and the left side of the bounds
            // This way the object can't stretch out of bounds
        }
        // Right Downsizing (moving left of the object right)
        else if (deltaX > 0 && rectTransform.rect.width > minWindowSize.y)
        {   // if mouse is moving right && object is above the minimum width

            min = rectTransform.rect.width - minWindowSize.x;
            // The positive value representing the difference between current and minimum width

            offsetMinX += Mathf.Min((deltaX / canvasScaleFactor), min);
            // Downsize the object right, subtracting the minimum width between:
            // The distance the mouse has travelled, and the min
            // This way the object can't shrink below min width
        }

        return offsetMinX;
    }
}