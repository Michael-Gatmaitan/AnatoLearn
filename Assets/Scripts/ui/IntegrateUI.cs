using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.Video;

public class IntegrateUI : MonoBehaviour
{
    // public IntegrateUI instance;
    public UIDocument uiDocument;
    private VisualElement root;
    private VideoPlayer videoPlayer;
    public RenderTexture videoRenderTexture;

    // Add static instance
    public static IntegrateUI Instance { get; private set; }

    // Pages
    private VisualElement splashScreen;
    private VisualElement homePage;
    private VisualElement loginScreen;
    private VisualElement registrationScreen;
    private VisualElement popUpPage;
    private VisualElement quizPage;
    private VisualElement progressPage;
    private VisualElement progressTopicsPage; //added
    private VisualElement settingsPage;

    // Home page btn
    private Button progressBtn;
    private Button progressBackBtn;
    private Button settingsBtn;
    private Button logoutBtn;
    private Button exitSettingsBtn;

    // Page childs
    /// Home
    /// Popup
    private VisualElement videoContainer;
    private VisualElement progressionPage;
    private VisualElement exploreMorePage;

    private VisualElement lessonBtn;
    private VisualElement exploreBtn;
    private VisualElement quizBtn;

    // Lesson
    private Button playBtn;
    private VisualElement prevBtn;
    private VisualElement forwBtn;
    private VisualElement fsBtn;

    // Explore more BUTTONS AR/3D
    private Button btnAR;
    private Button btn3D;
    private Button exploreMoreBackBtn;

    // Quiz
    public static VisualElement mcqPage;
    public static VisualElement tofPage;
    public Button doneMcqButton; // Show and call TOF on click
    public Button doneTofButton; // Show and call Score page
    private static VisualElement scoresPage;
    private static Button scorePageContinueBtn;
    private static VisualElement sumScorePage;

    // Mcq splash
    private static VisualElement mcqSplash;
    private static Label mcqSplashQuestionIndex;
    private static Label mcqSplashQuestion;
    private static VisualElement choicesContainer;

    private Button lessonVideoFinishBtn;

    // Controllers
    private static ActScoresController actScoresController;
    private static ActQuestionController actQuestionController;
    private static TopicController topicController;
    private static TotalScoresController totalScoresController;
    private static UserTopicProgressController userTopicProgressController;

    private static bool isVideoPlaying = false;
    private static bool currentVideoValidToSkip = false;

    private static bool takingQuiz = false;

    private List<Topic> allTopics;

    public Sprite[] systemTopicSprites;
    public Sprite[] progressSprites;
    public Sprite[] progressLogoSprites;
    public Sprite[] progressionTopicTitle;

    public VideoPlayer fsVp;

    // Main root visual elements
    private VisualElement V_Main;
    private VisualElement V_VC;

    private string videoPath;
    private string portrait_vid_src;
    private SliderInt S_VC;

    private Label L_quizTimer;
    private float timeRemaining;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void OnEnable()
    {
        root = uiDocument.rootVisualElement;
        videoPlayer = GetComponent<VideoPlayer>();

        videoPlayer.seekCompleted += (VideoPlayer vp) =>
        {
            Debug.Log("Seek completed");
        };

        // Main UI
        V_Main = root.Q<VisualElement>("V_Main");
        V_VC = root.Q<VisualElement>("V_VC");

        S_VC = V_VC.Q<SliderInt>("S_VC");

        Debug.Log("Slider: " + S_VC);

        // Initialize progress
        progressPage = V_Main.Q<VisualElement>("progressPage");

        // Initialize home
        splashScreen = V_Main.Q<VisualElement>("splashScreen");
        homePage = V_Main.Q<VisualElement>("homePage");
        loginScreen = V_Main.Q<VisualElement>("loginScreen");
        registrationScreen = V_Main.Q<VisualElement>("registrationScreen");
        popUpPage = V_Main.Q<VisualElement>("popUpPage");
        quizPage = V_Main.Q<VisualElement>("quizPage");

        progressBtn = homePage.Q<Button>("progressBtn");
        progressBackBtn = V_Main.Q<Button>("progressBackBtn");
        progressTopicsPage = progressPage.Q<VisualElement>("progressTopicsPage"); //added
        settingsPage = V_Main.Q<VisualElement>("settingsPage");

        settingsBtn = homePage.Q<Button>("settingsBtn");
        exitSettingsBtn = settingsPage.Q<Button>("exitSettingsBtn");
        logoutBtn = settingsPage.Q<Button>("B_Logout");

        progressionPage = popUpPage.Q<VisualElement>("progressionPage"); //added

        lessonVideoFinishBtn = popUpPage.Q<Button>("lessonVideoFinishBtn");

        lessonBtn = progressionPage.Q<VisualElement>("lessonBtn");
        exploreBtn = progressionPage.Q<VisualElement>("exploreBtn");
        quizBtn = progressionPage.Q<VisualElement>("quizBtn");

        // Under progression
        exploreMorePage = popUpPage.Q<VisualElement>("exploreMorePage"); //added
        btnAR = exploreMorePage.Q<Button>("arModeBtn");
        btn3D = exploreMorePage.Q<Button>("3dModeBtn");
        exploreMoreBackBtn = exploreMorePage.Q<Button>("ExploreMoreBackBtn");

        // Initialize popup
        videoContainer = popUpPage.Q<VisualElement>("lessonVideoPage");

        // Lesson
        playBtn = videoContainer.Q<Button>("playBtn");
        prevBtn = videoContainer.Q<VisualElement>("prevBtn");
        forwBtn = videoContainer.Q<VisualElement>("forwBtn");
        fsBtn = videoContainer.Q<VisualElement>("fsBtn");

        // Initialize mcq, tof and scores
        mcqPage = quizPage.Q<VisualElement>("mcqPage");
        L_quizTimer = mcqPage.Q<Label>("L_quizTimer");
        tofPage = quizPage.Q<VisualElement>("tofPage");
        doneMcqButton = quizPage.Q<Button>("doneMcqActBtn");
        doneTofButton = V_Main.Q<Button>("doneTofActBtn"); //in quizPage, tofPage

        // Mcq splash
        mcqSplash = mcqPage.Q<VisualElement>("V_McqSplash");
        mcqSplashQuestionIndex = mcqSplash.Q<Label>("L_QuestionIndex");
        mcqSplashQuestion = mcqSplash.Q<Label>("L_McqQuestion");
        choicesContainer = mcqSplash.Q<VisualElement>("V_Choices");

        scoresPage = V_Main.Q<VisualElement>("scoresPage");
        scorePageContinueBtn = V_Main.Q<Button>("scorePageContinueBtn"); //in scoresPage

        sumScorePage = V_Main.Q<VisualElement>("sumScorePage");

        actScoresController = GetComponent<ActScoresController>();
        actQuestionController = GetComponent<ActQuestionController>();
        topicController = GetComponent<TopicController>();
        totalScoresController = GetComponent<TotalScoresController>();
        userTopicProgressController = GetComponent<UserTopicProgressController>();
    }

