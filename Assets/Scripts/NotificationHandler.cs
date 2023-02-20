using UnityEngine;

public enum AssistantEmotion
{
    angry,
    concerned,
    crazy,
    happy,
    none
}

public class NotificationHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMessage[] messages;
    [SerializeField] private GameObject messageWindow;
    [SerializeField] private CanvasHandler canvasHandler;

    bool isWindowOpen;
    GameObject openWindow;

    [Header("Graphics")]
    [SerializeField] private Texture2D[] assistantEmotions;

    [Header("Notification Prefabs")]
    [SerializeField] private GameObject assistantNotification;

    [Header("Debug")]
    [SerializeField] private string messageText;
    [SerializeField] private AssistantEmotion assEmotion;


    private void Start()
    {
        SpawnWindow();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && !isWindowOpen)
        {
            SpawnWindow();
        }
     
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isWindowOpen)
            {
                openWindow.SendMessage("DisplayMessage", new TextMessage(messageText, true, assEmotion));
            }
            else
            {
                SendAssistantMessage(new TextMessage(messageText, true, assEmotion));
            }
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (isWindowOpen)
            {
                openWindow.SendMessage("DisplayMessage", new TextMessage(messageText, false, AssistantEmotion.none));
            }
            else
            {
                SendAssistantMessage(new TextMessage(messageText, false, AssistantEmotion.none));
            }
        }
    }

    public void GetMessageHistory(TextMessage[] arrayOfMessages)
    {
        messages = new TextMessage[arrayOfMessages.Length];
        messages = arrayOfMessages;
        isWindowOpen = false;
    }

    void SpawnWindow()
    {
        openWindow = Instantiate(messageWindow, transform.parent);

        openWindow.BroadcastMessage("OnWindowStart");
        openWindow.BroadcastMessage("OnColourUpdate", canvasHandler.palettes[canvasHandler.currentPalette]);
        openWindow.SendMessage("GetNotifications", this);

        if (messages != null)
        {
            for (int i = 0; i < messages.Length; i++)
            {
                openWindow.SendMessage("DisplayMessage", messages[i]);
            }
        }

        isWindowOpen = true;
    }

    public void SendAssistantMessage(TextMessage message)
    {
        GameObject notif = Instantiate(assistantNotification, transform);
        notif.SendMessage("DisplayMessage", message);
        notif.SendMessage("OnWindowStart");
    }
}
