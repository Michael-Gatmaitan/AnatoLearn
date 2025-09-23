using UnityEngine;

public class LabelFollow : MonoBehaviour
{
    public Transform target; // The target point on the cube
    public Camera mainCam;
    public RectTransform labelUI;

    void Update()
    {
        Vector3 screenPos = mainCam.WorldToScreenPoint(target.position);
        labelUI.position = screenPos;
    }
}
