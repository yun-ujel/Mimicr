using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveInertia : DragFunction, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private RectTransform rectTransform;
    private float canvasScaleFactor;

    private Vector2 velocity;
    [SerializeField] private float drag = 10;
    List<Vector2> eventDataDeltas = new List<Vector2>();
    int maxDeltas = 3;

    public override void GetInfo(UIMaster uIMaster)
    {
        rectTransform = uIMaster.rectTransform;
        canvasScaleFactor = uIMaster.canvas.scaleFactor;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        UpdateDeltas(eventData.delta);
        rectTransform.anchoredPosition += eventData.delta / canvasScaleFactor;
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

    public override void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        velocity = Vector2.zero;
    }

    private void FixedUpdate()
    {
        velocity *= 1 - (Time.deltaTime * drag);
        rectTransform.anchoredPosition += velocity;

        for (int axis = 0; axis < 2; axis++)
        {
            if (Mathf.Abs(velocity[axis]) < 0.4f)
            {
                velocity[axis] = 0f;
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

    private void SetDrag(float newDrag)
    {
        drag = newDrag;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        UpdateDeltas(eventData.delta);
    }
}
