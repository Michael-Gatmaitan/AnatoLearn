using System;
using System.Collections.Generic;
using System.Reflection;
using Mediapipe.Tasks.Components.Containers;
using Mediapipe.Tasks.Vision.PoseLandmarker;
using Mediapipe.Unity;
using Mediapipe.Unity.Sample.PoseLandmarkDetection;
using UnityEngine;
using UnityEngine.UIElements;

public class HumanoidPoseMapper : MonoBehaviour
{
    [Header("Main")]
    public PoseLandmarkerRunner poseLandmarkerRunner;
    public GameObject landmarkPrefab;

    [Header("Humanoid Model")]
    private Animator humanoidAnimator; // The Mixamo model with Animator component

    [Header("Settings")]
    // [SerializeField]
    // private float smoothingFactor = 0.1f;

    // [SerializeField]
    // private bool enableUpperBody = true;

    // [SerializeField]
    // private bool enableLowerBody = true;

    // [SerializeField]
    // private bool enableArms = true;

    // [SerializeField]
    // private bool enableLegs = true;

    private FieldInfo currentTargetField;
    private List<NormalizedLandmark> landmarks;

    public float screenDepth = 100f;
    private int LANDMARK_LENGTH = 33;

    // private GameObject[] landmarkPrefabs;

    private GameObject positionPref;

    public UIDocument uiDocument;
    public static HumanoidPoseMapper instance;

    // private Label lx,
    //     ly,
    //     lz;

    void OnEnable()
    {
        // var root = uiDocument.rootVisualElement;
        instance = this;

        // lx = root.Q<Label>("L_X");
        // ly = root.Q<Label>("L_Y");
        // lz = root.Q<Label>("L_Z");
    }

    // MediaPipe landmark indices (33 total landmarks)
    private enum MediaPipeLandmark
    {
        NOSE = 0,
        LEFT_EYE_INNER = 1,
        LEFT_EYE = 2,
        LEFT_EYE_OUTER = 3,
        RIGHT_EYE_INNER = 4,
        RIGHT_EYE = 5,
        RIGHT_EYE_OUTER = 6,
        LEFT_EAR = 7,
        RIGHT_EAR = 8,
        MOUTH_LEFT = 9,
        MOUTH_RIGHT = 10,
        LEFT_SHOULDER = 11,
        RIGHT_SHOULDER = 12,
        LEFT_ELBOW = 13,
        RIGHT_ELBOW = 14,
        LEFT_WRIST = 15,
        RIGHT_WRIST = 16,
        LEFT_PINKY = 17,
        RIGHT_PINKY = 18,
        LEFT_INDEX = 19,
        RIGHT_INDEX = 20,
        LEFT_THUMB = 21,
        RIGHT_THUMB = 22,
        LEFT_HIP = 23,
        RIGHT_HIP = 24,
        LEFT_KNEE = 25,
        RIGHT_KNEE = 26,
        LEFT_ANKLE = 27,
        RIGHT_ANKLE = 28,
        LEFT_HEEL = 29,
        RIGHT_HEEL = 30,
        LEFT_FOOT_INDEX = 31,
        RIGHT_FOOT_INDEX = 32,
    }

    void Start()
    {
        humanoidAnimator = GetComponent<Animator>();

        positionPref = Instantiate(landmarkPrefab);
        positionPref.name = "Position pref";

        if (humanoidAnimator == null)
        {
            Debug.LogError("Humanoid Animator is null. Please assign your Mixamo model.");
            // lx.text = "HUmanoid animator is null";
            return;
        }

        humanoidAnimator = GetComponent<Animator>();

        currentTargetField = typeof(PoseLandmarkerResultAnnotationController).GetField(
            "_currentTarget",
            BindingFlags.NonPublic | BindingFlags.Instance
        );

        if (currentTargetField == null)
        {
            Debug.LogError("Could not find _currentTarget field");
            // lx.text = "_currentTarget not found";
            return;
        }

        // InitializeBoneReferences();
        // InitializePreviousRotations();

        Debug.Log("Humanoid Pose Mapper Started");

        // InitializeLandmarkPrefabs();

        Debug.Log("Start");

        // Control camera
    }

