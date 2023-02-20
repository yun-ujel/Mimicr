using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class NotificationInfo
{
    public string titleText;
    public string contentText;
    public bool fromAssistant;
    public Texture2D backIconTexture;
    public Texture2D frontIconTexture;

    public NotificationInfo(string titleText, string contentText, bool fromAssistant, Texture2D backIconTexture, Texture2D frontIconTexture)
    {
        this.titleText = titleText;
        this.contentText = contentText;
        this.fromAssistant = fromAssistant;
        this.backIconTexture = backIconTexture;
        this.frontIconTexture = frontIconTexture;
    }
}

[RequireComponent(typeof(CanvasGroup))]
public class NotificationDisplay : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI content;
    [SerializeField] private RawImage behindIcon;
    [SerializeField] private RawImage frontIcon;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Opening Animation")]
    [SerializeField] private bool useOpeningAnimation;
    [SerializeField] private RectTransform rectTransform;
    bool isOpeningAnim;
    float targetHeight;
    float t;

    public void DisplayNotification(NotificationInfo info)
    {
        if (title != null)
            title.text = info.titleText;

        if (title != null && info.fromAssistant)
        {
            content.text = string.Format("\"{0}\"", info.contentText);
        }
        else
        {
            content.text = info.contentText;
        }

        if (behindIcon != null)
            behindIcon.texture = info.backIconTexture;

        if (info.fromAssistant)
            frontIcon.texture = info.frontIconTexture;
        else
            frontIcon.color = new Color(0f, 0f, 0f, 0f);

        TriggerOpenAnimation();
    }

    private void TriggerOpenAnimation()
    {
        if (rectTransform != null && useOpeningAnimation)
        {
            targetHeight = rectTransform.rect.height;
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, 0f);
            t = 0.0f;
            canvasGroup.alpha = 0f;
            isOpeningAnim = true;
        }
    }

    private void Update()
    {
        if (isOpeningAnim)
        {
            if (canvasGroup.alpha < 1f)
            {
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, Mathf.Lerp(rectTransform.sizeDelta.y, targetHeight, t));
                canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1f, t);
                Debug.Log(canvasGroup.alpha);
            }
            else
            {
                canvasGroup.alpha = 1f;
                rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, targetHeight);
                isOpeningAnim = false;
            }
        }
        t += 2 * Time.deltaTime;
    }
}
