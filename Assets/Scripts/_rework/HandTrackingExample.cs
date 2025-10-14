using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Example script showing how to use the improved hand tracking system
/// This demonstrates various ways to access and use hand landmark data
/// </summary>
public class HandTrackingExample : MonoBehaviour
{
    // Switch conversion of landmarks using this boolean
    private readonly bool isMobile = true;

    [Header("Hand Tracking")]
    public HTS_Improved handTracker;

    [Header("Visualization")]
    public GameObject[] landmarkSpheres; // 21 spheres for hand landmarks
    public bool showLandmarkSpheres = true;

    [Header("Gesture Detection")]
    public float pinchThreshold = 0.05f;
    public float pointThreshold = 0.02f;

    [Header("Playground")]
    public GameObject cube;
    public bool useHandRotation = true;

    [Header("Smoothing")]
    public float positionSmoothing = 5f;
    public float scaleSmoothing = 5f;
    public float rotationSmoothing = 5f;

    private List<Vector3> worldPositions;

    // Smoothing variables
    private Vector3 targetPosition;
    private Vector3 targetScale;
    private Quaternion targetRotation;
    private bool isInitialized = false;

    void Start()
    {
        if (handTracker == null)
        {
            handTracker = FindFirstObjectByType<HTS_Improved>();
        }

        if (handTracker == null)
        {
            Debug.LogError("[HandTrackingExample] HTS_Improved not found!");
            return;
        }

        if (cube == null)
        {
            Debug.LogError("Cube for playground is null");
            return;
        }
        // else
        // {
        //     cube = Instantiate(cube);
        // }

        // Create landmark spheres if they don't exist
        if (landmarkSpheres == null || landmarkSpheres.Length == 0)
        {
            CreateLandmarkSpheres();
        }
    }

    void Update()
    {
        worldPositions = handTracker.GetLatestWorldPositions();

        if (handTracker == null)
            return;

        // Check if we have valid landmarks
        if (handTracker.HasValidLandmarks())
        {
            UpdateLandmarkVisualization();
            DetectGestures();
            PlaceCube();
        }
        else
        {
            HideLandmarkSpheres();
        }
    }

    void PlaceCube()
    {
        // Get in world point position
        Vector3 thumbPos = GetScreenToWorldPoint(
            worldPositions[HTS_Improved.HandLandmarkIndices.THUMB_TIP]
        );
        Vector3 pinkyPos = GetScreenToWorldPoint(
            worldPositions[HTS_Improved.HandLandmarkIndices.PINKY_TIP]
        );

        // Calculate distance between thumb and pinky
        float thumbPinkyDistance = Vector3.Distance(thumbPos, pinkyPos);

        // Calculate target scale based on the distance (clamp to avoid too small/large)
        float minScale = 0.1f;
        float maxScale = 10.0f;
        float scale = Mathf.Clamp(thumbPinkyDistance * 0.1f, minScale, maxScale); // 0.01f is a scaling factor, adjust as needed
        targetScale = new Vector3(scale, scale, scale);

        // Calculate target position (center between thumb and pinky)
        Vector3 centerPos = (thumbPos + pinkyPos) / 2f;
        centerPos.z = 50;
        targetPosition = centerPos;

        // Calculate hand rotation and apply to cube (if enabled)
        if (useHandRotation)
        {
            targetRotation = CalculateHandRotation();
        }

        // Initialize targets on first frame
        if (!isInitialized)
        {
            cube.transform.localScale = targetScale;
            cube.transform.position = targetPosition;
            if (useHandRotation)
            {
                cube.transform.rotation = targetRotation;
            }
            isInitialized = true;
        }
        else
        {
            // Smooth interpolation for position and scale
            float deltaTime = Time.deltaTime;
            cube.transform.position = Vector3.Lerp(
                cube.transform.position,
                targetPosition,
                positionSmoothing * deltaTime
            );
            cube.transform.localScale = Vector3.Lerp(
                cube.transform.localScale,
                targetScale,
                scaleSmoothing * deltaTime
            );

            // Apply rotation only if hand rotation is enabled
            if (useHandRotation)
            {
                cube.transform.rotation = Quaternion.Lerp(
                    cube.transform.rotation,
                    targetRotation,
                    rotationSmoothing * deltaTime
                );
            }
        }
    }

