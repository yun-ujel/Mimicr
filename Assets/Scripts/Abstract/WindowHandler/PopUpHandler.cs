using UnityEngine;

// PopUpHandler should be a newer form of MinigameHandler built as a base for windows in the Stack.
public class PopUpHandler : WindowHandler
{
    [Header("References")]
    private Canvas canvas;

    public override void Awake()
    {
        base.Awake();
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
    }

    public override void OnWindowFail()
    {
        base.OnWindowFail();
        canvas.SendMessage("AcceptMonetPolicy");
    }
}