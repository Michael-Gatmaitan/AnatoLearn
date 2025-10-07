using System.Collections.Generic;
using System.Reflection;
using Mediapipe.Tasks.Components.Containers;
using Mediapipe.Tasks.Vision.HandLandmarker;
using Mediapipe.Unity;
using Mediapipe.Unity.Sample.HandLandmarkDetection;
using UnityEngine;

public class HTS : MonoBehaviour
{
    [Header("Hand Landmark Detection")]
    public HandLandmarkerRunner handLandmarkerRunner;

    [Header("Settings")]
    public bool useEventBasedApproach = true;
    public bool debugLogging = true;

    // Thread-safe queue for event-based approach
    private readonly Queue<List<NormalizedLandmark>> landmarkQueue =
        new Queue<List<NormalizedLandmark>>();
    private readonly object queueLock = new object();

    // Latest processed landmarks
    private List<NormalizedLandmark> latestLandmarks = new List<NormalizedLandmark>();

    // Reflection-based approach (fallback)
    private FieldInfo currentTargetField;
    private HandLandmarkerResultAnnotationController annotationController;

    void Start()
    {
        if (handLandmarkerRunner == null)
        {
            handLandmarkerRunner = FindFirstObjectByType<HandLandmarkerRunner>();
        }

        if (handLandmarkerRunner == null)
        {
            Debug.LogError(
                "[HTS] HandLandmarkerRunner not found. Please assign it in the inspector."
            );
            return;
        }

        if (useEventBasedApproach)
        {
            SetupEventBasedApproach();
        }
        else
        {
            SetupReflectionApproach();
        }
    }

    void SetupEventBasedApproach()
    {
        // This approach requires modifying the HandLandmarkerRunner to expose events
        // For now, we'll use the reflection approach as fallback
        Debug.Log("[HTS] Event-based approach not yet implemented. Using reflection approach.");
        SetupReflectionApproach();
    }

    void SetupReflectionApproach()
    {
        // Get the annotation controller
        annotationController =
            handLandmarkerRunner.GetComponent<HandLandmarkerResultAnnotationController>();

        if (annotationController == null)
        {
            Debug.LogError("[HTS] Could not find HandLandmarkerResultAnnotationController");
            return;
        }

        // Try to find the current target field
        currentTargetField = typeof(HandLandmarkerResultAnnotationController).GetField(
            "_currentTarget",
            BindingFlags.NonPublic | BindingFlags.Instance
        );

        if (currentTargetField == null)
        {
            Debug.LogError("[HTS] Could not find _currentTarget field");
            return;
        }

        Debug.Log("[HTS] Reflection approach setup complete");
    }

    void Update()
    {
        if (!useEventBasedApproach)
        {
            UpdateReflectionApproach();
        }
        else
        {
            UpdateEventBasedApproach();
        }
    }

