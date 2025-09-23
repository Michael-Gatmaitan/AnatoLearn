using UnityEngine;
using UnityEngine.UIElements;

public class UIScreenManager : MonoBehaviour
{
    public static UIScreenManager Instance { get; private set; }

    private VisualElement splashScreen;
    private VisualElement loginScreen;
    private VisualElement registrationScreen;
    private VisualElement homePage;
    private VisualElement popUpPage;
    private VisualElement modeOfLearningPage;
    private VisualElement videoContainer;
    private VisualElement quizPage;
    private VisualElement mcqPage;
    private VisualElement tofPage;
    private VisualElement scoresPage;
    private VisualElement sumScorePage;
    private VisualElement certPage;

    private VisualElement progressPage; //added
    private VisualElement progressTopicsPage; //added
    private VisualElement progressTopicTotalScoresPage; //added
    private VisualElement progressTopicActScoresPage; //added
    private VisualElement progressionPage; //added
    private VisualElement lessonVideoPage; //added

    private VisualElement exploreBtn;
    private VisualElement exploreLockIcon;
    private VisualElement quizBtn;
    private VisualElement quizLockIcon;
    private VisualElement exploreMorePage;

    // Switches / Flags    // ginawa ko lng global to (added)
    bool doneMcqAct = false;
    bool doneTofAct = false;

    bool onProgressTopicTotalScoresPage = false;
    bool onProgressTopicsPage = false;