    Quaternion CalculateHandRotation()
    {
        if (worldPositions == null || worldPositions.Count < 21)
        {
            return Quaternion.identity;
        }

        // Get key landmarks for palm orientation calculation
        Vector3 wrist = GetScreenToWorldPoint(
            worldPositions[HTS_Improved.HandLandmarkIndices.WRIST]
        );
        Vector3 middleFingerMcp = GetScreenToWorldPoint(
            worldPositions[HTS_Improved.HandLandmarkIndices.MIDDLE_FINGER_MCP]
        );
        Vector3 thumbTip = GetScreenToWorldPoint(
            worldPositions[HTS_Improved.HandLandmarkIndices.THUMB_TIP]
        );
        Vector3 pinkyTip = GetScreenToWorldPoint(
            worldPositions[HTS_Improved.HandLandmarkIndices.PINKY_TIP]
        );

        // Calculate palm normal using cross product of palm vectors
        // Vector from wrist to middle finger MCP (palm center direction)
        Vector3 palmCenter = (middleFingerMcp - wrist).normalized;

        // Vector from thumb to pinky (palm width direction)
        Vector3 palmWidth = (pinkyTip - thumbTip).normalized;

        // Cross product to get palm normal (perpendicular to palm surface)
        Vector3 palmNormal = Vector3.Cross(palmCenter, palmWidth).normalized;

        // Ensure the normal points towards the camera (positive Z direction)
        if (palmNormal.z < 0)
        {
            palmNormal = -palmNormal;
        }

        // Calculate palm up direction (corrected palm center)
        Vector3 palmUp = Vector3.Cross(palmWidth, palmNormal).normalized;

        // Create rotation from palm orientation
        // The cube will align its Y-axis with the palm up direction and Z-axis with palm normal
        Quaternion palmRotation = Quaternion.LookRotation(palmNormal, palmUp);

        // Optional: Add rotation around the palm normal based on finger orientation
        // This gives more detailed rotation based on how the fingers are oriented
        Vector3 indexTip = GetScreenToWorldPoint(
            worldPositions[HTS_Improved.HandLandmarkIndices.INDEX_FINGER_TIP]
        );
        Vector3 indexMcp = GetScreenToWorldPoint(
            worldPositions[HTS_Improved.HandLandmarkIndices.INDEX_FINGER_MCP]
        );

        // Calculate finger direction in palm space
        Vector3 fingerDirection = (indexTip - indexMcp).normalized;
        Vector3 palmSpaceFinger = Quaternion.Inverse(palmRotation) * fingerDirection;

        // Calculate roll rotation around palm normal
        float rollAngle = Mathf.Atan2(palmSpaceFinger.y, palmSpaceFinger.x) * Mathf.Rad2Deg;
        Quaternion rollRotation = Quaternion.AngleAxis(rollAngle, Vector3.forward);

        // Combine palm rotation with finger roll
        return palmRotation * rollRotation;
    }

