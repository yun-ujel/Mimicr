using TMPro;
using UnityEngine;

public class AgreeMinigame : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private RectTransform contentTransform;

    [Header("Sizes")]
    [SerializeField] private Vector2 fontSizeRange = new Vector2(24, 36); // (Min size, Max size)

    [SerializeField] private Vector2 contentSizeRange = new Vector2(560, 2400); // (Min size, Max size)

    void OnWindowStart()
    {
        text.fontSize = Random.Range(fontSizeRange.x, fontSizeRange.y); // Randomize size of font
        contentTransform.sizeDelta = new Vector2
        (
            Random.Range(contentSizeRange.x, contentSizeRange.y),
            contentTransform.sizeDelta.y
        );
        
    }
}
