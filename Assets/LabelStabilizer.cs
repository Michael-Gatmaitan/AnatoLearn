using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class LabelStabilizer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // public GameObject label;
    private Camera cam;
    private List<Quaternion> initialRotations;
    private List<GameObject> labels;
    public UIDocument uiDocument;
    private VisualElement root;

    void OnEnable()
    {
        root = uiDocument.rootVisualElement;
    }

    void Awake()
    {
        // Initialize the lists
        initialRotations = new List<Quaternion>();
        labels = GameObject.FindGameObjectsWithTag("BodyLabel").ToList();

        for (int i = 0; i < labels.Count; i++)
        {
            GameObject label = labels[i];
            Debug.Log($"Label rotation: {label.transform.rotation}");
            initialRotations.Add(label.transform.rotation);
        }
    }

    void Start()
    {
        cam = Camera.main;
        Debug.Log($"ROOT: " + root);
    }

    private bool hasLoggedHit = false;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (!hasLoggedHit)
                {
                    // Raycast hit an object
                    // Access information about the hit object through the 'hit' variable
                    Debug.Log("Hit object: " + hit.collider.name);
                    Debug.Log("Hit point: " + hit.point);
                    Debug.Log("Hit tag name: " + hit.collider.tag);
                    hasLoggedHit = true;

                    root.Q<VisualElement>("lungsDescriptionCon").style.display = DisplayStyle.Flex;
                }
            }
            else
            {
                // Raycast did not hit any object
            }
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // label.transform.rotation = new Quaternion(0, 0, 0, 0);
        for (int i = 0; i < labels.Count; i++)
        {
            GameObject label = labels[i];
            label.transform.rotation = initialRotations[i];
        }
    }
}