    void CreateLandmarkSpheres()
    {
        landmarkSpheres = new GameObject[21];

        for (int i = 0; i < 21; i++)
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.name = $"Landmark_{i}";
            sphere.transform.localScale = Vector3.one * 3f; // Small spheres
            sphere.GetComponent<Renderer>().material.color = GetLandmarkColor(i);

            // Remove collider to avoid physics interference
            Destroy(sphere.GetComponent<Collider>());

            landmarkSpheres[i] = sphere;
        }
    }

    Color GetLandmarkColor(int index)
    {
        // Color code different parts of the hand
        if (index == HTS_Improved.HandLandmarkIndices.WRIST)
            return Color.white;
        else if (index >= 1 && index <= 4) // Thumb
            return Color.red;
        else if (index >= 5 && index <= 8) // Index finger
            return Color.green;
        else if (index >= 9 && index <= 12) // Middle finger
            return Color.blue;
        else if (index >= 13 && index <= 16) // Ring finger
            return Color.yellow;
        else if (index >= 17 && index <= 20) // Pinky
            return Color.magenta;
        else
            return Color.gray;
    }

    void UpdateLandmarkVisualization()
    {
        if (!showLandmarkSpheres || landmarkSpheres == null)
            return;

        for (int i = 0; i < landmarkSpheres.Length && i < worldPositions.Count; i++)
        {
            Vector3 worldPosition = GetScreenToWorldPoint(worldPositions[i]);

            landmarkSpheres[i].transform.position = worldPosition;
            landmarkSpheres[i].SetActive(true);
        }

        // Hide unused spheres
        for (int i = worldPositions.Count; i < landmarkSpheres.Length; i++)
        {
            landmarkSpheres[i].SetActive(false);
        }
    }

    private Vector3 GetScreenToWorldPoint(Vector3 landmark)
    {
        float vx,
            vy;

        if (isMobile)
        {
            // Mobile
            vx = 1f - landmark.y;
            vy = 1f - landmark.x;
        }
        else
        {
            // Desktop
            vx = landmark.x;
            vy = 1f - landmark.y;
        }

        Vector3 stwpoint = Camera.main.ViewportToWorldPoint(new Vector3(vx, vy, 100f));

        return stwpoint;
    }

    void HideLandmarkSpheres()
    {
        if (landmarkSpheres == null)
            return;

        foreach (var sphere in landmarkSpheres)
        {
            if (sphere != null)
                sphere.SetActive(false);
        }
    }

    void DetectGestures()
    {
        // Example gesture detection

        // Pinch gesture (thumb and index finger)
        if (DetectPinchGesture())
        {
            Debug.Log("[HandTrackingExample] Pinch gesture detected!");
            OnPinchDetected();
        }

        // Point gesture (index finger extended, others curled)
        if (DetectPointGesture())
        {
            Debug.Log("[HandTrackingExample] Point gesture detected!");
            OnPointDetected();
        }
    }

    bool DetectPinchGesture()
    {
        var worldPos = handTracker.GetLatestWorldPositions();
        if (worldPos.Count < 21)
            return false;

        Vector3 thumbTip = worldPos[HTS_Improved.HandLandmarkIndices.THUMB_TIP];
        Vector3 indexTip = worldPos[HTS_Improved.HandLandmarkIndices.INDEX_FINGER_TIP];

        float distance = Vector3.Distance(thumbTip, indexTip);
        return distance < pinchThreshold;
    }

    bool DetectPointGesture()
    {
        var worldPos = handTracker.GetLatestWorldPositions();
        if (worldPos.Count < 21)
            return false;

        // Check if index finger is extended (tip is further from palm than PIP)
        Vector3 indexTip = worldPos[HTS_Improved.HandLandmarkIndices.INDEX_FINGER_TIP];
        Vector3 indexPip = worldPos[HTS_Improved.HandLandmarkIndices.INDEX_FINGER_PIP];
        Vector3 wrist = worldPos[HTS_Improved.HandLandmarkIndices.WRIST];

        float indexExtension =
            Vector3.Distance(indexTip, wrist) - Vector3.Distance(indexPip, wrist);

        // Check if other fingers are curled (tips closer to palm than PIPs)
        bool middleCurled = IsFingerCurled(
            worldPos,
            HTS_Improved.HandLandmarkIndices.MIDDLE_FINGER_TIP,
            HTS_Improved.HandLandmarkIndices.MIDDLE_FINGER_PIP,
            wrist
        );
        bool ringCurled = IsFingerCurled(
            worldPos,
            HTS_Improved.HandLandmarkIndices.RING_FINGER_TIP,
            HTS_Improved.HandLandmarkIndices.RING_FINGER_PIP,
            wrist
        );
        bool pinkyCurled = IsFingerCurled(
            worldPos,
            HTS_Improved.HandLandmarkIndices.PINKY_TIP,
            HTS_Improved.HandLandmarkIndices.PINKY_PIP,
            wrist
        );

        return indexExtension > pointThreshold && middleCurled && ringCurled && pinkyCurled;
    }

    bool IsFingerCurled(List<Vector3> worldPos, int tipIndex, int pipIndex, Vector3 wrist)
    {
        float tipDistance = Vector3.Distance(worldPos[tipIndex], wrist);
        float pipDistance = Vector3.Distance(worldPos[pipIndex], wrist);
        return tipDistance < pipDistance; // Curled if tip is closer to wrist than PIP
    }

    void OnPinchDetected()
    {
        // Handle pinch gesture
        // Example: Grab/select objects, zoom, etc.

        // Get the pinch point (midpoint between thumb and index)
        var worldPos = handTracker.GetLatestWorldPositions();
        Vector3 thumbTip = worldPos[HTS_Improved.HandLandmarkIndices.THUMB_TIP];
        Vector3 indexTip = worldPos[HTS_Improved.HandLandmarkIndices.INDEX_FINGER_TIP];
        Vector3 pinchPoint = (thumbTip + indexTip) * 0.5f;

        Debug.Log($"[HandTrackingExample] Pinch at position: {pinchPoint}");
    }

    void OnPointDetected()
    {
        // Handle pointing gesture
        // Example: UI interaction, object selection, etc.

        var worldPos = handTracker.GetLatestWorldPositions();
        Vector3 indexTip = worldPos[HTS_Improved.HandLandmarkIndices.INDEX_FINGER_TIP];

        Debug.Log($"[HandTrackingExample] Pointing at position: {indexTip}");
    }

    // Public methods for other scripts to access gesture detection
    public bool IsPinching()
    {
        return handTracker != null && handTracker.HasValidLandmarks() && DetectPinchGesture();
    }

    public bool IsPointing()
    {
        return handTracker != null && handTracker.HasValidLandmarks() && DetectPointGesture();
    }

    public Vector3 GetPointingDirection()
    {
        if (handTracker == null || !handTracker.HasValidLandmarks())
            return Vector3.zero;

        var worldPos = handTracker.GetLatestWorldPositions();
        if (worldPos.Count < 21)
            return Vector3.zero;

        Vector3 indexTip = worldPos[HTS_Improved.HandLandmarkIndices.INDEX_FINGER_TIP];
        Vector3 indexPip = worldPos[HTS_Improved.HandLandmarkIndices.INDEX_FINGER_PIP];

        return (indexTip - indexPip).normalized;
    }
}