    void UpdateReflectionApproach()
    {
        if (annotationController == null || currentTargetField == null)
            return;

        try
        {
            var resultValue = currentTargetField.GetValue(annotationController);
            var result = (HandLandmarkerResult)resultValue;

            if (resultValue == null)
            {
                if (debugLogging)
                    Debug.Log("[HTS] No hand landmark result available");
                return;
            }

            if (result.handLandmarks == null || result.handLandmarks.Count == 0)
            {
                if (debugLogging)
                    Debug.Log("[HTS] No hand landmarks detected");
                return;
            }

            if (
                result.handLandmarks[0].landmarks == null
                || result.handLandmarks[0].landmarks.Count == 0
            )
            {
                if (debugLogging)
                    Debug.Log("[HTS] No landmarks in first hand");
                return;
            }

            latestLandmarks = result.handLandmarks[0].landmarks;

            if (debugLogging)
            {
                Debug.Log(
                    $"[HTS] Got {latestLandmarks.Count} landmarks. First landmark: {latestLandmarks[0].x}, {latestLandmarks[0].y}, {latestLandmarks[0].z}"
                );
            }

            // Process landmarks here
            ProcessLandmarks(latestLandmarks);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"[HTS] Error in reflection approach: {e.Message}");
        }
    }

    void UpdateEventBasedApproach()
    {
        // Process queued landmarks on main thread
        List<NormalizedLandmark> landmarks = null;
        lock (queueLock)
        {
            if (landmarkQueue.Count > 0)
            {
                landmarks = landmarkQueue.Dequeue();
            }
        }

        if (landmarks != null)
        {
            latestLandmarks = landmarks;
            ProcessLandmarks(latestLandmarks);
        }
    }

    void ProcessLandmarks(List<NormalizedLandmark> landmarks)
    {
        if (landmarks == null || landmarks.Count == 0)
            return;

        // Convert normalized landmarks to world positions
        var worldLandmarks = ConvertToWorldPositions(landmarks);

        // Your landmark processing logic here
        // For example:
        // - Detect gestures
        // - Update UI elements
        // - Control game objects

        if (debugLogging)
        {
            Debug.Log($"[HTS] Processing {worldLandmarks.Count} world landmarks");
        }
    }

    List<Vector3> ConvertToWorldPositions(List<NormalizedLandmark> normalizedLandmarks)
    {
        var worldPositions = new List<Vector3>();

        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogWarning("[HTS] No Camera.main found for coordinate conversion");
            return worldPositions;
        }

        foreach (var landmark in normalizedLandmarks)
        {
            // MediaPipe normalized coordinates: x: left->right (0..1), y: top->bottom (0..1)
            // For ViewportToWorldPoint we need y from bottom->top, so use (1 - y)
            float vx = landmark.x;
            float vy = 1f - landmark.y;
            float vz = Mathf.Abs(landmark.z); // Use absolute z value

            // Choose a suitable depth - you can adjust this or use landmark.z appropriately
            float distance = 1.0f; // 1 meter in front of camera
            Vector3 worldPos = cam.ViewportToWorldPoint(new Vector3(vx, vy, distance));
            worldPositions.Add(worldPos);
        }

        return worldPositions;
    }

    // Public methods to access landmark data
    public List<NormalizedLandmark> GetLatestLandmarks()
    {
        return latestLandmarks;
    }

    public List<Vector3> GetLatestWorldPositions()
    {
        if (latestLandmarks == null || latestLandmarks.Count == 0)
            return new List<Vector3>();

        return ConvertToWorldPositions(latestLandmarks);
    }

    public bool HasValidLandmarks()
    {
        return latestLandmarks != null && latestLandmarks.Count > 0;
    }
}

// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Reflection;
// using Mediapipe.Tasks.Vision.HandLandmarker;
// using Mediapipe.Unity.Sample.HandLandmarkDetection; // HandLandmarkerRunner
// using UnityEngine;

// public class HTS_EventBased : MonoBehaviour
// {
//     [Tooltip("Assign the HandLandmarkerRunner from the scene (or leave empty to auto-find).")]
//     public HandLandmarkerRunner handLandmarkerRunner;

//     // Thread-safe queue -> we'll copy landmarks on callback then process on main thread
//     private readonly Queue<List<Vector3>> _landmarkQueue = new Queue<List<Vector3>>();
//     private readonly object _queueLock = new object();

//     // Latest processed landmarks (world positions) accessible on main thread
//     private List<Vector3> latestWorldLandmarks = new List<Vector3>();

//     void OnEnable()
//     {
//         if (handLandmarkerRunner == null)
//         {
//             handLandmarkerRunner = FindFirstObjectByType<HandLandmarkerRunner>();
//         }

//         if (handLandmarkerRunner == null)
//         {
//             Debug.LogError(
//                 "[HTS_EventBased] HandLandmarkerRunner not found in scene. Assign it in inspector."
//             );
//             return;
//         }

//         // Subscribe to the public UnityAction<HandLandmarkerResult>
//         handLandmarkerRunner.ProcessHandLandmark += OnProcessHandLandmark;
//         Debug.Log(
//             "[HTS_EventBased] Subscribed to ProcessHandLandmark. Make sure runner.RunningMode is LIVE_STREAM."
//         );
//     }

//     void OnDisable()
//     {
//         if (handLandmarkerRunner != null)
//         {
//             handLandmarkerRunner.ProcessHandLandmark -= OnProcessHandLandmark;
//         }
//     }

//     // This callback **may** be called on a non-main thread, so keep it minimal and thread-safe.
//     private void OnProcessHandLandmark(HandLandmarkerResult result)
//     {
//         if (result == null)
//             return;

//         try
//         {
//             // Try common property name "handLandmarks" (fallbacks included).
//             var resultType = result.GetType();

//             // foreach (handLandmarksProp)
//             var handLandmarksProp =
//                 resultType.GetProperty("handLandmarks") ?? resultType.GetProperty("HandLandmarks");
//             object handLandmarksObj =
//                 handLandmarksProp != null ? handLandmarksProp.GetValue(result) : null;

