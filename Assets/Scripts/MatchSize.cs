using UnityEngine;

public class MatchSize : MonoBehaviour
{
    RectTransform rectTransform;
    Vector2 actualSize;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        CalculateActualSize();
        Debug.Log(actualSize);

        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, actualSize.x);
    }

    private void CalculateActualSize()
    {
        RectTransform parentTransform = rectTransform.parent.GetComponent<RectTransform>();

        actualSize = new Vector2
        (
            (parentTransform.sizeDelta.x * Mathf.Abs(rectTransform.anchorMax.x - rectTransform.anchorMin.x)) + (rectTransform.offsetMax.x - rectTransform.offsetMin.x),
            (parentTransform.sizeDelta.y * Mathf.Abs(rectTransform.anchorMax.y - rectTransform.anchorMin.y)) + (rectTransform.offsetMax.y - rectTransform.offsetMin.y)
        );
    }
}
