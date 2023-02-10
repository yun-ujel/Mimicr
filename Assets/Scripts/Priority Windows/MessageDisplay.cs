using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextMessage
{
    public string text;
    public bool isLeft;

    public TextMessage(string newText, bool isNewLeft)
    {
        text = newText;
        isLeft = isNewLeft;
    }
}

public class MessageDisplay : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject leftTextBubble;
    [SerializeField] private GameObject rightTextBubble;
    [SerializeField] public RectTransform referenceTransformWidth;
    [SerializeField] private RectTransform viewport;

    [Header("Text Options")]
    [SerializeField] private string messageText;
    private Vector2 bottomRightPadding = new Vector2(25, 30);
    private float fontSize = 28;
    private float pixelsPerUnitMultiplier = 30;

    List<TextMessage> messages = new List<TextMessage>();

    private Notifications notifs;

    private void Awake()
    {
        if (referenceTransformWidth == null)
        {
            referenceTransformWidth = transform.Find("Width Reference").GetComponent<RectTransform>();
        }
    }

    GameObject chosenSide;

    public void DisplayMessage(TextMessage textMessage)
    {
        if (textMessage.isLeft)
        {
            chosenSide = leftTextBubble;
        }
        else
        {
            chosenSide = rightTextBubble;
        }

        GameObject messageObject = Instantiate(chosenSide, viewport.transform);

        TextMeshProUGUI messageContent = messageObject.GetComponentInChildren<TextMeshProUGUI>();
        MessageResize messageResize = messageObject.GetComponentInChildren<MessageResize>();

        messageContent.text = textMessage.text;
        messageResize.bottomRightPadding = bottomRightPadding;
        messageContent.fontSize = fontSize;
        messageObject.GetComponentInChildren<Image>().pixelsPerUnitMultiplier = pixelsPerUnitMultiplier;

        messages.Add(textMessage);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DisplayMessage(new TextMessage(messageText, true));
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            DisplayMessage(new TextMessage(messageText, false));
        }
    }

    void GetNotifications(Notifications notifications)
    {
        notifs = notifications;
    }

    void OnWindowComplete()
    {
        notifs.SendMessage("GetMessageHistory", messages.ToArray());
    }
}
