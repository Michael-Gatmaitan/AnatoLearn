using System.Collections.Generic;
using UnityEngine;

public class Circulatory_HeartViewedTagClickManager : MonoBehaviour
{
    private Camera cam;

    [Header("Assign your viewed tags here")]
    public GameObject viewedTagRightAtrium;
    public GameObject viewedTagTricuspidValve;
    public GameObject viewedTagRightVentricle;
    public GameObject viewedTagInferiorVenaCava;
    public GameObject viewedTagSuperiorVenaCava;
    public GameObject viewedTagPulmonaryValve;
    public GameObject viewedTagPulmonaryArtery;
    public GameObject viewedTagAorticValve;
    public GameObject viewedTagPulmonaryVein;
    public GameObject viewedTagLeftVentricle;
    public GameObject viewedTagLeftAtrium;
    public GameObject viewedTagMitralValve;

    private Dictionary<string, GameObject> labelToViewedTag;

    void Start()
    {
        cam = Camera.main;

        // Initialize dictionary with mappings from labelID to GameObject
        labelToViewedTag = new Dictionary<string, GameObject>
        {
            { "rightAtriumDescriptionCon", viewedTagRightAtrium },
            { "tricuspidValveDescriptionCon", viewedTagTricuspidValve },
            { "rightVentricleDescriptionCon", viewedTagRightVentricle },
            { "inferiorVenaCavaDescriptionCon", viewedTagInferiorVenaCava },
            { "superiorVenaCavaDescriptionCon", viewedTagSuperiorVenaCava },
            { "pulmonaryValveDescriptionCon", viewedTagPulmonaryValve },
            { "pulmonaryArteryDescriptionCon", viewedTagPulmonaryArtery },
            { "aorticValveDescriptionCon", viewedTagAorticValve },
            { "pulmonaryVeinDescriptionCon", viewedTagPulmonaryVein },
            { "leftVentricleDescriptionCon", viewedTagLeftVentricle },
            { "leftAtriumDescriptionCon", viewedTagLeftAtrium },
            { "mitralValveDescriptionCon", viewedTagMitralValve },
        };
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            DetectHit(ray);
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray ray = cam.ScreenPointToRay(Input.GetTouch(0).position);
            DetectHit(ray);
        }
    }

    void DetectHit(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log("Ray hit: " + hit.collider.name);

            if (hit.collider.CompareTag("Clickable"))
            {
                var label = hit.collider.GetComponentInParent<TagTarget>();
                if (label == null)
                    return;

                // Look up and activate corresponding viewed tag
                if (labelToViewedTag.TryGetValue(label.labelID, out GameObject viewedTag))
                {
                    if (viewedTag != null)
                    {
                        viewedTag.SetActive(true);
                    }
                    else
                    {
                        Debug.LogWarning(
                            $"GameObject for '{label.labelID}' is not assigned in the inspector."
                        );
                    }
                }
            }
        }
    }
}
