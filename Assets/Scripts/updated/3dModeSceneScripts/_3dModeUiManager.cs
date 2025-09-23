using System;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class _3dModeUIManager : MonoBehaviour
{
    // private VisualElement menuBtn;
    // private Button menuBtn;
    private Button homeBtn;
    private Button hideBtn;
    private Button langBtn;
    private Button finishBtn;

    private VisualElement contentsBtns;
    private VisualElement blackBgAbsoluteClassDivVidCon;
    private VisualElement boneClassCon;
    private VisualElement boneDivCon;
    private VisualElement navigationBtns;
    private VisualElement tapMeActCon;

    private VisualElement tapMeActInstructPage;
    private VisualElement mainVidLessonCon;
    private VisualElement blackBgAbsoluteTagDescriptPage;
    private VisualElement skullDescriptionCon;
    private VisualElement femurDescriptionCon;
    private VisualElement skeletalContentsBtns;
    private VisualElement digestiveContentsBtns;
    private VisualElement circulatoryContentsBtns;
    private VisualElement nervousContentsBtns;

    private VisualElement blackBgAbsoluteFunFactPage;
    private VisualElement blackBgAbsoluteNeuronsCardsPage;
    private VisualElement blackBgAbsoluteChooseLanguagePage; //added 7 24
    private VisualElement blackBgAbsoluteHomePromptPage;

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        homeBtn = root.Q<Button>("homeBtn");
        hideBtn = root.Q<Button>("hideBtn");
        finishBtn = root.Q<Button>("finishBtn");
        // langBtn = root.Q<Button>("langBtn"); //added 724

        var yesHomeBtn = root.Q<Button>("yesHomeBtn");
        var noHomeBtn = root.Q<Button>("noHomeBtn");

        contentsBtns = root.Q<VisualElement>("contentsBtns");
        skeletalContentsBtns = root.Q<VisualElement>("skeletalContentsBtns");
        digestiveContentsBtns = root.Q<VisualElement>("digestiveContentsBtns");
        circulatoryContentsBtns = root.Q<VisualElement>("circulatoryContentsBtns");
        nervousContentsBtns = root.Q<VisualElement>("nervousContentsBtns");

        blackBgAbsoluteClassDivVidCon = root.Q<VisualElement>("blackBgAbsoluteClassDivVidCon");
        boneClassCon = root.Q<VisualElement>("boneClassCon");
        boneDivCon = root.Q<VisualElement>("boneDivCon");
        navigationBtns = root.Q<VisualElement>("navigationBtns");

        tapMeActCon = root.Q<VisualElement>("tapMeActCon");
        tapMeActInstructPage = root.Q<VisualElement>("tapMeActInstructPage");

        mainVidLessonCon = root.Q<VisualElement>("mainVidLessonCon");
        blackBgAbsoluteTagDescriptPage = root.Q<VisualElement>("blackBgAbsoluteTagDescriptPage"); //added
        // skullDescriptionCon = root.Q<VisualElement>("skullDescriptionCon"); //added
        // femurDescriptionCon = root.Q<VisualElement>("femurDescriptionCon"); //added 2

        blackBgAbsoluteFunFactPage = root.Q<VisualElement>("blackBgAbsoluteFunFactPage");
        blackBgAbsoluteNeuronsCardsPage = root.Q<VisualElement>("blackBgAbsoluteNeuronsCardsPage"); //added 2
        blackBgAbsoluteChooseLanguagePage = root.Q<VisualElement>(
            "blackBgAbsoluteChooseLanguagePage"
        ); // added
        blackBgAbsoluteHomePromptPage = root.Q<VisualElement>("blackBgAbsoluteHomePromptPage");

        // Buttons
        langBtn = root.Q<Button>("langBtn"); //added 724
        var exitLanguageBtn = root.Q<Button>("exitLanguageBtn"); //added 7 24
        var englishVerBtn = root.Q<Button>("englishVerBtn"); //added 7 24
        var tagalogVerBtn = root.Q<Button>("tagalogVerBtn"); //added 7 24

        var menuBtn = root.Q<Button>("menuBtn");
        var boneClassBtn = root.Q<Button>("boneClassBtn");
        var boneDivBtn = root.Q<Button>("boneDivBtn");
        var exitClassBtn = root.Q<Button>("exitClassBtn");
        var exitDivBtn = root.Q<Button>("exitDivBtn");
        // var quizBtn = root.Q<Button>("quizBtn");
        var infoBtn = root.Q<Button>("infoBtn");
        var exitMainVidLessonBtn = root.Q<Button>("exitMainVidLessonBtn");
        var exitSkullDesBtn = root.Q<Button>("exitSkullDesBtn"); // x button in "skullDescriptionCon" //added
        var exitFemurDesBtn = root.Q<Button>("exitFemurDesBtn"); // x button in "FemurDescriptionCon" //added 2

        var exitHomePromptBtn = root.Q<Button>("exitHomePromptBtn");

        // Find ALL buttons named "exitDescriptionConBtn"
        var exitDescriptionConBtn = root.Query<Button>("exitDescriptionConBtn").ToList(); // exit button in tagDescriptionPage

        var funFactBtn = root.Q<Button>("funFactBtn");
        var funFactBackBtns = root.Query<Button>("funFactBackBtn").ToList();

        var neuronsBtn = root.Q<Button>("neuronsBtn");
        var neuronsCardBackBtns = root.Query<Button>("neuronsCardBackBtn").ToList();

        //Tap Me Act Instruc Page btns
        var exitTapMeActInstructPageBtn = root.Q<Button>("exitTapMeActInstructPageBtn");

        // Button Callbacks
        menuBtn?.RegisterCallback<ClickEvent>(evt => ToggleMenuBtn());
        hideBtn?.RegisterCallback<ClickEvent>(evt => ToggleHideBtn());
        langBtn?.RegisterCallback<ClickEvent>(evt => ShowChooseLangPage()); //added 7 24

        englishVerBtn?.RegisterCallback<ClickEvent>(evt =>
            SetAndHideChooseLangPage("englishVersion")
        ); //added 7 24
        tagalogVerBtn?.RegisterCallback<ClickEvent>(evt =>
            SetAndHideChooseLangPage("tagalogVersion")
        ); //added 7 24

        boneClassBtn?.RegisterCallback<ClickEvent>(evt => ShowBoneClassPage());
        boneDivBtn?.RegisterCallback<ClickEvent>(evt => ShowBoneDivPage());
        exitClassBtn?.RegisterCallback<ClickEvent>(evt => HideBoneClassPage());
        exitDivBtn?.RegisterCallback<ClickEvent>(evt => HideBoneDivPage());
        exitLanguageBtn?.RegisterCallback<ClickEvent>(evt => SetAndHideChooseLangPage(""));
        // quizBtn?.RegisterCallback<ClickEvent>(evt => ShowTapActPage()); //edited 6-20
        exitMainVidLessonBtn?.RegisterCallback<ClickEvent>(evt => HideMainVidLessonPage());

        infoBtn?.RegisterCallback<ClickEvent>(evt => ShowMainVidLessonPage());

        //Button call backs
        homeBtn?.RegisterCallback<ClickEvent>(evt => ShowHomePromptPage());
        exitHomePromptBtn?.RegisterCallback<ClickEvent>(evt => HideHomePromptPage());
        yesHomeBtn?.RegisterCallback<ClickEvent>(evt => HideHomePromptPage());
        noHomeBtn?.RegisterCallback<ClickEvent>(evt => HideHomePromptPage());

        // exitSkullDesBtn?.RegisterCallback<ClickEvent>(evt => HideTagDescriptionPage()); //added
        // exitFemurDesBtn?.RegisterCallback<ClickEvent>(evt => HideTagDescriptionPage()); //added 2

        foreach (var exitBtn in exitDescriptionConBtn)
        {
            exitBtn.RegisterCallback<ClickEvent>(evt => HideTagDescriptionPage());
            UnityEngine.Debug.LogWarning(
                "Total exitDescriptionConBtn found: " + exitDescriptionConBtn.Count
            );
        }

        // Buttons In blackBgAbsoluteClassDivVidCon
        RegisterBoneClassDivVidConButtons(root, "longBonesBtn");
        RegisterBoneClassDivVidConButtons(root, "shortBonesBtn");
        RegisterBoneClassDivVidConButtons(root, "flatBonesBtn");
        RegisterBoneClassDivVidConButtons(root, "irregBonesBtn");
        RegisterBoneClassDivVidConButtons(root, "axialBonesBtn");
        RegisterBoneClassDivVidConButtons(root, "appendBonesBtn");

        funFactBtn?.RegisterCallback<ClickEvent>(evt => ShowFunFactPage()); //added 2

        foreach (var funFactBackBtn in funFactBackBtns)
        {
            funFactBackBtn.RegisterCallback<ClickEvent>(evt => HideFunFactPage());
        }

        neuronsBtn?.RegisterCallback<ClickEvent>(evt => ShowNeuronCardsPage());

        foreach (var neuronsCardBackBtn in neuronsCardBackBtns)
        {
            neuronsCardBackBtn.RegisterCallback<ClickEvent>(evt => HideNeuronCardsPage());
        }

        //Tap Me Act PAge
        exitTapMeActInstructPageBtn?.RegisterCallback<ClickEvent>(evt =>
        {
            tapMeActInstructPage.style.display = DisplayStyle.None;
            SceneData.exitInstrucTapMeActPageBtnIsClicked = true;
        });

        // ==================================================
        // Showing/Hiding of needed UI Buttons in Explore Mode
        // if (UserState.Instance.TopicId == null)
        // {
        //     UserState.Instance.SetTopicId(1); // Debugging purposes
        // }

        try
        {
            if (SceneData.studyingSkeletal || UserState.Instance.TopicId == 1)
            {
                skeletalContentsBtns.style.display = DisplayStyle.Flex;
            }
            else if (SceneData.studyingIntegumentary || UserState.Instance.TopicId == 2)
            {
                // skeletalContentsBtns.style.display = DisplayStyle.None;
            }
            else if (SceneData.studyingDigestive || UserState.Instance.TopicId == 3)
            {
                digestiveContentsBtns.style.display = DisplayStyle.Flex;
                // skeletalContentsBtns.style.display = DisplayStyle.None;
            }
            else if (SceneData.studyingCirculatory || UserState.Instance.TopicId == 5)
            {
                circulatoryContentsBtns.style.display = DisplayStyle.Flex;
                finishBtn.style.display = DisplayStyle.None;
                // skeletalContentsBtns.style.display = DisplayStyle.None;
            }
            else if (SceneData.studyingNervous || UserState.Instance.TopicId == 6)
            {
                nervousContentsBtns.style.display = DisplayStyle.Flex;
                // skeletalContentsBtns.style.display = DisplayStyle.None;
            }

            if (SceneData.studyingCirculatoryHeart)
            {
                circulatoryContentsBtns.style.display = DisplayStyle.None;
                finishBtn.style.display = DisplayStyle.Flex;
                UnityEngine.Debug.Log("circuContentBTn = NONE");
                SceneData.studyingCirculatoryHeart = false;
                // skeletalContentsBtns.style.display = DisplayStyle.None;s
            }

            //FOR TAP ME ACTIVITY
            //displaying of needed UXML and hiding unesceerary UXML
            // SceneData.showTapActPage = true;
            // if (SceneData.showTapActPage)
            // {
            //     ShowTapActPage();
            //     ShowTapMeActInstructionPage();
            //     UnityEngine.Debug.Log("ShowTapActPage and ShowTapActInstrucPage run -- 3dModeUiManager");

            //     SceneData.showTapActPage = false;
            // }
            UnityEngine.Debug.Log("last line of OnEnable in 3dModeUImAnager");
        }
        catch (Exception _)
        {
            UnityEngine.Debug.Log("Error occured in ui, entering dev mode");
            skeletalContentsBtns.style.display = DisplayStyle.Flex;
        }
    }

    private void Start()
    {
        UnityEngine.Debug.Log(
            $"SceneData.showTapActPage = {SceneData.showTapActPage} -- 3dModeUiManager"
        );
        // SceneData.showTapActPage = true;
        if (SceneData.showTapActPage)
        {
            ShowTapActPage(); //3dModeScene
            ShowTapMeActInstructionPage();
            UnityEngine.Debug.Log("✅ ShowTapActPage and ShowTapMeActInstrucPage run in Start()");
            SceneData.showTapActPage = false;
        }

        UnityEngine.Debug.Log("✅ Start() finished in 3dModeUIManager");
    }

    private void ShowHomePromptPage()
    {
        blackBgAbsoluteHomePromptPage.style.display = DisplayStyle.Flex;
    }

    private void HideHomePromptPage()
    {
        blackBgAbsoluteHomePromptPage.style.display = DisplayStyle.None;
    }

    private void ShowTapMeActInstructionPage()
    {
        tapMeActInstructPage.style.display = DisplayStyle.Flex;
    }

    private void HideFunFactPage()
    {
        blackBgAbsoluteFunFactPage.style.display = DisplayStyle.None;
    }

    private void HideNeuronCardsPage()
    {
        blackBgAbsoluteNeuronsCardsPage.style.display = DisplayStyle.None;
    }

    private void ShowFunFactPage()
    {
        blackBgAbsoluteFunFactPage.style.display = DisplayStyle.Flex;
    }

    private void ShowNeuronCardsPage()
    {
        blackBgAbsoluteNeuronsCardsPage.style.display = DisplayStyle.Flex;
    }

    // private void ShowTagDescriptionPage() //added
    // {
    //     blackBgAbsoluteTagDescriptPage.style.display = DisplayStyle.Flex;
    //     skullDescriptionCon.style.display = DisplayStyle.Flex;
    // }
    private void HideTagDescriptionPage() //added
    {
        blackBgAbsoluteTagDescriptPage.style.display = DisplayStyle.None;
        UnityEngine.Debug.LogWarning("NAGCLOSE HIDETAGDECRIPTIONPAGE");
    }

    private void ToggleMenuBtn()
    {
        // Toggle between None and Flex
        if (
            homeBtn.style.display == DisplayStyle.None
            && hideBtn.style.display == DisplayStyle.None
        )
        {
            homeBtn.style.display = DisplayStyle.Flex;
            hideBtn.style.display = DisplayStyle.Flex;
        }
        else
        {
            homeBtn.style.display = DisplayStyle.None;
            hideBtn.style.display = DisplayStyle.None;
        }
    }

    private void ToggleHideBtn()
    {
        if (contentsBtns.style.display == DisplayStyle.None)
        {
            contentsBtns.style.display = DisplayStyle.Flex;
        }
        else
        {
            contentsBtns.style.display = DisplayStyle.None;
            homeBtn.style.display = DisplayStyle.None;
            hideBtn.style.display = DisplayStyle.None;
        }
    }

    private void ShowBoneClassPage()
    {
        blackBgAbsoluteClassDivVidCon.style.display = DisplayStyle.Flex;
        boneClassCon.style.display = DisplayStyle.Flex;
    }

    private void ShowBoneDivPage()
    {
        blackBgAbsoluteClassDivVidCon.style.display = DisplayStyle.Flex;
        boneDivCon.style.display = DisplayStyle.Flex;
    }

    private void HideBoneClassPage()
    {
        blackBgAbsoluteClassDivVidCon.style.display = DisplayStyle.None;
        boneClassCon.style.display = DisplayStyle.None;
    }

    private void HideBoneDivPage()
    {
        blackBgAbsoluteClassDivVidCon.style.display = DisplayStyle.None;
        boneDivCon.style.display = DisplayStyle.None;
    }

    private void ShowTapActPage()
    {
        navigationBtns.style.display = DisplayStyle.None;
        tapMeActCon.style.display = DisplayStyle.Flex;
    }

    private void ShowMainVidLessonPage()
    {
        blackBgAbsoluteClassDivVidCon.style.display = DisplayStyle.Flex;
        mainVidLessonCon.style.display = DisplayStyle.Flex;
    }

    private void HideMainVidLessonPage()
    {
        blackBgAbsoluteClassDivVidCon.style.display = DisplayStyle.None;
        mainVidLessonCon.style.display = DisplayStyle.None;
    }

    private void RegisterBoneClassDivVidConButtons(VisualElement root, string name)
    {
        var ve = root.Q<VisualElement>(name);
        if (ve != null)
        {
            ve.RegisterCallback<ClickEvent>(evt =>
            {
                HideBoneClassPage();
                HideBoneDivPage();
            });
        }
    }

    private void ShowChooseLangPage() //added 7 24
    {
        blackBgAbsoluteChooseLanguagePage.style.display = DisplayStyle.Flex;
    }

    //TAGALOG-ENGLISH VERSION
    // Reference to TagClickManager (assign this from inspector or find it)
    public TagClickManager tagClickManager;
    public FunFactsLanguageManager funFactsLanguageManager;
    public NeuronsCardLanguageManager neuronsCardLanguageManager;

    private void SetAndHideChooseLangPage(string languageVersion) //added 7 24
    {
        blackBgAbsoluteChooseLanguagePage.style.display = DisplayStyle.None;

        SceneData.LanguageVersion = languageVersion;

        if (tagClickManager != null)
            tagClickManager.SetLanguage(languageVersion);

        UnityEngine.Debug.Log($"Language set to: {languageVersion}");

        if (funFactsLanguageManager != null)
            funFactsLanguageManager.SetLanguage(languageVersion);

        if (neuronsCardLanguageManager != null)
            neuronsCardLanguageManager.SetLanguage(languageVersion);
    }
}
