using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AgreeMinigame : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI text;
    private RectTransform textBody;
    [SerializeField] private RectTransform fullWindow;

    [Header("Sizes")]
    [SerializeField] private Vector2 fontSizeRange = new Vector2(24, 36);

    [SerializeField] private Vector2 minBodySize = new Vector2(600, 400);
    [SerializeField] private Vector2 maxBodySize = new Vector2(1400, 1000);

    [SerializeField] private Vector2 minWindowSize = new Vector2(400, 200);
    [SerializeField] private Vector2 maxWindowSize = new Vector2(600, 400);
    void Awake()
    {
        textBody = text.GetComponent<RectTransform>();
    }
    void OnWindowStart()
    {
        Debug.Log("Spawned Agree Window");
        text.fontSize = Random.Range(fontSizeRange.x, fontSizeRange.y);

        textBody.sizeDelta = new Vector2
        (
            Random.Range(minBodySize.x, maxBodySize.x),
            Random.Range(minBodySize.y, maxBodySize.y)
        );

        fullWindow.sizeDelta = new Vector2
        (
            Random.Range(minWindowSize.x, maxWindowSize.x),
            Random.Range(minWindowSize.y, maxWindowSize.y)
        );
    }
}