    private void OnEnable()
    {
        Instance = this;
        var root = GetComponent<UIDocument>().rootVisualElement;

        splashScreen = root.Q<VisualElement>("splashScreen");
        loginScreen = root.Q<VisualElement>("loginScreen");
        registrationScreen = root.Q<VisualElement>("registrationScreen");
        homePage = root.Q<VisualElement>("homePage");
        popUpPage = root.Q<VisualElement>("popUpPage");
        modeOfLearningPage = root.Q<VisualElement>("modeOfLearningPage");
        videoContainer = root.Q<VisualElement>("videoContainer");
        quizPage = root.Q<VisualElement>("quizPage");
        mcqPage = root.Q<VisualElement>("mcqPage");
        tofPage = root.Q<VisualElement>("tofPage");
        scoresPage = root.Q<VisualElement>("scoresPage");
        sumScorePage = root.Q<VisualElement>("sumScorePage");
        certPage = root.Q<VisualElement>("certPage");

        progressPage = root.Q<VisualElement>("progressPage"); //added
        // progressTopicsPage = root.Q<VisualElement>("progressTopicsPage"); //added
        progressTopicTotalScoresPage = root.Q<VisualElement>("progressTopicTotalScoresPage"); //added
        progressTopicActScoresPage = root.Q<VisualElement>("progressTopicActScoresPage"); //added

        progressionPage = root.Q<VisualElement>("progressionPage"); //added
        lessonVideoPage = root.Q<VisualElement>("lessonVideoPage"); //added
        exploreLockIcon = root.Q<VisualElement>("exploreLockIcon"); //added
        quizLockIcon = root.Q<VisualElement>("quizLockIcon"); //added
        exploreMorePage = root.Q<VisualElement>("exploreMorePage"); //added

        // Buttons
        var startSplashButton = root.Q<VisualElement>("splashScreen"); // in splashScreen
        var toRegisterButton = root.Q<Button>("toRegisterButton"); // in loginScreen
        var toLoginButton = root.Q<Button>("toLoginButton"); // in registrationScreen
        var goToSplash = root.Q<VisualElement>("lastSystem"); //in homePage, last system
        var skeletalBtn = root.Q<VisualElement>("skeletal"); //in homePage, first system
        var nextBtn = root.Q<Button>("nextBtn"); //in popUpPage, in vidContainer Element
        var doneMcqActBtn = root.Q<Button>("doneMcqActBtn"); //in quizPage, in mcqPage
        var doneTofActBtn = root.Q<Button>("doneTofActBtn"); //in quizPage, tofPage
        var scorePageContinueBtn = root.Q<Button>("scorePageContinueBtn"); //in scoresPage
        var viewCertBtn = root.Q<Button>("viewCertificateBtn"); //in sumScorePage

        // var progressBtn = root.Q<Button>("progressBtn"); //added
        var progressBackBtn = root.Q<Button>("progressBackBtn"); //added

        // var skeletalProgress = root.Q<VisualElement>("skeletalProgress"); //added //button in progressTopicPage
        var showActScoreBtn = root.Q<Button>("showActScoreBtn"); //added //button in progressTopicTotalScoresPage
        var actScoresPageBackBtn = root.Q<Button>("actScoresPageBackBtn"); //added //button in progressTopicActScoresPage

        var progressionPageBackBtn = root.Q<Button>("progressionPageBackBtn"); //added //button in progressionPage
        var lessonBtn = root.Q<VisualElement>("lessonBtn"); //added //button in progressionPage
        var lessonVideoPageBackBtn = root.Q<VisualElement>("lessonVideoPageBackBtn"); //added //button in lessonVideoPage
        var lessonVideoFinishBtn = root.Q<VisualElement>("lessonVideoFinishBtn"); //added //button in lessonVideoPage
        exploreBtn = root.Q<VisualElement>("exploreBtn"); //added //button in progressionPage
        quizBtn = root.Q<VisualElement>("quizBtn"); //added //button in progressionPage

        // Button Callbacks
        startSplashButton?.RegisterCallback<ClickEvent>(evt => ShowLogin());
        toRegisterButton?.RegisterCallback<ClickEvent>(evt => ShowLogin());
        toLoginButton?.RegisterCallback<ClickEvent>(evt => ShowHomePage());
        goToSplash?.RegisterCallback<ClickEvent>(evt => ShowSplash());
        skeletalBtn?.RegisterCallback<ClickEvent>(evt => ShowProgressionPage());
        nextBtn?.RegisterCallback<ClickEvent>(evt => ShowModeOfLearningPage());

        viewCertBtn?.RegisterCallback<ClickEvent>(evt =>
        {
            certPage.style.display = DisplayStyle.Flex;
            Debug.Log("viewcertbtn clicked!");
        });

        // doneTofActBtn?.RegisterCallback<ClickEvent>(evt =>
        // {
        //     ShowScorePage();
        //     doneTofAct = true;
        // });

        scorePageContinueBtn?.RegisterCallback<ClickEvent>(evt =>
        {
            if (doneMcqAct)
            {
                ShowTofPage();
                // hideMcqPage
                mcqPage.style.display = DisplayStyle.None;
                // hideFScore page
                scoresPage.style.display = DisplayStyle.None;
                UnityEngine.Debug.Log("DoneMCQ trueee");
                doneMcqAct = false;
            }
            else if (doneTofAct)
            {
                //show sumscore
                sumScorePage.style.display = DisplayStyle.Flex;
                // hide scoresPage
                scoresPage.style.display = DisplayStyle.None;

                UnityEngine.Debug.Log("DoneTOf trueee");

                doneTofAct = false;
            }
            else
            {
                ShowQuizPage();
                ShowMcqPage();
                // Hide scorepage
                scoresPage.style.display = DisplayStyle.None;
            }
        }); //in scorePage

        // doneMcqActBtn?.RegisterCallback<ClickEvent>(evt =>
        // {
        //     ShowScorePage();
        //     mcqPage.style.display = DisplayStyle.None;
        //     tofPage.style.display = DisplayStyle.Flex;
        //     doneMcqAct = true;
        // });

        // progressBtn?.RegisterCallback<ClickEvent>(evt => ShowProgressTopicsPage()); //added
        // progressBackBtn?.RegisterCallback<ClickEvent>(evt => HideProgressPage()); //added
        // progressBackBtn?.RegisterCallback<ClickEvent>(evt => toggleProgressBackBtn()); //added

        // skeletalProgress?.RegisterCallback<ClickEvent>(evt => ShowProgressTopicTotalScoresPage()); //added
        showActScoreBtn?.RegisterCallback<ClickEvent>(evt => ShowProgressTopicActScoresPage()); //added
        actScoresPageBackBtn?.RegisterCallback<ClickEvent>(evt => HideProgressTopicActScoresPage()); //added

        // progressionPageBackBtn?.RegisterCallback<ClickEvent>(evt => HideProgressionPage()); //added

        // lessonBtn?.RegisterCallback<ClickEvent>(evt => ShowLessonVideoPage()); //added

        // lessonVideoPageBackBtn?.RegisterCallback<ClickEvent>(evt => HideLessonVideoPage(false)); //adde

        // lessonVideoFinishBtn?.RegisterCallback<ClickEvent>(evt => HideLessonVideoPage(true)); //adde

        // exploreBtn?.RegisterCallback<ClickEvent>(evt => ShowExploreMorePage()); //added

        // For QUIZES PAGE
        // Show proper screen based on SceneData. This code is for the button in 3dModeScene when click, it will show quiz page, on top of it is scorepage
        if (SceneData.showScorePage) //if it is set to true showscorepage()
        {
            SceneData.showScorePage = false; // reset it
            ShowScorePage();
            ShowQuizPage();
            ShowMcqPage();
        }
        // From 3dMode to UI scene 1. PopUpPage, ProgressionPage = flex.
        else if (SceneData.showProgressionPage) //if it is set to true showscorepage()
        {
            SceneData.showProgressionPage = false;
            ShowHomePage();
            // ShowProgressionPage();

            // exploreBtn.SetEnabled(true);
            // exploreLockIcon.style.display = DisplayStyle.None;

            // quizBtn.SetEnabled(true);
            // quizLockIcon.style.display = DisplayStyle.None;
        }
        else
        {
            // if (UserState.Instance.Id != 0)
            // {
            //     ShowHomePage();
            // }
            // else
            // {
            //     ShowSplash(); // Initial screen
            // }
        }
    }

