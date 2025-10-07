# Hand Tracking System - Improved Implementation

This directory contains an improved hand tracking system for MediaPipe Unity that provides reliable access to hand landmark data without using fragile reflection techniques.

## Files Overview

### 1. `HTS.cs` (Original - Fixed)

The original script with improved error handling and better structure. This still uses reflection but with proper error handling.

### 2. `CustomHandLandmarkerRunner.cs` (Recommended)

A custom implementation of HandLandmarkerRunner that exposes events for hand landmark detection. This is the recommended approach as it's more reliable and maintainable.

### 3. `HTS_Improved.cs` (Recommended)

An improved hand tracking script that uses the CustomHandLandmarkerRunner with event-based approach. This provides the most reliable access to hand landmark data.

### 4. `HandTrackingExample.cs`

An example script showing how to use the improved hand tracking system for gesture detection and visualization.

## Setup Instructions

### Option 1: Using the Improved Event-Based Approach (Recommended)

1. **Replace the HandLandmarkerRunner in your scene:**

   - Remove the existing `HandLandmarkerRunner` component
   - Add the `CustomHandLandmarkerRunner` component instead
   - Configure it the same way as the original runner

2. **Use HTS_Improved script:**

   - Add the `HTS_Improved` script to a GameObject in your scene
   - Assign the `CustomHandLandmarkerRunner` to the `handLandmarkerRunner` field
   - The script will automatically subscribe to hand landmark events

3. **Optional: Add gesture detection:**
   - Add the `HandTrackingExample` script for gesture detection examples
   - Customize the gesture detection logic for your needs

### Option 2: Using the Fixed Original Approach

1. Use the improved `HTS.cs` script
2. Assign your existing `HandLandmarkerRunner` to the script
3. The script will use reflection but with better error handling

## Key Features

### HTS_Improved.cs Features:

- **Event-based approach**: No reflection needed, more reliable
- **Thread-safe access**: Safe access to landmark data from multiple threads
- **Coordinate conversion**: Converts MediaPipe normalized coordinates to Unity world positions
- **Public API**: Easy access methods for other scripts
- **Debug logging**: Configurable logging for troubleshooting

### HandTrackingExample.cs Features:

- **Visualization**: Shows hand landmarks as colored spheres
- **Gesture detection**: Pinch and point gesture detection
- **Color-coded landmarks**: Different colors for different parts of the hand
- **Extensible**: Easy to add more gesture detection

## Hand Landmark Indices

The hand has 21 landmarks with the following indices:

```csharp
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
```

## Usage Examples

### Basic Usage

```csharp
public class MyHandController : MonoBehaviour
{
    public HTS_Improved handTracker;

    void Update()
    {
        if (handTracker.HasValidLandmarks())
        {
            // Get all landmarks
            var landmarks = handTracker.GetLatestLandmarks();
            var worldPositions = handTracker.GetLatestWorldPositions();

            // Get specific landmark
            Vector3 wristPos = handTracker.GetLandmarkWorldPosition(HTS_Improved.HandLandmarkIndices.WRIST);
            Vector3 indexTip = handTracker.GetLandmarkWorldPosition(HTS_Improved.HandLandmarkIndices.INDEX_FINGER_TIP);
        }
    }
}
```

### Gesture Detection

```csharp
public class MyGestureController : MonoBehaviour
{
    public HandTrackingExample gestureDetector;

    void Update()
    {
        if (gestureDetector.IsPinching())
        {
            // Handle pinch gesture
            Debug.Log("User is pinching!");
        }

        if (gestureDetector.IsPointing())
        {
            // Handle pointing gesture
            Vector3 direction = gestureDetector.GetPointingDirection();
            Debug.Log($"User is pointing in direction: {direction}");
        }
    }
}
```

## Troubleshooting

### Common Issues:

1. **"CustomHandLandmarkerRunner not found"**

   - Make sure you've replaced the original HandLandmarkerRunner with CustomHandLandmarkerRunner
   - Check that the runner is properly configured and running

2. **"No hand landmarks detected"**

   - Ensure your camera is properly set up for MediaPipe
   - Check that your hand is visible in the camera view
   - Verify the MediaPipe configuration settings

3. **Landmarks not updating**

   - Check that the HandLandmarkerRunner is in the correct running mode (LIVE_STREAM for real-time)
   - Verify that the camera permissions are granted

4. **Coordinate conversion issues**
   - Adjust the `coordinateConversionDistance` in HTS_Improved
   - Ensure Camera.main is properly assigned

### Debug Tips:

1. Enable `debugLogging` in HTS_Improved to see detailed logs
2. Use the HandTrackingExample to visualize landmarks
3. Check the Unity Console for error messages
4. Verify MediaPipe is properly initialized

## Performance Considerations

- The event-based approach is more efficient than reflection
- Coordinate conversion happens on the main thread for safety
- Consider caching frequently accessed landmark positions
- Use `HasValidLandmarks()` before accessing landmark data

## Extending the System

### Adding New Gestures:

1. Create detection methods in HandTrackingExample.cs
2. Use the landmark indices to access specific hand parts
3. Implement your gesture logic using distance and angle calculations

### Custom Visualization:

1. Modify the `CreateLandmarkSpheres()` method in HandTrackingExample
2. Add custom rendering or UI elements
3. Use the world positions for 3D object manipulation

This improved system provides a solid foundation for hand tracking applications in Unity with MediaPipe.
