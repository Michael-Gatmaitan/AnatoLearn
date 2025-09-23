using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// using UnityEditor.SearchService;

public class TagClickManager : MonoBehaviour
{
    private Camera cam;
    private Dictionary<string, VisualElement> labelPanels = new();

    private VisualElement blackBackground; //for labelkpannels
    private VisualElement blackBgAbsoluteHomePromptPage; // for homepromptpage added 9-05
    private VisualElement blackBgAbsoluteChooseLanguagePage;

    private VisualElement blackBgAbsoluteClassDivVidCon;
    private VisualElement blackBgAbsoluteTagDescriptPage;
    private VisualElement blackBgAbsoluteFunFactPage;
    private VisualElement blackBgAbsoluteNeuronsCardsPage;

    void Start()
    {
        cam = Camera.main;

        var uiDoc = Object.FindFirstObjectByType<UIDocument>();
        if (uiDoc != null)
        {
            var root = uiDoc.rootVisualElement;
            blackBgAbsoluteClassDivVidCon = root.Q<VisualElement>("blackBgAbsoluteClassDivVidCon"); //added 9-05
            blackBgAbsoluteTagDescriptPage = root.Q<VisualElement>(
                "blackBgAbsoluteTagDescriptPage"
            ); //added 9-05
            blackBgAbsoluteFunFactPage = root.Q<VisualElement>("blackBgAbsoluteFunFactPage"); //added 9-05
            blackBgAbsoluteNeuronsCardsPage = root.Q<VisualElement>(
                "blackBgAbsoluteNeuronsCardsPage"
            ); //added 9-05
            blackBgAbsoluteChooseLanguagePage = root.Q<VisualElement>(
                "blackBgAbsoluteChooseLanguagePage"
            ); //added 9-05
            blackBgAbsoluteHomePromptPage = root.Q<VisualElement>("blackBgAbsoluteHomePromptPage"); //added 9-05
            blackBackground = root.Q<VisualElement>("blackBgAbsoluteTagDescriptPage");
            if (blackBackground != null)
                blackBackground.style.display = DisplayStyle.None;

            // Register all label panels you expect to show (add here as needed)

            //SKELETAL - TAG DESCRIPTION
            labelPanels["skullDescriptionCon"] = root.Q<VisualElement>("skullDescriptionCon");
            labelPanels["ribsDescriptionCon"] = root.Q<VisualElement>("ribsDescriptionCon");
            labelPanels["clavicleDescriptionCon"] = root.Q<VisualElement>("clavicleDescriptionCon");
            labelPanels["sternumDescriptionCon"] = root.Q<VisualElement>("sternumDescriptionCon");
            labelPanels["scapulaDescriptionCon"] = root.Q<VisualElement>("scapulaDescriptionCon");
            labelPanels["spineDescriptionCon"] = root.Q<VisualElement>("spineDescriptionCon");
            labelPanels["radiusDescriptionCon"] = root.Q<VisualElement>("radiusDescriptionCon");
            labelPanels["ulnaDescriptionCon"] = root.Q<VisualElement>("ulnaDescriptionCon");
            labelPanels["carpalsDescriptionCon"] = root.Q<VisualElement>("carpalsDescriptionCon");
            labelPanels["metacarpalsDescriptionCon"] = root.Q<VisualElement>(
                "metacarpalsDescriptionCon"
            );
            labelPanels["phalange_FingersDescriptionCon"] = root.Q<VisualElement>(
                "phalangeFingersDescriptionCon"
            );
            labelPanels["femurDescriptionCon"] = root.Q<VisualElement>("femurDescriptionCon");
            labelPanels["patellaDescriptionCon"] = root.Q<VisualElement>("patellaDescriptionCon");
            labelPanels["tibiaDescriptionCon"] = root.Q<VisualElement>("tibiaDescriptionCon");
            labelPanels["fibulaDescriptionCon"] = root.Q<VisualElement>("fibulaDescriptionCon");
            labelPanels["tarsalDescriptionCon"] = root.Q<VisualElement>("tarsalDescriptionCon");
            labelPanels["metatarsalDescriptionCon"] = root.Q<VisualElement>(
                "metatarsalDescriptionCon"
            );
            labelPanels["phalange_ToesDescriptionCon"] = root.Q<VisualElement>(
                "phalangeToesDescriptionCon"
            );
            labelPanels["humerusDescriptionCon"] = root.Q<VisualElement>("humerusDescriptionCon");
            labelPanels["coccyxDescriptionCon"] = root.Q<VisualElement>("coccyxDescriptionCon");
            labelPanels["pelvic_GirdleDescriptionCon"] = root.Q<VisualElement>(
                "pelvicGirdleDescriptionCon"
            );

            //INTEGUMENTARY - TAG DESCRIPTION
            labelPanels["hairShaftDescriptionCon"] = root.Q<VisualElement>(
                "hairShaftDescriptionCon"
            );
            labelPanels["sweatGlandDescriptionCon"] = root.Q<VisualElement>(
                "sweatGlandDescriptionCon"
            );
            labelPanels["hairRootDescriptionCon"] = root.Q<VisualElement>("hairRootDescriptionCon");
            labelPanels["pore_Of_GlandDescriptionCon"] = root.Q<VisualElement>(
                "poreOfGlandDescriptionCon"
            );
            labelPanels["epidermisDescriptionCon"] = root.Q<VisualElement>(
                "epidermisDescriptionCon"
            );
            labelPanels["dermisDescriptionCon"] = root.Q<VisualElement>("dermisDescriptionCon");
            labelPanels["hypodermisDescriptionCon"] = root.Q<VisualElement>(
                "hypodermisDescriptionCon"
            );

            //DIGESTIVE - TAG DESCRIPTION
            labelPanels["mouthDescriptionCon"] = root.Q<VisualElement>("mouthDescriptionCon");
            labelPanels["esophagusDescriptionCon"] = root.Q<VisualElement>(
                "esophagusDescriptionCon"
            );
            labelPanels["stomachDescriptionCon"] = root.Q<VisualElement>("stomachDescriptionCon");
            labelPanels["large_IntestineDescriptionCon"] = root.Q<VisualElement>(
                "largeIntestineDescriptionCon"
            );
            labelPanels["small_IntestineDescriptionCon"] = root.Q<VisualElement>(
                "smallIntestineDescriptionCon"
            );
            labelPanels["rectumDescriptionCon"] = root.Q<VisualElement>("rectumDescriptionCon");

            //RESPIRATORY - TAG DESCRIPTION
            labelPanels["pharynxDescriptionCon"] = root.Q<VisualElement>("pharynxDescriptionCon");
            labelPanels["nasal_CavityDescriptionCon"] = root.Q<VisualElement>(
                "nasalCavityDescriptionCon"
            );
            labelPanels["tracheaDescriptionCon"] = root.Q<VisualElement>("tracheaDescriptionCon");
            labelPanels["lungsDescriptionCon"] = root.Q<VisualElement>("lungsDescriptionCon");
            labelPanels["bronchiDescriptionCon"] = root.Q<VisualElement>("bronchiDescriptionCon");
            labelPanels["larynxDescriptionCon"] = root.Q<VisualElement>("larynxDescriptionCon");

            //NERVOUS - TAG DESCRIPTION
            labelPanels["cerebrumDescriptionCon"] = root.Q<VisualElement>("cerebrumDescriptionCon");
            labelPanels["hypothalamusDescriptionCon"] = root.Q<VisualElement>(
                "hypothalamusDescriptionCon"
            );
            labelPanels["medulla_OblongataDescriptionCon"] = root.Q<VisualElement>(
                "medullaOblongataDescriptionCon"
            );
            labelPanels["cerebellumDescriptionCon"] = root.Q<VisualElement>(
                "cerebellumDescriptionCon"
            );
            labelPanels["brain_StemDescriptionCon"] = root.Q<VisualElement>(
                "brainStemDescriptionCon"
            );

            //CIRCULATORY - TAG DESCRIPTION
            labelPanels["capillariesDescriptionCon"] = root.Q<VisualElement>(
                "capillariesDescriptionCon"
            );
            labelPanels["heartDescriptionCon"] = root.Q<VisualElement>("heartDescriptionCon");
            labelPanels["arteriesDescriptionCon"] = root.Q<VisualElement>("arteriesDescriptionCon");
            labelPanels["veinsDescriptionCon"] = root.Q<VisualElement>("veinsDescriptionCon");
            labelPanels["blood_VesselsDescriptionCon"] = root.Q<VisualElement>(
                "bloodVesselsDescriptionCon"
            );

            //CIRCULATORY --HEART - TAG DESCRIPTION
            labelPanels["right_AtriumDescriptionCon"] = root.Q<VisualElement>(
                "rightAtriumDescriptionCon"
            );
            labelPanels["tricuspid_ValveDescriptionCon"] = root.Q<VisualElement>(
                "tricuspidValveDescriptionCon"
            );
            labelPanels["right_VentricleDescriptionCon"] = root.Q<VisualElement>(
                "rightVentricleDescriptionCon"
            );
            labelPanels["inferior_VenaCavaDescriptionCon"] = root.Q<VisualElement>(
                "inferiorVenaCavaDescriptionCon"
            );
            labelPanels["pulmonary_ArteryDescriptionCon"] = root.Q<VisualElement>(
                "pulmonaryArteryDescriptionCon"
            );
            labelPanels["aortic_ValveDescriptionCon"] = root.Q<VisualElement>(
                "aorticValveDescriptionCon"
            );
            labelPanels["pulmonary_VeinDescriptionCon"] = root.Q<VisualElement>(
                "pulmonaryVeinDescriptionCon"
            );
            labelPanels["left_AtriumDescriptionCon"] = root.Q<VisualElement>(
                "leftAtriumDescriptionCon"
            );
            labelPanels["left_VentricleDescriptionCon"] = root.Q<VisualElement>(
                "leftVentricleDescriptionCon"
            );
            labelPanels["mitral_ValveDescriptionCon"] = root.Q<VisualElement>(
                "mitralValveDescriptionCon"
            );
            labelPanels["pulmonary_ValveDescriptionCon"] = root.Q<VisualElement>(
                "pulmonaryValveDescriptionCon"
            );
            labelPanels["superior_VenaCavaDescriptionCon"] = root.Q<VisualElement>(
                "superiorVenaCavaDescriptionCon"
            );

            //MUSCULAR - TAG DESCRIPTION

            //EXCRETORY - TAG DESCRIPTION
            labelPanels["kidneysDescriptionCon"] = root.Q<VisualElement>("kidneysDescriptionCon");
            labelPanels["uretersDescriptionCon"] = root.Q<VisualElement>("uretersDescriptionCon");
            labelPanels["urethraDescriptionCon"] = root.Q<VisualElement>("urethraDescriptionCon");
            labelPanels["urinary_BladderDescriptionCon"] = root.Q<VisualElement>(
                "urinaryBladderDescriptionCon"
            );

            // Hide all by default
            foreach (var panel in labelPanels.Values)
                panel.style.display = DisplayStyle.None;

            //set the tagName and Description to English
            SetLanguage("englishVersion");
        }
    }

