using System.Collections.Generic;
using UnityEngine;

public class AutoSpawning : MonoBehaviour
{
    [Header("Windows")]
    [SerializeField] private GameObject[] stackWindows; // Windows built for the "Stack", the main gameplay loop
    // These are specific minigames that will usually be more difficult, and will be placed in a specific order

    [SerializeField] private List<GameObject> popUpWindows = new List<GameObject>(); // Windows built for pop-ups, will spawn at random intervals
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

            // Set Stack Window Position to where the last Stack Window was
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

    public void SpawnPopUp()
    {
        int selection = Random.Range(0, popUpWindows.Count);
        GameObject newWindow = Instantiate(popUpWindows[selection], transform);

        // Randomize position of the object
        RectTransform rT = newWindow.GetComponent<RectTransform>();
        Vector2 randomizedPosition = rT.ReanchorPosition(new Vector2
        (
            Random.Range(0f, canvasRectTransform.sizeDelta.x - rT.sizeDelta.x),
            Random.Range(0f, canvasRectTransform.sizeDelta.y - rT.sizeDelta.y)
        ));

        rT.anchoredPosition = randomizedPosition;

        newWindow.BroadcastMessage("OnWindowStart");
        newWindow.SendMessage("TriggerShake", new Vector2(0.25f, 10f), SendMessageOptions.DontRequireReceiver);
        newWindow.BroadcastMessage("OnColourUpdate", canvasHandler.palettes[canvasHandler.currentPalette]);

        popUpsOpen += 1;
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
            SpawnPopUp();
            timeSinceLastPopUp = 0f;
        }
        else
        {
            timeSinceLastPopUp += Time.deltaTime;
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
