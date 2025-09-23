using UnityEngine;
using UnityEngine.UIElements;

public class RegistrationPage : MonoBehaviour
{
    // private RegistrationManager registrationManager;

    // void OnEnable()
    // {
    //     registrationManager = GetComponent<RegistrationManager>();
    //     VisualElement root = GetComponent<UIDocument>().rootVisualElement;
    //     VisualElement V_Main = root.Q<VisualElement>("V_Main");
    //     VisualElement registrationPage = V_Main.Q<VisualElement>("registrationScreen");

    //     VisualElement loginPage = V_Main.Q<VisualElement>("loginScreen");

    //     Button registerBtn = registrationPage.Q<Button>("B_RegisterBtn");
    //     Button loginBtn = registrationPage.Q<Button>("B_LoginBtn");

    //     registerBtn?.RegisterCallback<ClickEvent>(_ =>
    //     {
    //         TextField name = registrationPage.Q<TextField>("T_Username");
    //         TextField email = registrationPage.Q<TextField>("T_Email");
    //         TextField password = registrationPage.Q<TextField>("T_Password");
    //         TextField cpassword = registrationPage.Q<TextField>("T_ConfirmPassword");
    //         TextField fname = registrationPage.Q<TextField>("T_FirstName");
    //         TextField mname = registrationPage.Q<TextField>("T_MiddleName");
    //         TextField lname = registrationPage.Q<TextField>("T_LastName");

    //         if (!password.text.Equals(cpassword.text))
    //         {
    //             Debug.Log("Confirm password");
    //         }

    //         Debug.Log(
    //             $"email: {email.text} name: {name.text} password: {password.text} fname: {fname.text} mname: {mname.text} lname: {lname.text}"
    //         );

    //         RegistrationManager.SignupRequest bodyRequest = new()
    //         {
    //             name = name.text,
    //             email = email.text,
    //             password = password.text,
    //             fname = fname.text,
    //             mname = mname.text,
    //             lname = lname.text,
    //         };

    //         registrationManager.StartSignup(bodyRequest);
    //     });

    //     loginBtn?.RegisterCallback<ClickEvent>(_ =>
    //     {
    //         registrationPage.style.display = DisplayStyle.None;
    //         loginPage.style.display = DisplayStyle.Flex;
    //     });
    // }
    private VisualElement root;
    private VisualElement V_Main;
    private VisualElement V_LoginPage;

    private VisualElement V_RegistrationPages;
    private Button B_BackRegistration;

    // Registration 1
    private VisualElement V_Registration_1;
    private TextField T_Email;
    private TextField T_Pass;
    private TextField T_ConfirmPass;
    private Button B_Proceed;

    // Registration 2
    private VisualElement V_Registration_2;
    private TextField T_Code_1;
    private TextField T_Code_2;
    private TextField T_Code_3;
    private TextField T_Code_4;
    private TextField T_Code_5;
    private TextField T_Code_6;
    private Button B_ConfirmAccount;

    // Registration 3
    private VisualElement V_Registration_3;

    private TextField T_Username;
    private TextField T_FirstName;
    private TextField T_MiddleName;
    private TextField T_LastName;
    private Button B_CreateAccount;

    private EmailVerificationController emailVerificationController;

    private RegistrationManager registrationManager;
    private string user_username;
    private string user_firstname;
    private string user_middlename;
    private string user_lastname;
    private string user_email;
    private string user_pass;

    private VisualElement loginPage;

