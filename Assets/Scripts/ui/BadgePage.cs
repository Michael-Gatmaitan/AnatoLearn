using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class BadgePage : MonoBehaviour
{
    private VisualElement V_Main;
    private VisualElement popUpPage;
    private VisualElement V_BadgePage;
    private VisualElement V_BadgeImage;
    private Button B_CloseBadgePage;

    public Sprite[] badgeTopicSprites;

    void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        V_Main = root.Q<VisualElement>("V_Main");
        popUpPage = V_Main.Q<VisualElement>("popUpPage");
        V_BadgePage = popUpPage.Q<VisualElement>("badgePage");
        V_BadgeImage = V_BadgePage.Q<VisualElement>("V_BadgeImage");

        B_CloseBadgePage = popUpPage.Q<Button>("closeBadgePageBtn");
        B_CloseBadgePage?.RegisterCallback<ClickEvent>(_ => HideBadgePage());
    }

    public void ShowBadgePage(int topic_id)
    {
        Debug.Log($"Openning badge for topic id: {topic_id}");

        Debug.Log($"Popup page: " + popUpPage);
        Debug.Log($"Badge page: " + V_BadgePage);
        Debug.Log($"Badge Image: " + V_BadgeImage);

        popUpPage.style.display = DisplayStyle.Flex;
        V_BadgePage.style.display = DisplayStyle.Flex;
        V_BadgeImage.style.backgroundImage = new StyleBackground(badgeTopicSprites[topic_id - 1]);
    }

    public void HideBadgePage()
    {
        popUpPage.style.display = DisplayStyle.None;
        V_BadgePage.style.display = DisplayStyle.None;
    }
}
