using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
// WindowHandler should be a newer form of MinigameHandler built as a base for all window types.
public abstract class WindowHandler : MonoBehaviour
{
    public abstract void OnWindowStart();

    public abstract void OnWindowComplete();

    public abstract void OnWindowFail();
}