    void OnEnable()
    {
        emailVerificationController = GetComponent<EmailVerificationController>();
        registrationManager = GetComponent<RegistrationManager>();

        root = GetComponent<UIDocument>().rootVisualElement;
        V_Main = root.Q<VisualElement>("V_Main");

        loginPage = V_Main.Q<VisualElement>("loginScreen");

        V_RegistrationPages = V_Main.Q<VisualElement>("V_RegistrationPages");
        B_BackRegistration = V_RegistrationPages.Q<Button>("B_BackRegistration");

        // Registration 1

        V_Registration_1 = V_RegistrationPages.Q<VisualElement>("V_Registration_1");

        T_Email = V_Registration_1.Q<TextField>("T_Email");
        T_Pass = V_Registration_1.Q<TextField>("T_Pass");
        T_ConfirmPass = V_Registration_1.Q<TextField>("T_ConfirmPass");

        B_Proceed = V_Registration_1.Q<Button>("B_Proceed");

        // Registration 2
        V_Registration_2 = V_RegistrationPages.Q<VisualElement>("V_Registration_2");
        T_Code_1 = V_Registration_2.Q<TextField>("T_Code_1");
        T_Code_2 = V_Registration_2.Q<TextField>("T_Code_2");
        T_Code_3 = V_Registration_2.Q<TextField>("T_Code_3");
        T_Code_4 = V_Registration_2.Q<TextField>("T_Code_4");
        T_Code_5 = V_Registration_2.Q<TextField>("T_Code_5");
        T_Code_6 = V_Registration_2.Q<TextField>("T_Code_6");
        B_ConfirmAccount = V_Registration_2.Q<Button>("B_ConfirmAccount");

        V_Registration_3 = V_RegistrationPages.Q<VisualElement>("V_Registration_3");
        T_Username = V_Registration_3.Q<TextField>("T_Username");
        T_FirstName = V_Registration_3.Q<TextField>("T_FirstName");
        T_MiddleName = V_Registration_3.Q<TextField>("T_MiddleName");
        T_LastName = V_Registration_3.Q<TextField>("T_LastName");

        B_CreateAccount = V_Registration_3.Q<Button>("B_CreateAccount");

        B_BackRegistration?.RegisterCallback<ClickEvent>(_ =>
        {
            Debug.Log("Back registration");

            V_RegistrationPages.style.display = DisplayStyle.None;
            V_Registration_1.style.display = DisplayStyle.Flex;
            V_Registration_2.style.display = DisplayStyle.None;
            V_Registration_3.style.display = DisplayStyle.None;

            loginPage.style.display = DisplayStyle.Flex;
        });

        void ClearForm1()
        {
            T_Email.value = "";
            T_Pass.value = "";
            T_ConfirmPass.value = "";
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
            T_Username.value = "";
            T_FirstName.value = "";
            T_MiddleName.value = "";
            T_LastName.value = "";
        }

        B_Proceed?.RegisterCallback<ClickEvent>(_ =>
        {
            if (T_Pass.value.Length < 8)
            {
                Debug.Log("Password too short");
                IntegrateUI.MessageBox(V_RegistrationPages, "Password too short");
                return;
            }

            if (T_Pass.value != T_ConfirmPass.value)
            {
                Debug.Log("Password should match");
                IntegrateUI.MessageBox(V_RegistrationPages, "Password should match");
                return;
            }

            B_Proceed.SetEnabled(false);

            // Check regex first for email before assigning
            user_email = T_Email.value;
            user_pass = T_Pass.value;

            emailVerificationController.IsEmailValid(
                user_email,
                (r) =>
                {
                    IntegrateUI.MessageBox(V_RegistrationPages, r.message);

                    if (r.success)
                    {
                        Debug.Log("Email is valid!!!");

                        emailVerificationController.CreateEmailVerification(
                            user_email,
                            "creation",
                            (r) =>
                            {
                                Debug.Log(r);

                                V_Registration_1.style.display = DisplayStyle.None;
                                V_Registration_2.style.display = DisplayStyle.Flex;

                                ClearForm1();

                                T_Code_1.Focus();

                            },
                            (e) =>
                            {
                                Debug.Log(e);
                            }
                        );
                    }
                    else
                    {
                        Debug.Log("Email is not valid!!!");
                    }

                    B_Proceed.SetEnabled(true);
                },
                (e) =>
                {
                    Debug.Log("Error: " + e);
                    B_Proceed.SetEnabled(true);
                }
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
                IntegrateUI.MessageBox(V_RegistrationPages, "Code should be 6 digits");
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

            B_ConfirmAccount.SetEnabled(false);
            emailVerificationController.VerifyCode(
                user_email,
                assembledCode,
                (r) =>
                {
                    // if success, clear the form


                    IntegrateUI.MessageBox(V_RegistrationPages, r.message);
                    if (r.success)
                    {
                        Debug.Log("Code matched: " + r.success);
                        V_Registration_2.style.display = DisplayStyle.None;
                        V_Registration_3.style.display = DisplayStyle.Flex;

                        ClearForm2();
                    }
                    B_ConfirmAccount.SetEnabled(true);

                },
                (e) =>
                {
                    Debug.Log("Verify code error: " + e);

                    B_ConfirmAccount.SetEnabled(true);
                }
            );

            // if (true)
            // {
            //     V_Registration_2.style.display = DisplayStyle.None;
            //     V_Registration_3.style.display = DisplayStyle.Flex;
            // }
        });

        B_CreateAccount?.RegisterCallback<ClickEvent>(_ =>
        {
            if (T_Username.value.Length <= 5)
            {
                IntegrateUI.MessageBox(V_RegistrationPages, "Username should 6 characters or long");
                return;
            }

            if (T_FirstName.value.Length == 0)
            {
                IntegrateUI.MessageBox(V_RegistrationPages, "First name required");
                T_FirstName.Focus();
                return;
            }

            if (T_LastName.value.Length == 0)
            {
                IntegrateUI.MessageBox(V_RegistrationPages, "Last name required");
                T_LastName.Focus();
                return;
            }

            user_username = T_Username.value;
            user_firstname = T_FirstName.value;
            user_middlename = T_MiddleName.value;
            user_lastname = T_LastName.value;
            // UI Hides in side the StartSignup Function
            // or maybe move the hiding of UI here
            registrationManager.StartSignup(
                new()
                {
                    name = user_username,
                    email = user_email,
                    password = user_pass,
                    fname = user_firstname,
                    mname = user_middlename,
                    lname = user_lastname,
                }
            );

            user_email = "";
            user_pass = "";
            user_username = "";
            user_firstname = "";
            user_middlename = "";
            user_lastname = "";

            ClearForm3();

            // Trigger create account then if succeeds
            // void HideRegistationAndDirectToLoginPage()
            // {

            // }
        });
    }
}