    void Start()
    {
        if (Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Debug.Log("Camera permission granted");
        }
        else
        {
            Debug.Log("Camera permission not granted");
            Permission.RequestUserPermission(Permission.Camera);
        }

        UserState.Instance.LoadUserData();

        if (actScoresController == null || actQuestionController == null)
        {
            Debug.LogError("ActScoresController component not found!");
            Debug.LogError("Controllers not found");
            return; // Exit early if controller is not found
        }

        splashScreen?.RegisterCallback<ClickEvent>(_ =>
        {
            splashScreen.style.display = DisplayStyle.None;
            loginScreen.style.display = DisplayStyle.Flex;
        });

        // SetupHomePageTouch();
        SetupPopUpPageButtons();
        SetupFullScreenVideoControl();
        // SetupPopUpPage();
        // SetupProgressPage(progressPage);
        SetupQuizPage();
        SetupSumScorePage();

        if (UserState.Instance.Id != 0)
        {
            //
            // UserState.Instance.SetTopicId(1);
            // SetupQuizes.SetupMCQContent2(mcqPage);

            // Hide splash screen if there's an id
            splashScreen.style.display = DisplayStyle.None;

            SetupHomeSystems(homePage);
            SetupProgressPage(progressPage);
            // SetupSettingsPage(settingsP);

            int topic_id = UserState.Instance.TopicId;
            bool isFromTapMe = UserState.Instance.isFromTapMe;

            if (topic_id != 0 && isFromTapMe)
            {
                homePage.style.display = DisplayStyle.None;
                UserState.Instance.isFromTapMe = false;
                // Get timer from UserState

                timeRemaining = UserState.Instance.QuizTimeRemaining;
                SetupQuizes.SetupMCQContent2(mcqPage);
                Debug.Log($"✅ Topic id is: {topic_id} and fromTapMe is: {isFromTapMe}");
            }
            else
            {
                // Normal flow is happening
                Debug.Log($"❌ Topic id is: {topic_id} and fromTapMe is: {isFromTapMe}");
                // homePage.style.display = DisplayStyle.Flex;
            }

            Debug.Log("Show progression page: " + UserState.Instance.showProgressionPage);
            if (UserState.Instance.GetShowProgressionPage())
            {
                ShowUnlockedProgression();
                UserState.Instance.showProgressionPage = false;
                UserState.Instance.SetShowProgressionPage(false);
            }
        }
        else
        {
            Debug.Log("Not loading yet because Id is 0");
            // Proceed to splash
            splashScreen.style.display = DisplayStyle.Flex;
            homePage.style.display = DisplayStyle.None;
            homePage.style.display = DisplayStyle.None;
        }

        // One time registration of callbacks
        progressBtn?.RegisterCallback<ClickEvent>(_ =>
        {
            SetupProgressPage(progressPage);

            progressPage.style.display = DisplayStyle.Flex;
            progressTopicsPage.style.display = DisplayStyle.Flex;
            homePage.style.display = DisplayStyle.None;
        });

        settingsBtn?.RegisterCallback<ClickEvent>(_ =>
        {
            popUpPage.style.display = DisplayStyle.Flex;
            settingsPage.style.display = DisplayStyle.Flex;
        });

        logoutBtn?.RegisterCallback<ClickEvent>(_ =>
        {
            UserState.Instance.ClearUserData();

            // Hide popup and settings page
            popUpPage.style.display = DisplayStyle.None;
            settingsPage.style.display = DisplayStyle.None;

            // Hide home page and show login paage
            loginScreen.style.display = DisplayStyle.Flex;
            homePage.style.display = DisplayStyle.None;

            Debug.Log("Checking if logged out: ");
            Debug.Log(UserState.Instance.Id);
            Debug.Log(UserState.Instance.Username);

            MessageBox(homePage, "Logged out");
        });

        exitSettingsBtn?.RegisterCallback<ClickEvent>(_ =>
        {
            popUpPage.style.display = DisplayStyle.None;
            settingsPage.style.display = DisplayStyle.None;
        });

        progressBackBtn?.RegisterCallback<ClickEvent>(_ =>
        {
            progressPage.style.display = DisplayStyle.None;
        });
    }

    // public void SetupSettingsPage(VisualElement settingsPage)
    // {
    //     Debug.Log($"Settings: {settingsPage}");

    //     Button logoutBtn = settingsPage.Q<Button>("B_Logout");
    //     Button closeBtn = settingsPage.Q<Button>("B_CloseSettings");

    //     closeBtn?.RegisterCallback<ClickEvent>(_ =>
    //     {
    //         Debug.Log("Closing settings");
    //         settingsPage.style.display = DisplayStyle.None;
    //         homePage.style.display = DisplayStyle.Flex;
    //     });

    //     logoutBtn?.RegisterCallback<ClickEvent>(_ =>
    //     {
    //         Debug.Log("Logging out");
    //         settingsPage.style.display = DisplayStyle.None;
    //         homePage.style.display = DisplayStyle.None;

    //         loginScreen.style.display = DisplayStyle.Flex;

    //         UserState.Instance.ClearUserData();
    //     });
    // }

    private void SetupSumScorePage()
    {
        Button homeBtn = sumScorePage.Q<Button>("B_HomeBtn");

        homeBtn?.RegisterCallback<ClickEvent>(_ =>
        {
            homePage.style.display = DisplayStyle.Flex;
            sumScorePage.style.display = DisplayStyle.None;
            tofPage.style.display = DisplayStyle.None;
            quizPage.style.display = DisplayStyle.None;
            popUpPage.style.display = DisplayStyle.None;

            SetupHomeSystems(homePage);
        });
    }

    private void SetupQuizPage()
    {
        // doneMcqButton.SetEnabled(false);
        // doneMcqButton?.RegisterCallback<ClickEvent>(evt =>
        // {
        //     // ShowScorePage();
        //     SetupQuizes.SetupScore();
        //     mcqPage.style.display = DisplayStyle.None;
        //     tofPage.style.display = DisplayStyle.Flex;
        //     // doneMcq = true;
        // });

        // doneTofButton?.RegisterCallback<ClickEvent>(evt =>
        // {
        //     SetupQuizes.SetupScore();
        //     // doneTof = true;
        // });

        scorePageContinueBtn?.RegisterCallback<ClickEvent>(_ =>
        {
            int tapScore = UserState.Instance.CurrentTapScore;
            int mcqScore = UserState.Instance.CurrentMCQScore;
            int tofScore = UserState.Instance.CurrentTOFScore;

            if (tapScore != -1 && mcqScore != -1 && tofScore != -1)
            {
                // Manipulate sumScorePage elements
                sumScorePage.style.display = DisplayStyle.Flex;
                // hide scoresPage
                scoresPage.style.display = DisplayStyle.None;

                int allScores = tapScore + mcqScore + tofScore;

                Debug.Log($"{tapScore} + {mcqScore} + {tofScore}");


                Label score = sumScorePage.Q<Label>("L_SSPScore");
                Label correctScore = sumScorePage.Q<Label>("correctScore");
                Label incorrectScore = sumScorePage.Q<Label>("incorrectScore");

                score.text = $"{(100 / 15) * allScores}%";

                // Display accuracy / performance

                correctScore.text = $"{allScores}";
                incorrectScore.text = $"{15 - allScores}";

                UserState.Instance.ResetAllScores();
            }
            else
            {
                Debug.LogError("Insufficient scores!");
            }
        });

        // void ShowScorePage()
        // {

        // }
    }

    private void ControlLockIconDisplay(VisualElement button, DisplayStyle displayStyle)
    {
        VisualElement lockIcon = button.parent.Q<VisualElement>("lockIcon");
        lockIcon.style.display = displayStyle;
    }

    public void ShowUnlockedProgression()
    {
        homePage.style.display = DisplayStyle.Flex;
        popUpPage.style.display = DisplayStyle.Flex;
        progressionPage.style.display = DisplayStyle.Flex;
        // Show modal first
        // int user_id = UserState.Instance.Id;
        int user_id = UserState.Instance.Id;
        int topic_id = UserState.Instance.TopicId;

        VisualElement exploreBtn = popUpPage.Q<VisualElement>("exploreBtn");
        VisualElement quizBtn = popUpPage.Q<VisualElement>("quizBtn");

        exploreBtn.SetEnabled(false);
        quizBtn.SetEnabled(false);

        VisualElement progressionTitleImg = progressionPage.Q<VisualElement>("V_ProgressTitleImg");

        progressionTitleImg.style.backgroundImage = new StyleBackground(
            progressionTopicTitle[topic_id - 1]
        );

        userTopicProgressController.GetOrCreateUserTopicProgressController(
            topic_id,
            user_id,
            (r) =>
            {
                if (r.lesson_completed)
                {
                    // Hide Lock Icon and enable the button
                    exploreBtn.SetEnabled(true);
                    ControlLockIconDisplay(exploreBtn, DisplayStyle.None);
                }
                else
                {
                    // Lock and disable the button if not
                    Debug.Log("Lesson not completed yet");
                    exploreBtn.SetEnabled(false);
                    ControlLockIconDisplay(exploreBtn, DisplayStyle.Flex);
                }

                Debug.Log(
                    $"Completed = lesson: {r.lesson_completed}, explore: {r.explore_completed}, act: {r.activities_completed}"
                );

                if (r.explore_completed)
                {
                    quizBtn.SetEnabled(true);
                    ControlLockIconDisplay(quizBtn, DisplayStyle.None);
                }
                else
                {
                    Debug.Log("Quiz not completed yet");
                    quizBtn.SetEnabled(false);
                    ControlLockIconDisplay(quizBtn, DisplayStyle.Flex);
                }
            },
            (e) => Debug.Log(e)
        );
    }

