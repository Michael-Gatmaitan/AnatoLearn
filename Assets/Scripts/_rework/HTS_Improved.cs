using System;
using System.Collections.Generic;
using Mediapipe.Tasks.Components.Containers;
using Mediapipe.Tasks.Vision.HandLandmarker;
using Mediapipe.Unity.Sample.HandLandmarkDetection;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Improved Hand Tracking Script that uses event-based approach for reliable hand landmark detection
/// This script subscribes to hand landmark events from CustomHandLandmarkerRunner
/// </summary>
public class HTS_Improved : MonoBehaviour
{
    [Header("Hand Landmark Detection")]
    public CustomHandLandmarkerRunner handLandmarkerRunner;

    [Header("Settings")]
    public bool debugLogging = true;
    public float coordinateConversionDistance = 1.0f; // Distance from camera for world position conversion

    [Header("UI Document")]
    public UIDocument uiDocument;
    private Label L_HandInstruction;

    // Latest processed landmarks
    private List<NormalizedLandmark> latestLandmarks = new List<NormalizedLandmark>();
    private List<Vector3> latestWorldPositions = new();

    private Camera cam;

    // Thread-safe access
    private readonly object landmarkLock = new();
    private bool handDetected = false;

    void OnEnable()
    {
        VisualElement root = uiDocument.rootVisualElement;
        L_HandInstruction = root.Q<Label>("L_HandInstruction");
    }

    void Start()
    {
        cam = Camera.main;
        if (handLandmarkerRunner == null)
        {
            handLandmarkerRunner = FindFirstObjectByType<CustomHandLandmarkerRunner>();
        }

        if (handLandmarkerRunner == null)
        {
            Debug.LogError(
                "[HTS_Improved] CustomHandLandmarkerRunner not found. Please assign it in the inspector or add it to the scene."
            );
            return;
        }

        // Subscribe to hand landmark events
        handLandmarkerRunner.OnHandLandmarkDetected += OnHandLandmarkDetected;
        handLandmarkerRunner.OnHandLandmarkDetectedAsync += OnHandLandmarkDetectedAsync;

        ShowHandInstruction();
        Debug.Log("[HTS_Improved] Successfully subscribed to hand landmark events");
    }

    void OnDestroy()
    {
        // Unsubscribe from events to prevent memory leaks
        if (handLandmarkerRunner != null)
        {
            handLandmarkerRunner.OnHandLandmarkDetected -= OnHandLandmarkDetected;
            handLandmarkerRunner.OnHandLandmarkDetectedAsync -= OnHandLandmarkDetectedAsync;
        }
    }

    void OnHandLandmarkDetected(HandLandmarkerResult result)
    {
        ProcessHandLandmarkResult(result);
    }

    void OnHandLandmarkDetectedAsync(
        HandLandmarkerResult result,
        Mediapipe.Image img,
        long timestamp
    )
    {
        ProcessHandLandmarkResult(result);
    }

    void ProcessHandLandmarkResult(HandLandmarkerResult result)
    {
        if (result.handLandmarks == null || result.handLandmarks.Count == 0)
        {
            handDetected = false;
            if (debugLogging)
                Debug.Log("[HTS_Improved] No hand landmarks detected");
            // ShowHandInstruction();
            return;
        }
        else
        {
            handDetected = true;
        }
        // else
        // {
        //     HideHandInstruction();
        // }

        // Get landmarks from the first detected hand
        var firstHandLandmarks = result.handLandmarks[0].landmarks;

        if (firstHandLandmarks == null || firstHandLandmarks.Count == 0)
        {
            if (debugLogging)
                Debug.Log("[HTS_Improved] No landmarks in first detected hand");
            return;
        }

        // Thread-safe update of landmark data
        lock (landmarkLock)
        {
            latestLandmarks = firstHandLandmarks;
            latestWorldPositions = ConvertToWorldPositions(firstHandLandmarks);
        }

        if (debugLogging)
        {
            Debug.Log($"[HTS_Improved] Processed {latestLandmarks.Count} hand landmarks");
            if (latestLandmarks.Count > 0)
            {
                var firstLandmark = latestLandmarks[0];
                Debug.Log(
                    $"[HTS_Improved] First landmark: ({firstLandmark.x:F3}, {firstLandmark.y:F3}, {firstLandmark.z:F3})"
                );
            }
        }

        // Process the landmarks
        ProcessLandmarks(latestLandmarks, latestWorldPositions);
    }

    // Hiding and showing instruction
    void ShowHandInstruction()
    {
        if (L_HandInstruction != null)
        {
            L_HandInstruction.style.display = DisplayStyle.Flex;
        }
    }

    void HideHandInstruction()
    {
        if (L_HandInstruction != null)
        {
            L_HandInstruction.style.display = DisplayStyle.None;
        }
    }

