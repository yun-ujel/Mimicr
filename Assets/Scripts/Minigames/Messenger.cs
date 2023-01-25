using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Messenger : MonoBehaviour
{
    [SerializeField] private GameObject leftTextBubble;
    [SerializeField] private GameObject rightTextBubble;

    [SerializeField] private string messageText;
    void InstantiateMessage(string newText, GameObject gameObject)
    {
        GameObject messageObject = Instantiate(gameObject, transform);
        TextMeshProUGUI message = messageObject.GetComponentInChildren<TextMeshProUGUI>();
        HorizontalLayoutGroup horizontalLayoutGroup = messageObject.GetComponentInChildren<HorizontalLayoutGroup>();
        Rect rect = messageObject.GetComponent<RectTransform>().rect;

        message.text = newText;
        Debug.Log(message.preferredWidth);
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
