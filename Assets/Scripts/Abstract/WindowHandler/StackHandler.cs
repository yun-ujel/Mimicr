using UnityEngine;
using UnityEngine.UI;
using TMPro;

// StackHandler should be a newer form of MinigameHandler built as a base for windows in the Stack.
public class StackHandler : WindowHandler
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI header;
    private Canvas canvas;
    private AccountInfo accountInfo;


    public override void Awake()
    {
        base.Awake();
        minRandomWindowSize = GetComponent<UIMaster>().minWindowSize; // Auto-set the minimum window size to the size set on UIMaster
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
    }
    public override void OnWindowComplete()
    {
        base.OnWindowComplete();
        canvas.SendMessage("CompleteStackWindow", accountInfo);
    }

    public override void OnWindowFail()
    {
        base.OnWindowFail();
        canvas.SendMessage("FailStackWindow", accountInfo);
    }

    void OnStackStart(AccountInfo inAccountInfo)
    {
        inAccountInfo.windows[inAccountInfo.CurrentWindowIndex] = gameObject;
        accountInfo = inAccountInfo;
        header.text = gameObject.name + " - " + "Account #" + (accountInfo.AccountIndex + 1).ToString();
    }
}
