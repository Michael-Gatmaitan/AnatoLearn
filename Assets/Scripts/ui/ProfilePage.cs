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

    private Button B_BackProfilePage,
        B_DeleteProfile;

    private ScrollView S_ProfileScrollView;
    private VisualElement V_ProfileModals,
        V_DeleteProfileModal,
        V_EditUsernameModal;

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

    void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        V_Main = root.Q<VisualElement>("V_Main");
        homePage = V_Main.Q<VisualElement>("homePage");
        profileButton = homePage.Q<VisualElement>("profPic");

        popupPage = V_Main.Q<VisualElement>("popUpPage");
        profilePage = popupPage.Q<VisualElement>("profilePage");

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
    }

    void Start()
    {
        L_Username.text = UserState.Instance.Username;
    }
}
