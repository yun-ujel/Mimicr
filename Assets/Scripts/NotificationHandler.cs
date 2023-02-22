using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class NotificationHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] List<NotificationInfo> notificationsInfo = new List<NotificationInfo>();
    [SerializeField] private CanvasHandler canvasHandler;
    [SerializeField] private VerticalLayoutGroup verticalLayoutGroup;
    private List<GameObject> notifications = new List<GameObject>();

    [SerializeField] private MessageDisplay messagesWindow;

    [Header("Graphics")]
    [SerializeField] private Texture2D[] assistantEmotions;

    [Header("Notification Prefabs")]
    [SerializeField] private GameObject notificationPrefab;
    [SerializeField] private int maxNotifications = 7;

    [Header("Debug")]
    [SerializeField] private NotificationInfo debugNotification;

    private void Start()
    {
        
    }

    private void Update()
    {
     
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ReceiveNotification(debugNotification);
        }

        if (notifications.Count > maxNotifications)
        {
            notifications[0].SendMessage("StartFadeOut");
            notifications.RemoveAt(0);
        }
    }

    public void ReceiveNotification(NotificationInfo info)
    {
        GameObject notif = Instantiate(notificationPrefab, verticalLayoutGroup.transform);
        notificationsInfo.Add(info);
        notifications.Add(notif);
        notif.SendMessage("DisplayNotification", info);


        messagesWindow.SendMessage("DisplayMessage", info, SendMessageOptions.DontRequireReceiver);
        
    }
}
