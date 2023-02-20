using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveInertiaAndCollision : DragFunction, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private RectTransform rectTransform;
    private RectTransform boundsRectTransform;
    private float canvasScaleFactor;

    private Vector2 minPosition; // The position of the object from the bottom left
    private Vector2 maxPosition; // The position of the object from the top right

    private Vector2 minDistance; // The distance between the object and the bottom left of the bounds (negative)
    private Vector2 maxDistance; // The distance between the object and the top right of the bounds   (positive)

    Padding padding; // Padding for the parent bounds to slightly adjust their size

    bool gotInfo = false;

    private Vector2 velocity;
    [SerializeField] private float drag = 4;
    List<Vector2> eventDataDeltas = new List<Vector2>();
    int maxDeltas = 3;

    public override void GetInfo(UIMaster uIMaster)
    {
        rectTransform = uIMaster.rectTransform;
        canvasScaleFactor = uIMaster.canvas.scaleFactor;

        boundsRectTransform = rectTransform.parent.GetComponent<RectTransform>();
        padding = uIMaster.padding;
        
        gotInfo = true;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        UpdateDeltas(eventData.delta);
        Move(eventData.delta / canvasScaleFactor);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        UpdateDeltas(eventData.delta);

        velocity = Vector2.zero;
        for (int i = 0; i < eventDataDeltas.Count; i++)
        {
            velocity += eventDataDeltas[i];
        }
        eventDataDeltas.Clear();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        velocity = Vector2.zero;
    }

    private void FixedUpdate()
    {
        if (gotInfo)
        {
            velocity *= 1 - (Time.deltaTime * drag);
            Move(velocity);

            for (int axis = 0; axis < 2; axis++)
            {
                if (Mathf.Abs(velocity[axis]) < 0.4f)
                {
                    velocity[axis] = 0f;
                }
            }
        }
    }

    private void UpdateDeltas(Vector2 delta)
    {
        if (eventDataDeltas.Count < maxDeltas)
        {
            eventDataDeltas.Add(delta);
        }
        else
        {
            eventDataDeltas.RemoveAt(0);
            eventDataDeltas.Add(delta);
        }
    }

    void Move(Vector2 vector)
    {
        CalculateBounds();
        CalculateDistance();

        Vector2 anchoredPos = rectTransform.anchoredPosition;
        for (int axis = 0; axis < 2; axis++)
        {
            // Upward or Right Movement
            if (vector[axis] > 0 && maxPosition[axis] < boundsRectTransform.rect.size[axis])
            {
                anchoredPos[axis] += Mathf.Min(vector[axis], maxDistance[axis]);
            }
            // Downward or Left Movement
            else if (vector[axis] < 0 && minPosition[axis] > 0)
            {
                anchoredPos[axis] += Mathf.Max(vector[axis], minDistance[axis]);
            }
        }
        rectTransform.anchoredPosition = anchoredPos;
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

    private void SetDrag(float newDrag)
    {
        drag = newDrag;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        UpdateDeltas(eventData.delta);
    }
}
