using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessageResize : MonoBehaviour
{
    private RectTransform rectTransform;
    private TextMeshProUGUI TMPMesh;
    private RectTransform textMeshTransform;
    private RectTransform referenceTransformWidth;
    private Vector2 bottomRightPadding = new Vector2(30f, 30f);
    
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        TMPMesh = GetComponentInChildren<TextMeshProUGUI>();
        textMeshTransform = TMPMesh.GetComponent<RectTransform>();

        MessageDisplay parentMessenger = GetComponentInParent<MessageDisplay>();
        referenceTransformWidth = parentMessenger.referenceTransformWidth;
    }

    private void Start()
    {
        UpdateSize();
    }

    void LateUpdate()
    {
        bottomRightPadding = new Vector2(textMeshTransform.anchorMin.x + TMPMesh.fontSize, textMeshTransform.anchorMax.y + TMPMesh.fontSize);

        UpdateSize();
    }

    void UpdateSize()
    {
        float width = referenceTransformWidth.rect.width - rectTransform.anchoredPosition.x;

        rectTransform.sizeDelta = new Vector2
        (
            Mathf.Min(TMPMesh.preferredWidth + bottomRightPadding.x, width),
            TMPMesh.preferredHeight + bottomRightPadding.y
        );
    }
}
