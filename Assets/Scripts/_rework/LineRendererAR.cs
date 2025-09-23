using UnityEngine;

public class LineRendererAR : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public LineRenderer lineRenderer;
    public Transform endPoint;
    Gradient gradient = new Gradient();

    void Start()
    {
        lineRenderer.positionCount = 2;
        gradient.SetKeys(
            new GradientColorKey[]
            {
                new GradientColorKey(Color.red, 0.0f),
                new GradientColorKey(Color.blue, 1.0f),
            },
            new GradientAlphaKey[]
            {
                new GradientAlphaKey(1.0f, 0.0f),
                new GradientAlphaKey(1.0f, 1.0f),
            }
        );

        lineRenderer.colorGradient = gradient;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // endPoint = animator.GetBoneTransform(HumanBodyBones.Spine);
        Debug.Log($"END POINT: {endPoint.position}");

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, endPoint.position);
    }
}
