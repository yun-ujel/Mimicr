using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Messenger : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject leftTextBubble;
    [SerializeField] private GameObject rightTextBubble;
    [SerializeField] public RectTransform referenceTransformWidth;

    [Header("Text Options")]
    [SerializeField] private string messageText;
    [SerializeField] private Vector2 bottomRightPadding = new Vector2(25, 30);
    [SerializeField] private float fontSize = 28;
    [SerializeField] private float pixelsPerUnitMultiplier = 30;


    private void Awake()
    {
        if (referenceTransformWidth == null)
        {
            referenceTransformWidth = transform.Find("Width Reference").GetComponent<RectTransform>();
        }
    }

    void InstantiateMessage(string newText, GameObject gameObject)
    {
        GameObject messageObject = Instantiate(gameObject, transform);
        TextMeshProUGUI messageContent = messageObject.GetComponentInChildren<TextMeshProUGUI>();
        MessageResize messageResize = messageObject.GetComponentInChildren<MessageResize>();
    
        messageContent.text = messageText;
        messageResize.bottomRightPadding = bottomRightPadding;
        messageContent.fontSize = fontSize;
        messageObject.GetComponentInChildren<Image>().pixelsPerUnitMultiplier = pixelsPerUnitMultiplier;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            InstantiateMessage(messageText, leftTextBubble);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            InstantiateMessage(messageText, rightTextBubble);
        }
    }
}