    private void SetupPopUpPageButtons()
    {
        Button progressionPageBackBtn = popUpPage.Q<Button>("progressionPageBackBtn");
        Button lessonVideoPageBackBtn = popUpPage.Q<Button>("lessonVideoPageBackBtn");
        // Button lessonVideoFinishBtn = popUpPage.Q<Button>("lessonVideoFinishBtn");

        // Video controls
        playBtn?.RegisterCallback<ClickEvent>(_ =>
        {
            if (videoPlayer.isPlaying)
            {
                MessageBox(homePage, "Paused");
                videoPlayer.Pause();
            }
            else
            {
                MessageBox(homePage, "Resumed");
                videoPlayer.Play();
            }
        });

        prevBtn?.RegisterCallback<ClickEvent>(_ =>
        {
            Debug.Log("Previous");
            MessageBox(homePage, "Video seeked -10 secons");
            videoPlayer.time -= 10;
        });

        forwBtn?.RegisterCallback<ClickEvent>(_ =>
        {
            Debug.Log("Forward");
            MessageBox(homePage, "Video seeked 10 secons");
            videoPlayer.time += 10;
        });

        fsVp.Pause();

        fsBtn?.RegisterCallback<ClickEvent>(_ =>
        {
            V_Main.style.display = DisplayStyle.None;
            V_VC.style.display = DisplayStyle.Flex;

            // videoPath = $"vids/lessons/{systemName}.mp4";
            int index = videoPath.LastIndexOf("/");
            string portrait_str = "portrait/";
            string updated_portrait_vid_src = videoPath.Insert(index + 1, portrait_str);

            if (portrait_vid_src != updated_portrait_vid_src)
            {
                Debug.Log($"Updated videoPath = {videoPath}");
                portrait_vid_src = updated_portrait_vid_src;
                fsVp.url = portrait_vid_src;
            }

            fsVp.Play();
            fsVp.time = videoPlayer.time;

            Debug.Log("Setting full screen");
            // fsVp.url = videoPlayer.url;
            // fsVp.time = videoPlayer.time;
            videoPlayer.Pause();

            // Exit fs should update the original vp currentTime and play it.
            // inject to exitfs btn
            // videoPlayer.time = fsVp.time;
            // videoPlayer.Play();
            // V_Main.style.display = DisplayStyle.Flex;
            // fsVp.Pause();
            // V_VC.style.display = DisplayStyle.None;

            // fsVp.
        });

        S_VC?.RegisterValueChangedCallback(
            (e) =>
            {
                Debug.Log($"Slider seeked: {e.newValue}");

                // Only update video time if fsVp is available and has a valid duration
                if (fsVp != null && fsVp.length > 0)
                {
                    // Map slider value (0-100) to video time (0 to video duration)
                    float normalizedValue = e.newValue / 100f;
                    float targetTime = normalizedValue * (float)fsVp.length;

                    fsVp.time = targetTime;
                    Debug.Log($"Set video time to: {targetTime} seconds (slider: {e.newValue}%)");
                }
                else
                {
                    Debug.Log($"FSVP: {fsVp}");
                    Debug.Log($"FSVP length: {fsVp.length}");
                }
            }
        );

        // S_VC?.RegisterCallback<ChangeEvent<int>>(
        //     (e) =>
        //     {
        //         Debug.Log($"Slider seeked: {e.newValue}");

        //         // Only update video time if fsVp is available and has a valid duration
        //         if (fsVp != null && fsVp.length > 0)
        //         {
        //             // Map slider value (0-100) to video time (0 to video duration)
        //             float normalizedValue = e.newValue / 100f;
        //             float targetTime = normalizedValue * (float)fsVp.length;

        //             fsVp.time = targetTime;
        //             Debug.Log($"Set video time to: {targetTime} seconds (slider: {e.newValue}%)");
        //         }
        //         else
        //         {
        //             Debug.Log($"FSVP: {fsVp}");
        //             Debug.Log($"FSVP length: {fsVp.length}");
        //         }
        //     }
        // );

        progressionPageBackBtn?.RegisterCallback<ClickEvent>(_ =>
        {
            UIScreenManager.Instance.HideProgressionPage();
        });

        lessonBtn?.RegisterCallback<ClickEvent>(_ =>
        {
            progressionPage.style.display = DisplayStyle.None;
            var topicId = UserState.Instance.TopicId;
            var topic = allTopics?.Find(t => t.id == topicId);
            if (topic != null)
            {
                ShowIntroVidPage(topic.id, topic.topic_name);
            }
            else
            {
                ShowIntroVidPage(topicId, "skeletal"); // Fallback
            }
        });

        lessonVideoPageBackBtn?.RegisterCallback<ClickEvent>(_ =>
        {
            UIScreenManager.Instance.HideLessonVideoPage(false);
            videoPlayer.Pause();

            isVideoPlaying = false;
        });

        lessonVideoFinishBtn?.RegisterCallback<ClickEvent>(_ =>
        {
            int currentTopicId = UserState.Instance.TopicId;
            string currentVideoUrl = videoPlayer.url;
            bool urlContainsSkeletal = currentVideoUrl.Contains("skeletal.mp4");
            bool urlContainsMuscoloskeletal = currentVideoUrl.Contains("muscoloskeletal");

            isVideoPlaying = false;

            // If the current topic is skeletal and it contains "skeletal"
            // and not contains "muscoloskeletal", change the video into skeletal
            if (currentTopicId == 1 && urlContainsSkeletal && !urlContainsMuscoloskeletal)
            {
                Debug.Log("Setup for muscoloskeletal video");
                ShowIntroVidPage(currentTopicId, "muscoloskeletal");
                return;
            }

            UIScreenManager.Instance.HideLessonVideoPage(false);
            videoPlayer.Pause();

            // Run this if the user finished the video
            userTopicProgressController.UpdateUserTopicProgress(
                UserState.Instance.Id, // user_id
                UserState.Instance.TopicId,
                "lesson",
                (r) =>
                {
                    Debug.Log("Explore unlocked: " + r);
                    ShowUnlockedProgression();
                },
                (e) => Debug.Log(e)
            );

            // ShowUnlockedProgression();
        });

        exploreBtn?.RegisterCallback<ClickEvent>(_ =>
        {
            Debug.Log("Opening explore more");
            popUpPage.style.display = DisplayStyle.Flex;
            exploreMorePage.style.display = DisplayStyle.Flex;
            progressionPage.style.display = DisplayStyle.None;
        });

        exploreMoreBackBtn.RegisterCallback<ClickEvent>(_ =>
        {
            Debug.Log("Hiding explore more page");
            exploreMorePage.style.display = DisplayStyle.None;
            ShowUnlockedProgression();
        });

        btnAR?.RegisterCallback<ClickEvent>(_ =>
        {
            var topic = allTopics?.Find(t => t.id == UserState.Instance.TopicId);
            Debug.Log("You chose AR for topic: " + (topic?.topic_name ?? "N/A"));

            if (topic?.topic_name != null)
            {
                // Set the topic_id for db interaction inside AR_Mode scenes
                UserState.Instance.SetTopicId(topic.id);
                PlayerPrefs.SetString("ChosenAR", topic.topic_name);
                Debug.Log($"{topic.topic_name} chosen in 3d mode.");
                SceneManager.LoadScene("Pose Landmark Detection");
            }
            else
            {
                Debug.LogError("Topic is not defined in AR Mode.");
            }
        });

        btn3D?.RegisterCallback<ClickEvent>(_ =>
        {
            SceneData.showTapActPage = false;

            var topic = allTopics?.Find(t => t.id == UserState.Instance.TopicId);
            Debug.Log("You chose 3D for topic: " + (topic?.topic_name ?? "N/A"));

            if (topic?.topic_name != null)
            {
                Debug.Log($"{topic.topic_name} chosen in 3d mode.");

                // Set the topic_id for db interaction inside 3d_Mode scenes
                // UserState.Instance.SetTopicId(topic.id);

                // Load scene for AR Mode
                // Temporarily unlock quiz by using "explore" as lesson_to_unlock?
                Debug.Log(UserState.Instance.TopicId);

                if (topic.topic_name == "skeletal")
                    SceneManager.LoadScene("3dModeSkeletalScene");
                else if (topic.topic_name == "integumentary")
                    SceneManager.LoadScene("3dModeIntegumentaryScene");
                else if (topic.topic_name == "respiratory")
                    SceneManager.LoadScene("3dModeRespiratoryScene");
                else if (topic.topic_name == "digestive")
                    SceneManager.LoadScene("3dModeDigestiveScene");
                else if (topic.topic_name == "circulatory")
                    SceneManager.LoadScene("3dModeCirculatoryScene");
                else if (topic.topic_name == "nervous")
                    SceneManager.LoadScene("3dModeNervousScene");
                else if (topic.topic_name == "excretory")
                    SceneManager.LoadScene("3dModeExcretoryScene");

                // userTopicProgressController.UpdateUserTopicProgress(
                //     UserState.Instance.Id,
                //     UserState.Instance.TopicId,
                //     "explore",
                //     (r) =>
                //     {
                //         Debug.Log("Activity unlocked: " + r);
                //         ShowUnlockedProgression();

                //         PlayerPrefs.SetString("Chosen3D", topic.topic_name);

                //         if (topic.topic_name == "skeletal")
                //         {
                //             SceneManager.LoadScene("3dModeSkeletalScene");
                //         }
                //         else if (topic.topic_name == "integumentary")
                //         {
                //             SceneManager.LoadScene("3dModeIntegumentaryScene");
                //         }
                //         else if (topic.topic_name == "respiratory")
                //         {
                //             SceneManager.LoadScene("3dModeRespiratoryScene");
                //         }
                //         else if (topic.topic_name == "digestive")
                //         {
                //             SceneManager.LoadScene("3dModeDigestiveScene");
                //         }
                //         else if (topic.topic_name == "circulatory")
                //         {
                //             SceneManager.LoadScene("3dModeCirculatoryScene");
                //         }
                //         else if (topic.topic_name == "nervous")
                //         {
                //             SceneManager.LoadScene("3dModeNervousScene");
                //         }
                //         else if (topic.topic_name == "excretory")
                //         {
                //             SceneManager.LoadScene("3dModeExcretoryScene");
                //         }
                //     },
                //     (e) =>
                //     {
                //         Debug.LogError(e);
                //     }
                // );

                exploreMorePage.style.display = DisplayStyle.None;
                // Close and refresh the progression page
            }
            else
            {
                Debug.LogError("Topic is not defined in AR Mode.");
            }
        });

        // TODO: Create or migrate a Modal popup after clicking QuizBtn
        quizBtn?.RegisterCallback<ClickEvent>(_ =>
        {
            var topic = allTopics?.Find(t => t.id == UserState.Instance.TopicId);

            var mcqPage = quizPage.Q<VisualElement>("mcqPage");
            var mcqSplash = quizPage.Q<VisualElement>("mcqSplash");

            // taking quiz = true
            takingQuiz = true;
            // Set quiz ui - get time remaining in UserState # UPDATE()

            progressionPage.style.display = DisplayStyle.None;
            homePage.style.display = DisplayStyle.None;

            // From revo -- if true, 3d mode will be tap me act
            SceneData.showTapActPage = true;

            Debug.Log($"Studying topic {topic.topic_name} in TapMe 3d Mode");

            // SceneData.ResetAllPartFlags();

            // Adter modal pops up, run code below to change the scene
            if (topic.topic_name == "skeletal")
            {
                SceneData.studyingSkeletal = true;
                SceneManager.LoadScene("3dModeSkeletalScene");
            }
            else if (topic.topic_name == "integumentary")
            {
                SceneData.studyingIntegumentary = true;
                SceneManager.LoadScene("3dModeIntegumentaryScene");
            }
            else if (topic.topic_name == "respiratory")
            {
                SceneData.studyingRespiratory = true;
                SceneManager.LoadScene("3dModeRespiratoryScene");
            }
            else if (topic.topic_name == "digestive")
            {
                SceneData.studyingDigestive = true;
                SceneManager.LoadScene("3dModeDigestiveScene");
            }
            else if (topic.topic_name == "circulatory")
            {
                SceneData.studyingCirculatory = true;
                SceneManager.LoadScene("3dModeCirculatoryScene");
            }
            else if (topic.topic_name == "nervous")
            {
                SceneData.studyingNervous = true;
                SceneManager.LoadScene("3dModeNervousScene");
            }

            // PlayerPrefs.SetString("tapActClicked");

            // SetupQuizes.SetupMCQContent(mcqPage); // Modify this
            // SetupQuizes.SetupMCQContent2(mcqPage);
        });
    }

