using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AccountInfo
{
    public int PIN { get; set; }
    public Texture2D[] Posts { get; set; }

    public GameObject[] windows;
    public int CurrentWindowIndex { get; set; }

    public int CurrentPostIndex { get; set; }

    public bool isComplete;// { get; set; }

    public bool hasWindowOpen { get; set; }

    public int accountIndex { get; set; }

    public AccountInfo(int pIN, Texture2D[] posts, GameObject[] newWindows, int currentWindowIndex, int currentPostIndex, int newAccountIndex)
    {
        PIN = pIN;
        Posts = posts;
        CurrentWindowIndex = currentWindowIndex;
        CurrentPostIndex = currentPostIndex;
        accountIndex = newAccountIndex;

        windows = new GameObject[newWindows.Length];
        System.Array.Copy(newWindows, windows, newWindows.Length);

        isComplete = false;
        hasWindowOpen = false;
    }
}

public class AutoSpawning : MonoBehaviour
{
    [Header("Windows")]
    [SerializeField] private GameObject[] priorityWindows; // Windows that stay throughout the whole game, and can be closed/reopened

    [Header("Stack Windows")]
    [SerializeField] private GameObject[] defaultStackWindows; // Windows built for the "Stack", the main gameplay loop
    // These are specific minigames that will usually be more difficult, and will be placed in a specific order

    [SerializeField] private Texture2D[] postImages;
    [SerializeField] public List<AccountInfo> allAccounts = new List<AccountInfo>();
    [SerializeField] private List<AccountInfo> incompleteAccounts = new List<AccountInfo>();

    [Header("Pop Ups")]
    [SerializeField] private List<GameObject> popUpWindows = new List<GameObject>(); // Windows built for pop-ups, will spawn at random intervals
    // These should be less difficult to complete/close

    [SerializeField] private GameObject[] adWindows; // Pop-up ads, windows that can simply be clicked to close
    // These have a separate array so that when an ad is clicked, the pop-up spawn can also be an ad

    private int popUpsOpen;
    private float timeSinceLastPopUp;
    public float timeToNextPopUp;

    [Header("References")]
    [SerializeField] private RectTransform canvasRectTransform; // The RectTransform of the canvas. Used for random spawning.
    [SerializeField] private CanvasHandler canvasHandler; // The CanvasHandler. Used to reference palettes.
    
    [SerializeField] private PinGenerator pinGenerator;
    
    private void Start()
    {
        incompleteAccounts.Add(CreateNewAccount());
        incompleteAccounts.Add(CreateNewAccount());
        UpdateAccountView();
    }

    private AccountInfo CreateNewAccount()
    {
        int imagesCount = 0;
        for (int i = 0; i < defaultStackWindows.Length; i++)
        {
            if (!defaultStackWindows[i].TryGetComponent(out SliderMinigame sliM))
            {
                continue;
            }
            else
            {
                imagesCount += 1;
            }
        }

        List<Texture2D> potentialImages = new List<Texture2D>(postImages);

        int chosenImagesSize = Mathf.Min(imagesCount, potentialImages.Count);

        Texture2D[] chosenImages = new Texture2D[chosenImagesSize];

        for (int i = 0; i < chosenImagesSize; i++)
        {
            int selection = Random.Range(0, potentialImages.Count);
            
            chosenImages[i] = potentialImages[selection];
            potentialImages.Remove(potentialImages[selection]);
        }

        AccountInfo newAccount = new AccountInfo(pinGenerator.GeneratePin(), chosenImages, defaultStackWindows, 0, 0, allAccounts.Count);
        allAccounts.Add(newAccount);

        return newAccount;
    }


    void SpawnStack(AccountInfo accountInfo)
    {
        // Instantiate window as a child of this object
        GameObject newWindow = Instantiate(accountInfo.windows[accountInfo.CurrentWindowIndex], transform);

        // Set Stack Window Position to where the last Stack Window was
        RectTransform rT = newWindow.GetComponent<RectTransform>();
        rT.anchoredPosition = rT.ReanchorPosition(new Vector2
        (
            Random.Range(0f, canvasRectTransform.sizeDelta.x - rT.sizeDelta.x),
            Random.Range(0f, canvasRectTransform.sizeDelta.y - rT.sizeDelta.y)
        ));

        newWindow.name = accountInfo.windows[accountInfo.CurrentWindowIndex].name;

        newWindow.BroadcastMessage("OnWindowStart");
        newWindow.BroadcastMessage("OnStackStart", accountInfo);
        newWindow.BroadcastMessage("OnColourUpdate", canvasHandler.palettes[canvasHandler.currentPalette]);

        accountInfo.hasWindowOpen = true;
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
        newWindow.BroadcastMessage("OnColourUpdate", canvasHandler.palettes[canvasHandler.currentPalette]);

        popUpsOpen += 1;
    }

    public void SpawnAd()
    {
        int selection = Random.Range(0, adWindows.Length);
        GameObject newWindow = Instantiate(adWindows[selection], transform);

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

    void CompleteStackWindow(AccountInfo accountInfo)
    {
        accountInfo.hasWindowOpen = false;

        if (accountInfo.windows[accountInfo.CurrentWindowIndex].TryGetComponent(out SliderMinigame sliM))
        {
            accountInfo.CurrentPostIndex += 1;
        }

        accountInfo.CurrentWindowIndex += 1;

        if (accountInfo.CurrentWindowIndex >= accountInfo.windows.Length)
        {
            accountInfo.isComplete = true;
        }
    }

    private void Update()
    {
        if (incompleteAccounts.Count > 0)
        {
            for (int i = 0; i < incompleteAccounts.Count; i++)
            {
                if (incompleteAccounts[i].isComplete)
                {
                    incompleteAccounts.RemoveAt(i);
                    continue;
                }
                else if (StackSpawnViable(incompleteAccounts[i]))
                {
                    SpawnStack(incompleteAccounts[i]);
                }
            }
        }
        else
        {
            Debug.Log("All Accounts Complete");
        }

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

    private bool StackSpawnViable(AccountInfo accountInfo)
    {
        // return true if the account doesn't have a stack window open
        return !accountInfo.hasWindowOpen;
    }

    void UpdateAccountView()
    {
        GameObject[] viewers = GameObject.FindGameObjectsWithTag("PriorityWindow");
        for (int i = 0; i < viewers.Length; i++)
        {
            viewers[i].SendMessage("UpdateAccountView", allAccounts.ToArray());
        }
    }
}
