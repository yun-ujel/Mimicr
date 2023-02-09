using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screenshake : MonoBehaviour
{
    private float shakeDuration;
    [SerializeField] private float shakeMagnitude = 0.7f;

    private float dampingSpeed = 1.0f;
    Vector3 initialPosition;

    private void Awake()
    {
        initialPosition = transform.localPosition;
    }

    private void Update()
    {
        Shake();
    }

    void Shake()
    {
        if (shakeDuration > 0f)
        {
            Vector2 initialPos2D = initialPosition;
            Vector2 shakePos2D = initialPos2D + Random.insideUnitCircle * (shakeMagnitude / 10);
            transform.localPosition = new Vector3(shakePos2D.x, shakePos2D.y, initialPosition.z);

            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            shakeDuration = 0f;
            transform.localPosition = initialPosition;
        }
    }

    public void TriggerScreenShake(float shakeLength)
    {
        shakeDuration = shakeLength;
    }
}
