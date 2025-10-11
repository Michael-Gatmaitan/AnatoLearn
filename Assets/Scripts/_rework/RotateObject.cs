using UnityEngine;

public class RotateObject : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField]
    private GameObject targetObject;

    [SerializeField]
    private float rotationSpeed = 50f;

    [SerializeField]
    private bool invertX = false;

    [SerializeField]
    private bool invertY = false;

    [Header("Touch Settings")]
    [SerializeField]
    private float minTouchDistance = 0.1f;

    [SerializeField]
    private bool allowRotationOnX = true;

    [SerializeField]
    private bool allowRotationOnY = true;

    private Vector2 lastTouchPosition;
    private bool isRotating = false;
    private Camera mainCamera;

    void Start()
    {
        // If no target object is assigned, use this GameObject
        if (targetObject == null)
        {
            targetObject = gameObject;
        }

        // Get the main camera for touch-to-world conversion
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            mainCamera = FindObjectOfType<Camera>();
        }
    }

    void Update()
    {
        HandleTouchInput();
    }

    void HandleTouchInput()
    {
        // Handle touch input for mobile devices
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    lastTouchPosition = touch.position;
                    isRotating = true;
                    break;

                case TouchPhase.Moved:
                    if (isRotating)
                    {
                        Vector2 touchDelta = touch.position - lastTouchPosition;

                        // Check if touch movement is significant enough
                        if (touchDelta.magnitude > minTouchDistance)
                        {
                            RotateTarget(touchDelta);
                            lastTouchPosition = touch.position;
                        }
                    }
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    isRotating = false;
                    break;
            }
        }
        // Handle mouse input for testing in editor
        else if (Application.isEditor)
        {
            HandleMouseInput();
        }
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastTouchPosition = Input.mousePosition;
            isRotating = true;
        }
        else if (Input.GetMouseButton(0) && isRotating)
        {
            Vector2 mouseDelta = (Vector2)Input.mousePosition - lastTouchPosition;

            if (mouseDelta.magnitude > minTouchDistance)
            {
                RotateTarget(mouseDelta);
                lastTouchPosition = Input.mousePosition;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isRotating = false;
        }
    }

    void RotateTarget(Vector2 deltaPosition)
    {
        if (targetObject == null)
            return;

        // Calculate rotation amounts
        float rotationX = 0f;
        float rotationY = 0f;

        // Apply rotation based on settings
        if (allowRotationOnY)
        {
            rotationY = deltaPosition.x * rotationSpeed * Time.deltaTime;
            if (invertY)
                rotationY = -rotationY;
        }

        if (allowRotationOnX)
        {
            rotationX = -deltaPosition.y * rotationSpeed * Time.deltaTime;
            if (invertX)
                rotationX = -rotationX;
        }

        // Apply rotation to the target object
        targetObject.transform.Rotate(rotationX, rotationY, 0, Space.World);
    }

    // Public method to set target object at runtime
    public void SetTargetObject(GameObject newTarget)
    {
        targetObject = newTarget;
    }

    // Public method to enable/disable rotation
    public void SetRotationEnabled(bool enabled)
    {
        enabled = enabled;
    }

    // Public method to adjust rotation speed
    public void SetRotationSpeed(float speed)
    {
        rotationSpeed = speed;
    }

    // Public method to reset rotation
    public void ResetRotation()
    {
        if (targetObject != null)
        {
            targetObject.transform.rotation = Quaternion.identity;
        }
    }

    void OnValidate()
    {
        // Ensure rotation speed is positive
        rotationSpeed = Mathf.Max(0.1f, rotationSpeed);

        // Ensure minimum touch distance is positive
        minTouchDistance = Mathf.Max(0.01f, minTouchDistance);
    }
}