    void Update()
    {
        // Debug.Log(humanoidAnimator.GetBoneTransform(HumanBodyBones.Spine));
        try
        {
            var controller =
                poseLandmarkerRunner.GetComponent<PoseLandmarkerResultAnnotationController>();
            if (controller == null)
            {
                // lx.text = "Controller is null";
                return;
            }

            var result = (PoseLandmarkerResult)currentTargetField.GetValue(controller);

            if (
                result.poseWorldLandmarks == null
                || result.poseWorldLandmarks.Count == 0
                || result.poseWorldLandmarks[0].landmarks.Count == 0
            )
            {
                // lx.text = "Pose landmarks statement problem";
                return;
            }

            landmarks = result.poseLandmarks[0].landmarks;

            if (landmarks.Count < 33)
            {
                Debug.LogWarning("Insufficient landmarks detected");
                // lx.text = "Insufficient landmarks";
                return;
            }

            if (move)
            {
                SetAvatarWorldPosition();
                SetAvatarScaleBaseOnTorsoHeight();
            }

            // ApplyPoseToHumanoid();
        }
        catch (Exception err)
        {
            Debug.LogError($"Error in UPDATE: {err}");
        }
    }

    private Vector3 prevLeftElbowPos;
    private Vector3 prevRightElbowPos;
    private Vector3 prevLeftHandPos;
    private Vector3 prevRightHandPos;
    private Vector3 prevLeftKneePos;
    private Vector3 prevRightKneePos;
    private Vector3 prevLeftFootPos;

    private Vector3 prevRightFootPos;
    private Vector3 prevHeadPos;

    // Rotation
    private bool hasPrevIK = false;
    public float avatarReferenceToShoulderWidth = 0.5f;
    public float positionZOffset = 20;
    public float adjustedHipYDivider = 1.5f; // 2.5-ish for skeletal
    public float avatarReferenceTorsoHeight = 0.65f; // 1 for skeletal

    public float ikPositionWeight = 1;

    public bool move = true;

    public void SetAvatarScaleBaseOnTorsoHeight()
    {
        // Convert to Unity space
        Vector3 unityLeftShoulder = ConvertMediaPipeToUnitySpace(
            landmarks[(int)MediaPipeLandmark.LEFT_SHOULDER]
        );
        Vector3 unityRightShoulder = ConvertMediaPipeToUnitySpace(
            landmarks[index: (int)MediaPipeLandmark.RIGHT_SHOULDER]
        );
        Vector3 unityLeftHip = ConvertMediaPipeToUnitySpace(
            landmarks[(int)MediaPipeLandmark.LEFT_HIP]
        );
        Vector3 unityRightHip = ConvertMediaPipeToUnitySpace(
            landmarks[(int)MediaPipeLandmark.RIGHT_HIP]
        );

        // Calculate midpoint of shoulders
        Vector3 unityMidShoulder = (unityLeftShoulder + unityRightShoulder) / 2f;
        // Calculate midpoint of hips
        Vector3 unityMidHip = (unityLeftHip + unityRightHip) / 2f;

        // Debug.Log($"Mid Shoulder position: {unityMidShoulder}");
        // Debug.Log($"Mid Hip position: {unityMidHip}");

        // Calculate the vertical distance between shoulders and hips
        float detectedTorsoHeight = Mathf.Abs(unityMidShoulder.y - unityMidHip.y);

        // IMPORTANT: You need to define this value based on your avatar's actual torso height
        // (e.g., measure it in your 3D modeling software when the avatar is T-posed)

        if (avatarReferenceTorsoHeight <= 0)
        {
            Debug.LogError(
                "avatarReferenceTorsoHeight must be greater than 0 to avoid division by zero."
            );
            return;
        }

        float calculatedScale = detectedTorsoHeight / avatarReferenceTorsoHeight;

        float currentScale = humanoidAnimator.transform.localScale.x; // Assuming uniform scale
        // float smooth = Mathf.Clamp01(Time.deltaTime * 5f); // Adjust smoothing factor as needed
        // float newScale = Mathf.Lerp(currentScale, calculatedScale, smooth);

        // humanoidAnimator.transform.localScale = new Vector3(newScale, newScale, newScale);
        humanoidAnimator.transform.localScale = new Vector3(
            calculatedScale,
            calculatedScale,
            calculatedScale
        );
    }