    void Update()
    {
        //Because user is now in the tapMeAct, this script must be turneOFF, so that the descrip page will not be opened, when orange label displays and clicked for showing the correct answer in tap me act.
        if (SceneData.quizBtnIsClicked)
        {
            UnityEngine.Debug.Log("TagClickManager disabled");
            // Must be disabled when in the tapMeAct , so that the rayHit will be disabled. bec there is also a raycast in "TapMeActManager"
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            DetectHit(ray);

            UnityEngine.Debug.Log("tagClickmanager script runs");
        }
        else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray ray = cam.ScreenPointToRay(Input.GetTouch(0).position);
            DetectHit(ray);
        }
    }

    void DetectHit(Ray ray)
    {
        if (IsUIBlockingInput())
            return;

        //check this if user is in the quizmode or tapmeact then turn off the raycast so that it won't open the descripPage while in the tapMeAct **
        // if (SceneData.quizBtnIsClicked)
        //     return;

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log("Ray hit: " + hit.collider.name);

            if (hit.collider.CompareTag("Clickable"))
            {
                var label = hit.collider.GetComponentInParent<TagTarget>();
                if (label != null && labelPanels.TryGetValue(label.labelID, out var panel))
                {
                    Debug.Log("Displaying panel for: " + label.labelID);

                    // Hide all other panels first
                    foreach (var p in labelPanels.Values)
                        p.style.display = DisplayStyle.None;

                    // Show only the correct panel
                    panel.style.display = DisplayStyle.Flex;

                    // Show black background
                    blackBackground.style.display = DisplayStyle.Flex;

                    // Activate viewed tag only if this is a fresh click (panel was previously closed)
                    var skeletalViewedTagManager =
                        // FindObjectOfType<SkeletalViewedTagClickManager>();
                        FindFirstObjectByType<SkeletalViewedTagClickManager>();
                    if (
                        skeletalViewedTagManager != null
                        && !skeletalViewedTagManager.IsTagActivated(label.labelID)
                    )
                    {
                        skeletalViewedTagManager.ActivateTag(label.labelID);
                    }

                    var respiratoryViewedTagManager =
                        FindFirstObjectByType<RespiratoryViewedTagClickManager>();
                    if (
                        respiratoryViewedTagManager != null
                        && !respiratoryViewedTagManager.IsTagActivated(label.labelID)
                    )
                    {
                        respiratoryViewedTagManager.ActivateTag(label.labelID);
                    }

                    var nervousViewedTagManager =
                        FindFirstObjectByType<NervousViewedTagClickManager>();
                    if (
                        nervousViewedTagManager != null
                        && !nervousViewedTagManager.IsTagActivated(label.labelID)
                    )
                    {
                        nervousViewedTagManager.ActivateTag(label.labelID);
                    }

                    var integumentaryViewedTagManager =
                        FindFirstObjectByType<IntegumentaryViewedTagClickManager>();
                    if (
                        integumentaryViewedTagManager != null
                        && !integumentaryViewedTagManager.IsTagActivated(label.labelID)
                    )
                    {
                        integumentaryViewedTagManager.ActivateTag(label.labelID);
                    }

                    var excretoryViewedTagManager =
                        FindFirstObjectByType<ExcretoryViewedTagClickManager>();
                    if (
                        excretoryViewedTagManager != null
                        && !excretoryViewedTagManager.IsTagActivated(label.labelID)
                    )
                    {
                        excretoryViewedTagManager.ActivateTag(label.labelID);
                    }

                    var digestiveViewedTagManager =
                        FindFirstObjectByType<DigestiveSkeletalViewedTagClickManager>();
                    if (
                        digestiveViewedTagManager != null
                        && !digestiveViewedTagManager.IsTagActivated(label.labelID)
                    )
                    {
                        digestiveViewedTagManager.ActivateTag(label.labelID);
                    }

                    var circulatoryViewedTagManager =
                        FindFirstObjectByType<CirculatoryViewedTagClickManager>();
                    if (
                        circulatoryViewedTagManager != null
                        && !circulatoryViewedTagManager.IsTagActivated(label.labelID)
                    )
                    {
                        circulatoryViewedTagManager.ActivateTag(label.labelID);
                    }

                    // var circulatoryHeartViewedTagManager = FindObjectOfType<Circulatory_HeartViewedTagClickManager>();
                    // if (circulatoryHeartViewedTagManager != null && !circulatoryHeartViewedTagManager.IsTagActivated(label.labelID))
                    // {
                    //     circulatoryHeartViewedTagManager.ActivateTag(label.labelID);
                    // }
                }
                else
                {
                    Debug.LogWarning("TagTarget component missing or unknown labelID");
                }
            }
        }
    }

    public bool IsUIBlockingInput()
    {
        if (
            blackBgAbsoluteClassDivVidCon != null
            && blackBgAbsoluteClassDivVidCon.style.display == DisplayStyle.Flex
        )
        {
            UnityEngine.Debug.Log("true = tagclick");
            return true;
        }

        if (
            blackBgAbsoluteTagDescriptPage != null
            && blackBgAbsoluteTagDescriptPage.style.display == DisplayStyle.Flex
        )
        {
            UnityEngine.Debug.Log("true = tagclick");
            return true;
        }

        if (
            blackBgAbsoluteFunFactPage != null
            && blackBgAbsoluteFunFactPage.style.display == DisplayStyle.Flex
        )
        {
            UnityEngine.Debug.Log("true = tagclick");
            return true;
        }

        if (
            blackBgAbsoluteNeuronsCardsPage != null
            && blackBgAbsoluteNeuronsCardsPage.style.display == DisplayStyle.Flex
        )
        {
            UnityEngine.Debug.Log("true = tagclick");
            return true;
        }

        if (
            blackBgAbsoluteChooseLanguagePage != null
            && blackBgAbsoluteChooseLanguagePage.style.display == DisplayStyle.Flex
        )
        {
            UnityEngine.Debug.Log("true = tagclick");
            return true;
        }

        if (blackBackground != null && blackBackground.style.display == DisplayStyle.Flex)
        {
            UnityEngine.Debug.Log("true = tagclick");
            return true;
        }
        if (
            blackBgAbsoluteHomePromptPage != null
            && blackBgAbsoluteHomePromptPage.style.display == DisplayStyle.Flex
        )
        {
            UnityEngine.Debug.Log("true = tagclick");
            return true;
        }

        UnityEngine.Debug.Log("false = tagclick");
        return false;
    }

    public void SetLanguage(string languageVersion)
    {
        // Go through each label panel
        foreach (var pair in labelPanels)
        {
            string labelID = pair.Key; // Example: "skullDescriptionCon"
            VisualElement panel = pair.Value; // This is the UI container for the label

            // Find the label that shows the description
            Label descriptionLabel = panel.Q<Label>("TagDescription");

            // Find the label that shows the tag name (e.g., Skull or Bungo)
            Label nameLabel = panel.Q<Label>("tagName");

            // Set the description text based on the selected language
            if (
                descriptionLabel != null
                && LocalizedText.TagDescriptions.TryGetValue(labelID, out var desc)
            )
            {
                if (languageVersion == "tagalogVersion")
                    descriptionLabel.text = desc.tl; // Tagalog description
                else
                    descriptionLabel.text = desc.en; // English description
            }

            // Set the tag name text based on the selected language
            if (nameLabel != null && LocalizedText.TagNames.TryGetValue(labelID, out var name))
            {
                if (languageVersion == "tagalogVersion")
                    nameLabel.text = name.tl; // Tagalog name
                else
                    nameLabel.text = name.en; // English name
            }
        }
    }
}
