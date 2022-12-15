using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    public Vector3 screenPosition;
    public Vector3 worldPosition;
    public Camera orthographicCamera;
    void Start()
    {
        
    }

    void Update()
    {
        screenPosition = Input.mousePosition;
        screenPosition.z = orthographicCamera.nearClipPlane + 1;

        worldPosition = orthographicCamera.ScreenToWorldPoint(screenPosition);

        transform.position = worldPosition;
    }
}