    private void SetupFullScreenVideoControl()
    {
        Button B_Pause = V_VC.Q<Button>("B_PlayPause");
        Button B_Forw = V_VC.Q<Button>("B_Forw");

        B_Pause?.RegisterCallback<ClickEvent>(_ =>
        {
            Debug.Log("Pause");

            if (fsVp.isPaused)
            {
                fsVp.Play();
            }
            else
            {
                fsVp.Pause();
            }
        });

        B_Forw?.RegisterCallback<ClickEvent>(_ =>
        {
            fsVp.Pause();
            videoPlayer.time = fsVp.time;
            videoPlayer.Play();
            V_Main.style.display = DisplayStyle.Flex;
            V_VC.style.display = DisplayStyle.None;
        });
    }

    public void SetupHomeSystems(VisualElement homePage)
    {
        homePage.style.display = DisplayStyle.Flex;
        ScrollView sv = homePage.Q<ScrollView>("ScrollView");
        sv.Clear();

        // Setup profile username
        Label profileLabel = homePage.Q<Label>("profileLabel");
        profileLabel.text = UserState.Instance.Username;

        // int index = 0;

        topicController.GetAllTopics(
            (r) =>
            {
                allTopics = r.data;
                foreach (var topic in r.data)
                {
                    Debug.Log(topic.topic_name);

                    VisualElement systemContainer = new();
                    systemContainer.AddToClassList("V_SystemButton");

                    var capitalizedTopicName =
                        char.ToUpper(topic.topic_name[0]) + topic.topic_name[1..];
                    Label systemLabel = new(text: $"{capitalizedTopicName} System");
                    systemLabel.AddToClassList("SystemLabel");

                    // Change backgroundImage here of this element base on topic.id
                    VisualElement systemImage = new();
                    systemImage.AddToClassList("SystemImage");

                    // systemImage.style.backgroundImage = new StyleBackground(
                    //     Resources.Load<Sprite>($"Images/HomePageSystem/system{topic.id}.png")
                    // );

                    systemImage.style.backgroundImage = new StyleBackground(
                        systemTopicSprites[topic.id - 1]
                    );

                    // Debug.Log("Background image: " + systemImage.);

                    VisualElement topicLocked = new();
                    topicLocked.AddToClassList("TopicLocked");

                    systemContainer.Add(systemLabel);
                    systemContainer.Add(systemImage);
                    systemContainer.Add(topicLocked);

                    sv.Add(systemContainer);

                    bool locked = false;

                    if (topic.id == 1)
                    {
                        // Enabled
                        topicLocked.style.display = DisplayStyle.None;
                        locked = false;
                    }
                    else
                    {
                        // Check topic - 1 total_score
                        // If there is at least 1 passing score at previou topic, enable the topic
                        Debug.Log("User id in total scores controller: " + UserState.Instance.Id);
                        totalScoresController.GetTotalScoresByUserIdAndTopicId(
                            UserState.Instance.Id,
                            topic.id - 1,
                            true,
                            (r) =>
                            {
                                Debug.Log(r.data);
                                if (r.data.Count >= 1)
                                {
                                    // Enable here
                                    // systemLabel.text = "UnLocked!!!";

                                    Debug.Log($"Unlocked: {topic.topic_name}");
                                    topicLocked.style.display = DisplayStyle.None;
                                    locked = false;
                                }
                                else
                                {
                                    // systemLabel.text = "Locked!!!";
                                    Debug.Log($"Locked: {topic.topic_name}");
                                    topicLocked.style.display = DisplayStyle.Flex;
                                    locked = true;
                                }
                            },
                            (e) => Debug.LogError(e)
                        );
                    }

                    systemContainer?.RegisterCallback<ClickEvent>(_ =>
                    {
                        // UIScreenManager.Instance.ShowProgressionPage();

                        if (!locked)
                        {
                            UserState.Instance.SetTopicId(topic.id);
                            ShowUnlockedProgression();
                        }
                        else
                        {
                            Debug.Log(
                                "This topic is still locked, please passed the activity of recent topic first"
                            );
                            MessageBox(
                                root,
                                "You need to pass at previous topic before proceeding to this topic"
                            );
                        }
                    });
                }
            },
            (e) => Debug.Log(e)
        );
    }

