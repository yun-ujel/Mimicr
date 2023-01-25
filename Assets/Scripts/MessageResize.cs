using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessageResize : MonoBehaviour
{
    private RectTransform rectTransform;
    private TextMeshProUGUI TMPMesh;
    [SerializeField] private RectTransform topSpacingTransform;
    [SerializeField] private Vector2 bottomRightPadding = new Vector2(20f, 20f);
    
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        TMPMesh = GetComponentInChildren<TextMeshProUGUI>();
    }

    void Update()
    {
        rectTransform.sizeDelta = new Vector2
        (
            Mathf.Min(TMPMesh.preferredWidth + bottomRightPadding.x, topSpacingTransform.rect.width),
            TMPMesh.preferredHeight + bottomRightPadding.y
        );
    }
}
