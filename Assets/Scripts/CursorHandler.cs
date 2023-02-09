using UnityEngine;

public class CursorHandler : MonoBehaviour
{
    private Vector3 screenPosition;
    private Vector3 worldPosition;
    [SerializeField] private Camera cursorCamera;
    [SerializeField] private bool hideCursorOnStart;
    [SerializeField] private float zPosition;
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

        worldPosition = cursorCamera.ScreenToWorldPoint(screenPosition);

        transform.position = worldPosition;
    }
}