    public void SetupProgressPage(VisualElement progressPage)
    {
        VisualElement progressTopicsPage = progressPage.Q<VisualElement>("progressTopicsPage"); //added

        VisualElement progressTopicTotalScoresPage = progressPage.Q<VisualElement>(
            "progressTopicTotalScoresPage"
        ); //added

        VisualElement progressTopicActScoresPage = progressPage.Q<VisualElement>(
            "progressTopicActScoresPage"
        ); //added

        Button progressBackBtn = progressPage.Q<Button>("progressBackBtn"); //added
        // Debug.Log("BACKGROUND IMAGE: " + progressBackBtn.style.backgroundImage);

        ScrollView progressScrollView = progressTopicsPage.Q<ScrollView>("ProgressScrollView");
        progressScrollView.Clear();

        Debug.Log("Setting up progress page");
        Debug.Log(topicController);

        topicController.GetAllTopics(
            (response) => DisplayTopics(response.data),
            (error) => Debug.LogError("Error on getting all topics")
        );

        void DisplayTopics(List<Topic> topics)
        {
            foreach (var topic in topics)
            {
                VisualElement progContainer = new();
                progContainer.AddToClassList("progContainer");

                // Prog image
                VisualElement progImageContainer = new();
                progImageContainer.AddToClassList("progImageContainer");

                VisualElement progImage = new();
                progImage.AddToClassList("progImage");

                progImage.style.backgroundImage = new StyleBackground(
                    progressSprites[topic.id - 1]
                );

                progImageContainer.Add(progImage);

                // Prog infos
                VisualElement progInfos = new();
                progInfos.AddToClassList("progInfos");

                Label progTitle = new(
                    text: char.ToUpper(topic.topic_name[0]) + topic.topic_name[1..]
                );
                progTitle.AddToClassList("progTitle");

                progInfos.Add(progTitle);

                VisualElement progDataFlex = new();
                progDataFlex.AddToClassList("progDataFlex");

                VisualElement progDataContainerL = new();
                progDataContainerL.AddToClassList("progDataContainerL");

                VisualElement progDataLCL = new();
                progDataLCL.AddToClassList("progDataLC");

                // Get attempts

                Label progDataLabelL = new(text: "...");
                progDataLabelL.AddToClassList("progDataLabel");

                VisualElement progDataAttempts = new();
                progDataAttempts.AddToClassList("progDataAttempts");

                Label progDataLabelAttempts = new(text: "Attempts");
                progDataLabelAttempts.AddToClassList("progDataLabelAttempts");

                VisualElement progDataContainerR = new();
                progDataContainerR.AddToClassList("progDataContainerR");

                VisualElement progDataLCR = new();
                progDataLCR.AddToClassList("progDataLC");

                // Get higest score

                Label progDataLabelR = new(text: "...");
                progDataLabelR.AddToClassList("progDataLabel");

                VisualElement progDataHighest = new();
                progDataHighest.AddToClassList("progDataAttempts");

                Label progDataLabelHighest = new(text: "Highest");
                progDataLabelHighest.AddToClassList("progDataLabelAttempts");

                progDataLCL.Add(progDataLabelL);
                progDataAttempts.Add(progDataLabelAttempts);

                progDataContainerL.Add(progDataLCL);
                progDataContainerL.Add(progDataAttempts);

                progDataLCR.Add(progDataLabelR);
                progDataHighest.Add(progDataLabelHighest);

                progDataContainerR.Add(progDataLCR);
                progDataContainerR.Add(progDataHighest);

                progDataFlex.Add(progDataContainerL);
                progDataFlex.Add(progDataContainerR);

                progInfos.Add(progDataFlex);

                progContainer.Add(progImageContainer);
                progContainer.Add(progInfos);

                progressScrollView.Add(progContainer);

                Debug.Log(UserState.Instance.Id);
                Debug.Log(topic.id);

                // This should be requested 1 time
                // instead of requesting every fucking topic

                totalScoresController.GetTotalAttempts(
                    UserState.Instance.Id,
                    topic.id,
                    (r) => progDataLabelL.text = $"{r.count}",
                    (e) => Debug.Log(e)
                );

                totalScoresController.GetTotalScoresByUserIdAndTopicId(
                    UserState.Instance.Id,
                    topic.id,
                    true,
                    (r) =>
                    {
                        // string t = r.data.Count > 0 ?  : "N/A";

                        progDataHighest.RemoveFromClassList("failedAccuracy");
                        progDataHighest.RemoveFromClassList("passedAccuracy");

                        if (r.data.Count > 0)
                        {
                            progDataLabelR.text = $"{r.data[0].total_score}";

                            if (r.data[0].accuracy < 60)
                                progDataHighest.AddToClassList("failedAccuracy");
                            else
                                progDataHighest.AddToClassList("passedAccuracy");
                        }
                        else
                        {
                            progDataLabelR.text = "N/A";
                        }
                    },
                    (e) => Debug.Log(e)
                );

                progContainer?.RegisterCallback<ClickEvent>(_ =>
                {
                    Debug.Log("Showing data from: " + topic.id);
                    // Function fow showing act history
                    DisplayProgressTopicTotalScores(topic.id);
                });

                progressTopicsPage.style.display = DisplayStyle.None;
                progressTopicsPage.style.display = DisplayStyle.Flex;
            }

            // Display specific scores of 1 topic
            void DisplayProgressTopicTotalScores(int topic_id)
            {
                // Access userid in user state
                progressTopicsPage.style.display = DisplayStyle.None;
                progressTopicTotalScoresPage.style.display = DisplayStyle.Flex;

                Debug.Log("History of activity");

                var mainContainer = progressTopicTotalScoresPage.Q("topicSumScoreMainContainer");
                Debug.Log("Main container? " + mainContainer);
                mainContainer.Clear();

                // GET TotalScores of this topic using topic_id and user_id
                totalScoresController.GetTotalScoresByUserIdAndTopicId(
                    UserState.Instance.Id,
                    topic_id,
                    false,
                    (response) =>
                    {
                        if (response != null)
                        {
                            foreach (var totalScore in response.data)
                            {
                                Debug.Log(totalScore.total_score);

                                VisualElement TotalSumScoreContainer = new();
                                TotalSumScoreContainer.AddToClassList("TopicSumScoreContainer");

                                VisualElement TopicLogo = new(); // Inside TotalSumScoreContainer
                                TopicLogo.AddToClassList("TopicLogo");
                                TopicLogo.style.backgroundImage = new StyleBackground(
                                    progressLogoSprites[totalScore.topic_id - 1]
                                );

                                VisualElement ScoreContainer = new(); // Inside TotalSumScoreContainer
                                ScoreContainer.AddToClassList("ScoreContainer");

                                // Button, V, V, Label, Padding
                                Button ShowActScoreBtn = new(); // Inside ScoreContainer
                                ShowActScoreBtn.AddToClassList("ShowActScoreBtn");

                                ShowActScoreBtn?.RegisterCallback<ClickEvent>(_ =>
                                { // Fetch and manipulate UI
                                    actScoresController.GetActScoresByTotalScoresId(
                                        totalScore.id,
                                        (response) =>
                                        {
                                            List<VisualElement> actScoresContainers =
                                                new List<VisualElement>
                                                {
                                                    progressTopicActScoresPage.Q<VisualElement>(
                                                        "tapScoreCon"
                                                    ),
                                                    progressTopicActScoresPage.Q<VisualElement>(
                                                        "mcqScoreCon"
                                                    ),
                                                    progressTopicActScoresPage.Q<VisualElement>(
                                                        "tofScoreCon"
                                                    ),
                                                };

                                            Debug.Log(response.data);
                                            foreach (var actScore in response.data)
                                            {
                                                Debug.Log(
                                                    $"Act_type_id: {actScore.act_type_id} score: {actScore.score}"
                                                );

                                                VisualElement currentScoreCon = actScoresContainers[
                                                    actScore.act_type_id - 1
                                                ];
                                                Label correctScoreLabel = currentScoreCon.Q<Label>(
                                                    "correctScore"
                                                );
                                                Label incorrectScoreLabel =
                                                    currentScoreCon.Q<Label>("incorrectScore");

                                                correctScoreLabel.text = $"{actScore.score}";
                                                incorrectScoreLabel.text = $"{5 - actScore.score}";
                                            }

                                            Label totalScoreDate =
                                                progressTopicActScoresPage.Q<Label>(
                                                    "totalScoreDate"
                                                );
                                            // totalScoreDate.text = totalScore.created_at;
                                            totalScoreDate.text =
                                                $"{DateTimeOffset.Parse(totalScore.created_at).ToLocalTime()}";

                                            progressTopicActScoresPage.style.display =
                                                DisplayStyle.Flex;
                                        },
                                        (error) =>
                                        {
                                            Debug.Log(
                                                "Something went wrong getting acts of total scores: "
                                                    + error
                                            );
                                        }
                                    );
                                });

                                VisualElement ScorePercentContainer = new(); // Inside ScoreContainer
                                ScorePercentContainer.AddToClassList("ScorePercentContainer");

                                VisualElement ScoreCon = new(); // Inside ScoreContainer
                                ScoreCon.AddToClassList("ScoreCon");

                                Label ScoreTime = new(text: "05:04"); // Inside ScoreContainer
                                ScoreTime.AddToClassList("ScoreTime");

                                VisualElement Padding = new(); // Inside ScoreContainer
                                Padding.AddToClassList("Padding");

                                ScoreContainer.Add(ShowActScoreBtn);
                                ScoreContainer.Add(ScorePercentContainer);
                                ScoreContainer.Add(ScoreCon);
                                ScoreContainer.Add(ScoreTime);
                                ScoreContainer.Add(Padding);

                                // ScoreParentContainer
                                VisualElement ScorePercentContent = new();
                                ScorePercentContent.AddToClassList("ScorePercentContent");

                                Label ScorePercent = new(text: $"{totalScore.accuracy}"); // Inside ScorePercentContent
                                ScorePercent.AddToClassList("ScorePercent");

                                Label ScoreLabel = new(text: "Performance");
                                ScoreLabel.AddToClassList("ScoreLabel");

                                ScorePercentContent.Add(ScorePercent);
                                ScorePercentContent.Add(ScoreLabel);

                                ScorePercentContainer.Add(ScorePercentContent);

                                VisualElement CScoreCon = new();
                                CScoreCon.AddToClassList("CScoreCon");

                                int totalItems = 15;

                                Label CScore = new(text: $"{totalScore.total_score}");
                                CScore.AddToClassList("CScore");

                                Label MScoreLabel = new(text: "Correct");
                                MScoreLabel.AddToClassList("MScoreLabel");

                                CScoreCon.Add(CScore);
                                CScoreCon.Add(MScoreLabel);

                                VisualElement IScoreCon = new();
                                IScoreCon.AddToClassList("IScoreCon");

                                Label IScore = new(text: $"{totalItems - totalScore.total_score}");
                                IScore.AddToClassList("IScore");

                                Label MScoreLabel2 = new(text: "Incorrect");
                                MScoreLabel2.AddToClassList("MScoreLabel");

                                IScoreCon.Add(IScore);
                                IScoreCon.Add(MScoreLabel2);

                                ScoreCon.Add(CScoreCon);
                                ScoreCon.Add(IScoreCon);

                                TotalSumScoreContainer.Add(ScoreContainer);
                                TotalSumScoreContainer.Add(TopicLogo);

                                mainContainer.Add(TotalSumScoreContainer);
                            }
                        }
                    },
                    (err) =>
                    {
                        Debug.Log(
                            "There was an error getting total score by user id and topic id: " + err
                        );
                    }
                );
            }

            // List<VisualElement> progressButtons = progressPage.Query<VisualElement>(className: "progressContainer").ToList();

            // foreach (var)
            progressBackBtn?.RegisterCallback<ClickEvent>(_ =>
            {
                Debug.Log(progressTopicTotalScoresPage.style.display);
                StyleEnum<DisplayStyle> topicTotalScorePageDisplay = progressTopicTotalScoresPage
                    .style
                    .display;

                Debug.Log($"Style of topic eme eme: {topicTotalScorePageDisplay}");

                if (topicTotalScorePageDisplay == DisplayStyle.Flex)
                {
                    progressTopicTotalScoresPage.style.display = DisplayStyle.None;
                    progressTopicsPage.style.display = DisplayStyle.Flex;
                }
                else
                {
                    progressPage.style.display = DisplayStyle.None;
                    progressPage.parent.Q("homePage").style.display = DisplayStyle.Flex;
                }
            });
        }
    }

