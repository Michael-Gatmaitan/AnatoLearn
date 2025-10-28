using System.Collections.Generic;
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

    // Delete profile modal
    private Button B_DeletePofileBack;

    // Sub pages
    private VisualElement V_ProfileBody,
        V_Badges,
        V_EditProfile;

    private Label L_Username;

    private static TotalScoresController totalScoresController;
    private static UserController userController;

    public Sprite[] avatarSprites;
    private readonly List<string> avatarNames = new()
    {
        "default",
        "boy-kid",
        "boy",
        "girl-kid",
        "girl",
    };

    private readonly Topics[] topicsArray = new Topics[]
    {
        new() { id = 1, topic_name = "skeletal" },
        new() { id = 2, topic_name = "integumentary" },
        new() { id = 3, topic_name = "digestive" },
        new() { id = 4, topic_name = "respiratory" },
        new() { id = 5, topic_name = "circulatory" },
        new() { id = 6, topic_name = "nervous" },
        new() { id = 7, topic_name = "excretory" },
    };

    private List<TotalScore> badgedScores;

    // Edit profile page variables
    private TextField T_NewFirstname,
        T_NewMiddlename,
        T_NewLastname;
    private Button B_EditButton;

    // private Profiel
    private VisualElement V_Avatar;
    private VisualElement V_HomeAvatar;
    private Button B_UpdateAvatar;

    private List<VisualElement> profiles;

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

    void ClearEditProfileInputs()
    {
        T_NewFirstname.value = "";
        T_NewMiddlename.value = "";
        T_NewLastname.value = "";
    }

    void ShowDeleteProfileModal()
    {
        V_ProfileModals.style.display = DisplayStyle.Flex;
        V_DeleteProfileModal.style.display = DisplayStyle.Flex;

        // userController.DeleteUser(
        //     UserState.Instance.Email,
        //     (r) =>
        //     {
        //         NavigateToProfileBody();
        //         HideProfilePage();
        //         ClearEditProfileInputs();
        //         IntegrateUI.MessageBox("Account successfully deleted");
        //         IntegrateUI.Instance.LogoutFunc();

        //         // Debug.Log(r);
        //     },
        //     (e) => Debug.Log(e)
        // );
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
        totalScoresController = GetComponent<TotalScoresController>();
        userController = GetComponent<UserController>();

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

        // Delete profile
        B_DeletePofileBack?.RegisterCallback<ClickEvent>(_ => HideDeleteProfileModal());
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

        // Profile variables initialization
        V_Avatar = V_ProfileBody.Q<VisualElement>("V_Avatar");
        V_HomeAvatar = homePage.Q<VisualElement>("V_HomeAvatar");
        B_UpdateAvatar = V_ProfileBody.Q<Button>("B_UpdateAvatar");
        B_UpdateAvatar.SetEnabled(false);

        profiles = V_ProfileBody.Query(className: "profile").ToList();

        VisualElement selected = null;
        int selectedIndex = 0;

        foreach (var p in profiles)
        {
            p?.RegisterCallback<ClickEvent>(_ =>
            {
                if (selected != null && p == selected)
                {
                    selected = null;
                    p.RemoveFromClassList("profileSelected");

                    B_UpdateAvatar.SetEnabled(false);
                    return;
                }

                foreach (var _p in profiles)
                {
                    if (p == _p)
                    {
                        // Add selected class
                        selected = p;
                        selectedIndex = profiles.IndexOf(p);
                        Debug.Log($"Selected: {selected}");
                        p.AddToClassList("profileSelected");

                        // Enable edit profile button
                        B_UpdateAvatar.SetEnabled(true);
                    }
                    else
                    {
                        // Remove selected class
                        _p.RemoveFromClassList("profileSelected");
                    }
                }

                Debug.Log($"Selected index: {selectedIndex}");
            });
        }

        B_UpdateAvatar?.RegisterCallback<ClickEvent>(_ =>
        {
            UpdateAvatarFunction(selectedIndex);
        });

        // Edit profile variables initializations

        T_NewFirstname = V_EditProfile.Q<TextField>("T_NewFirstname");
        T_NewMiddlename = V_EditProfile.Q<TextField>("T_NewMiddlename");
        T_NewLastname = V_EditProfile.Q<TextField>("T_NewLastname");

        B_EditButton = V_EditProfile.Q<Button>("B_EditButton");
    }

    public void UpdateAvatarFunction(int selectedIndex)
    {
        if (selectedIndex == 0)
            return;

        // Update avatar of user in database
        string email = UserState.Instance.Email;
        userController.EditAvatar(
            email,
            avatarNames[selectedIndex + 1],
            (r) =>
            {
                Debug.Log("Edit avatar in db: " + r);
            },
            (e) =>
            {
                Debug.LogError("Edit avatar in db error");
            }
        );

        V_Avatar.style.backgroundImage = new StyleBackground(avatarSprites[selectedIndex + 1]);
        V_HomeAvatar.style.backgroundImage = new StyleBackground(avatarSprites[selectedIndex + 1]);
    }

    public void InitializeHomePage()
    {
        Debug.Log("Initializing home page");
        L_Username.text = UserState.Instance.Username;

        DisplayProfile();
        DisplayBadges();
        DisplayEditProfile();
    }

    public void DisplayProfile()
    {
        Debug.Log("Default profile: " + UserState.Instance.Avatar);

        string avatar = UserState.Instance.Avatar;
        int index = avatarNames.IndexOf(avatar);

        Debug.Log("Avatar: " + avatar);

        if (index == -1)
        {
            Debug.LogError("Avatar not found");
        }
        else
        {
            Debug.Log($"Avatar index found: {index} {avatarSprites[index]}");
            V_Avatar.style.backgroundImage = new StyleBackground(avatarSprites[index]);
            V_HomeAvatar.style.backgroundImage = new StyleBackground(avatarSprites[index]);
        }
    }

    public void DisplayBadges()
    {
        List<VisualElement> badgesContainer = V_Badges
            .Query<VisualElement>(className: "badge-container")
            .ToList();

        totalScoresController.GetAllTotalScores(
            UserState.Instance.Id,
            true,
            (r) =>
            {
                Debug.Log("Result from getting all topic scores: " + r.data);
                badgedScores = r.data;

                Debug.Log("BADGED SCORES: " + badgedScores);

                foreach (var topic in topicsArray)
                {
                    // Find existing badged scores based on topic id
                    if (badgedScores.Find(b => b.topic_id == topic.id) != null)
                    {
                        Debug.Log("User has a badge of " + topic.topic_name + ": " + topic.id);
                    }
                    else
                    {
                        badgesContainer[topic.id - 1].SetEnabled(false);
                    }
                }

                foreach (var badgedScore in badgedScores)
                {
                    Debug.Log("Badged score: " + badgedScore.topic_id);
                }
            },
            (e) => Debug.Log(e)
        );

        Debug.Log(badgesContainer.Count);
    }

    public void DisplayEditProfile()
    {
        // Username
        T_NewFirstname.value = UserState.Instance.Firstname;
        T_NewMiddlename.value = UserState.Instance.Middlename;
        T_NewLastname.value = UserState.Instance.Lastname;

        B_EditButton.RegisterCallback<ClickEvent>(
            (evt) =>
            {
                // Assume NewFirstname, NewMiddlename, and NewLastname are TextField elements
                string email = UserState.Instance.Email;
                string newFirstname = T_NewFirstname.value;
                string newMiddlename = T_NewMiddlename.value;
                string newLastname = T_NewLastname.value;

                Debug.Log($"Email: {email}");

                if (newFirstname.Trim() == "" || newLastname.Trim() == "")
                {
                    IntegrateUI.MessageBox("Names cannot be empty");
                    return;
                }

                userController.EditName(
                    email,
                    newFirstname,
                    newMiddlename,
                    newLastname,
                    (r) =>
                    {
                        Debug.Log("Edit name successful: " + r.message);
                        IntegrateUI.MessageBox(r.message);

                        UserState.Instance.Firstname = newFirstname;
                        UserState.Instance.Middlename = newMiddlename;
                        UserState.Instance.Lastname = newLastname;

                        ClearEditProfileInputs();
                        // Optionally update UI or user state here
                    },
                    (e) =>
                    {
                        Debug.LogError("Edit name failed: " + e);
                        IntegrateUI.MessageBox(e);
                        ClearEditProfileInputs();
                        // Optionally notify user about error here
                    }
                );
            }
        );
    }
}
