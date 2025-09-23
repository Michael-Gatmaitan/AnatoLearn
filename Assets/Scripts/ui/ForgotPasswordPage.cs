using UnityEngine;
using UnityEngine.UIElements;

public class ForgotPasswordPage : MonoBehaviour
{
    private VisualElement root;
    private VisualElement V_Main;

    // Confirm code
    // private VisualElement V_RegistrationPages;
    // private VisualElement V_Registration_2;

    // Send verification

    private VisualElement V_ForgotPasswordPages;
    private Button B_BackForgotPassword;
    private VisualElement V_ForgotPassword_1;
    private TextField T_Email;
    private Button B_SendCode;

    // Confirm code
    private VisualElement V_ForgotPassword_2;
    private TextField T_Code_1;
    private TextField T_Code_2;
    private TextField T_Code_3;
    private TextField T_Code_4;
    private TextField T_Code_5;
    private TextField T_Code_6;
    private Button B_ConfirmAccount;

    // Change password
    private VisualElement V_ForgotPassword_3;
    private TextField T_NewPass;
    private TextField T_ConfirmNewPass;
    private Button B_ChangePassword;

    // Controller
    private EmailVerificationController emailVerificationController;

    private string user_email;

    private VisualElement loginPage;

    void OnEnable()
    {
        emailVerificationController = GetComponent<EmailVerificationController>();

        root = GetComponent<UIDocument>().rootVisualElement;
        V_Main = root.Q<VisualElement>("V_Main");
        V_ForgotPasswordPages = V_Main.Q<VisualElement>("V_ForgotPasswordPages");
        B_BackForgotPassword = V_ForgotPasswordPages.Q<Button>("B_BackForgotPassword");

        loginPage = V_Main.Q<VisualElement>("loginScreen");

        // Send code
        V_ForgotPassword_1 = V_ForgotPasswordPages.Q<VisualElement>("V_ForgotPassword_1");
        T_Email = V_ForgotPassword_1.Q<TextField>("T_Email");
        B_SendCode = V_ForgotPassword_1.Q<Button>("B_SendCode");

        // Registration 2
        V_ForgotPassword_2 = V_ForgotPasswordPages.Q<VisualElement>("V_ForgotPassword_2");
        T_Code_1 = V_ForgotPassword_2.Q<TextField>("T_Code_1");
        T_Code_2 = V_ForgotPassword_2.Q<TextField>("T_Code_2");
        T_Code_3 = V_ForgotPassword_2.Q<TextField>("T_Code_3");
        T_Code_4 = V_ForgotPassword_2.Q<TextField>("T_Code_4");
        T_Code_5 = V_ForgotPassword_2.Q<TextField>("T_Code_5");
        T_Code_6 = V_ForgotPassword_2.Q<TextField>("T_Code_6");
        B_ConfirmAccount = V_ForgotPassword_2.Q<Button>("B_ConfirmAccount");

        // Change password
        V_ForgotPassword_3 = V_ForgotPasswordPages.Q<VisualElement>("V_ForgotPassword_3");
        T_NewPass = V_ForgotPassword_3.Q<TextField>("T_NewPass");
        T_ConfirmNewPass = V_ForgotPassword_3.Q<TextField>("T_ConfirmNewPass");
        B_ChangePassword = V_ForgotPassword_3.Q<Button>("B_ChangePassword");

        B_BackForgotPassword?.RegisterCallback<ClickEvent>(_ =>
        {
            Debug.Log("Back forgot password");
            V_ForgotPasswordPages.style.display = DisplayStyle.None;
            V_ForgotPassword_1.style.display = DisplayStyle.Flex;
            V_ForgotPassword_2.style.display = DisplayStyle.None;
            V_ForgotPassword_3.style.display = DisplayStyle.None;

            loginPage.style.display = DisplayStyle.Flex;
        });

        void ClearForm1()
        {
            T_Email.value = "";
        }

        void ClearForm2()
        {
            T_Code_1.value = "";
            T_Code_2.value = "";
            T_Code_3.value = "";
            T_Code_4.value = "";
            T_Code_5.value = "";
            T_Code_6.value = "";
        }

        void ClearForm3()
        {
            T_NewPass.value = "";
            T_ConfirmNewPass.value = "";
        }

        B_SendCode?.RegisterCallback<ClickEvent>(_ =>
        {
            // ^[a-z0-9](\.?[a-z0-9]){5,}@g(oogle)?mail\.com$
            string email = T_Email.value;

            IntegrateUI.MessageBox(V_ForgotPasswordPages, "Sending verification code");

            B_SendCode.SetEnabled(false);

            emailVerificationController.IsEmailValid(
                email,
                (r) =>
                {
                    Debug.Log("Forgot password is email valid: " + r.message + r.success);

                    if (!r.success)
                    {
                        // Send code
                        emailVerificationController.CreateEmailVerification(
                            email,
                            "recovery",
                            (r) =>
                            {
                                Debug.Log("Forgot password verification sent: " + r);

                                // V_ForgotPassword_3.style.display = DisplayStyle.Flex;
                                V_ForgotPassword_2.style.display = DisplayStyle.Flex;
                                V_ForgotPassword_1.style.display = DisplayStyle.None;
                                user_email = T_Email.value;

                                IntegrateUI.MessageBox(
                                    V_ForgotPasswordPages,
                                    "Code sent to your email."
                                );

                                ClearForm1();

                                B_SendCode.SetEnabled(true);
                            },
                            (e) =>
                            {
                                Debug.Log("Forgot password verification error" + e);

                                B_SendCode.SetEnabled(true);
                            }
                        );
                    }
                },
                (e) => Debug.Log("Forgot password is email valid error" + e)
            );
        });

        T_Code_1?.RegisterCallback<ChangeEvent<string>>(_ =>
        {
            if (_.newValue.Length >= 1)
                T_Code_2.Focus();
        });
        T_Code_2?.RegisterCallback<ChangeEvent<string>>(_ =>
        {
            if (_.newValue.Length >= 1)
                T_Code_3.Focus();
        });
        T_Code_3?.RegisterCallback<ChangeEvent<string>>(_ =>
        {
            if (_.newValue.Length >= 1)
                T_Code_4.Focus();
        });
        T_Code_4?.RegisterCallback<ChangeEvent<string>>(_ =>
        {
            if (_.newValue.Length >= 1)
                T_Code_5.Focus();
        });
        T_Code_5?.RegisterCallback<ChangeEvent<string>>(_ =>
        {
            if (_.newValue.Length >= 1)
                T_Code_6.Focus();
        });

        B_ConfirmAccount?.RegisterCallback<ClickEvent>(_ =>
        {
            if (
                T_Code_1.value.Length != 1
                || T_Code_2.value.Length != 1
                || T_Code_3.value.Length != 1
                || T_Code_4.value.Length != 1
                || T_Code_5.value.Length != 1
                || T_Code_6.value.Length != 1
            )
            {
                IntegrateUI.MessageBox(V_ForgotPasswordPages, "Code should be 6 digits");
                return;
            }

            string assembledCode =
                T_Code_1.value
                + T_Code_2.value
                + T_Code_3.value
                + T_Code_4.value
                + T_Code_5.value
                + T_Code_6.value;
            Debug.Log("Code: " + assembledCode);

            // Call API to check if the code is valid

            B_ConfirmAccount.SetEnabled(false);

            emailVerificationController.VerifyCode(
                user_email,
                assembledCode,
                (r) =>
                {
                    // if success, clear the form


                    IntegrateUI.MessageBox(V_ForgotPasswordPages, r.message);
                    if (r.success)
                    {
                        Debug.Log("Code matched: " + r.success);
                        V_ForgotPassword_2.style.display = DisplayStyle.None;
                        V_ForgotPassword_3.style.display = DisplayStyle.Flex;

                        ClearForm2();
                    }

                    B_ConfirmAccount.SetEnabled(true);
                },
                (e) =>
                {
                    Debug.Log("Verify code error: " + e);
                    B_ConfirmAccount.SetEnabled(true);
                    // IntegrateUI.MessageBox(V_ForgotPasswordPages, )
                }
            );
        });

        B_ChangePassword?.RegisterCallback<ClickEvent>(_ =>
        {
            string newPass = T_NewPass.value;
            string confirmNewPass = T_ConfirmNewPass.value;

            if (newPass != confirmNewPass)
            {
                IntegrateUI.MessageBox(V_ForgotPasswordPages, "Password should match");
                T_ConfirmNewPass.Focus();
                return;
            }

            Debug.Log("Change pass: " + newPass + confirmNewPass);

            B_ChangePassword.SetEnabled(false);
            emailVerificationController.ResetPassword(
                user_email,
                newPass,
                (r) =>
                {
                    Debug.Log("Rest password result: " + r);

                    if (r.success)
                    {
                        ClearForm3();
                        // Show login page
                        IntegrateUI.MessageBox(V_ForgotPasswordPages, r.message);
                        V_ForgotPasswordPages.style.display = DisplayStyle.None;
                        V_ForgotPassword_3.style.display = DisplayStyle.None;
                        loginPage.style.display = DisplayStyle.Flex;
                    }
                    B_ChangePassword.SetEnabled(true);
                },
                (e) =>
                {
                    Debug.Log("Error in change pasword: " + e);
                    B_ChangePassword.SetEnabled(true);
                }
            );
        });
    }
}
