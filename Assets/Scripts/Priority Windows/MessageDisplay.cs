using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextMessage
{
    public string text;
    public bool isLeft;
    public AssistantEmotion emotion;
    public TextMessage(string newText, bool isNewLeft, AssistantEmotion emote)
    {
        text = newText;
        isLeft = isNewLeft;
        emotion = emote;
    }
}

public class MessageDisplay : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject leftTextBubble;
    [SerializeField] private GameObject rightTextBubble;
    [SerializeField] public RectTransform referenceTransformWidth;
    [SerializeField] private RectTransform viewport;

    [Header("Graphics")]
    [SerializeField] private Texture2D[] assistantEmotions;

    private float fontSize = 28;
    private float pixelsPerUnitMultiplier = 30;

    List<TextMessage> messages = new List<TextMessage>();

    private NotificationHandler notificationsHandler;

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

        messageContent.text = textMessage.text;
        messageContent.fontSize = fontSize;
        messageObject.GetComponentInChildren<Image>().pixelsPerUnitMultiplier = pixelsPerUnitMultiplier;

        if (textMessage.emotion != AssistantEmotion.none)
        {
            messageObject.transform.Find("Circle").Find("Face").GetComponent<RawImage>().texture = GetEmotionTexture(textMessage.emotion);
        }

        messages.Add(textMessage);
    }

    void GetNotifications(NotificationHandler notifications)
    {
        notificationsHandler = notifications;
    }

    void OnWindowComplete()
    {
        notificationsHandler.SendMessage("GetMessageHistory", messages.ToArray());
    }

    Texture2D GetEmotionTexture(AssistantEmotion emote)
    {
        if (emote == AssistantEmotion.angry)
        {
            return assistantEmotions[0];
        }
        else if (emote == AssistantEmotion.concerned)
        {
            return assistantEmotions[1];
        }
        else if (emote == AssistantEmotion.crazy)
        {
            return assistantEmotions[2];
        }
        else // emote == happy
        {
            return assistantEmotions[3];
        }
    }
}