    private void ShowExploreMorePage()
    {
        popUpPage.style.display = DisplayStyle.Flex;
        exploreMorePage.style.display = DisplayStyle.Flex;
        progressionPage.style.display = DisplayStyle.None;
    }

    public void HideLessonVideoPage(bool lessonVideoFinishBtnIsClicked) //added
    {
        lessonVideoPage.style.display = DisplayStyle.None;
        popUpPage.style.display = DisplayStyle.Flex;
        progressionPage.style.display = DisplayStyle.Flex;

        // Update UserTopicProgress - lesson

        // if (lessonVideoFinishBtnIsClicked == true)
        // {
        //     UnityEngine.Debug.Log("Lesson finished! Now doing something extra.");
        //     //call the function responsible for hiding the lockIcon visual elem
        //     exploreBtn.SetEnabled(true);
        //     exploreLockIcon.style.display = DisplayStyle.None;
        // }
    }

    public void ShowLessonVideoPage() //added
    {
        popUpPage.style.display = DisplayStyle.Flex;
        lessonVideoPage.style.display = DisplayStyle.Flex;
        progressionPage.style.display = DisplayStyle.None;
    }

    private void HideProgressTopicActScoresPage() //added
    {
        progressTopicActScoresPage.style.display = DisplayStyle.None;
    }

    private void ShowProgressTopicActScoresPage() //added
    {
        progressTopicActScoresPage.style.display = DisplayStyle.Flex;
    }

    // private void ShowProgressTopicTotalScoresPage() //added
    // {
    //     onProgressTopicTotalScoresPage = true;
    //     progressTopicsPage.style.display = DisplayStyle.None;
    //     progressTopicTotalScoresPage.style.display = DisplayStyle.Flex;
    // }

