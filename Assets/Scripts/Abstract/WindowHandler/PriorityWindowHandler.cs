using UnityEngine;

public class PriorityWindowHandler : WindowHandler
{
    [Header("References")]
    private Canvas canvas;

    public override void Awake()
    {
        base.Awake();
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
    }
}
