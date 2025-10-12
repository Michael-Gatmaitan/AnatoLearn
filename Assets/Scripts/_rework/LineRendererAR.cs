using UnityEngine;

public class LineRendererAR : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public LineRenderer lineRenderer;
    public Transform endPoint;

    void Start()
    {
        lineRenderer.positionCount = 2;

        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.green;

        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, endPoint.position);
    }
}
