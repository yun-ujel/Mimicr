using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MessageResize : MonoBehaviour
{
    private RectTransform rectTransform;
    private TextMeshProUGUI TMPMesh;
    private RectTransform referenceTransformWidth;
    [SerializeField] private Vector2 bottomRightPadding = new Vector2(20f, 20f);
    
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        TMPMesh = GetComponentInChildren<TextMeshProUGUI>();

        Messenger parentMessenger = GetComponentInParent<Messenger>();
        referenceTransformWidth = parentMessenger.referenceTransformWidth;
    }

    void LateUpdate()
    {
        rectTransform.sizeDelta = new Vector2
        (
            Mathf.Min(TMPMesh.preferredWidth + bottomRightPadding.x, referenceTransformWidth.rect.width),
            TMPMesh.preferredHeight + bottomRightPadding.y
        );
    }
}