//             if (handLandmarksObj == null)
//             {
//                 // nothing to enqueue
//                 return;
//             }

//             var handsList = handLandmarksObj as System.Collections.IList;
//             if (handsList == null || handsList.Count == 0)
//                 return;

//             // We'll only copy the first detected hand here. Modify if you want multiple hands.
//             var firstHand = handsList[0];

//             // Get the 'landmarks' member on the first hand
//             var firstHandType = firstHand.GetType();
//             var landmarksProp =
//                 firstHandType.GetProperty("landmarks") ?? firstHandType.GetProperty("Landmarks");
//             object landmarksObj = landmarksProp != null ? landmarksProp.GetValue(firstHand) : null;
//             var landmarksList = landmarksObj as System.Collections.IList;
//             if (landmarksList == null || landmarksList.Count == 0)
//                 return;

//             // Copy normalized positions into a plain List<Vector3> (x,y,z) - thread safe to enqueue
//             var copy = new List<Vector3>(landmarksList.Count);
//             for (int i = 0; i < landmarksList.Count; i++)
//             {
//                 var lm = landmarksList[i];
//                 float x = GetFloatMember(lm, "x");
//                 float y = GetFloatMember(lm, "y");
//                 float z = GetFloatMember(lm, "z");
//                 copy.Add(new Vector3(x, y, z)); // normalized coords (x,y in [0..1], z is relative depth)
//             }

//             lock (_queueLock)
//             {
//                 _landmarkQueue.Enqueue(copy);
//             }
//         }
//         catch (Exception e)
//         {
//             Debug.LogException(e);
//         }
//     }

//     void Update()
//     {
//         // Process queued landmark copies on the main thread (safe to touch Unity objects)
//         List<Vector3> copy = null;
//         lock (_queueLock)
//         {
//             if (_landmarkQueue.Count > 0)
//             {
//                 copy = _landmarkQueue.Dequeue();
//             }
//         }

//         if (copy != null)
//         {
//             // convert normalized (x, y, z) to Unity world positions
//             latestWorldLandmarks.Clear();

//             Camera cam = Camera.main;
//             if (cam == null)
//             {
//                 Debug.LogWarning("[HTS_EventBased] No Camera.main found.");
//                 return;
//             }

//             for (int i = 0; i < copy.Count; i++)
//             {
//                 // MediaPipe normalized coordinates: x: left->right (0..1), y: top->bottom (0..1)
//                 // For ViewportToWorldPoint we need y from bottom->top, so use (1 - y).
//                 var normalized = copy[i];
//                 float vx = normalized.x;
//                 float vy = 1f - normalized.y;
//                 float vz = Mathf.Abs(normalized.z); // z may be negative; interpret depth as positive

//                 // Choose a suitable depth to convert viewport -> world.
//                 // If you want to use the model-reported z, you may scale it appropriately. Here we use a fixed distance.
//                 float distance = 1.0f; // 1 meter in front of camera; tweak this or use vz appropriately
//                 Vector3 worldPos = cam.ViewportToWorldPoint(new Vector3(vx, vy, distance));
//                 latestWorldLandmarks.Add(worldPos);
//             }

//             // For debugging:
//             Debug.Log(
//                 $"[HTS_EventBased] Received {latestWorldLandmarks.Count} landmarks. First (world): {(latestWorldLandmarks.Count > 0 ? latestWorldLandmarks[0].ToString("F3") : "N/A")}"
//             );

//             // TODO: Use latestWorldLandmarks to move spheres, detect gestures, etc.
//             // Example: move a debug sphere to wrist (landmark index 0) if you created one.
//         }
//     }

//     // Helper: safely read numeric member (x/y/z) by property or field (case-insensitive)
//     private float GetFloatMember(object obj, string name)
//     {
//         if (obj == null)
//             return 0f;
//         var t = obj.GetType();

//         // property
//         var p = t.GetProperty(name) ?? t.GetProperty(char.ToUpper(name[0]) + name.Substring(1));
//         if (p != null)
//         {
//             var val = p.GetValue(obj);
//             if (val != null)
//                 return Convert.ToSingle(val);
//         }

//         // field fallback
//         var f = t.GetField(name) ?? t.GetField(char.ToUpper(name[0]) + name.Substring(1));
//         if (f != null)
//         {
//             var val = f.GetValue(obj);
//             if (val != null)
//                 return Convert.ToSingle(val);
//         }

//         return 0f;
//     }
// }