    void ShowIntroVidPage(int topicId, string systemName)
    {
        isVideoPlaying = true;

        popUpPage.style.display = DisplayStyle.Flex;
        videoContainer.style.display = DisplayStyle.Flex;
        exploreMorePage.style.display = DisplayStyle.None;

        // Change image of title
        // lessonVideoFinishBtn.SetEnabled(false);


        int user_id = UserState.Instance.Id;
        int topic_id = UserState.Instance.TopicId;

        userTopicProgressController.GetOrCreateUserTopicProgressController(
            topic_id,
            user_id,
            (r) =>
            {
                Debug.Log($"Is lesson video finished playing? {r.lesson_completed}");
                if (r.lesson_completed)
                {
                    lessonVideoFinishBtn.SetEnabled(true);
                    // lessonVideoFinishBtn.style.backgroundImage = new StyleBackground(
                    //     Resources.Load<Sprite>("Images/enabledSkipBtn.png")
                    // );

                    // systemImage.style.backgroundImage = new StyleBackground(
                    //     Resources.Load<Sprite>($"Images/HomePageSystem/system{topic.id}.png")
                    // );
                }
                else
                {
                    lessonVideoFinishBtn.SetEnabled(false);
                    // lessonVideoFinishBtn.style.backgroundImage = new StyleBackground(
                    //     Resources.Load<Sprite>("Images/enabledSkipBtn.png")
                    // );
                }
            },
            (e) => Debug.Log(e)
        );

        videoPath = Path.Combine(Application.streamingAssetsPath, $"vids/lessons/{systemName}.mp4");

        Debug.Log(videoPath);

        Image videoImage = root.Q<Image>("videoImage");
        videoImage.image = videoRenderTexture;

        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = videoPath;
        videoPlayer.targetTexture = videoRenderTexture;

        // Play
        videoPlayer.time = 0;
        videoPlayer.Play();
    }

    public class SetupQuizes
    {
        // Manipulate questionaires here
        // Map through questionares, create element and classes,
        // and add a event logics
        public static void SetupMCQContent2(VisualElement mcqPage)
        {
            VisualElement nextQuestionBtnContainer = mcqPage.Q<VisualElement>(
                "V_NextQuestionBtnContainer"
            );
            Label scoreLabel = mcqPage.Q<Label>("scoreLabel");
            int currentScore = 0;

            mcqPage.parent.style.display = DisplayStyle.Flex;
            mcqPage.style.display = DisplayStyle.Flex;

            mcqSplash.style.display = DisplayStyle.Flex;

            int actId = UserState.Instance.SetActivityId("mcq");
            int topicId = UserState.Instance.TopicId;

            // Change the mcq title image here

            var quizTitleImg = mcqPage.Q<VisualElement>("quizTitleImg");
            Debug.Log("Quix title img: " + quizTitleImg);
            int topic_id = UserState.Instance.TopicId;

            quizTitleImg.style.backgroundImage = new StyleBackground(
                IntegrateUI.Instance.progressionTopicTitle[topicId - 1]
            );

            Debug.Log("Quiz title background image: " + quizTitleImg.style.backgroundImage);

            void DisplayQuestion(int index, List<QuestionMCQ> mcqQuestions)
            {
                var currentQuestion = mcqQuestions[index];

                mcqSplashQuestionIndex.text = $"Question {index + 1} of {mcqQuestions.Count}";
                mcqSplashQuestion.text = currentQuestion.question;

                choicesContainer.Clear();

                var choiceFields = typeof(QuestionChoices).GetFields();

                foreach (var choiceField in choiceFields)
                {
                    string key = choiceField.Name;
                    string value = choiceField.GetValue(currentQuestion.choices)?.ToString();

                    Label choiceLabel = new(text: $"{key}. {value}");
                    choiceLabel.AddToClassList("mcqChoice");
                    choiceLabel.AddToClassList("unselectedMcqChoice");
                    choiceLabel.EnableInClassList("selectedMcqChoice", false);

                    choicesContainer.Add(choiceLabel);
                }

                // Register click events of choices
                List<Label> choices = mcqPage.Query<Label>(className: "mcqChoice").ToList();
                bool corrected = false;
                int questionsLen = mcqQuestions.Count;

                // Disable the next question button first.
                Button nextQuestionBtn = mcqPage.Q<Button>("nextQuestionBtn");
                nextQuestionBtn.SetEnabled(false);

                foreach (var choice in choices)
                {
                    choice?.RegisterCallback<ClickEvent>(_ =>
                    {
                        // Then able the next question button here after clicking a choice.
                        nextQuestionBtn.SetEnabled(true);

                        var currChoice = choice;
                        foreach (var c in choices)
                        {
                            if (currChoice == c)
                            {
                                c.EnableInClassList("selectedMcqChoice", true);
                                c.EnableInClassList("unselectedMcqChoice", false);

                                string answer = choice.text[0].ToString();
                                if (currentQuestion.answer.Equals(answer))
                                {
                                    if (!corrected)
                                    {
                                        currentScore++;
                                        corrected = true;
                                        Debug.Log(
                                            $"Correct answer! Your score is now: {currentScore}"
                                        );
                                    }
                                }
                                else
                                {
                                    if (corrected)
                                    {
                                        currentScore--;
                                        corrected = false;
                                    }
                                }
                            }
                            else
                            {
                                c.EnableInClassList("selectedMcqChoice", false);
                                c.EnableInClassList("unselectedMcqChoice", true);
                            }
                        }

                        UserState.Instance.SetMCQScore(currentScore);
                        Debug.Log($"Current MCQ Score: {currentScore}");
                    });
                }
            }

            Debug.Log(nextQuestionBtnContainer);
            nextQuestionBtnContainer.Clear();

            actQuestionController.GetActQuestionByTopicId<QuestionMCQ>(
                UserState.Instance.TopicId,
                "mcq",
                (response) =>
                {
                    int questionIndex = 0;

                    // Create a button
                    Button nextQuestionBtn = new();
                    nextQuestionBtn.text = "Next question";
                    nextQuestionBtn.AddToClassList("nextQuestionBtn");
                    nextQuestionBtn.name = "nextQuestionBtn";

                    nextQuestionBtnContainer.Add(nextQuestionBtn);

                    nextQuestionBtn.RegisterCallback<ClickEvent>(_ =>
                    {
                        Debug.Log("Next btn clicked");
                        questionIndex++;

                        if (questionIndex >= response.data.Count)
                        {
                            // Call SetupTOF
                            SetupTOFContent2(mcqPage);
                            return;
                        }
                        else
                            DisplayQuestion(questionIndex, response.data);
                    });

                    DisplayQuestion(questionIndex, response.data);
                },
                (error) => Debug.LogError(error)
            );

            //Show splash
        }

