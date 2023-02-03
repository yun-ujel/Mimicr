using UnityEngine;
using UnityEngine.EventSystems;

public class ResizeWithCollision : ResizeType
{
    private float min;

    public override RectTransform ResizeAll(RectTransform rectTransform, int anchorIndex, PointerEventData eventData, float canvasScaleFactor, Vector2 boundsSize, Vector2 minSize)
    {
        // Calculate Bounds
        Vector2 minPosition = rectTransform.UnanchorPosition();
        Vector2 maxPosition = new Vector2
        (
            minPosition.x + rectTransform.rect.width,
            minPosition.y + rectTransform.rect.height
        );

        // Calculate Distance
        Vector2 minDistance = -minPosition;
        Vector2 maxDistance = boundsSize - maxPosition;

        float offsetMinX = rectTransform.offsetMin.x;
        float offsetMinY = rectTransform.offsetMin.y;

        float offsetMaxX = rectTransform.offsetMax.x;
        float offsetMaxY = rectTransform.offsetMax.y;

        // Top Left
        if (anchorIndex == 0)
        {
            // Upward Upsizing (moving top of the object up)
            if (eventData.delta.y > 0 && maxPosition.y < boundsSize.y)
            {   // if mouse is moving upwards && object isn't touching the top of the Bounds

                offsetMaxY += Mathf.Min((eventData.delta.y / canvasScaleFactor), maxDistance.y);

                // Upsize the object upwards, adding the minimum height between: 
                // the distance the mouse has travelled, and the distance between the object and the top of the bounds
                // This way the object can't stretch out of bounds
            }
            // Downward Downsizing (moving top of the object down)
            else if (eventData.delta.y < 0 && rectTransform.rect.height > minSize.y)
            {   // if mouse is moving downwards && object is above the minimum height

                min = minSize.y - rectTransform.rect.height;
                // The negative value representing the difference between current and minimum height

                offsetMaxY += Mathf.Max((eventData.delta.y / canvasScaleFactor), min);
                // Downsize the object down, subtracting the minimum height between:
                // The distance the mouse has travelled, and the min
                // This way the object can't shrink below min height
            }

            // Left Upsizing (moving left of the object left)
            if (eventData.delta.x < 0 && minPosition.x > 0)
            {   // if the mouse is moving left && object isn't touching the left side of the bounds

                offsetMinX += Mathf.Max((eventData.delta.x / canvasScaleFactor), minDistance.x);

                // Upsize the object left, adding the minimum length between: 
                // the distance the mouse has travelled, and the distance between the object and the left side of the bounds
                // This way the object can't stretch out of bounds
            }
            // Right Downsizing (moving left of the object right)
            else if (eventData.delta.x > 0 && rectTransform.rect.width > minSize.y)
            {   // if mouse is moving right && object is above the minimum width

                min = rectTransform.rect.width - minSize.x;
                // The positive value representing the difference between current and minimum width

                offsetMinX += Mathf.Min((eventData.delta.x / canvasScaleFactor), min);
                // Downsize the object right, subtracting the minimum width between:
                // The distance the mouse has travelled, and the min
                // This way the object can't shrink below min width
            }
        }

        // Bottom Right
        if (anchorIndex == 8)
        {
            // Downward Upsizing (moving bottom of the object down)
            if (eventData.delta.y < 0 && minPosition.y > 0)
            {   // If mouse is moving downwards && object isn't touching the bottom of the bounds

                offsetMinY += Mathf.Max((eventData.delta.y / canvasScaleFactor), minDistance.y);

                // Upsize the object down, adding the minimum height between:
                // The distance the mouse has travelled, and the distance from the bottom of the bounds
                // This way the object can't stretch out of bounds
            }
            // Upward Downsizing (moving bottom of the object up)
            else if (eventData.delta.y > 0 && rectTransform.rect.height > minSize.y)
            {   // If mouse is moving upwards && object is above minimum height

                min = rectTransform.rect.height - minSize.y;
                // The positive value representing the difference between current and minimum height

                offsetMinY += Mathf.Min((eventData.delta.y / canvasScaleFactor), min);
                // Downsize the object up, subtracting the minimum height between:
                // The distance the mouse has travelled, and the min
                // This way the object can't shrink below min height
            }

            // Right Upsizing (moving right of the object right)
            if (eventData.delta.x > 0 && maxPosition.x < boundsSize.x)
            {   // If mouse is moving right && object isn't touching the right side of the bounds

                offsetMaxX += Mathf.Min((eventData.delta.x / canvasScaleFactor), maxDistance.x);

                // Upsize the object right, adding the minimum height between:
                // The distance the mouse has travelled, and the distance from the right side of the bounds
                // This way the object can't stretch out of bounds
            }
            // Left Downsizing (moving right of the object left)
            else if (eventData.delta.x < 0 && rectTransform.rect.width > minSize.x)
            {   // If mouse is moving left && object is above minimum width

                min = minSize.x - rectTransform.rect.width;
                // The negative value representing the difference between current and minimum width

                offsetMaxX += Mathf.Max((eventData.delta.x / canvasScaleFactor), min);

                // Downsize the object left, subtracting the minimum height between:
                // The distance the mouse has travelled, and the min
                // This way the object can't shrink below min width
            }
        }

        // Top
        if (anchorIndex == 1)
        {
            // Upward Upsizing
            if (eventData.delta.y > 0 && maxPosition.y < boundsSize.y)
            {
                offsetMaxY += Mathf.Min((eventData.delta.y / canvasScaleFactor), maxDistance.y);
            }
            // Downward Downsizing
            else if (eventData.delta.y < 0 && rectTransform.rect.height > minSize.y)
            {
                min = minSize.y - rectTransform.rect.height;
                offsetMaxY += Mathf.Max((eventData.delta.y / canvasScaleFactor), min);
            }
        }

        // Top Right
        if (anchorIndex == 2)
        {
            // Upward Upsizing
            if (eventData.delta.y > 0 && maxPosition.y < boundsSize.y)
            {
                offsetMaxY += Mathf.Min((eventData.delta.y / canvasScaleFactor), maxDistance.y);
            }
            // Downward Downsizing
            else if (eventData.delta.y < 0 && rectTransform.rect.height > minSize.y)
            {
                min = minSize.y - rectTransform.rect.height;
                offsetMaxY += Mathf.Max((eventData.delta.y / canvasScaleFactor), min);
            }

            // Right Upsizing
            if (eventData.delta.x > 0 && maxPosition.x < boundsSize.x)
            {
                offsetMaxX += Mathf.Min((eventData.delta.x / canvasScaleFactor), maxDistance.x);
            }
            // Left Downsizing
            else if (eventData.delta.x < 0 && rectTransform.rect.width > minSize.x)
            {
                min = minSize.x - rectTransform.rect.width;
                offsetMaxX += Mathf.Max((eventData.delta.x / canvasScaleFactor), min);
            }
        }

        // Left
        if (anchorIndex == 3)
        {
            // Left Upsizing
            if (eventData.delta.x < 0 && minPosition.x > 0)
            {
                offsetMinX += Mathf.Max((eventData.delta.x / canvasScaleFactor), minDistance.x);
            }
            // Right Downsizing
            else if (eventData.delta.x > 0 && rectTransform.rect.width > minSize.y)
            {
                min = rectTransform.rect.width - minSize.x;
                offsetMinX += Mathf.Min((eventData.delta.x / canvasScaleFactor), min);
            }
        }

        // Right
        if (anchorIndex == 5)
        {
            // Right Upsizing
            if (eventData.delta.x > 0 && maxPosition.x < boundsSize.x)
            {
                offsetMaxX += Mathf.Min((eventData.delta.x / canvasScaleFactor), maxDistance.x);
            }
            // Left Downsizing
            else if (eventData.delta.x < 0 && rectTransform.rect.width > minSize.x)
            {
                min = minSize.x - rectTransform.rect.width;
                offsetMaxX += Mathf.Max((eventData.delta.x / canvasScaleFactor), min);
            }
        }

        // Bottom Left
        if (anchorIndex == 6)
        {
            // Downward Upsizing
            if (eventData.delta.y < 0 && minPosition.y > 0)
            {
                offsetMinY += Mathf.Max((eventData.delta.y / canvasScaleFactor), minDistance.y);
            }
            // Upward Downsizing
            else if (eventData.delta.y > 0 && rectTransform.rect.height > minSize.y)
            {
                min = rectTransform.rect.height - minSize.y;
                offsetMinY += Mathf.Min((eventData.delta.y / canvasScaleFactor), min);
            }

            // Left Upsizing
            if (eventData.delta.x < 0 && minPosition.x > 0)
            {
                offsetMinX += Mathf.Max((eventData.delta.x / canvasScaleFactor), minDistance.x);
            }
            // Right Downsizing
            else if (eventData.delta.x > 0 && rectTransform.rect.width > minSize.y)
            {
                min = rectTransform.rect.width - minSize.x;
                offsetMinX += Mathf.Min((eventData.delta.x / canvasScaleFactor), min);
            }
        }

        // Bottom 
        if (anchorIndex == 7)
        {
            // Downward Upsizing
            if (eventData.delta.y < 0 && minPosition.y > 0)
            {
                offsetMinY += Mathf.Max((eventData.delta.y / canvasScaleFactor), minDistance.y);
            }
            // Upward Downsizing
            else if (eventData.delta.y > 0 && rectTransform.rect.height > minSize.y)
            {
                min = rectTransform.rect.height - minSize.y;
                offsetMinY += Mathf.Min((eventData.delta.y / canvasScaleFactor), min);
            }
        }

        rectTransform.offsetMin = new Vector2(offsetMinX, offsetMinY);
        rectTransform.offsetMax = new Vector2(offsetMaxX, offsetMaxY);

        return rectTransform;
    }
}

