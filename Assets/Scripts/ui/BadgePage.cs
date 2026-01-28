using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class BadgePage : MonoBehaviour
{
    private VisualElement V_Main;
    private VisualElement popUpPage;
    private VisualElement profilePage;
    private VisualElement V_ProfileModals;

    // Badge
    private VisualElement V_BadgePage;
    private VisualElement V_BadgeImage;
    private Button B_CloseBadgePage;

    private VisualElement V_noBadgePromptCon;
    private VisualElement V_NoBadgeImage;
    private Button B_CloseNoBadgePrompt;

    private Button B_BadgeInstruction;

    // Modal
    private VisualElement V_BadgeInstructionModal;

    // Close modal
    private Button B_CloseBadgeInstruction;


    public Sprite[] badgeTopicSprites;

    void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        V_Main = root.Q<VisualElement>("V_Main");

        // Badge
        popUpPage = V_Main.Q<VisualElement>("popUpPage");
        V_BadgePage = popUpPage.Q<VisualElement>("badgePage");
        V_BadgeImage = V_BadgePage.Q<VisualElement>("V_BadgeImage");

        V_noBadgePromptCon = popUpPage.Q<VisualElement>("noBadgePromptCon");
        V_NoBadgeImage = V_noBadgePromptCon.Q<VisualElement>("badge");
        B_CloseNoBadgePrompt = V_noBadgePromptCon.Q<Button>("hasBadgeOkayBtn");

        B_CloseBadgePage = popUpPage.Q<Button>("closeBadgePageBtn");
        B_CloseBadgePage?.RegisterCallback<ClickEvent>(_ => HideBadgePage());
        B_CloseNoBadgePrompt?.RegisterCallback<ClickEvent>(_ => HideNoBadgePrompt());
    }

    public void ShowBadgePage(int topic_id)
    {
        popUpPage.style.display = DisplayStyle.Flex;
        V_BadgePage.style.display = DisplayStyle.Flex;
        V_BadgeImage.style.backgroundImage = new StyleBackground(badgeTopicSprites[topic_id - 1]);
    }

    public void HideBadgePage()
    {
        popUpPage.style.display = DisplayStyle.None;
        V_BadgePage.style.display = DisplayStyle.None;
    }

    public void ShowNoBadgePrompt(int topic_id)
    {
        popUpPage.style.display = DisplayStyle.Flex;
        V_noBadgePromptCon.style.display = DisplayStyle.Flex;
        V_NoBadgeImage.style.backgroundImage = new StyleBackground(badgeTopicSprites[topic_id - 1]);
    }

    public void HideNoBadgePrompt()
    {
        popUpPage.style.display = DisplayStyle.None;
        V_noBadgePromptCon.style.display = DisplayStyle.None;
    }
}
