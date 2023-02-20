using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class NotificationHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] List<NotificationInfo> notifications = new List<NotificationInfo>();
    [SerializeField] private GameObject messageWindow;
    [SerializeField] private CanvasHandler canvasHandler;
    [SerializeField] private VerticalLayoutGroup verticalLayoutGroup;

    bool isWindowOpen;
    GameObject openWindow;

    [Header("Graphics")]
    [SerializeField] private Texture2D[] assistantEmotions;

    [Header("Notification Prefabs")]
    [SerializeField] private GameObject notificationPrefab;

    [Header("Debug")]
    [SerializeField] private NotificationInfo debugNotification;

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
            ReceiveNotification(debugNotification);
        }
    }

    void SpawnWindow()
    {
        openWindow = Instantiate(messageWindow, transform.parent);

        openWindow.BroadcastMessage("OnWindowStart");
        openWindow.BroadcastMessage("OnColourUpdate", canvasHandler.palettes[canvasHandler.currentPalette]);

        if (notifications != null)
        {
            for (int i = 0; i < notifications.Count; i++)
            {
                if (!notifications[i].fromAssistant)
                    continue;
                else
                    openWindow.SendMessage("DisplayMessage", notifications[i]);
            }
        }

        isWindowOpen = true;
    }

    public void ReceiveNotification(NotificationInfo info)
    {
        GameObject notif = Instantiate(notificationPrefab, verticalLayoutGroup.transform);
        notifications.Add(info);
        notif.SendMessage("DisplayNotification", info);

        if (isWindowOpen)
        {
            openWindow.SendMessage("DisplayMessage", info);
        }
    }
}
