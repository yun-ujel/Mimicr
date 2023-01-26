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

    [Header("Text")]
    [SerializeField] private string messageText;

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
        TextMeshProUGUI message = messageObject.GetComponentInChildren<TextMeshProUGUI>();
        
        message.text = newText;
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
