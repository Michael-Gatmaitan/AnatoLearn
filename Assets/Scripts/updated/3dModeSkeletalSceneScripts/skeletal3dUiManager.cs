using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class skeletal3dUiManager : MonoBehaviour
{
    // private VisualElement menuBtn;
    // private Button menuBtn;
    private Button homeBtn;
    private Button hideBtn;
    private VisualElement skelatalContentsBtns;
    private VisualElement blackBgAbsoluteClassDivVidCon;
    private VisualElement boneClassCon;
    private VisualElement boneDivCon;
    private VisualElement navigationBtns;
    private VisualElement tapMeActCon;

    private VisualElement mainVidLessonCon;
    private VisualElement blackBgAbsoluteTagDescriptPage; //added
    private VisualElement skullDescriptionCon; //added
    private VisualElement femurDescriptionCon; //added 2

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        // Debug.Log($"User state from another scene: {UserState.Instance.TopicId}");
        Debug.Log($"Player prefs topic: {PlayerPrefs.GetString("Chosen3D")}");

        homeBtn = root.Q<Button>("homeBtn");
        hideBtn = root.Q<Button>("hideBtn");
        skelatalContentsBtns = root.Q<VisualElement>("skeletalContentsBtns");
        blackBgAbsoluteClassDivVidCon = root.Q<VisualElement>("blackBgAbsoluteClassDivVidCon");
        boneClassCon = root.Q<VisualElement>("boneClassCon");
        boneDivCon = root.Q<VisualElement>("boneDivCon");
        navigationBtns = root.Q<VisualElement>("navigationBtns");
        tapMeActCon = root.Q<VisualElement>("tapMeActCon");
        mainVidLessonCon = root.Q<VisualElement>("mainVidLessonCon");
        blackBgAbsoluteTagDescriptPage = root.Q<VisualElement>("blackBgAbsoluteTagDescriptPage"); //added
        skullDescriptionCon = root.Q<VisualElement>("skullDescriptionCon"); //added
        femurDescriptionCon = root.Q<VisualElement>("femurDescriptionCon"); //added 2

        // Buttons
        var menuBtn = root.Q<Button>("menuBtn");
        var boneClassBtn = root.Q<Button>("boneClassBtn");
        var boneDivBtn = root.Q<Button>("boneDivBtn");
        var exitClassBtn = root.Q<Button>("exitClassBtn");
        var exitDivBtn = root.Q<Button>("exitDivBtn");
        // var finishBtn = root.Q<Button>("finishBtn");
        var infoBtn = root.Q<Button>("infoBtn");
        var exitMainVidLessonBtn = root.Q<Button>("exitMainVidLessonBtn");
        var exitSkullDesBtn = root.Q<Button>("exitSkullDesBtn"); // x button in "skullDescriptionCon" //added
        var exitFemurDesBtn = root.Q<Button>("exitFemurDesBtn"); // x button in "FemurDescriptionCon" //added 2

        // Button Callbacks
        menuBtn?.RegisterCallback<ClickEvent>(evt => ToggleMenuBtn());
        hideBtn?.RegisterCallback<ClickEvent>(evt => ToggleHideBtn());
        boneClassBtn?.RegisterCallback<ClickEvent>(evt => ShowBoneClassPage());
        boneDivBtn?.RegisterCallback<ClickEvent>(evt => ShowBoneDivPage());
        exitClassBtn?.RegisterCallback<ClickEvent>(evt => HideBoneClassPage());
        exitDivBtn?.RegisterCallback<ClickEvent>(evt => HideBoneDivPage());
        // finishBtn?.RegisterCallback<ClickEvent>(evt => ShowTapActPage());
        exitMainVidLessonBtn?.RegisterCallback<ClickEvent>(evt => HideMainVidLessonPage());

        infoBtn?.RegisterCallback<ClickEvent>(evt => ShowMainVidLessonPage());

        exitSkullDesBtn?.RegisterCallback<ClickEvent>(evt => HideTagDescriptionPage()); //added
        exitFemurDesBtn?.RegisterCallback<ClickEvent>(evt => HideTagDescriptionPage()); //added 2
    }

    private void ShowTagDescriptionPage() //added
    {
        blackBgAbsoluteTagDescriptPage.style.display = DisplayStyle.Flex;
        skullDescriptionCon.style.display = DisplayStyle.Flex;
    }

    private void HideTagDescriptionPage() //added
    {
        blackBgAbsoluteTagDescriptPage.style.display = DisplayStyle.None;
        skullDescriptionCon.style.display = DisplayStyle.None;
        femurDescriptionCon.style.display = DisplayStyle.None;
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
        if (skelatalContentsBtns.style.display == DisplayStyle.None)
        {
            skelatalContentsBtns.style.display = DisplayStyle.Flex;
        }
        else
        {
            skelatalContentsBtns.style.display = DisplayStyle.None;
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
}
