using System;
using UnityEngine;

[System.Serializable]
public class DeleteUserBody
{
    public string email;
}

[System.Serializable]
public class ResponseMessage
{
    public string message;
    public bool success;
}

class UserController : MonoBehaviour
{
    private HTTPManager httpManager;

    void Awake()
    {
        httpManager = GetComponent<HTTPManager>();
        if (httpManager == null)
        {
            Debug.LogError("HTTPManager component not found!");
        }
    }

    public void DeleteUser(string email, Action<ResponseMessage> onSuccess, Action<string> onError)
    {
        string url = $"{Constants.API_URL}/api/users?email={email}";

        StartCoroutine(
            httpManager.DeleteRequest<ResponseMessage>(
                url,
                (r) => onSuccess?.Invoke(r),
                (e) => onError?.Invoke(e)
            )
        );
    }
}
