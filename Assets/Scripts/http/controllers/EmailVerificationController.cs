using System;
using UnityEngine;

public class EmailVerificationController : MonoBehaviour
{
    [System.Serializable]
    public class CreateVerificationBody
    {
        public string email;
        public string verificationType;
    }

    [System.Serializable]
    public class CreateVerificationResponse
    {
        public string message;
        public bool success;
    }

    [System.Serializable]
    public class VerifyCodeBody
    {
        public string email;
        public string code;
    }

    [System.Serializable]
    public class ResetPasswordBody
    {
        public string email;
        public string newPassword;
    }

    private HTTPManager httpManager;

    void Awake()
    {
        httpManager = GetComponent<HTTPManager>();
        if (httpManager == null)
        {
            Debug.Log("Http manager component not found in act qa controller");
        }
    }

    public void IsEmailValid(
        string email,
        Action<CreateVerificationResponse> onSuccess,
        Action<string> onError
    )
    {
        string url = $"{Constants.API_URL}/is-email-valid?email={email}";
        StartCoroutine(
            httpManager.GetRequest<CreateVerificationResponse>(
                url,
                (r) => onSuccess?.Invoke(r),
                (e) => onError?.Invoke(e)
            )
        );
    }

    public void CreateEmailVerification(
        string email,
        string verificationType,
        Action<CreateVerificationResponse> onSuccess,
        Action<string> onError
    )
    {
        CreateVerificationBody body = new() { email = email, verificationType = verificationType };

        string jsonData = JsonUtility.ToJson(body);
        string url = $"{Constants.API_URL}/send-verification";

        StartCoroutine(
            httpManager.PostRequest<CreateVerificationResponse>(
                url,
                jsonData,
                (r) => onSuccess?.Invoke(r),
                (e) => onError?.Invoke(e)
            )
        );
    }

    public void VerifyCode(
        string email,
        string code,
        Action<CreateVerificationResponse> onSuccess,
        Action<string> onError
    )
    {
        VerifyCodeBody body = new() { email = email, code = code };

        string jsonData = JsonUtility.ToJson(body);
        string url = $"{Constants.API_URL}/verify-code";

        StartCoroutine(
            httpManager.PostRequest<CreateVerificationResponse>(
                url,
                jsonData,
                (r) => onSuccess?.Invoke(r),
                (e) => onError?.Invoke(e)
            )
        );
    }

    public void ResetPassword(
        string email,
        string newPassword,
        Action<CreateVerificationResponse> onSuccess,
        Action<string> onError
    )
    {
        ResetPasswordBody body = new() { email = email, newPassword = newPassword };

        string jsonData = JsonUtility.ToJson(body);
        string url = $"{Constants.API_URL}/reset-password";

        StartCoroutine(
            httpManager.PutRequest<CreateVerificationResponse>(
                url,
                jsonData,
                (r) => onSuccess?.Invoke(r),
                (e) => onError?.Invoke(e)
            )
        );
    }
}
