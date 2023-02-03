using UnityEngine;

public class AutoSpawning : MonoBehaviour
{
    [Header("Windows")]
    [SerializeField] private GameObject[] stackWindows; // Windows built for the "Stack", the main gameplay loop
    // These are specific minigames that will usually be more difficult, and will be placed in a specific order

    [SerializeField] private GameObject[] popUpWindows; // Windows built for pop-ups, will spawn at random intervals
    // These should be less difficult

    [SerializeField] private GameObject[] priorityWindows; // Windows that stay throughout the whole game, and can be closed/reopened

    [Header("References")]
    [SerializeField] RectTransform canvasRectTransform; // The RectTransform of the canvas. Used for random spawning.
    [SerializeField] CanvasHandler canvasHandler; // The CanvasHandler. Used to reference palettes.

    [Header("Rules")]
    bool stackWindowOpen;
    int popUpsOpen;

    float timeSinceLastPopUp;
    public float timeToNextPopUp;

    Vector2 lastStackPosition;

    void SpawnStack()
    {
        if (StackSpawnViable())
        {
            // Randomly select and Instantiate window as a child of this object
            // May be worth eventually changing selection to an index that counts up, so that stack moves in a specific order
            int selection = Random.Range(0, stackWindows.Length - 1);
            GameObject newWindow = Instantiate(stackWindows[selection], transform);
            RectTransform rT = newWindow.GetComponent<RectTransform>();

            rT.anchoredPosition = rT.ReanchorPosition(new Vector2
            (
                lastStackPosition.x,
                lastStackPosition.y
            ));

            newWindow.BroadcastMessage("OnWindowStart");
            newWindow.BroadcastMessage("OnColourUpdate", canvasHandler.palettes[canvasHandler.currentPalette]);

            stackWindowOpen = true;
        }
    }

    void CompleteStackWindow(GameObject stackWindow)
    {
        stackWindowOpen = false;

        lastStackPosition = stackWindow.GetComponent<RectTransform>().UnanchorPosition();
    }

    private void Update()
    {
        SpawnStack();

        if (timeSinceLastPopUp > timeToNextPopUp)
        {
            InstantiateWindow(popUpWindows);
            timeSinceLastPopUp = 0f;
        }
        else
        {
            timeSinceLastPopUp += Time.deltaTime;
        }
    }

    void InstantiateWindow(GameObject[] windowSelection)
    {
        int selection = Random.Range(0, windowSelection.Length - 1);

        GameObject newWindow = Instantiate(windowSelection[selection], transform);

        if (newWindow.GetComponent<StackHandler>() != null)
        {
            stackWindowOpen = true;
        }
        else if (newWindow.GetComponent<PopUpHandler>() != null)
        {
            popUpsOpen += 1;
        }
    }

    private bool StackSpawnViable()
    {
        if (stackWindowOpen == false)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
