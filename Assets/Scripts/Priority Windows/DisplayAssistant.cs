using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisplayAssistant : MonoBehaviour
{
    [Header("References")]
    [SerializeField] RawImage face;
    [SerializeField] TextMeshProUGUI uGUI;
    [SerializeField] RectTransform rectTransform;

    [Header("Graphics")]
    [SerializeField] private Texture2D[] assistantEmotions;
    bool isOpeningAnim;
    float targetHeight;
    float t;

    void DisplayMessage(TextMessage textMessage)
    {
        uGUI.text = string.Format("\"{0}\"", textMessage.text);
        face.texture = GetEmotionTexture(textMessage.emotion);
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

    private void OnWindowStart()
    {
        if (rectTransform == null)
        {
            rectTransform = GetComponent<RectTransform>();
        }
        targetHeight = rectTransform.rect.height;
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 0f);
        t = 0.0f;
        isOpeningAnim = true;
    }

    private void Update()
    {
        if (isOpeningAnim)
        {
            if (rectTransform.rect.height < targetHeight)
            {
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, Mathf.Lerp(rectTransform.sizeDelta.y, targetHeight, t));
            }
            else
            {
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, targetHeight);
                isOpeningAnim = false;
            }
        }
        t += 2 * Time.deltaTime;
    }
}
