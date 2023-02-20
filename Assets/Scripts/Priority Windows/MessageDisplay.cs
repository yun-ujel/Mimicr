using UnityEngine;

public class MessageDisplay : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject assistantMessage;
    [SerializeField] private RectTransform viewport;
    public RectTransform referenceTransformWidth;

    private void Awake()
    {
        if (referenceTransformWidth == null)
        {
            referenceTransformWidth = transform.Find("Width Reference").GetComponent<RectTransform>();
        }
    }

    public void DisplayMessage(NotificationInfo info)
    {
        GameObject messageObject = Instantiate(assistantMessage, viewport.transform);
        messageObject.SendMessage("DisplayNotification", info);
    }
}
