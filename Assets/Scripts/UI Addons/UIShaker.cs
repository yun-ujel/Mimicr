using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class UIShaker : MonoBehaviour
{
    private RectTransform rectTransform;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        initialOffsetMax = rectTransform.offsetMax;
        initialOffsetMin = rectTransform.offsetMin;
    }

    private float shakeDuration = 0f;
    private float shakeMagnitude = 1.0f;

    Vector2 initialOffsetMin;
    Vector2 initialOffsetMax;

    bool isShaking;

    private void Update()
    {
        TryShake();
    }

    void TryShake()
    {
        if (shakeDuration > 0f && isShaking)
        {
            Vector2 randomOffset = Random.insideUnitCircle;

            rectTransform.offsetMax = initialOffsetMax + (randomOffset * shakeMagnitude);
            rectTransform.offsetMin = initialOffsetMin + (randomOffset * shakeMagnitude);

            shakeDuration -= Time.deltaTime;
        }
        else if (isShaking) // If duration is over but object hasn't reset from shaking state
        {
            shakeDuration = 0f;
            rectTransform.offsetMin = initialOffsetMin;
            rectTransform.offsetMax = initialOffsetMax;

            isShaking = false;
        }
        else
        {
            shakeDuration = 0f;
        }
    }

    public void TriggerShake(Vector2 durationMagnitude) // durationMagnitude stores duration as X and magnitude as Y
    {                                                   // These are stored in one parameter so SendMessage() can be used
        initialOffsetMin = rectTransform.offsetMin;
        initialOffsetMax = rectTransform.offsetMax;

        shakeDuration = durationMagnitude.x;
        shakeMagnitude = durationMagnitude.y;

        isShaking = true;
    }
}
