using UnityEngine;
using UnityEngine.EventSystems;

public class ResizeBasic : DragFunction
{
    private RectTransform rectTransform;
    private int anchorIndex;
    private Vector2 minWindowSize;

    private float canvasScaleFactor;

    public override void GetInfo(UIMaster uIMaster)
    {
        rectTransform = uIMaster.rectTransform;
        anchorIndex = uIMaster.anchorIndex;
        minWindowSize = uIMaster.minWindowSize;

        canvasScaleFactor = uIMaster.canvas.scaleFactor;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        // Top Left
        if (anchorIndex == 0)
        {
            if (eventData.delta.x < 0 || rectTransform.sizeDelta.x > minWindowSize.x) // If the mouse is moving left or if the width is above minimum
            {
                rectTransform.offsetMin = new Vector2
                (
                    rectTransform.offsetMin.x + eventData.delta.x / canvasScaleFactor, // left
                    rectTransform.offsetMin.y
                );
            }
            if (eventData.delta.y > 0 || rectTransform.sizeDelta.y > minWindowSize.y) // If the mouse is moving up or if the height is above minimum
            {
                rectTransform.offsetMax = new Vector2
                (
                    rectTransform.offsetMax.x,
                    rectTransform.offsetMax.y + eventData.delta.y / canvasScaleFactor  // top
                );
            }
        }

        // Top
        else if (anchorIndex == 1)
        {
            if (eventData.delta.y > 0 || rectTransform.sizeDelta.y > minWindowSize.y) // If the mouse is moving up or the height is above minimum
            {
                rectTransform.offsetMax = new Vector2
                (
                    rectTransform.offsetMax.x,
                    rectTransform.offsetMax.y + eventData.delta.y / canvasScaleFactor  // top
                );
            }
        }

        // Top Right
        else if (anchorIndex == 2)
        {
            if (eventData.delta.x > 0 || rectTransform.sizeDelta.x > minWindowSize.x) // If the mouse is moving right or if the width is above minimum
            {
                rectTransform.offsetMax = new Vector2
                (
                    rectTransform.offsetMax.x + eventData.delta.x / canvasScaleFactor, // right
                    rectTransform.offsetMax.y
                );
            }
            if (eventData.delta.y > 0 || rectTransform.sizeDelta.y > minWindowSize.y) // If the mouse is moving up or if the height is above minimum
            {
                rectTransform.offsetMax = new Vector2
                (
                    rectTransform.offsetMax.x,
                    rectTransform.offsetMax.y + eventData.delta.y / canvasScaleFactor  // top
                );
            }
        }

        // Left
        else if (anchorIndex == 3)
        {
            if (eventData.delta.x < 0 || rectTransform.sizeDelta.x > minWindowSize.x) // If the mouse is moving left or if the width is above minimum
            {
                rectTransform.offsetMin = new Vector2
                (
                    rectTransform.offsetMin.x + eventData.delta.x / canvasScaleFactor, // left
                    rectTransform.offsetMin.y
                );
            }
        }

        // Right
        else if (anchorIndex == 5)
        {
            if (eventData.delta.x > 0 || rectTransform.sizeDelta.x > minWindowSize.x) // If the mouse is moving right or if the width is above minimum
            {
                rectTransform.offsetMax = new Vector2
                (
                    rectTransform.offsetMax.x + eventData.delta.x / canvasScaleFactor, // right
                    rectTransform.offsetMax.y
                );
            }
        }

        // Bottom Left
        else if (anchorIndex == 6)
        {
            if (eventData.delta.x < 0 || rectTransform.sizeDelta.x > minWindowSize.x) // If the mouse is moving left or if the width is above minimum
            {
                rectTransform.offsetMin = new Vector2
                (
                    rectTransform.offsetMin.x + eventData.delta.x / canvasScaleFactor, // left
                    rectTransform.offsetMin.y
                );
            }
            if (eventData.delta.y < 0 || rectTransform.sizeDelta.y > minWindowSize.y) // If the mouse is moving down or if the height is above minimum
            {
                rectTransform.offsetMin = new Vector2
                (
                    rectTransform.offsetMin.x,
                    rectTransform.offsetMin.y + eventData.delta.y / canvasScaleFactor // bottom
                );
            }
        }

        // Bottom
        else if (anchorIndex == 7)
        {
            if (eventData.delta.y < 0 || rectTransform.sizeDelta.y > minWindowSize.y) // If the mouse is moving down or if the height is above minimum
            {
                rectTransform.offsetMin = new Vector2
                (
                    rectTransform.offsetMin.x,
                    rectTransform.offsetMin.y + eventData.delta.y / canvasScaleFactor // bottom
                );
            }
        }

        // Bottom Right
        else if (anchorIndex == 8)
        {
            if (eventData.delta.x > 0 || rectTransform.sizeDelta.x > minWindowSize.x) // If the mouse is moving right or if the width is above minimum
            {
                rectTransform.offsetMax = new Vector2
                (
                    rectTransform.offsetMax.x + eventData.delta.x / canvasScaleFactor, // right
                    rectTransform.offsetMax.y
                );
            }
            if (eventData.delta.y < 0 || rectTransform.sizeDelta.y > minWindowSize.y) // If the mouse is moving down or if the height is above minimum
            {
                rectTransform.offsetMin = new Vector2
                (
                    rectTransform.offsetMin.x,
                    rectTransform.offsetMin.y + eventData.delta.y / canvasScaleFactor // bottom
                );
            }
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        
    }

}
