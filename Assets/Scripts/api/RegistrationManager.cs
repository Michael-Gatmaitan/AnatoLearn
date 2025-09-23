using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public class RegistrationManager : MonoBehaviour
{
    [System.Serializable]
    public class SignupRequest
    {
        public string name;
        public string email;
        public string password;
        public string fname;
        public string mname;
        public string lname;
    }

    [System.Serializable]
    public class SignupResponse
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

        // For errors
        public string message;
    }

    [System.Serializable]
    public class UserError
    {
        public string message;
        public string type;
    }

    private VisualElement root;

    // private VisualElement registrationPage;
    private VisualElement V_RegistrationPages;
    private VisualElement V_Registration_3;

    // private VisualElement homePage;
    private VisualElement loginScreen;

    void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        // registrationPage = root.Q<VisualElement>("registrationScreen");
        V_RegistrationPages = root.Q<VisualElement>("V_RegistrationPages");
        V_Registration_3 = V_RegistrationPages.Q<VisualElement>("V_Registration_3");
        // homePage = root.Q<VisualElement>("homePage");
        loginScreen = root.Q<VisualElement>("loginScreen");
    }

    public void StartSignup(SignupRequest requestBody)
    {
        // SignupRequest newUser = new SignupRequest
        // {
        //     name = "mike123",
        //     email = "mike123@gmail.com",
        //     password = "mikepogi",
        //     fname = "mike",
        //     mname = "z",
        //     lname = "gatmaitan",
        // };
        StartCoroutine(Signup(requestBody));
    }

    IEnumerator Signup(SignupRequest requestBody)
    {
        // Set loading here
        // Button signupButton = registrationPage.Q<Button>("B_RegisterBtn");
        // signupButton.SetEnabled(false);
        // signupButton.text = "Creating account...";
        Button B_CreateAccount = V_Registration_3.Q<Button>("B_CreateAccount");

        string json = JsonUtility.ToJson(requestBody);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        UnityWebRequest request = new UnityWebRequest($"{Constants.API_URL}/auth/signup", "POST");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            IntegrateUI.MessageBox(V_RegistrationPages, "Account successfully created");
            Debug.Log("Signup successful: " + request.downloadHandler.text);
            B_CreateAccount.SetEnabled(true);
            // B_CreateAccount.text = "Register";

            SignupResponse response = JsonUtility.FromJson<SignupResponse>(
                request.downloadHandler.text
            );

            // Debug.Log("Welcome, " + response.user.name);
            Debug.Log("New user created: " + response.user.email);

            // Redirect to login page
            // registrationPage.style.display = DisplayStyle.None;
            V_RegistrationPages.style.display = DisplayStyle.None;
            V_Registration_3.style.display = DisplayStyle.None;
            // homePage.style.display = DisplayStyle.Flex;
            loginScreen.style.display = DisplayStyle.Flex;
        }
        else
        {
            B_CreateAccount.SetEnabled(true);
            // signupButton.text = "Register";
            // Debug.LogError("Signup message: " + request.user.message);
            Debug.LogError("Signup failed: " + request.error);
            Debug.LogError(request.downloadHandler.text);

            UserError error = JsonUtility.FromJson<UserError>(request.downloadHandler.text);

            Debug.LogError("Error Message: " + error.message);
            IntegrateUI.MessageBox(V_RegistrationPages, error.message);

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
