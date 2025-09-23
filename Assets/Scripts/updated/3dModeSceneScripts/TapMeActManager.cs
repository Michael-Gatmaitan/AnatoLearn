using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class TapMeActManager : MonoBehaviour
{
    private Camera cam;

    private Label questionLabel;
    private Label correctScoreLabel;
    private Label wrongScoreLabel;
    private Label timeLimitLabel;
    private VisualElement tapMeActFinalAnswerPromptPage;
    private VisualElement tapMeActWrongAnswerPromptPage;
    private VisualElement tapMeActCorrectAnswerPromptPage;
    private Button nextTapQuestionBtn;

    private List<string> skeletalTapObjectsLabelId = new()
    {
        "skull",
        "scapula",
        "clavicle",
        "humerus",
        "ulna",
        "radius",
        "carpals",
        "metacarpals",
        "phalange",
        "femur",
        "patella",
        "tibia",
        "fibula",
        "tarsal",
        "metatarsal",
        "sternum",
        "ribs",
        "coccyx",
        "spine",
        "pelvicGirdle",
    };

    private List<string> digestiveTapObjectsLabelId = new()
    {
        "mouth",
        "esophagus",
        "stomach",
        "smallIntestine",
        "largeIntestine",
        "rectum",
    };

    private List<string> respiratoryTapObjectsLabelId = new()
    {
        "nasalCavity",
        "pharynx",
        "larynx",
        "lungs",
        "bronchi",
        "trachea",
    };

    private List<string> nervousTapObjectsLabelId = new()
    {
        "cerebrum",
        "hypothalamus",
        "medullaOblongata",
        "brainStem",
        "cerebellum",
    };
    private List<string> integumentaryTapObjectsLabelId = new()
    {
        "poreOfGland",
        "epidermis",
        "dermis",
        "hypodermis",
        "sweatGland",
        "hairRoot",
        "hairShaft",
    };
    private List<string> circulatoryTapObjectsLabelId = new()
    {
        "bloodVessels",
        "heart",
        "arteries",
        "veins",
        "capillaries",
    };

    private List<string> circulatoryHeartTapObjectsLabelId = new()
    {
        "rightAtrium",
        "leftAtrium",
        "mitralValve",
        "pulmonaryVein",
        "aorticValve",
        "inferiorVenaCava",
        "pulmonaryArtery",
        "superiorVenaCava",
        "leftVentricle",
        "tricuspidValve",
        "rightVentricle",
    };

    private List<string> excretoryTapObjectsLabelId = new()
    {
        "kidneys",
        "ureters",
        "urinaryBladder",
        "urethra",
    };

    private List<string> selectedQuestions = new();
    private int currentQuestionIndex = 0;
    private int correctScore = 0;
    private string pendingTappedID;
    private int wrongScore = 0;

    private float timeRemaining = 300f;
    private float finalTimeTaken = 0f;
    private bool isTapMeActActive = false;
    private bool hasInitializedTapMeAct = false;

    void Start()
    {
        HideTapObjectsMeshes();

        //check this by Michael if they're already in the quiz/tapMeAct, then HideAllOrangeLabels **
        // you may not use the "quizBtnIsClickd" in the sceneData, and use your owrn boolean.
        if (SceneData.quizBtnIsClicked)
        {
            HideAllOrangeLabels();
            // SceneData.quizBtnIsClicked = false; //I don't know if it's needed to turn it to false again, but its still working if not turn back to false.
        }

        // ShowTapCuesRedObjects();

        // SceneData.quizBtnIsClicked = true;
        //When the user closed the intruction pop up page, the this method will only run.
        // So that on the first run of 3dModeScene this will not run, and on the 2nd
        // run of scene it will run because the exit btn of  intruction page is now clicked
        if (!SceneData.exitInstrucTapMeActPageBtnIsClicked) //triggerd true in clicking quiz button
        {
            // return;
        }

        isTapMeActActive = true;
        cam = Camera.main;

        var uiDoc = GetComponent<UIDocument>();
        var root = uiDoc.rootVisualElement;

        questionLabel = root.Q<Label>("questionLabel");
        correctScoreLabel = root.Q<Label>("correctScoreLabel");
        wrongScoreLabel = root.Q<Label>("wrongScoreLabel");
        timeLimitLabel = root.Q<Label>("timeLimitLabel");

        tapMeActFinalAnswerPromptPage = root.Q<VisualElement>("tapMeActFinalAnswerPromptPage");
        tapMeActWrongAnswerPromptPage = root.Q<VisualElement>("tapMeActWrongAnswerPromptPage");
        tapMeActCorrectAnswerPromptPage = root.Q<VisualElement>("tapMeActCorrectAnswerPromptPage");

        var exitFinalAnswerPromptPageBtn = root.Q<Button>("exitFinalAnswerPromptPageBtn");
        var yesFinalAnswerBtn = root.Q<Button>("yesFinalAnswerBtn");
        var noFinalAnswerBtn = root.Q<Button>("noFinalAnswerBtn");
        var exitWrongAnswerPromptPageBtn = root.Q<Button>("exitWrongAnswerPromptPageBtn");
        var exitCorrectAnswerPromptPageBtn = root.Q<Button>("exitCorrectAnswerPromptPageBtn");

        nextTapQuestionBtn = root.Q<Button>("nextTapQuestionBtn");

        //Button Calbacks
        exitFinalAnswerPromptPageBtn?.RegisterCallback<ClickEvent>(evt =>
            HideFinalAnswerPromptPage()
        );
        // yesFinalAnswerBtn?.RegisterCallback<ClickEvent>(evt => HideFinalAnswerPromptPage());
        noFinalAnswerBtn?.RegisterCallback<ClickEvent>(evt => HideFinalAnswerPromptPage());

        yesFinalAnswerBtn?.RegisterCallback<ClickEvent>(evt =>
        {
            HideFinalAnswerPromptPage();
            if (!string.IsNullOrEmpty(pendingTappedID))
            {
                CheckAnswer(pendingTappedID);
                pendingTappedID = null; // reset
            }
        });

        exitCorrectAnswerPromptPageBtn?.RegisterCallback<ClickEvent>(evt =>
        {
            HideCorrectAnswerPromptPage();
            // turn OFF the raycast;
            SetRaycastActive(false);
        });

        exitWrongAnswerPromptPageBtn?.RegisterCallback<ClickEvent>(evt =>
        {
            HideWrongAnswerPromptPage();
            // turn OFF the raycast;
            SetRaycastActive(false);
        });

        nextTapQuestionBtn?.RegisterCallback<ClickEvent>(evt =>
        {
            HideNextTapQuestionBtn();
            //Do not call HideOrangeLabels if wanted to remain displaying the labels on the 3d mode while answering the tapMeAct **
            HideAllOrangeLabels();

            // When this Button is clciked then enable the raycast
            SetRaycastActive(true);

            // show the next question
            currentQuestionIndex++;
            if (currentQuestionIndex < selectedQuestions.Count)
            {
                ShowNextQuestion();
            }
            else
            {
                EndGame("Done!");
                GoToMcqPage();
            }
        });

        correctScoreLabel.text = "0";
        wrongScoreLabel.text = "0";

        // =====NOTE======
        // SceneData.studying(topicname), what ever it is, it will only be true when the user click the chosen topic on the homepage.
        // Choose random 5 questions based on what activity is taken
        // if (SceneData.studyingSkeletal)
        //     selectedQuestions = skeletalTapObjectsLabelId.OrderBy(x => Random.value).Take(5).ToList();
        // else if (SceneData.studyingDigestive)
        //     selectedQuestions = digestiveTapObjectsLabelId.OrderBy(x => Random.value).Take(5).ToList();
        // else if (SceneData.studyingRespiratory)
        //     selectedQuestions = respiratoryTapObjectsLabelId.OrderBy(x => Random.value).Take(5).ToList();
        // else if (SceneData.studyingNervous)
        //     selectedQuestions = nervousTapObjectsLabelId.OrderBy(x => Random.value).Take(5).ToList();
        // else if (SceneData.studyingIntegumentary)
        //     selectedQuestions = integumentaryTapObjectsLabelId.OrderBy(x => Random.value).Take(5).ToList();
        // else if (SceneData.studyingCirculatory)
        //     selectedQuestions = circulatoryTapObjectsLabelId.OrderBy(x => Random.value).Take(5).ToList();
        // else
        // {
        //     Debug.Log("Nothing chosen in TapMeActManager.cs");
        // }
    }

    void Update()
    {
        // =====NOTE======
        //When the user closed the intruction pop up page, the this method will only run.
        // So that on the first run of 3dModeScene this will not run, and on the 2nd
        // run of scene it will run because the exit btn of  intruction page is now clicked = "true"
        if (!SceneData.exitInstrucTapMeActPageBtnIsClicked)
        {
            return;
        }

        // if (!isTapMeActActive) return;

        if (SceneData.exitInstrucTapMeActPageBtnIsClicked && !hasInitializedTapMeAct)
        {
            InitializeTapMeAct();
        }

        if (!isTapMeActActive)
            return;

        // ⏱ Timer
        timeRemaining -= Time.deltaTime;
        timeRemaining = Mathf.Max(timeRemaining, 0);
        UpdateTimerLabel();

        if (timeRemaining <= 0)
        {
            EndGame("Time's up!");
            return;
        }

        // 👆 Input Detection
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray ray = cam.ScreenPointToRay(Input.GetTouch(0).position);
            DetectHit(ray);
        }
        else if (Input.GetMouseButtonDown(0) && Input.touchCount == 0)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            DetectHit(ray);
        }
        // else if (Input.GetMouseButtonUp(0) && Input.touchCount == 0)
    }

    void InitializeTapMeAct()
    {
        ShowTapCuesRedObjects();
        // HideAllOrangeLabels();


        if (hasInitializedTapMeAct)
            return;

        hasInitializedTapMeAct = true;
        isTapMeActActive = true;

        List<string> sourceList = null;

        if (SceneData.studyingSkeletal)
            sourceList = skeletalTapObjectsLabelId;
        else if (SceneData.studyingDigestive)
            sourceList = digestiveTapObjectsLabelId;
        else if (SceneData.studyingRespiratory)
            sourceList = respiratoryTapObjectsLabelId;
        else if (SceneData.studyingNervous)
            sourceList = nervousTapObjectsLabelId;
        else if (SceneData.studyingIntegumentary)
            sourceList = integumentaryTapObjectsLabelId;
        else if (SceneData.studyingCirculatory)
            sourceList = circulatoryTapObjectsLabelId;
        else if (SceneData.studyingExcretory)
            sourceList = excretoryTapObjectsLabelId;

        if (sourceList == null)
        {
            Debug.Log("Nothing chosen in TapMeActManager.cs");
            return;
        }

        // Pick random items (max 5)
        selectedQuestions = sourceList.OrderBy(x => Random.value).Take(5).ToList();

        // If there are fewer than 5, duplicate a random one from the chosen list
        while (selectedQuestions.Count < 5)
        {
            string duplicate = sourceList[Random.Range(0, sourceList.Count)];
            selectedQuestions.Add(duplicate);
        }

        ShowNextQuestion();
    }

    void DetectHit(Ray ray)
    {
        if (IsUIBlockingInput())
        {
            return;
        }

        //if exitButton IS CLICKED, then return
        if (!isRaycastActive)
        {
            return;
        }

        if (!isTapMeActActive || currentQuestionIndex >= selectedQuestions.Count)
            return;

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log("RAYCAST ******");
            var tagTarget = hit.collider.GetComponentInParent<TagTarget>();
            if (tagTarget == null)
                return;

            // Store tappedID temporarily
            pendingTappedID = tagTarget.labelID;

            // Show confirmation prompt
            // 1 ==displays a prompt page asking the user if it is his final answer (no & yes btns)
            ShowFinalAnswerPromptPage();
            // inside the ShowFinalAnswerPromptPage method are the yes and no btn logic
            // 2 == if no, do not proceed, dont  check if its correct or wrong - "return"
            // 3 == if yes, then proceed to if, else if statements checking the
        }
    }

    void EndGame(string finalMessage)
    {
        isTapMeActActive = false;
        finalTimeTaken = 300f - timeRemaining;
        questionLabel.text = finalMessage;

        Debug.Log($"✅ Final Time Taken: {finalTimeTaken:F2} seconds");
        Debug.Log($"✅ Correct Answers: {correctScore}");
        Debug.Log($"✅ Wrong Answers: {wrongScore}");

        UserState.Instance.SetQuizTimeRemaining(timeRemaining);
        UserState.Instance.SetTapScore(correctScore);
        UserState.Instance.isFromTapMe = true;
        SceneManager.LoadScene("UIScene1");

        // Store to global state if needed
        // SceneData.tapMeCorrect = correctScore;
        // SceneData.tapMeWrong = wrongScore;
        // SceneData.tapMeTime = finalTimeTaken;
    }

    void ShowNextQuestion()
    {
        if (currentQuestionIndex < selectedQuestions.Count)
        {
            string question = FormatCamelCase(selectedQuestions[currentQuestionIndex]);
            questionLabel.text = (currentQuestionIndex + 1) + ". " + question;
        }
    }

    string FormatCamelCase(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        // Insert space before every capital letter (except first char)
        string formatted = Regex.Replace(input, "(\\B[A-Z])", " $1");

        // Capitalize the very first character
        return char.ToUpper(formatted[0]) + formatted.Substring(1);
    }

    // string Capitalize(string text)
    // {
    //     return string.IsNullOrEmpty(text) ? text : char.ToUpper(text[0]) + text.Substring(1);
    // }

    void UpdateTimerLabel()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60f);
        timeLimitLabel.text = $"{minutes:00}:{seconds:00}";
    }

    void GoToMcqPage()
    {
        UnityEngine.Debug.Log(
            "Input your next code in the GoToMcqPage method, because the tap me act is finished now"
        );
    }

    void HideTapObjectsMeshes()
    {
        GameObject tapObjectsContainer = GameObject.Find("tapObjectsContainer");

        if (tapObjectsContainer == null)
        {
            Debug.LogWarning("❌ TapObjectsContainer not found.");
            return;
        }

        var meshRenderers = tapObjectsContainer.GetComponentsInChildren<MeshRenderer>(true);
        foreach (var renderer in meshRenderers)
        {
            renderer.enabled = false;
        }

        Debug.Log($"✅ Disabled {meshRenderers.Length} mesh renderers.");
    }

    //Red Marks
    public GameObject tapCuesContainer;

    void ShowTapCuesRedObjects()
    {
        // GameObject tapCuesContainer = GameObject.Find("tapCuesContainer");

        if (tapCuesContainer != null)
        {
            tapCuesContainer.SetActive(true);

            // foreach (Transform child in tapCuesContainer.transform)

            // {
            //     child.gameObject.SetActive(true);
            //     Debug.LogWarning("NAGTRUE CHILDREN CUES");
            // }
        }
        else
        {
            Debug.LogWarning("tapCuesContainer not found.");
        }
    }

    void ShowFinalAnswerPromptPage()
    {
        tapMeActFinalAnswerPromptPage.style.display = DisplayStyle.Flex;
    }

    void HideFinalAnswerPromptPage()
    {
        tapMeActFinalAnswerPromptPage.style.display = DisplayStyle.None;
    }

    void ShowWrongAnswerPromptPage()
    {
        tapMeActWrongAnswerPromptPage.style.display = DisplayStyle.Flex;
    }

    void HideWrongAnswerPromptPage()
    {
        tapMeActWrongAnswerPromptPage.style.display = DisplayStyle.None;
    }

    void ShowCorrectAnswerPromptPage()
    {
        tapMeActCorrectAnswerPromptPage.style.display = DisplayStyle.Flex;
    }

    void HideCorrectAnswerPromptPage()
    {
        tapMeActCorrectAnswerPromptPage.style.display = DisplayStyle.None;
    }

    void ShowNextTapQuestionBtn()
    {
        nextTapQuestionBtn.style.display = DisplayStyle.Flex;
    }

    void HideNextTapQuestionBtn()
    {
        nextTapQuestionBtn.style.display = DisplayStyle.None;
    }

    void CheckAnswer(string tappedID)
    {
        string expectedID = selectedQuestions[currentQuestionIndex];

        if (expectedID == "bloodvessels")
        {
            if (tappedID == "bloodvessels" || tappedID == "veins" || tappedID == "arteries")
            {
                correctScore++;
                correctScoreLabel.text = correctScore.ToString();
            }
            else
            {
                wrongScore++;
                wrongScoreLabel.text = wrongScore.ToString();
            }
        }
        else if (tappedID.ToLower() == expectedID.ToLower())
        { // CORRECT ANSWER
            // DISPLAY PROMPT that his answer is CORRECT
            ShowCorrectAnswerPromptPage();
            // after closing the prompt
            // display the child of "tagsContainer" with a name of "tagBodyPart / tagSweatGland" create a conversion of text "sweatGland" to "tagSweatGland"
            DisplayCorrectOrangeLabelTag(expectedID);
            ShowNextTapQuestionBtn();

            // click the next button , hide the child object, then proceed to next question.
            correctScore++;
            correctScoreLabel.text = correctScore.ToString();
        }
        else
        {
            // WRONG ANSWER
            // DISPLAY PROMPT that his answer is WRONG
            ShowWrongAnswerPromptPage();
            // after closing the prompt
            // display the child of "tagsContainer" with a name of "tagBodyPart / tagSweatGland" create a conversion of text "sweatGland" to "tagSweatGland"
            DisplayCorrectOrangeLabelTag(expectedID);
            ShowNextTapQuestionBtn();

            // click the next button , hide the child object, then proceed to next question.
            wrongScore++;
            wrongScoreLabel.text = wrongScore.ToString();
        }

        // // Proceed to next
        // currentQuestionIndex++;

        // if (currentQuestionIndex < selectedQuestions.Count)
        // {
        //     ShowNextQuestion();
        // }
        // else
        // {
        //     EndGame("Done!");
        //     GoToMcqPage();
        // }
    }

    // =========================
    // 🔶 ORANGE LABEL HANDLERS
    // =========================
    public GameObject tagsContainer; // assign in Inspector

    void DisplayCorrectOrangeLabelTag(string bodyPartID)
    {
        if (string.IsNullOrEmpty(bodyPartID))
            return;
        if (tagsContainer == null)
        {
            Debug.LogWarning("❌ TagsContainer reference is missing in Inspector.");
            return;
        }

        // Convert "sweatGland" -> "tagSweatGland"
        string tagName = "tag" + char.ToUpper(bodyPartID[0]) + bodyPartID.Substring(1);

        Transform child = tagsContainer.transform.Find(tagName);
        if (child != null)
        {
            child.gameObject.SetActive(true);
            Debug.Log($"✅ Showing orange label: {tagName}");
        }
        else
        {
            Debug.LogWarning($"❌ Child not found: {tagName}");
        }
    }

    void HideAllOrangeLabels()
    {
        if (tagsContainer == null)
            return;

        foreach (Transform child in tagsContainer.transform)
        {
            child.gameObject.SetActive(false); // hide each child
        }

        // ✅ Important: Never disable parent
        if (!tagsContainer.activeSelf)
            tagsContainer.SetActive(true);
    }

    private bool IsUIBlockingInput()
    {
        if (
            tapMeActFinalAnswerPromptPage != null
            && tapMeActFinalAnswerPromptPage.style.display == DisplayStyle.Flex
        )
        {
            UnityEngine.Debug.Log("true = tagclick");
            return true;
        }

        if (
            tapMeActCorrectAnswerPromptPage != null
            && tapMeActCorrectAnswerPromptPage.style.display == DisplayStyle.Flex
        )
        {
            UnityEngine.Debug.Log("true = tagclick");
            return true;
        }

        if (
            tapMeActWrongAnswerPromptPage != null
            && tapMeActWrongAnswerPromptPage.style.display == DisplayStyle.Flex
        )
        {
            UnityEngine.Debug.Log("true = tagclick");
            return true;
        }

        //false if no ui blocks
        return false;
    }

    private bool isRaycastActive = true;

    private void SetRaycastActive(bool value)
    {
        isRaycastActive = value;
    }
}

// NEXT TASK -->>>> disabled/hide all the green check mark, so that it will be hidden [[[I THINK IT MUST BE DONE BY MICHAEL SICNE ITS BASE ON THE DATABASE]]]
//  -->>[[[[BUT IF I WILL EXECUTE IT, I WILL LOOK on **...viewedTagClickManager]]]]