    public void SetAvatarScaleBasedOnShoulderWidth()
    {
        // Convert to Unity space
        Vector3 unityLeftShoulder = ConvertMediaPipeToUnitySpace(
            landmarks[(int)MediaPipeLandmark.LEFT_SHOULDER]
        );
        Vector3 unityRightShoulder = ConvertMediaPipeToUnitySpace(
            landmarks[(int)MediaPipeLandmark.RIGHT_SHOULDER]
        );

        Debug.Log($"Left Shoulder position: {unityLeftShoulder}");
        Debug.Log($"Right Shoulder position: {unityRightShoulder}");

        // Calculate detected shoulder width (horizontal distance)
        float detectedShoulderWidth = Vector3.Distance(unityLeftShoulder, unityRightShoulder);

        float calculatedScale = detectedShoulderWidth * avatarReferenceToShoulderWidth;

        // Apply the calculated scale to the avatar
        humanoidAnimator.transform.localScale = new Vector3(
            calculatedScale,
            calculatedScale,
            calculatedScale
        );

        // IMPORTANT: You need to define this value based on your avatar's actual shoulder width
        // (e.g., measure it in your 3D modeling software when the avatar is T-posed)
        // public float avatarReferenceShoulderWidth = 0.5f; // Example: 0.5 meters

        // if (avatarReferenceToShoulderWidth <= 0)
        // {
        //     Debug.LogError(
        //         "avatarReferenceShoulderWidth must be greater than 0 to avoid division by zero."
        //     );
        //     lx.text = "Avatar reference problem";
        //     return;
        // }
    }

    public void SetAvatarWorldPosition()
    {
        Vector3 leftHip = ConvertMediaPipeToUnitySpace(landmarks[(int)MediaPipeLandmark.LEFT_HIP]);
        Vector3 rightHip = ConvertMediaPipeToUnitySpace(
            landmarks[(int)MediaPipeLandmark.RIGHT_HIP]
        );

        Vector3 averageHip = (leftHip + rightHip) / 2;

        Vector3 adjustedHip = new(averageHip.x, averageHip.y / adjustedHipYDivider, averageHip.z);

        humanoidAnimator.transform.localPosition = adjustedHip;
    }

    public float rotationSpeed = 10f;

    // Only for chest and hips
    private void SetSocketJointsRotation(
        Vector3 leftJoint,
        Vector3 rightJoint,
        HumanBodyBones boneId
    )
    {
        Transform humanBone = humanoidAnimator.GetBoneTransform(boneId);

        Vector3 targetWorldPosition = (leftJoint + rightJoint) / 2f;
        targetWorldPosition.y = 0;

        Vector4 directionToTarget = targetWorldPosition - humanBone.position;

        directionToTarget.x *= -1f;
        directionToTarget.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(-directionToTarget, Vector3.up);

        humanoidAnimator.SetBoneLocalRotation(boneId, targetRotation);
    }

    private int counter = 0;

