using TMPro;
using UnityEngine;

public class AgreeMinigame : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private RectTransform textBody;

    [Header("Sizes")]
    [SerializeField] private Vector2 fontSizeRange = new Vector2(24, 36);

    [SerializeField] private Vector2 minBodySize = new Vector2(600, 400);
    [SerializeField] private Vector2 maxBodySize = new Vector2(1400, 1000);

    private void Awake()
    {

    }

    private void Update()
    {
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
    }
}