        public static void SetupTOFContent2(VisualElement tofPage)
        {
            VisualElement nextQuestionBtnContainer = mcqPage.Q<VisualElement>(
                "V_NextQuestionBtnContainer"
            );
            Label scoreLabel = mcqPage.Q<Label>("scoreLabel");
            int currentScore = 0;

            int actId = UserState.Instance.SetActivityId("tof");
            int topicId = UserState.Instance.TopicId;

            void DisplayQuestion(int index, List<QuestionTOF> tofQuestions)
            {
                var currentQuestion = tofQuestions[index];

                mcqSplashQuestionIndex.text = $"Question {index + 1} of {tofQuestions.Count}";
                mcqSplashQuestion.text = currentQuestion.question;

                choicesContainer.Clear();

                // var choiceFields = typeof()
                Label choiceLabelT = new();
                Label choiceLabelF = new();

                choiceLabelT.AddToClassList("mcqChoice");
                choiceLabelT.AddToClassList("unselectedMcqChoice");
                choiceLabelT.EnableInClassList("selectedMcqChoice", false);

                choiceLabelF.AddToClassList("mcqChoice");
                choiceLabelF.AddToClassList("unselectedMcqChoice");
                choiceLabelF.EnableInClassList("selectedMcqChoice", false);

                choiceLabelT.text = "True";
                choiceLabelF.text = "False";

                choicesContainer.Add(choiceLabelT);
                choicesContainer.Add(choiceLabelF);

                List<Label> choices = tofPage.Query<Label>(className: "mcqChoice").ToList();
                bool corrected = false;
                int questionsLen = tofQuestions.Count;

                Button nextQuestionBtn = mcqPage.Q<Button>("nextQuestionBtn");
                nextQuestionBtn.SetEnabled(false);

                foreach (var choice in choices)
                {
                    choice?.RegisterCallback<ClickEvent>(_ =>
                    {
                        // Then able the next question button here after clicking a choice.
                        nextQuestionBtn.SetEnabled(true);

                        var currChoice = choice;
                        foreach (var c in choices)
                        {
                            if (c == currChoice)
                            {
                                c.EnableInClassList("selectedMcqChoice", true);
                                c.EnableInClassList("unselectedMcqChoice", false);

                                string answer = choice.text.ToString();
                                if (answer.ToLower().Equals(currentQuestion.answer))
                                {
                                    if (!corrected)
                                    {
                                        currentScore++;
                                        corrected = true;
                                        Debug.Log(
                                            $"Correct answer! Your score is now: {currentScore}"
                                        );
                                    }
                                }
                                else
                                {
                                    if (corrected)
                                    {
                                        currentScore--;
                                        corrected = false;
                                    }
                                }
                            }
                            else
                            {
                                c.EnableInClassList("selectedMcqChoice", false);
                                c.EnableInClassList("unselectedMcqChoice", true);
                            }
                        }
                        UserState.Instance.SetTOFScore(currentScore);
                        Debug.Log($"Current TOF Score: {currentScore}");
                    });
                }
            }

            nextQuestionBtnContainer.Clear();

            actQuestionController.GetActQuestionByTopicId<QuestionTOF>(
                UserState.Instance.TopicId,
                "tof",
                (response) =>
                {
                    // Create a button
                    Button nextQuestionBtn = new()
                    {
                        text = "Next question",
                        name = "nextQuestionBtn",
                    };

                    nextQuestionBtn.AddToClassList("nextQuestionBtn");

                    nextQuestionBtnContainer.Add(nextQuestionBtn);

                    int questionIndex = 0;
                    DisplayQuestion(questionIndex, response.data);

                    nextQuestionBtn.RegisterCallback<ClickEvent>(_ =>
                    {
                        questionIndex++;

                        if (questionIndex >= response.data.Count)
                        {
                            Debug.Log("Quiz Done");
                            // Show score
                            nextQuestionBtn.text = "Show results";

                            // Taking quiz false
                            takingQuiz = false;
                            SetupScore();
                            return;
                        }
                        else
                        {
                            Debug.Log(questionIndex);
                            Debug.Log(response.data.Count);
                            DisplayQuestion(questionIndex, response.data);
                        }
                    });
                },
                (error) => Debug.LogError(error)
            );
        }

        public static void SetupMCQContent(VisualElement mcqPage)
        {
            // Reset the state for PlayerPrefs.fromTapMe = 0
            PlayerPrefs.SetInt("fromTapeMe", 0);
            mcqPage.parent.style.display = DisplayStyle.Flex;
            mcqPage.style.display = DisplayStyle.Flex;

            // Set activity ID for DB/API actions
            int actId = UserState.Instance.SetActivityId("mcq");
            int topicId = UserState.Instance.TopicId;
            Debug.Log($"Recieved aid: {actId} tid: {topicId}");

            List<QuestionMCQ> _mcqQuestion;
            // get quizes
            actQuestionController.GetActQuestionByTopicId<QuestionMCQ>(
                UserState.Instance.TopicId,
                "mcq",
                (response) =>
                {
                    Debug.Log(response.data[0].choices);
                    if (response.data != null)
                    {
                        _mcqQuestion = response.data;
                        SetupMCQ(_mcqQuestion);
                        Debug.Log($"MCQ Successfully get");
                    }
                    else
                    {
                        Debug.LogError($"Cannot get MCQ of topic: {topicId}");
                    }
                },
                (error) =>
                {
                    Debug.LogError(error);
                }
            );

            void SetupMCQ(List<QuestionMCQ> mcqQuestion)
            {
                // mcqQuestion is not null
                int currentScore = 0;

                ScrollView mcqScrollView = mcqPage.Q<ScrollView>("ScrollView");
                mcqScrollView.Clear();

                Label scoreLabel = mcqPage.Q<Label>("scoreLabel");

                int questionsLen = mcqQuestion.Count;

                foreach (var questionItem in mcqQuestion)
                {
                    int index = mcqQuestion.IndexOf(questionItem) + 1;

                    VisualElement questionContainer = new();
                    questionContainer.AddToClassList("questionContainer");

                    VisualElement questionContent = new();
                    questionContent.AddToClassList("questionContent");

                    Label question = new();
                    question.AddToClassList("question");
                    question.text = questionItem.question;

                    questionContent.Add(question);

                    Dictionary<string, string> dict_choices = new Dictionary<string, string>();

                    var ps = typeof(QuestionChoices).GetFields();
                    Debug.Log(ps);
                    Debug.Log(ps.Length);

                    foreach (var p in ps)
                    {
                        string k = p.Name;
                        string v = p.GetValue(questionItem.choices)?.ToString();

                        Debug.Log($"Key: {k}, Value: {v}");

                        Label choiceLabel = new();
                        choiceLabel.AddToClassList("questionChoice");
                        choiceLabel.AddToClassList("questionChoiceHighlight");
                        choiceLabel.EnableInClassList("questionChoiceHighlight", false);

                        choiceLabel.text = $"{k} {v}";

                        questionContent.Add(choiceLabel);
                    }

                    Label questionLabel = new();
                    questionLabel.AddToClassList("questionLabel");
                    questionLabel.text = $"Question #{index}";

                    questionContainer.Add(questionContent);
                    questionContainer.Add(questionLabel);

                    // Add questionContainer to scrollView
                    mcqScrollView.Add(questionContainer);

                    List<Label> localChoices = questionContainer
                        .Query<Label>(className: "questionChoice")
                        .ToList();
                    Debug.Log(localChoices.Count);
                    bool corrected = false;
                    foreach (var localChoice in localChoices)
                    {
                        localChoice?.RegisterCallback<ClickEvent>(_ =>
                        {
                            var curLocalChoice = localChoice;
                            foreach (var _c in localChoices)
                            {
                                if (_c == curLocalChoice)
                                {
                                    _c.EnableInClassList("questionChoiceHighlight", true);
                                    _c.EnableInClassList("questionChoice", false);

                                    string answer = _c.text[0].ToString();
                                    Debug.Log(
                                        $"Selected answer: {answer}\nActual answer: {questionItem.answer}"
                                    );

                                    if (answer.Equals(questionItem.answer))
                                    {
                                        corrected = true;
                                        currentScore++;
                                        Debug.Log(
                                            $"Correct answer! Your score is now: {currentScore}"
                                        );
                                    }
                                    else
                                    {
                                        if (corrected)
                                        {
                                            currentScore--;
                                            corrected = false;
                                        }
                                    }
                                    scoreLabel.text = $"{currentScore} / {questionsLen}";
                                    UserState.Instance.SetMCQScore(currentScore);
                                }
                                else
                                {
                                    _c.EnableInClassList("questionChoiceHighlight", false);
                                    _c.EnableInClassList("questionChoice", true);
                                }
                            }
                        });
                    }
                }
                ;
            }
        }