    public void OnAnimatorIK()
    {
        counter++;
        Debug.Log("Animator running");
        // lz.text = $"Animator is now running {counter}";

        try
        {
            if (humanoidAnimator)
            {
                // UpdateLandmarkPrefabs();
                // lx.text = $"{humanoidAnimator.transform.position.x}";
                // ly.text = $"{humanoidAnimator.transform.position.y}";
                // lz.text = $"{humanoidAnimator.transform.position.z}";

                if (landmarks != null && landmarks.Count >= 33)
                {
                    // SetAvatarScaleBaseOnHeight();
                    // SetAvatarScaleBaseOnTorsoHeight();
                    // SetAvatarScaleBasedOnShoulderWidth();

                    // END TEST CODE HERE

                    Vector3 headTarget = ConvertMediaPipeToUnitySpace(
                        landmarks[(int)MediaPipeLandmark.NOSE]
                    ); // Nose

                    Vector3 leftElbowTarget = ConvertMediaPipeToUnitySpace(
                        landmarks[(int)MediaPipeLandmark.LEFT_ELBOW]
                    );

                    Vector3 rightElbowTarget = ConvertMediaPipeToUnitySpace(
                        landmarks[(int)MediaPipeLandmark.RIGHT_ELBOW]
                    );

                    Vector3 rightWristTarget = ConvertMediaPipeToUnitySpace(
                        landmarks[(int)MediaPipeLandmark.RIGHT_WRIST]
                    );
                    Vector3 leftWristTarget = ConvertMediaPipeToUnitySpace(
                        landmarks[(int)MediaPipeLandmark.LEFT_WRIST]
                    );

                    Vector3 leftFootTarget = ConvertMediaPipeToUnitySpace(
                        landmarks[(int)MediaPipeLandmark.LEFT_FOOT_INDEX]
                    );

                    Vector3 rightFootTarget = ConvertMediaPipeToUnitySpace(
                        landmarks[(int)MediaPipeLandmark.RIGHT_FOOT_INDEX]
                    );

                    Vector3 leftKneeTarget = ConvertMediaPipeToUnitySpace(
                        landmarks[(int)MediaPipeLandmark.LEFT_KNEE]
                    );

                    Vector3 rightKneeTarget = ConvertMediaPipeToUnitySpace(
                        landmarks[(int)MediaPipeLandmark.RIGHT_KNEE]
                    );

                    // prevLeftKneePos

                    // ------------ ROTATION & POSITIONING USING HIPS -------------- //

                    Vector3 leftShoulder = ConvertMediaPipeToUnitySpace(
                        landmarks[(int)MediaPipeLandmark.LEFT_SHOULDER]
                    );
                    Vector3 rightShoulder = ConvertMediaPipeToUnitySpace(
                        landmarks[(int)MediaPipeLandmark.RIGHT_SHOULDER]
                    );

                    // Get both l and r shoulder
                    // Calculate rotation using their positions (x)
                    // Get UpperChest bone
                    // Rotate animator's spine2 (Chest)
                    Vector3 leftHip = ConvertMediaPipeToUnitySpace(
                        landmarks[(int)MediaPipeLandmark.LEFT_HIP]
                    );
                    Vector3 rightHip = ConvertMediaPipeToUnitySpace(
                        landmarks[(int)MediaPipeLandmark.RIGHT_HIP]
                    );

                    // SetSocketJointsRotation(leftShoulder, rightShoulder, HumanBodyBones.Chest);
                    // SetSocketJointsRotation(leftHip, rightHip, HumanBodyBones.Hips);

                    if (!hasPrevIK)
                    {
                        prevLeftElbowPos = leftElbowTarget;
                        prevRightElbowPos = rightElbowTarget;

                        prevRightHandPos = rightWristTarget;
                        prevLeftHandPos = leftWristTarget;

                        prevLeftFootPos = leftFootTarget;
                        prevRightFootPos = rightFootTarget;

                        prevLeftKneePos = leftKneeTarget;
                        prevRightKneePos = rightKneeTarget;

                        prevHeadPos = headTarget;

                        hasPrevIK = true;
                    }

                    float smooth = Mathf.Clamp01(Time.deltaTime * 30f);

                    prevLeftElbowPos = Vector3.Lerp(prevLeftElbowPos, leftElbowTarget, smooth);
                    prevRightElbowPos = Vector3.Lerp(prevRightElbowPos, rightElbowTarget, smooth);

                    prevLeftHandPos = Vector3.Lerp(prevLeftHandPos, leftWristTarget, smooth);
                    prevRightHandPos = Vector3.Lerp(prevRightHandPos, rightWristTarget, smooth);

                    prevLeftFootPos = Vector3.Lerp(prevLeftFootPos, leftFootTarget, smooth);
                    prevRightFootPos = Vector3.Lerp(prevRightFootPos, rightFootTarget, smooth);

                    prevHeadPos = Vector3.Lerp(prevHeadPos, headTarget, smooth);

                    // Set left elbow -- User IKHint
                    humanoidAnimator.SetIKHintPositionWeight(
                        AvatarIKHint.LeftElbow,
                        ikPositionWeight
                    );
                    humanoidAnimator.SetIKHintPosition(AvatarIKHint.LeftElbow, prevLeftElbowPos);

                    // Set right elbow -- User IKHint
                    humanoidAnimator.SetIKHintPositionWeight(
                        AvatarIKHint.RightElbow,
                        ikPositionWeight
                    );
                    humanoidAnimator.SetIKHintPosition(AvatarIKHint.RightElbow, prevRightElbowPos);

                    // Set left hand
                    humanoidAnimator.SetIKPositionWeight(AvatarIKGoal.LeftHand, ikPositionWeight);
                    humanoidAnimator.SetIKPosition(AvatarIKGoal.LeftHand, prevLeftHandPos);

                    // Set right hand
                    humanoidAnimator.SetIKPositionWeight(AvatarIKGoal.RightHand, ikPositionWeight);
                    humanoidAnimator.SetIKPosition(AvatarIKGoal.RightHand, prevRightHandPos);

                    positionPref.transform.localPosition = prevRightHandPos;

                    // Set left knee
                    humanoidAnimator.SetIKHintPositionWeight(
                        AvatarIKHint.LeftKnee,
                        ikPositionWeight
                    );
                    humanoidAnimator.SetIKHintPosition(AvatarIKHint.LeftKnee, prevLeftKneePos);

                    // Set right knee
                    humanoidAnimator.SetIKHintPositionWeight(
                        AvatarIKHint.RightKnee,
                        ikPositionWeight
                    );
                    humanoidAnimator.SetIKHintPosition(AvatarIKHint.RightKnee, prevRightKneePos);

                    // Set left foot
                    humanoidAnimator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, ikPositionWeight);
                    humanoidAnimator.SetIKPosition(AvatarIKGoal.LeftFoot, prevLeftFootPos);

                    // Set right foot
                    humanoidAnimator.SetIKPositionWeight(AvatarIKGoal.RightFoot, ikPositionWeight);
                    humanoidAnimator.SetIKPosition(AvatarIKGoal.RightFoot, prevRightFootPos);

                    // Set LookAt for head
                    humanoidAnimator.SetLookAtPosition(prevHeadPos);
                    humanoidAnimator.SetLookAtWeight(ikPositionWeight);
                }
                else
                {
                    hasPrevIK = false;
                    Debug.Log("Landmark problem occurred");
                    // ly.text = "Landmark problem";
                }
            }
            else
            {
                Debug.Log("Animator not defined while runnung OnIKAnimator");
                // ly.text = "Animator not defined";
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error in OnAnimatorIK: {e}");
        }
    }

