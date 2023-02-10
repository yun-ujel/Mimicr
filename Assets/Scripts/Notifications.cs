using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notifications : MonoBehaviour
{
    [SerializeField] private TextMessage[] messages;
    [SerializeField] private GameObject messageWindow;

    bool isWindowOpen;

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
    }

    public void GetMessageHistory(TextMessage[] arrayOfMessages)
    {
        messages = new TextMessage[arrayOfMessages.Length];
        messages = arrayOfMessages;
        isWindowOpen = false;
    }

    void SpawnWindow()
    {
        GameObject newWindow = Instantiate(messageWindow, transform.parent);

        newWindow.SendMessage("GetNotifications", this);

        if (messages != null)
        {
            for (int i = 0; i < messages.Length; i++)
            {
                newWindow.SendMessage("DisplayMessage", messages[i]);
            }
        }

        isWindowOpen = true;
    }

    void OnMessageReceived()
    {

    }
}
