using System.Collections;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public class LoginManager : MonoBehaviour
{
    [System.Serializable]
    public class LoginRequest
    {
        public string email;
        public string password;
    }

    [System.Serializable]
    public class LoginResponse
    {
        public string message;
        public UserData user;
    }

    [System.Serializable]
    public class UserData
    {
        public int id;
        public string name;
        public string email;
        public string fname;
        public string mname;
        public string lname;
        public string avatar;

        // For errors
        public string message;
    }

    [System.Serializable]
    public class UserError
    {
        public string message;
        public string type;
    }

    // public UIHandler uiHandler;
    private VisualElement root;
    private VisualElement V_Main;
    private VisualElement loginPage;

    private VisualElement homePage;
    private VisualElement progressPage;
    private VisualElement settingsPage;

    private TextField T_Email, T_Pass;

    // private IntegrateUI integrateUI;

    private ProfilePage profilePage;

    void OnEnable()
    {
        // integrateUI = GetComponent<IntegrateUI>();
        root = GetComponent<UIDocument>().rootVisualElement;
        V_Main = root.Q<VisualElement>("V_Main");
        loginPage = V_Main.Q<VisualElement>("loginScreen");
        homePage = V_Main.Q<VisualElement>("homePage");
        progressPage = V_Main.Q<VisualElement>("progressPage");
        settingsPage = V_Main.Q<VisualElement>("settingsPage");

        profilePage = GetComponent<ProfilePage>();

        T_Email = loginPage.Q<TextField>("T_Email");
        T_Pass = loginPage.Q<TextField>("T_Pass");
    }

    public void StartLogin(LoginRequest requestBody)
    {
        StartCoroutine(Login(requestBody));
    }

    IEnumerator Login(LoginRequest requestBody)
    {
        // Set loading here
        Button loginButton = loginPage.Q<Button>("B_LoginBtn");
        loginButton.SetEnabled(false);
        loginButton.text = "Logging in...";

        // Parse body into json
        string json = JsonUtility.ToJson(requestBody);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        // Send a request
        UnityWebRequest request = new UnityWebRequest($"{Constants.API_URL}/auth/login", "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Clear text fields
            T_Email.value = "";
            T_Pass.value = "";

            loginButton.text = "Log in";
            loginButton.SetEnabled(true);

            LoginResponse response = JsonUtility.FromJson<LoginResponse>(
                request.downloadHandler.text
            );

            int id = response.user.id;
            string name = response.user.name;
            string email = response.user.email;
            string fname = response.user.fname;
            string lname = response.user.lname;
            string mname = response.user.mname;
            string avatar = response.user.avatar;

            // Set the data of user to localstorage
            UserState.Instance.SetUserData(id, name, email, fname, lname, mname, avatar);

            string loggedInMessage = $"Welcome, {name}!";
            Debug.Log("You logged in! " + response.user.name);

            IntegrateUI.MessageBox(loggedInMessage);

            loginPage.style.display = DisplayStyle.None;
            // homePage.style.display = DisplayStyle.Flex;

            IntegrateUI.Instance.SetupScoresAndAsyncPages(false);

            // IntegrateUI.Instance.SetupHomeSystems(homePage);
            // IntegrateUI.Instance.SetupProgressPage(progressPage);
            // profilePage.InitializeHomePage();
            // IntegrateUI.Instance.SetupSettingsPage(settingsPage);
        }
        else
        {
            // Debug.LogError("Login failed: " + request.error);
            // Debug.LogError(request.downloadHandler.text);

            loginButton.text = "Log in";
            loginButton.SetEnabled(true);
            Debug.Log(request.downloadHandler.text);
            UserError error = JsonUtility.FromJson<UserError>(request.downloadHandler.text);
            Debug.LogError("Login Error message: " + error.message);

            // Label loginEmailErrLabel = loginPage.Q<Label>("L_EmailErr");
            // Label loginPassErrLabel = loginPage.Q<Label>("L_PassErr");
            // Label loginUserErrLabel = loginPage.Q<Label>("L_UserErr");


            IntegrateUI.MessageBox(error.message);

            // if (error.type == "email")
            //     loginEmailErrLabel.text = error.message;
            // else if (error.type == "password")
            //     loginPassErrLabel.text = error.message;
            // else if (error.type == "user")
            //     loginUserErrLabel.text = error.message;

            // Display error message in UI

            // if (error.type == "email")
            // {
            //     Debug.LogError("Invalid email");
            // }
            // else if (error.type == "password")
            // {
            //     Debug.LogError("Password too short");
            // }
            // else if (error.type == "user")
            // {
            //     Debug.LogError("User not found");
            // }
        }
    }
}
