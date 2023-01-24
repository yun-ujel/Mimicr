using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Messenger : MonoBehaviour
{
    [SerializeField] private GameObject textBubble;

    void InstantiateMessage(string newText)
    {
        GameObject messageObject = Instantiate(textBubble, transform);
        TextMeshProUGUI message = messageObject.GetComponentInChildren<TextMeshProUGUI>();
        RectTransform rectTransform = messageObject.GetComponent<RectTransform>();

        message.text = newText;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            InstantiateMessage("oooooooooooooooaaaaaaaaaaaaa");
        }
    }
}