    public Vector3 ConvertMediaPipeToUnitySpace(NormalizedLandmark mediaPipePos)
    {
        float width = UnityEngine.Screen.width;
        float height = UnityEngine.Screen.height;
        float screenX;
        float screenY;

        // Desktop
        // screenX = mediaPipePos.x * width;
        // screenY = (1f - mediaPipePos.y) * height;

        // Mobile
        screenX = (1 - mediaPipePos.y) * width;
        screenY = (1f - mediaPipePos.x) * height;

        // Convert screen coordinates to world position at specified depth
        Vector3 screenPoint = new(screenX, screenY, screenDepth);
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPoint);

        // This will being the model in front of the detected body
        worldPosition.z -= positionZOffset;

        return worldPosition;
    }

    // private void ApplyPoseToHumanoid()
    // {
    //     // Apply rotations to different body parts
    //     if (enableUpperBody)
    //     {
    //         // ApplySpineRotation();
    //         // ApplyNeckAndHeadRotation();
    //     }

    //     if (enableArms)
    //     {
    //         // ApplyArmRotations();
    //         // RotateBoneBetweenLandmarks(leftUpperArm, ConvertMediaPipeToUnitySpace2(landmarks[11]), ConvertMediaPipeToUnitySpace2(landmarks[13]));
    //     }

    //     if (enableLegs)
    //     {
    //         // ApplyLegRotations();
    //     }
    // }

    // void RotateBoneBetweenLandmarks(Transform bone, Vector3 startLandmark, Vector3 endLandmark)
    // {
    //     Vector3 direction = (endLandmark - startLandmark).normalized;

    //     // Adjust local axis if needed (depends on your model’s bone orientations)
    //     Quaternion targetRotation = Quaternion.LookRotation(direction);

    //     // Optional: apply bone offset or use Quaternion.FromToRotation
    //     bone.rotation = targetRotation;
    // }

    // private void ApplySpineRotation()
    // {
    //     if (spine == null)
    //         return;

    //     Debug.Log("Applying spine");

    //     // Calculate spine rotation based on shoulder alignment
    //     Vector3 leftShoulder = ConvertMediaPipeToUnitySpace2(
    //         landmarks[(int)MediaPipeLandmark.LEFT_SHOULDER]
    //     );
    //     Vector3 rightShoulder = ConvertMediaPipeToUnitySpace2(
    //         landmarks[(int)MediaPipeLandmark.RIGHT_SHOULDER]
    //     );
    //     Vector3 leftHip = ConvertMediaPipeToUnitySpace2(landmarks[(int)MediaPipeLandmark.LEFT_HIP]);
    //     Vector3 rightHip = ConvertMediaPipeToUnitySpace2(
    //         landmarks[(int)MediaPipeLandmark.RIGHT_HIP]
    //     );

    //     Vector3 shoulderDirection = (rightShoulder - leftShoulder).normalized;
    //     Vector3 hipDirection = (rightHip - leftHip).normalized;
    //     Vector3 torsoUp = (
    //         ((leftShoulder + rightShoulder) * 0.5f) - ((leftHip + rightHip) * 0.5f)
    //     ).normalized;

    //     Quaternion targetRotation = Quaternion.LookRotation(
    //         Vector3.Cross(shoulderDirection, torsoUp),
    //         torsoUp
    //     );

    //     ApplySmoothRotation(spine, targetRotation);
    // }

    // private void ApplyNeckAndHeadRotation()
    // {
    //     if (head == null)
    //         return;

    //     Debug.Log("Applying Neck");

    //     Vector3 nose = ConvertMediaPipeToUnitySpace(landmarks[(int)MediaPipeLandmark.NOSE]);
    //     Vector3 leftEar = ConvertMediaPipeToUnitySpace(landmarks[(int)MediaPipeLandmark.LEFT_EAR]);
    //     Vector3 rightEar = ConvertMediaPipeToUnitySpace(
    //         landmarks[(int)MediaPipeLandmark.RIGHT_EAR]
    //     );

    //     Vector3 earDirection = (rightEar - leftEar).normalized;
    //     Vector3 faceForward = Vector3.Cross(earDirection, Vector3.up).normalized;

    //     Quaternion targetRotation = Quaternion.LookRotation(faceForward, Vector3.up);
    //     ApplySmoothRotation(head, targetRotation);
    // }

    // private void ApplyArmRotations()
    // {
    //     // Left Arm
    //     if (leftShoulder != null && leftUpperArm != null && leftLowerArm != null)
    //     {
    //         Debug.Log("Applying left arm");
    //         ApplyArmRotation(
    //             landmarks[(int)MediaPipeLandmark.LEFT_SHOULDER],
    //             landmarks[(int)MediaPipeLandmark.LEFT_ELBOW],
    //             landmarks[(int)MediaPipeLandmark.LEFT_WRIST],
    //             leftUpperArm,
    //             leftLowerArm,
    //             leftHand
    //         );
    //     }

    //     // Right Arm
    //     if (rightShoulder != null && rightUpperArm != null && rightLowerArm != null)
    //     {
    //         Debug.Log("Applying right arm");
    //         ApplyArmRotation(
    //             landmarks[(int)MediaPipeLandmark.RIGHT_SHOULDER],
    //             landmarks[(int)MediaPipeLandmark.RIGHT_ELBOW],
    //             landmarks[(int)MediaPipeLandmark.RIGHT_WRIST],
    //             rightUpperArm,
    //             rightLowerArm,
    //             rightHand
    //         );
    //     }
    // }

    // private void ApplyArmRotation(
    //     NormalizedLandmark shoulder,
    //     NormalizedLandmark elbow,
    //     NormalizedLandmark wrist,
    //     Transform upperArm,
    //     Transform lowerArm
    // )
    // {
    //     if (upperArm == null || lowerArm == null)
    //     {
    //         Debug.Log("Something is null in ARM rotation");
    //     }

    //     Vector3 shoulderPos = ConvertMediaPipeToUnitySpace(shoulder);
    //     Vector3 elbowPos = ConvertMediaPipeToUnitySpace(elbow);
    //     Vector3 wristPos = ConvertMediaPipeToUnitySpace(wrist);

    //     // Upper arm rotation (shoulder to elbow)
    //     Vector3 upperArmDirection = (elbowPos - shoulderPos).normalized;
    //     Quaternion upperArmRotation = Quaternion.LookRotation(upperArmDirection);

    //     Debug.Log(upperArmRotation);
    //     ApplySmoothRotation(upperArm, upperArmRotation);

    //     // Lower arm rotation (elbow to wrist)
    //     // Vector3 lowerArmDirection = (wristPos - elbowPos).normalized;
    //     // Quaternion lowerArmRotation = Quaternion.LookRotation(lowerArmDirection);
    //     // ApplySmoothRotation(lowerArm, lowerArmRotation);
    // }

    // private void ApplyArmRotation(
    //     NormalizedLandmark shoulderLm,
    //     NormalizedLandmark elbowLm,
    //     NormalizedLandmark wristLm,
    //     Transform upperArm,
    //     Transform lowerArm,
    //     Transform hand
    // )
    // {
    //     Vector3 shoulder = ConvertMediaPipeToUnitySpace2(shoulderLm);
    //     Vector3 elbow = ConvertMediaPipeToUnitySpace2(elbowLm);
    //     Vector3 wrist = ConvertMediaPipeToUnitySpace2(wristLm);

    //     // Upper arm
    //     Vector3 upperArmDir = (elbow - shoulder).normalized;
    //     Quaternion upperArmRotation = Quaternion.LookRotation(upperArmDir);
    //     ApplySmoothRotation(upperArm, upperArmRotation);

    //     // Lower arm
    //     // Vector3 lowerArmDir = (wrist - elbow).normalized;
    //     // Quaternion lowerArmRotation = Quaternion.LookRotation(lowerArmDir);
    //     // ApplySmoothRotation(lowerArm, lowerArmRotation);

    //     // Hand pointing forward (optional)
    //     // if (hand != null)
    //     // {
    //     //     Vector3 handForward = (wrist - elbow).normalized;
    //     //     Quaternion handRotation = Quaternion.LookRotation(handForward);
    //     //     ApplySmoothRotation(hand, handRotation);
    //     // }
    // }

    // private void ApplyLegRotations()
    // {
    //     // Left Leg
    //     if (leftUpperLeg != null && leftLowerLeg != null)
    //     {
    //         Debug.Log("Applying left leg");
    //         ApplyLegRotation(
    //             landmarks[(int)MediaPipeLandmark.LEFT_HIP],
    //             landmarks[(int)MediaPipeLandmark.LEFT_KNEE],
    //             landmarks[(int)MediaPipeLandmark.LEFT_ANKLE],
    //             leftUpperLeg,
    //             leftLowerLeg
    //         );
    //     }

    //     // Right Leg
    //     if (rightUpperLeg != null && rightLowerLeg != null)
    //     {
    //         Debug.Log("Applying right leg");
    //         ApplyLegRotation(
    //             landmarks[(int)MediaPipeLandmark.RIGHT_HIP],
    //             landmarks[(int)MediaPipeLandmark.RIGHT_KNEE],
    //             landmarks[(int)MediaPipeLandmark.RIGHT_ANKLE],
    //             rightUpperLeg,
    //             rightLowerLeg
    //         );
    //     }
    // }

    // private void ApplyLegRotation(
    //     NormalizedLandmark hip,
    //     NormalizedLandmark knee,
    //     NormalizedLandmark ankle,
    //     Transform upperLeg,
    //     Transform lowerLeg
    // )
    // {
    //     Vector3 hipPos = ConvertMediaPipeToUnitySpace(hip);
    //     Vector3 kneePos = ConvertMediaPipeToUnitySpace(knee);
    //     Vector3 anklePos = ConvertMediaPipeToUnitySpace(ankle);

    //     // Upper leg rotation (hip to knee)
    //     Vector3 upperLegDirection = (kneePos - hipPos).normalized;
    //     Quaternion upperLegRotation = Quaternion.LookRotation(upperLegDirection);
    //     ApplySmoothRotation(upperLeg, upperLegRotation);

    //     // Lower leg rotation (knee to ankle)
    //     Vector3 lowerLegDirection = (anklePos - kneePos).normalized;
    //     Quaternion lowerLegRotation = Quaternion.LookRotation(lowerLegDirection);
    //     ApplySmoothRotation(lowerLeg, lowerLegRotation);
    // }

    // private Vector3 ConvertMediaPipeToUnitySpace2(NormalizedLandmark landmark)
    // {
    //     // Convert MediaPipe world coordinates to Unity space
    //     // Adjust these multipliers based on your specific setup
    //     float x = (landmark.x - 0.5f) * UnityEngine.Screen.width;
    //     float y = (landmark.y - 0.5f) * UnityEngine.Screen.height;

    //     return new Vector3(x, y, landmark.z * 2f);
    //     // return new Vector3(
    //     //     landmark.x,
    //     //     -landmark.y,
    //     //     landmark.z
    //     // );
    // }

    // private Vector3 ConvertMediaPipeToUnitySpace(NormalizedLandmark landmark)
    // {
    //     // Convert MediaPipe world coordinates to Unity space
    //     // Adjust these multipliers based on your specific setup
    //     var c = new Vector3(
    //         -landmark.x * 2f, // Flip X axis and scale
    //         landmark.y * 2f, // Scale Y
    //         landmark.z * 2f // Scale Z
    //     );

    //     Debug.Log($"Converted location: {c}");
    //     return c;
    // }

    // private void ApplySmoothRotation(Transform bone, Quaternion targetRotation)
    // {
    //     if (bone == null)
    //     {
    //         Debug.Log("Bone is null");
    //         return;
    //     }

    //     Quaternion currentRotation = bone.localRotation;
    //     Quaternion smoothedRotation = Quaternion.Slerp(
    //         currentRotation,
    //         targetRotation,
    //         smoothingFactor
    //     );

    //     Debug.Log($"Current rotation: {currentRotation}");

    //     bone.localRotation = smoothedRotation;
    //     previousRotations[bone] = smoothedRotation;
    // }

    // private void ApplySmoothRotation(Transform bone, Quaternion targetRotation)
    // {
    //     if (bone == null)
    //         return;

    //     if (previousRotations.TryGetValue(bone, out var prevRotation))
    //     {
    //         Quaternion smoothedRotation = Quaternion.Slerp(
    //             prevRotation,
    //             targetRotation,
    //             smoothingFactor
    //         );
    //         bone.localRotation = smoothedRotation;
    //         previousRotations[bone] = smoothedRotation;
    //     }
    //     else
    //     {
    //         bone.localRotation = targetRotation;
    //         previousRotations[bone] = targetRotation;
    //     }
    // }

    // Public methods to toggle different body parts
    // public void ToggleUpperBody(bool enabled) => enableUpperBody = enabled;

    // public void ToggleLowerBody(bool enabled) => enableLowerBody = enabled;

    // public void ToggleArms(bool enabled) => enableArms = enabled;

    // public void ToggleLegs(bool enabled) => enableLegs = enabled;

    // public void SetSmoothingFactor(float factor) => smoothingFactor = Mathf.Clamp01(factor);

    // public void InitializeLandmarkPrefabs()
    // {
    //     landmarkPrefabs = new GameObject[LANDMARK_LENGTH];
    //     for (int i = 0; i < LANDMARK_LENGTH; i++)
    //     {
    //         landmarkPrefabs[i] = Instantiate(landmarkPrefab, transform);

    //         landmarkPrefabs[i].SetActive(false);
    //         landmarkPrefabs[i].name = $"PoseLandmark_{i}";
    //     }
    // }

    // public void UpdateLandmarkPrefabs()
    // {
    //     if (landmarks == null || landmarks.Count < 33)
    //         return;

    //     for (int i = 0; i < landmarks.Count; i++)
    //     {
    //         NormalizedLandmark ll = landmarks[i];

    //         Vector3 displayPosition;
    //         // displayPosition = ConvertMediaPipeToScreenSpace(ll, mainCamera);
    //         displayPosition = ConvertMediaPipeToUnitySpace(ll);
    //         landmarkPrefabs[i].transform.position = displayPosition;
    //         landmarkPrefabs[i].transform.localScale = new Vector3(1, 1, 1);
    //     }
    // }
}