    private void toggleProgressBackBtn() //added
    {
        if (onProgressTopicsPage)
        {
            progressPage.style.display = DisplayStyle.None;
            progressTopicsPage.style.display = DisplayStyle.None;
            homePage.style.display = DisplayStyle.Flex;
            onProgressTopicsPage = false;
        }

        if (onProgressTopicTotalScoresPage)
        {
            //     ShowProgressTopicsPage();
            progressTopicTotalScoresPage.style.display = DisplayStyle.None;
            onProgressTopicTotalScoresPage = false;
        }
    }

    // private void ShowProgressTopicsPage() //added
    // {
    //     onProgressTopicsPage = true;
    //     progressPage.style.display = DisplayStyle.Flex;
    //     progressTopicsPage.style.display = DisplayStyle.Flex;
    //     homePage.style.display = DisplayStyle.None;
    // }

    private void ShowScorePage()
    {
        scoresPage.style.display = DisplayStyle.Flex;
    }

    private void ShowSplash()
    {
        splashScreen.style.display = DisplayStyle.Flex;
        loginScreen.style.display = DisplayStyle.None;
        registrationScreen.style.display = DisplayStyle.None;
        homePage.style.display = DisplayStyle.None;
        popUpPage.style.display = DisplayStyle.None;
    }

    private void ShowLogin()
    {
        splashScreen.style.display = DisplayStyle.None;
        loginScreen.style.display = DisplayStyle.Flex;
        registrationScreen.style.display = DisplayStyle.None;
        homePage.style.display = DisplayStyle.None;
        popUpPage.style.display = DisplayStyle.None;
    }

    private void ShowRegister()
    {
        splashScreen.style.display = DisplayStyle.None;
        loginScreen.style.display = DisplayStyle.None;
        registrationScreen.style.display = DisplayStyle.Flex;
        homePage.style.display = DisplayStyle.None;
        popUpPage.style.display = DisplayStyle.None;
    }

    public void ShowHomePage()
    {
        splashScreen.style.display = DisplayStyle.None;
        loginScreen.style.display = DisplayStyle.None;
        registrationScreen.style.display = DisplayStyle.None;
        homePage.style.display = DisplayStyle.Flex;
        popUpPage.style.display = DisplayStyle.None;
    }

    public void ShowProgressionPage()
    {
        // splashScreen.style.display = DisplayStyle.None;
        // loginScreen.style.display = DisplayStyle.None;
        // registrationScreen.style.display = DisplayStyle.None;
        homePage.style.display = DisplayStyle.Flex;
        popUpPage.style.display = DisplayStyle.Flex;
        progressionPage.style.display = DisplayStyle.Flex;
    }

    public void HideProgressionPage()
    {
        popUpPage.style.display = DisplayStyle.None;
        progressionPage.style.display = DisplayStyle.None;
    }

    private void ShowModeOfLearningPage()
    {
        splashScreen.style.display = DisplayStyle.None;
        loginScreen.style.display = DisplayStyle.None;
        registrationScreen.style.display = DisplayStyle.None;
        popUpPage.style.display = DisplayStyle.Flex;
        homePage.style.display = DisplayStyle.Flex;
        modeOfLearningPage.style.display = DisplayStyle.Flex;
        videoContainer.style.display = DisplayStyle.None;
    }

    private void ShowQuizPage()
    {
        splashScreen.style.display = DisplayStyle.None;
        loginScreen.style.display = DisplayStyle.None;
        registrationScreen.style.display = DisplayStyle.None;
        popUpPage.style.display = DisplayStyle.None;
        homePage.style.display = DisplayStyle.None;
        modeOfLearningPage.style.display = DisplayStyle.None;
        videoContainer.style.display = DisplayStyle.None;

        // scoresPage.style.display = DisplayStyle.None;
        quizPage.style.display = DisplayStyle.Flex;
    }

    private void ShowMcqPage()
    {
        mcqPage.style.display = DisplayStyle.Flex;
    }

    private void ShowTofPage()
    {
        tofPage.style.display = DisplayStyle.Flex;
    }
}
