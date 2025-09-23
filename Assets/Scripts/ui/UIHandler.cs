using System.Collections.Generic;
using Google.Protobuf.WellKnownTypes;
using UnityEngine;
using UnityEngine.UIElements;

public class UIHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public UIDocument uiDocument;
    private VisualElement loginPage;
    private VisualElement signupPage;
    private VisualElement homePage;
    private List<VisualElement> pages;
    VisualElement root;

    private HTTPManager httpManager;

    void Start()
    {
        httpManager = FindAnyObjectByType<HTTPManager>();
    }

    void SetupHomePageEvents()
    {
        List<Button> systemButtons = homePage.Query<Button>(className: "B_System").ToList();
        VisualElement introVid = homePage.Q<VisualElement>("V_IntroVid");
        Button skip = introVid.Q<Button>("B_Skip");

        VisualElement chooseModeContainer = homePage.Q("V_ChooseMode");
        Button back = chooseModeContainer.Q<Button>("B_Back");

        string cur = "";
        for (int i = 0; i < systemButtons.Count; i++)
        {
            var systemButton = systemButtons[i];
            systemButton.clicked += () =>
            {
                cur = systemButton.name;
                introVid.style.display = DisplayStyle.Flex;
            };
            Debug.Log(systemButton);
        }

        skip.clicked += () =>
        {
            introVid.style.display = DisplayStyle.None;
            showModal();
        };

        Button modeAR = chooseModeContainer.Q<Button>("B_ARMode");
        Button mode3D = chooseModeContainer.Q<Button>("B_3DMode");

        void modeAREvent()
        {
            Debug.Log("Mode ar event: " + cur);
        }

        void mode3DEvent()
        {
            Debug.Log("Mode 3d event: " + cur);
        }

        modeAR.clicked += modeAREvent;
        mode3D.clicked += mode3DEvent;

        void showModal()
        {
            Debug.Log("Modal event");
            chooseModeContainer.style.display = DisplayStyle.Flex;
        }

        back.clicked += () =>
        {
            chooseModeContainer.style.display = DisplayStyle.None;
        };
    }

    void OnEnable()
    {
        root = uiDocument.rootVisualElement;

        // This will find all of the VisualElement that has ".page" class on it
        pages = root.Query(className: "page").ToList();

        loginPage = root.Q<VisualElement>("LoginPage");
        signupPage = root.Q<VisualElement>("SignupPage");

        homePage = root.Q<VisualElement>("HomePage");

        // From login to signup
        Button gotoSignup = loginPage.Q<Button>("B_GotoSignup");
        AddButtonRoute(gotoSignup, PagesMap.SignupPage);

        // From signup to login
        Button gotoLogin = signupPage.Q<Button>("B_GotoLogin");
        AddButtonRoute(gotoLogin, PagesMap.LoginPage);

        // Setup events
        SetupHomePageEvents();
    }

    // This just helps me to visualize in more verbose way of how I
    // handle routes
    void AddButtonRoute(Button btn, PagesMap pageName)
    {
        btn.clicked += () => ShowPage(pageName);
    }

    // Aight, I don't want to manuallt put if else here so I am gonna
    // use the pages and map them to use if else.
    public void ShowPage(PagesMap pageName)
    {
        VisualElement page = root.Q<VisualElement>(pageName.Value);

        if (pages.IndexOf(page) < 0)
            Debug.Log("Page not found");

        foreach (VisualElement currentPage in pages)
            currentPage.style.display = page == currentPage ? DisplayStyle.Flex : DisplayStyle.None;

        if (page.name == PagesMap.HomePage.Value)
        {
            SetupHomePageInstance();
        }
    }

    // UI Setups
    void SetupHomePageInstance()
    {
        Label usernameLabel = homePage.Q<Label>("L_NavUsername");

        string username = UserState.Instance.Username;
        usernameLabel.text = username;
        Debug.Log("Nav username has been set");

        // StartCoroutine(
        //     httpManager.GetRequest<Topics>(
        //         "http://localhost:8000/topics",
        //         (response) =>
        //         {
        //             Debug.Log(response);
        //         },
        //         (error) =>
        //         {
        //             Debug.LogError("Error: " + error);
        //         }
        //     )
        // );

        // void SetupTopicLabels(string response)
        // {
        //     var labels = root.Query<Label>(className: "L_Topic").ToList();

        //     for (int i = 0; i < labels.Count; i++)
        //     {
        //         labels[i].text = response[i];
        //     }
        // }
    }
}
