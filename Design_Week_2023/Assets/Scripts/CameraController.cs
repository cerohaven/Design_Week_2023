using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    public float zoomOutFactor = 1.5f;

    private float startingZoom;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
        startingZoom = cam.orthographicSize;
    }

    void LateUpdate()
    {
        Vector3 desiredPosition = target.position + offset;

        // Zoom out when player transforms
        if (target.GetComponent<Player_Behaviour>().playerTransformed)
        {
            cam.orthographicSize = startingZoom * zoomOutFactor;
        }
        else
        {
            cam.orthographicSize = startingZoom;
        }

        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, -10f);
    }
}