    List<Vector3> ConvertToWorldPositions(List<NormalizedLandmark> normalizedLandmarks)
    {
        var worldPositions = new List<Vector3>();

        if (cam == null)
        {
            Debug.LogWarning("[HTS_Improved] No Camera.main found for coordinate conversion");
            return worldPositions;
        }

        foreach (var landmark in normalizedLandmarks)
        {
            // MediaPipe normalized coordinates: x: left->right (0..1), y: top->bottom (0..1)
            // For ViewportToWorldPoint we need y from bottom->top, so use (1 - y)

            // Convert to world position
            // Vector3 worldPos = cam.ViewportToWorldPoint(
            //     new Vector3(vx, vy, coordinateConversionDistance)
            // );

            // Vector3 worldPosition = cam.ScreenToWorldPoint(screenPoint);
            // Debug.Log(worldPosition);
            // worldPositions.Add(worldPosition);

            // float width = Screen.width;
            // float height = Screen.height;

            // // Desktop
            // float vx = landmark.x * width;
            // float vy = (1f - landmark.y) * height;

            // Mobile
            // vx = (1 - landmark.y) * width;
            // vy = (1f - landmark.x) * height;
            Vector3 screenPoint = new(landmark.x, landmark.y, landmark.z);
            worldPositions.Add(screenPoint);
        }

        return worldPositions;
    }

    void ProcessLandmarks(
        List<NormalizedLandmark> normalizedLandmarks,
        List<Vector3> worldPositions
    )
    {
        if (normalizedLandmarks == null || normalizedLandmarks.Count == 0)
            return;

        // Your landmark processing logic here
        // Examples:
        // - Detect specific hand gestures
        // - Update UI elements based on hand position
        // - Control game objects
        // - Perform hand tracking for AR/VR applications

        // Example: Log specific landmark positions
        if (debugLogging && normalizedLandmarks.Count >= 21) // Hand has 21 landmarks
        {
            // Wrist (landmark 0)
            var wrist = worldPositions[0];
            Debug.Log($"[HTS_Improved] Wrist position: {wrist}");

            // Index finger tip (landmark 8)
            var indexTip = worldPositions[8];
            Debug.Log($"[HTS_Improved] Index finger tip: {indexTip}");
        }
    }

    // Public methods to access landmark data from other scripts
    public List<NormalizedLandmark> GetLatestLandmarks()
    {
        lock (landmarkLock)
        {
            return latestLandmarks;
        }
    }

    public List<Vector3> GetLatestWorldPositions()
    {
        lock (landmarkLock)
        {
            return latestWorldPositions;
        }
    }

    public bool HasValidLandmarks()
    {
        lock (landmarkLock)
        {
            return latestLandmarks != null && latestLandmarks.Count > 0;
        }
    }

    public int GetLandmarkCount()
    {
        lock (landmarkLock)
        {
            return latestLandmarks?.Count ?? 0;
        }
    }

    // Helper method to get specific landmark
    public Vector3 GetLandmarkWorldPosition(int landmarkIndex)
    {
        lock (landmarkLock)
        {
            if (
                latestWorldPositions != null
                && landmarkIndex >= 0
                && landmarkIndex < latestWorldPositions.Count
            )
            {
                return latestWorldPositions[landmarkIndex];
            }
            return Vector3.zero;
        }
    }

    public NormalizedLandmark GetLandmarkNormalized(int landmarkIndex)
    {
        lock (landmarkLock)
        {
            if (
                latestLandmarks != null
                && landmarkIndex >= 0
                && landmarkIndex < latestLandmarks.Count
            )
            {
                return latestLandmarks[landmarkIndex];
            }

            return default;
        }
    }

    // Common hand landmark indices for easy access
    public static class HandLandmarkIndices
    {
        public const int WRIST = 0;
        public const int THUMB_CMC = 1;
        public const int THUMB_MCP = 2;
        public const int THUMB_IP = 3;
        public const int THUMB_TIP = 4;
        public const int INDEX_FINGER_MCP = 5;
        public const int INDEX_FINGER_PIP = 6;
        public const int INDEX_FINGER_DIP = 7;
        public const int INDEX_FINGER_TIP = 8;
        public const int MIDDLE_FINGER_MCP = 9;
        public const int MIDDLE_FINGER_PIP = 10;
        public const int MIDDLE_FINGER_DIP = 11;
        public const int MIDDLE_FINGER_TIP = 12;
        public const int RING_FINGER_MCP = 13;
        public const int RING_FINGER_PIP = 14;
        public const int RING_FINGER_DIP = 15;
        public const int RING_FINGER_TIP = 16;
        public const int PINKY_MCP = 17;
        public const int PINKY_PIP = 18;
        public const int PINKY_DIP = 19;
        public const int PINKY_TIP = 20;
    }

    void Update()
    {
        ShowHandInstruction();
        // if (handDetected)
        // {
        // ShowHandInstruction();
        // }
        // else
        // {
        // HideHandInstruction();
        // }
    }
}
