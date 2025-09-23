using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SkeletalViewedTagClickManager : MonoBehaviour
{
    private Camera cam;

    [Header("Assign your viewed tags here")]
    public GameObject viewedTagSkull;
    public GameObject viewedTagScapula;
    public GameObject viewedTagClavicle;
    public GameObject viewedTagHumerus;
    public GameObject viewedTagUlna;
    public GameObject viewedTagRadius;
    public GameObject viewedTagCarpals;
    public GameObject viewedTagMetacarpals;
    public GameObject viewedTagPhalange;
    public GameObject viewedTagFemur;
    public GameObject viewedTagPatella;
    public GameObject viewedTagTibia;
    public GameObject viewedTagFibula;
    public GameObject viewedTagTarsal;
    public GameObject viewedTagPhalangeToes;
    public GameObject viewedTagMetatarsal;
    public GameObject viewedTagSternum;
    public GameObject viewedTagRibs;
    public GameObject viewedTagCoccyx;
    public GameObject viewedTagSpine;
    public GameObject viewedTagPelvicGirdle;
    private Dictionary<string, GameObject> labelToViewedTag;
    private TagClickManager tagClickManager; //903
    private HashSet<string> activatedTags = new HashSet<string>();

    // HTTP
    private UserTagViewsController userTagViewsController;

    void OnEnable()
    {
        userTagViewsController = GetComponent<UserTagViewsController>();
    }

    void Start()
    {
        cam = Camera.main;
        // tagClickManager = FindObjectOfType<TagClickManager>();
        tagClickManager = FindFirstObjectByType<TagClickManager>();

        // UserState.Instance.SetTopicId(1);

        // userTagViewsController.CheckValidity(
        //     UserState.Instance.Id,
        //     UserState.Instance.TopicId,
        //     (r) => Debug.Log($"Comparison result: {r.comparison_result}"),
        //     (e) => Debug.LogError(e)
        // );

        Debug.Log($"Controller: {userTagViewsController}");

        // Debug.Log("Current topic id: " + UserState.Instance.TopicId);

        // Initialize dictionary with mappings from labelID to GameObject
        labelToViewedTag = new Dictionary<string, GameObject>
        {
            { "skullDescriptionCon", viewedTagSkull },
            { "scapulaDescriptionCon", viewedTagScapula },
            { "clavicleDescriptionCon", viewedTagClavicle },
            { "humerusDescriptionCon", viewedTagHumerus },
            { "ulnaDescriptionCon", viewedTagUlna },
            { "radiusDescriptionCon", viewedTagRadius },
            { "carpalsDescriptionCon", viewedTagCarpals },
            { "metacarpalsDescriptionCon", viewedTagMetacarpals },
            { "phalange_FingersDescriptionCon", viewedTagPhalange },
            { "femurDescriptionCon", viewedTagFemur },
            { "patellaDescriptionCon", viewedTagPatella },
            { "tibiaDescriptionCon", viewedTagTibia },
            { "fibulaDescriptionCon", viewedTagFibula },
            { "tarsalDescriptionCon", viewedTagTarsal },
            { "phalange_ToesDescriptionCon", viewedTagPhalangeToes },
            { "metatarsalDescriptionCon", viewedTagMetatarsal },
            { "sternumDescriptionCon", viewedTagSternum },
            { "ribsDescriptionCon", viewedTagRibs },
            { "coccyxDescriptionCon", viewedTagCoccyx },
            { "spineDescriptionCon", viewedTagSpine },
            { "pelvicGirdleDescriptionCon", viewedTagPelvicGirdle },
        };

        // Check all parts if visited
        CheckAllPartsIfVisited();
    }

    void CheckAllPartsIfVisited()
    {
        userTagViewsController.GetUserTagViewsByUserIdAndTopicId(
            UserState.Instance.Id,
            UserState.Instance.TopicId,
            (r) =>
            {
                if (r.data.Count >= 1)
                {
                    foreach (var data in r.data)
                    {
                        string name = data.name;
                        Debug.Log(name);

                        if (name.Contains(" "))
                        {
                            var arr = name.Split(' ');

                            for (int i = 0; i < arr.Length; i++)
                            {
                                string s = arr[i];
                                arr[i] = char.ToUpper(s[0]) + s.Substring(1);
                            }

                            // int index = 0;
                            // foreach (string s in arr)
                            // {
                            //     // var newS = char.ToUpper(s[0]) + s.Substring(1);
                            //     arr[index] = newS;
                            //     index++;
                            // }

                            var joined = string.Join("_", arr);
                            joined = char.ToLower(joined[0]) + joined.Substring(1);

                            Debug.Log(joined);
                            name = joined;
                        }

                        string buildKey = name + "DescriptionCon";
                        Debug.Log("Builded key: " + buildKey);

                        if (labelToViewedTag[buildKey] == null)
                        {
                            Debug.LogError("Build key not found on label viewed tags variable");
                        }
                        else
                        {
                            Debug.Log("Buildkey found in label viewed tags variable");
                            labelToViewedTag[buildKey].SetActive(true);
                        }
                    }
                }
            },
            (e) => Debug.LogError(e)
        );

        foreach (var el in labelToViewedTag)
        {
            string key = el.Key;
            GameObject val = el.Value;
            Debug.Log($"Label to viewed tag: {val}");

            // string splitKey = key.Split("")[0];
            // splitKey = splitKey.Replace("_", " ").ToLower();
            // if (splitKey.Contains("_")) {
            // }

            Debug.Log($"Label original key: {key}");
        }
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
        if (tagClickManager.IsUIBlockingInput())
            return;

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log("Ray hit: " + hit.collider.name);
            if (!hit.collider.CompareTag("Clickable"))
                return;

            var label = hit.collider.GetComponentInParent<TagTarget>();
            if (label == null)
                return;

            // Activate viewedTag only if it hasn't been activated while panel is open
            if (!activatedTags.Contains(label.labelID))
            {
                if (labelToViewedTag.TryGetValue(label.labelID, out GameObject tagObj))
                {
                    if (tagObj != null)
                    {
                        tagObj.SetActive(true);
                        activatedTags.Add(label.labelID); // mark as activated
                    }
                }
            }

            // Look up and activate corresponding viewed tag
            if (labelToViewedTag.TryGetValue(label.labelID, out GameObject viewedTag))
            {
                if (viewedTag != null)
                {
                    string rawText = label
                        .labelID.Split("DescriptionCon")[0]
                        .Replace("_", " ")
                        .ToLower();

                    Debug.Log($"Raw text from label id on click: {rawText}");

                    userTagViewsController.CreateUserTagView(
                        UserState.Instance.Id,
                        rawText,
                        (r) =>
                        {
                            Debug.Log("Creation result: " + r);

                            // Check if unclocked all then able the finish button
                        },
                        (e) => Debug.LogError(e)
                    );

                    Debug.Log($"Tag view Label id: {label.labelID}");
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

    public bool IsTagActivated(string labelID)
    {
        return activatedTags.Contains(labelID);
    }

    public void ActivateTag(string labelID)
    {
        if (labelToViewedTag.TryGetValue(labelID, out GameObject viewedTag) && viewedTag != null)
        {
            viewedTag.SetActive(true);
            activatedTags.Add(labelID);
        }
    }

    public void ResetActivatedTags()
    {
        activatedTags.Clear();
    }
}
