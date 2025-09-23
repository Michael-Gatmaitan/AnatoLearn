using UnityEngine;

public class CameraTouchControl : MonoBehaviour
{
    public Transform target;
    public float rotateSpeed = 0.2f;
    public float zoomSpeed = 0.15f;
    public float minZoom = 2f;
    public float maxZoom = 0f;

    private Camera cam;
    private Vector3 lastMousePos;

    private float initialDistance;

    private void Start()
    {
        cam = Camera.main;
        initialDistance = Vector3.Distance(transform.position, target.position);

        // Allow zooming out to the original distance
        maxZoom = initialDistance * 1f;
        Debug.Log("MAXZOOM: " + maxZoom);
    }

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouse();
#endif
        HandleTouch();
    }

    void HandleMouse()
    {
        // Rotate like Unity Scene View
        if (Input.GetMouseButtonDown(0))
            lastMousePos = Input.mousePosition;

        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastMousePos;
            lastMousePos = Input.mousePosition;

            // Unity Scene View-like camera movement
            transform.RotateAround(target.position, transform.right, -delta.y * rotateSpeed); // vertical drag = top/bottom POV
            transform.RotateAround(target.position, Vector3.up, delta.x * rotateSpeed); // horizontal drag = left/right POV
        }

        // float scroll = Input.GetAxis("Mouse ScrollWheel");
        float scroll = Input.GetAxisRaw("Mouse ScrollWheel");
        Debug.Log("Raw scroll input: " + scroll);

        if (scroll != 0f)
            Zoom(scroll * 1000 * zoomSpeed);
    }

    void HandleTouch()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                Vector2 delta = touch.deltaPosition;

                transform.RotateAround(target.position, transform.right, -delta.y * rotateSpeed);
                transform.RotateAround(target.position, Vector3.up, delta.x * rotateSpeed);
            }
        }
        else if (Input.touchCount == 2)
        {
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);

            Vector2 prevT0 = t0.position - t0.deltaPosition;
            Vector2 prevT1 = t1.position - t1.deltaPosition;

            float prevDistance = (prevT0 - prevT1).magnitude;
            float currentDistance = (t0.position - t1.position).magnitude;

            float diff = currentDistance - prevDistance;
            float distance = Vector3.Distance(transform.position, target.position);

            // ZOOM
            Vector3 zoomCenter = GetWorldPointFromTouchMidpoint();
            ZoomTowardsPoint(-diff * zoomSpeed * distance * 0.02f, zoomCenter);

            // PANNING (not rotating)
            Vector2 avgDelta = (t0.deltaPosition + t1.deltaPosition) * 0.5f;

            // Convert screen delta to world movement
            Vector3 right = cam.transform.right;
            Vector3 up = cam.transform.up;

            // Adjust sensitivity (lower is slower)
            float panSpeed = 0.005f;
            Vector3 panMovement = (-right * avgDelta.x - up * avgDelta.y) * panSpeed;

            transform.position += panMovement;
            target.position += panMovement; // Keep rotation center in sync
        }
    }

    void Zoom(float increment)
    {
        Vector3 direction = (transform.position - target.position).normalized;
        float distance = Vector3.Distance(transform.position, target.position);

        float newDistance = Mathf.Clamp(distance + increment, minZoom, maxZoom);
        transform.position = target.position + direction * newDistance;
    }

    void ZoomTowardsPoint(float increment, Vector3 zoomPoint)
    {
        Vector3 direction = (transform.position - zoomPoint).normalized;
        float distance = Vector3.Distance(transform.position, zoomPoint);

        float newDistance = Mathf.Clamp(distance + increment, minZoom, maxZoom);
        transform.position = zoomPoint + direction * newDistance;
    }

    Vector3 GetWorldPointFromTouchMidpoint()
    {
        Vector2 screenMid = (Input.GetTouch(0).position + Input.GetTouch(1).position) * 0.5f;
        Ray ray = cam.ScreenPointToRay(screenMid);

        // You can raycast to a plane or just pick a point in front of the camera
        Plane plane = new Plane(-cam.transform.forward, target.position); // or use Vector3.up for ground
        if (plane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }

        return target.position;
    }
}
