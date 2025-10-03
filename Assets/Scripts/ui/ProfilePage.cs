using UnityEngine;
using UnityEngine.UIElements;

public class ProfilePage : MonoBehaviour
{
    private VisualElement root,
        V_Main,
        homePage,
        profileButton,
        popupPage,
        profilePage;

    private Label L_ProfilePage;

    private Button B_BackProfilePage,
        B_DeleteProfile;

    private ScrollView S_ProfileScrollView;
    private VisualElement V_ProfileModals,
        V_DeleteProfileModal,
        V_EditUsernameModal;

    // Navigation Buttons
    private Button B_Profile,
        B_Badges,
        B_EditProfile;

    // Sub pages
    private VisualElement V_ProfileBody,
        V_Badges,
        V_EditProfile;

    private Label L_Username;

    // Delete profile elements

    // Edit username elements

    void ShowProfilePage()
    {
        popupPage.style.display = DisplayStyle.Flex;
        profilePage.style.display = DisplayStyle.Flex;
    }

    void HideProfilePage()
    {
        popupPage.style.display = DisplayStyle.None;
        profilePage.style.display = DisplayStyle.None;
    }

    void ShowDeleteProfileModal()
    {
        V_ProfileModals.style.display = DisplayStyle.Flex;
        V_DeleteProfileModal.style.display = DisplayStyle.Flex;
    }

    void HideDeleteProfileModal()
    {
        V_ProfileModals.style.display = DisplayStyle.None;
        V_DeleteProfileModal.style.display = DisplayStyle.None;
    }

    void ShowEditUsernameModal()
    {
        V_ProfileModals.style.display = DisplayStyle.Flex;
        V_EditUsernameModal.style.display = DisplayStyle.Flex;
    }

    void HideEditUsernameModal()
    {
        V_ProfileModals.style.display = DisplayStyle.None;
        V_EditUsernameModal.style.display = DisplayStyle.None;
    }

    void NavigateToProfileBody()
    {
        V_ProfileBody.style.display = DisplayStyle.Flex;
        V_Badges.style.display = DisplayStyle.None;
        V_EditProfile.style.display = DisplayStyle.None;

        B_Profile.style.opacity = 1f;

        B_Badges.style.opacity = 0.2f;
        B_EditProfile.style.opacity = 0.2f;

        L_ProfilePage.text = "Profile";
    }

    void NavigateToBadges()
    {
        V_ProfileBody.style.display = DisplayStyle.None;
        V_Badges.style.display = DisplayStyle.Flex;
        V_EditProfile.style.display = DisplayStyle.None;

        B_Badges.style.opacity = 1f;

        B_Profile.style.opacity = 0.2f;
        B_EditProfile.style.opacity = 0.2f;

        L_ProfilePage.text = "Badges";
    }

    void NavigateToEditProfile()
    {
        V_ProfileBody.style.display = DisplayStyle.None;
        V_Badges.style.display = DisplayStyle.None;
        V_EditProfile.style.display = DisplayStyle.Flex;

        B_EditProfile.style.opacity = 1f;

        B_Badges.style.opacity = 0.2f;
        B_Profile.style.opacity = 0.2f;

        L_ProfilePage.text = "Edit Profile";
    }

    void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        V_Main = root.Q<VisualElement>("V_Main");
        homePage = V_Main.Q<VisualElement>("homePage");
        profileButton = homePage.Q<VisualElement>("profPic");

        popupPage = V_Main.Q<VisualElement>("popUpPage");
        profilePage = popupPage.Q<VisualElement>("profilePage");

        L_ProfilePage = profilePage.Q<Label>("L_ProfilePage");

        S_ProfileScrollView = profilePage.Q<ScrollView>("S_ProfileScrollView");

        B_BackProfilePage = profilePage.Q<Button>("B_BackProfilePage");
        B_DeleteProfile = profilePage.Q<Button>("B_DeleteProfile");

        V_ProfileModals = profilePage.Q<VisualElement>("V_ProfileModals");
        V_DeleteProfileModal = V_ProfileModals.Q<VisualElement>("V_DeleteProfileModal");
        V_EditUsernameModal = profilePage.Q<VisualElement>("V_EditUsernameModal");

        L_Username = S_ProfileScrollView.Q<Label>("L_Username");

        profileButton?.RegisterCallback<ClickEvent>(_ => ShowProfilePage());
        B_BackProfilePage?.RegisterCallback<ClickEvent>(_ => HideProfilePage());

        B_DeleteProfile?.RegisterCallback<ClickEvent>(_ => ShowDeleteProfileModal());

        L_Username?.RegisterCallback<ClickEvent>(_ => ShowEditUsernameModal());

        // Testing purposes
        V_DeleteProfileModal?.RegisterCallback<ClickEvent>(_ => HideDeleteProfileModal());
        V_EditUsernameModal?.RegisterCallback<ClickEvent>(_ => HideEditUsernameModal());

        B_Profile = profilePage.Q<Button>("B_Profile");
        B_Badges = profilePage.Q<Button>("B_Badges");
        B_EditProfile = profilePage.Q<Button>("B_EditProfile");

        B_Profile?.RegisterCallback<ClickEvent>(_ => NavigateToProfileBody());
        B_Badges?.RegisterCallback<ClickEvent>(_ => NavigateToBadges());
        B_EditProfile?.RegisterCallback<ClickEvent>(_ => NavigateToEditProfile());

        V_ProfileBody = profilePage.Q<VisualElement>("V_ProfileBody");
        V_Badges = profilePage.Q<VisualElement>("V_Badges");
        V_EditProfile = profilePage.Q<VisualElement>("V_EditProfile");
    }

    // void Start()
    // {
    //     InitializeHomePage();
    // }

    public void InitializeHomePage()
    {
        Debug.Log("Initializing home page");
        L_Username.text = UserState.Instance.Username;
    }
}