        public static void SetupTOFContent(VisualElement tofPage)
        {
            int actId = UserState.Instance.SetActivityId("tof");
            int topicId = UserState.Instance.TopicId;
            Debug.Log($"Recieved aid: {actId} tid: {topicId}");

            List<QuestionTOF> _tofQuestion;
            // get quizes
            actQuestionController.GetActQuestionByTopicId<QuestionTOF>(
                UserState.Instance.TopicId,
                "tof",
                (response) =>
                {
                    if (response.data != null)
                    {
                        _tofQuestion = response.data;

                        SetupTOF(_tofQuestion);
                    }
                },
                (error) =>
                {
                    Debug.Log(error);
                }
            );

            void SetupTOF(List<QuestionTOF> tofQuestion)
            {
                int currentScore = 0;

                ScrollView tofScrollView = tofPage.Q<ScrollView>("ScrollView");
                tofScrollView.Clear();

                Label scoreLabel = tofPage.Q<Label>("scoreLabel");

                foreach (var questionSet in tofQuestion)
                {
                    Debug.Log(questionSet.question);
                    int index = tofQuestion.IndexOf(questionSet) + 1;

                    VisualElement questionContainer = new();
                    questionContainer.AddToClassList("questionContainer");

                    VisualElement questionContent = new();
                    questionContent.AddToClassList("questionContent");

                    Label question = new();
                    question.AddToClassList("question");
                    question.text = questionSet.question;

                    questionContent.Add(question);

                    Label choiceLabelT = new();
                    Label choiceLabelF = new();

                    choiceLabelT.AddToClassList("questionChoice");
                    choiceLabelT.AddToClassList("questionChoiceHighlight");
                    choiceLabelT.EnableInClassList("questionChoiceHighlight", false);

                    choiceLabelF.AddToClassList("questionChoice");
                    choiceLabelF.AddToClassList("questionChoiceHighlight");
                    choiceLabelF.EnableInClassList("questionChoiceHighlight", false);

                    choiceLabelT.text = "True";
                    choiceLabelF.text = "False";

                    questionContent.Add(choiceLabelT);
                    questionContent.Add(choiceLabelF);

                    Label questionLabel = new();
                    questionLabel.AddToClassList("questionLabel");
                    questionLabel.text = $"Question #{index}";

                    questionContainer.Add(questionContent);
                    questionContainer.Add(questionLabel);

                    tofScrollView.Add(questionContainer);

                    List<Label> localChoices = questionContainer
                        .Query<Label>(className: "questionChoice")
                        .ToList();
                    Debug.Log(localChoices.Count);
                    bool corrected = false;
                    foreach (var localChoice in localChoices)
                    {
                        localChoice?.RegisterCallback<ClickEvent>(_ =>
                        {
                            var curLocalChoice = localChoice;
                            foreach (var _c in localChoices)
                            {
                                if (_c == curLocalChoice)
                                {
                                    _c.EnableInClassList("questionChoiceHighlight", true);
                                    _c.EnableInClassList("questionChoice", false);

                                    string answer = _c.text.ToString();
                                    Debug.Log(
                                        $"Selected answer: {answer}\nActual answer: {questionSet.answer}"
                                    );

                                    if (answer.ToLower().Equals(questionSet.answer))
                                    {
                                        corrected = true;
                                        currentScore++;
                                        Debug.Log(
                                            $"Correct answer! Your score is now: {currentScore}"
                                        );
                                    }
                                    else
                                    {
                                        if (corrected)
                                        {
                                            currentScore--;
                                            corrected = false;
                                        }
                                    }
                                    scoreLabel.text = $"{currentScore} / {_tofQuestion.Count}";
                                    UserState.Instance.SetTOFScore(currentScore);
                                }
                                else
                                {
                                    _c.EnableInClassList("questionChoiceHighlight", false);
                                    _c.EnableInClassList("questionChoice", true);
                                }
                            }
                        });
                    }
                }
            }
        }

        public static void SetupScore()
        {
            int tapScore = UserState.Instance.CurrentTapScore;
            int mcqScore = UserState.Instance.CurrentMCQScore;
            int tofScore = UserState.Instance.CurrentTOFScore;

            int userId = UserState.Instance.Id;
            int actTypeId = UserState.Instance.ActivityId;
            int topicId = UserState.Instance.TopicId;

            Debug.Log(
                $"Checking values: userid {userId} acttypeid: {actTypeId} topicid: {topicId}"
            );

            // if (userId == 0 || actTypeId == 0 || topicId == 0)
            // {
            //     Debug.Log("Error params in creating score cannot be null");
            //     return;
            // }

            scoresPage.style.display = DisplayStyle.Flex;
            mcqPage.parent.style.display = DisplayStyle.None;
            mcqPage.style.display = DisplayStyle.None;

            void DisplayScore(Label L_Correct, Label L_Incorrect, int score, int totalItems = 5)
            {
                L_Correct.text = $"{score}";
                L_Incorrect.text = $"{totalItems - score}";
            }

            if (tapScore != -1)
            {
                // Display it
                DisplayScore(
                    scoresPage.Q<Label>("L_TapMeCorrectScore"),
                    scoresPage.Q<Label>("L_TapMeIncorrectScore"),
                    tapScore
                );
            }

            if (mcqScore != -1)
            {
                // Display it
                DisplayScore(
                    scoresPage.Q<Label>("L_MCQCorrectScore"),
                    scoresPage.Q<Label>("L_MCQIncorrectScore"),
                    mcqScore
                );
            }

            if (tofScore != -1)
            {
                // Display it
                DisplayScore(
                    scoresPage.Q<Label>("L_TOFCorrectScore"),
                    scoresPage.Q<Label>("L_TOFIncorrectScore"),
                    tofScore
                );
            }

            Debug.Log($"tap: {tapScore} mcq: {mcqScore} tof: {tofScore}");
            if (tapScore != -1 && mcqScore != -1 && tofScore != -1)
            {
                // all score are now complete
                // store them all
                Debug.Log("All scores are greater than -1");

                CreateTotalScoreBody reqBody = new()
                {
                    scores = new()
                    {
                        tap = tapScore,
                        mcq = mcqScore,
                        tof = tofScore,
                    },
                    // We can get them from UserState instance
                    user_id = UserState.Instance.Id,
                    topic_id = UserState.Instance.TopicId,
                };

                actScoresController.CreateActScoresWithTotalScore(
                    reqBody,
                    (response) =>
                    {
                        Debug.Log(response.message);
                        // If the request returns success = true,
                        // rest all scores into -1 again (default -1)
                        // if (response.success)
                        // {
                        //     UserState.Instance.ResetAllScores();
                        // }
                    },
                    (error) => Debug.LogError(error)
                );
            }

            Debug.Log($"Scoreeeesss mcq: {mcqScore}, tof: {tofScore}");
        }
    }

    // Parameter vs should be a page of the parent of vs should be the root VisualElement
    public static void MessageBox(VisualElement vs, string message)
    {
        VisualElement root = vs.parent.parent.parent;
        VisualElement messageBoxContainer = root.Q<VisualElement>("messageBox");

        Label messageBoxLabel = messageBoxContainer.Q<Label>("L_Message");

        messageBoxLabel.text = message;

        messageBoxContainer.style.display = DisplayStyle.Flex;

        Instance.StartCoroutine(HideMessageBox());

        IEnumerator<WaitForSeconds> HideMessageBox()
        {
            yield return new WaitForSeconds(3);
            messageBoxContainer.style.display = DisplayStyle.None;
        }
    }

    void Update()
    {
        if (isVideoPlaying)
        {
            // double videoDuration = videoPlayer.length;
            double currentTime = videoPlayer.time;
            // double validTimeLength = videoDuration / 2;
            // double validTimeLength = 2;

            // if (currentTime + 1 > validTimeLength)
            Debug.Log($"{currentTime}, {videoPlayer.length}");
            if (currentTime + 1 > videoPlayer.length && videoPlayer.length != 0)
            {
                Debug.Log("User exceed the full time of the video");
                if (!lessonVideoFinishBtn.enabledSelf)
                    lessonVideoFinishBtn.SetEnabled(true);
            }

            Debug.Log($"Current time: {currentTime} Video length: {videoPlayer.length}");

            // if (currentTime + 1 >= videoPlayer.length)
            // {
            //     Debug.Log($"Video finished {UserState.Instance.TopicId}");
            // }
        }

        // Handle quiz time
        if (takingQuiz)
        {
            // update time UI
            timeRemaining -= Time.deltaTime;
            timeRemaining = Mathf.Max(timeRemaining, 0);

            int minutes = Mathf.FloorToInt(timeRemaining / 60f);
            int seconds = Mathf.FloorToInt(timeRemaining % 60f);
            L_quizTimer.text = $"{minutes:00}:{seconds:00}";
        }

        if (fsVp.isPlaying && fsVp.length > 0)
        {
            // Map current time to slider value (0-100)
            float normalizedValue = (float)(fsVp.time / fsVp.length);
            int newValue = Mathf.Clamp(Mathf.RoundToInt(normalizedValue * 100f), 0, 100);
            Debug.Log("new value: " + newValue);
            S_VC.value = newValue;
        }
    }
}
