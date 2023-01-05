using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    public Vector3 screenPosition;
    public Vector3 worldPosition;
    public Camera orthographicCamera;
    public bool hideCursorOnStart;
    public float zPosition;
    void Start()
    {
        if (hideCursorOnStart)
        {
            Cursor.visible = false;
        }
    }

    void Update()
    {
        screenPosition = Input.mousePosition;
        //screenPosition.z = orthographicCamera.nearClipPlane + 1;
        screenPosition.z = zPosition;

        worldPosition = orthographicCamera.ScreenToWorldPoint(screenPosition);

        transform.position = worldPosition;
    }
}
