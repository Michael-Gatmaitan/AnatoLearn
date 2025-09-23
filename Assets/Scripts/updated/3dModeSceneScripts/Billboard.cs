using UnityEngine;

public class BillboardLabel : MonoBehaviour
{
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (!cam) return;

        Vector3 direction = cam.transform.position - transform.position;
        // direction.y = 0; // Prevent flipping on vertical axis

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction); // Negative to face camera properly
            transform.rotation = lookRotation;
        }
    }
}
