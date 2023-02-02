using UnityEngine.EventSystems;
using UnityEngine;

public class UIMovementWithCollision : MonoBehaviour, IDragHandler
{
    public RectTransform rectTransform;
    public RectTransform boundsRectTransform;

    public Canvas canvas;
    public int anchorIndex;
    public Vector2 minSize;
    [SerializeField] private Vector2 maxBounds;
    [SerializeField] private Vector2 minBounds;

    // Setup: Create 8 'Resize Bars', one for each index. Make them all resize the same object
    // Then Make another object that will always be bigger (usually a parent) and set it to be the Bounds.

    void CalculateBounds()
    {
        minBounds = boundsRectTransform.rect.min;
        maxBounds = new Vector2(minBounds.x + boundsRectTransform.rect.size.x, minBounds.y + boundsRectTransform.rect.size.y);
    }


    // If the mouse is moving left and is not past the left side of the bounds, or is moving right and the width is above minimum
    public void OnDrag(PointerEventData eventData)
    {
        CalculateBounds();

        // Top Left
        if (anchorIndex == 0)
        {
            if ((eventData.delta.x < 0 && eventData.position.x > minBounds.x)   // If the mouse is moving left && is not past the left side of the bounds
                ||                                                              // OR
               (eventData.delta.x > 0 && rectTransform.rect.size.x > minSize.x))// If the mouse is moving right && the width is above minimum
            {
                rectTransform.offsetMin = new Vector2
                (
                    rectTransform.offsetMin.x + eventData.delta.x / canvas.scaleFactor,
                    rectTransform.offsetMin.y
                );
            }
            if ((eventData.delta.y > 0 && eventData.position.y < maxBounds.y)   // If the mouse is moving up && is below the top side of the bounds
                ||                                                              // OR
               (eventData.delta.y < 0 && rectTransform.rect.size.y > minSize.y))// If the mouse is moving down && the height is above minimum
            {
                rectTransform.offsetMax = new Vector2
                (
                    rectTransform.offsetMax.x,
                    rectTransform.offsetMax.y + eventData.delta.y / canvas.scaleFactor
                );
            }
        }

        // Top
        else if (anchorIndex == 1)
        {
            if ((eventData.delta.y > 0 && eventData.position.y < maxBounds.y)   // If the mouse is moving up && is below the top side of the bounds
                ||                                                              // OR
               (eventData.delta.y < 0 && rectTransform.rect.size.y > minSize.y))// If the mouse is moving down && the height is above minimum
            {
                rectTransform.offsetMax = new Vector2
                (
                    rectTransform.offsetMax.x,
                    rectTransform.offsetMax.y + eventData.delta.y / canvas.scaleFactor
                );
            }
        }

        // Top Right
        else if (anchorIndex == 2)
        {
            if ((eventData.delta.x > 0 && eventData.position.x < maxBounds.x)   // If the mouse is moving right && is not past the right side of the bounds
                ||                                                              // OR
               (eventData.delta.x < 0 && rectTransform.rect.size.x > minSize.x))// If the mouse is moving left && the width is above minimum
            {
                rectTransform.offsetMax = new Vector2
                (
                    rectTransform.offsetMax.x + eventData.delta.x / canvas.scaleFactor,
                    rectTransform.offsetMax.y
                );
            }
            if ((eventData.delta.y > 0 && eventData.position.y < maxBounds.y)   // If the mouse is moving up && is below the top side of the bounds
                ||                                                              // OR
               (eventData.delta.y < 0 && rectTransform.rect.size.y > minSize.y))// If the mouse is moving down && the height is above minimum
            {
                rectTransform.offsetMax = new Vector2
                (
                    rectTransform.offsetMax.x,
                    rectTransform.offsetMax.y + eventData.delta.y / canvas.scaleFactor
                );
            }
        }

        // Left
        else if (anchorIndex == 3)
        {
            if ((eventData.delta.x < 0 && eventData.position.x > minBounds.x)   // If the mouse is moving left && is not past the left side of the bounds
                ||                                                              // OR
               (eventData.delta.x > 0 && rectTransform.rect.size.x > minSize.x))// If the mouse is moving right && the width is above minimum
            {
                rectTransform.offsetMin = new Vector2
                (
                    rectTransform.offsetMin.x + eventData.delta.x / canvas.scaleFactor,
                    rectTransform.offsetMin.y
                );
            }
        }

        // Right
        else if (anchorIndex == 5)
        {
            if ((eventData.delta.x > 0 && eventData.position.x < maxBounds.x)   // If the mouse is moving right && is not past the right side of the bounds
                ||                                                              // OR
               (eventData.delta.x < 0 && rectTransform.rect.size.x > minSize.x))// If the mouse is moving left && the width is above minimum
            {
                rectTransform.offsetMax = new Vector2
                (
                    rectTransform.offsetMax.x + eventData.delta.x / canvas.scaleFactor,
                    rectTransform.offsetMax.y
                );
            }
        }

        // Bottom Left
        else if (anchorIndex == 6)
        {
            if ((eventData.delta.x < 0 && eventData.position.x > minBounds.x)   // If the mouse is moving left && is not past the left side of the bounds
                ||                                                              // OR
               (eventData.delta.x > 0 && rectTransform.rect.size.x > minSize.x))// If the mouse is moving right && the width is above minimum
            {
                rectTransform.offsetMin = new Vector2
                (
                    rectTransform.offsetMin.x + eventData.delta.x / canvas.scaleFactor,
                    rectTransform.offsetMin.y
                );
            }
            if ((eventData.delta.y < 0 && eventData.position.y > minBounds.y)   // If the mouse is moving down && is above the bottom side of the bounds
                ||                                                              // OR
               (eventData.delta.y > 0 && rectTransform.rect.size.y > minSize.y))// If the mouse is moving up && is the height is above minimum
            {
                rectTransform.offsetMin = new Vector2
                (
                    rectTransform.offsetMin.x,
                    rectTransform.offsetMin.y + eventData.delta.y / canvas.scaleFactor
                );
            }
        }

        // Bottom
        else if (anchorIndex == 7)
        {
            if ((eventData.delta.y < 0 && eventData.position.y > minBounds.y)   // If the mouse is moving down && is above the bottom side of the bounds
                ||                                                              // OR
               (eventData.delta.y > 0 && rectTransform.rect.size.y > minSize.y))// If the mouse is moving up && is the height is above minimum
            {
                rectTransform.offsetMin = new Vector2
                (
                    rectTransform.offsetMin.x,
                    rectTransform.offsetMin.y + eventData.delta.y / canvas.scaleFactor
                );
            }
        }

        // Bottom Right
        else if (anchorIndex == 8)
        {
            if ((eventData.delta.x > 0 && eventData.position.x < maxBounds.x)   // If the mouse is moving right && is not past the right side of the bounds
                ||                                                              // OR
               (eventData.delta.x < 0 && rectTransform.rect.size.x > minSize.x))// If the mouse is moving left && the width is above minimum
            {
                rectTransform.offsetMax = new Vector2
                (
                    rectTransform.offsetMax.x + eventData.delta.x / canvas.scaleFactor,
                    rectTransform.offsetMax.y
                );
            }
            if ((eventData.delta.y < 0 && eventData.position.y > minBounds.y)   // If the mouse is moving down && is above the bottom side of the bounds
                ||                                                              // OR
               (eventData.delta.y > 0 && rectTransform.rect.size.y > minSize.y))// If the mouse is moving up && is the height is above minimum
            {
                rectTransform.offsetMin = new Vector2
                (
                    rectTransform.offsetMin.x,
                    rectTransform.offsetMin.y + eventData.delta.y / canvas.scaleFactor
                );
            }
        }
    }

    private void Awake()
    {
        if (canvas == null) // Auto Assign Canvas
        {
            Transform testCanvasTransform = transform.parent;

            // Repeatedly run through parents until there's either no parent above it, or if the Canvas is found
            while (testCanvasTransform != null)
            {
                canvas = testCanvasTransform.GetComponent<Canvas>();
                if (canvas != null)
                {
                    break;
                }
                testCanvasTransform = testCanvasTransform.parent;
            }
        }
    }
}
