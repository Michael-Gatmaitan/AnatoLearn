using UnityEngine;
using UnityEngine.UIElements;

public class LoginPage : MonoBehaviour
{
    private LoginManager loginManager;
    private VisualElement V_Main;
    private VisualElement V_RegistrationPages;
    private VisualElement V_Registration_1;
    private VisualElement loginPage;

    void OnEnable()
    {
        loginManager = GetComponent<LoginManager>();
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        V_Main = root.Q<VisualElement>("V_Main");
        loginPage = V_Main.Q<VisualElement>("loginScreen");
        V_RegistrationPages = V_Main.Q<VisualElement>("V_RegistrationPages");
        V_Registration_1 = V_RegistrationPages.Q<VisualElement>("V_Registration_1");

        Button loginBtn = loginPage.Q<Button>("B_LoginBtn");
        Button registerBtn = loginPage.Q<Button>("B_RegisterBtn");
        Button B_ForgetPass = loginPage.Q<Button>("B_ForgotPass");

        VisualElement V_ForgotPasswordPages = V_Main.Q<VisualElement>("V_ForgotPasswordPages");
        VisualElement V_ForgotPassword_1 = V_ForgotPasswordPages.Q<VisualElement>(
            "V_ForgotPassword_1"
        );

        loginBtn?.RegisterCallback<ClickEvent>(_ =>
        {
            // T_Username
            // T_Pass
            string email = loginPage.Q<TextField>("T_Email").text;
            string password = loginPage.Q<TextField>("T_Pass").text;

            Debug.Log($"Email: {email} Password: {password}");

            LoginManager.LoginRequest request = new() { email = email, password = password };

            loginManager.StartLogin(request);
        });

        registerBtn?.RegisterCallback<ClickEvent>(_ =>
        {
            loginPage.style.display = DisplayStyle.None;
            // registrationPage.style.display = DisplayStyle.Flex;

            V_RegistrationPages.style.display = DisplayStyle.Flex;
            V_Registration_1.style.display = DisplayStyle.Flex;
        });

        B_ForgetPass?.RegisterCallback<ClickEvent>(_ =>
        {
            V_ForgotPasswordPages.style.display = DisplayStyle.Flex;
            V_ForgotPassword_1.style.display = DisplayStyle.Flex;
            loginPage.style.display = DisplayStyle.None;
        });
    }
}
