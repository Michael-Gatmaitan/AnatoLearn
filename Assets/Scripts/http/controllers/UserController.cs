using System;
using System.Threading;
using UnityEngine;

[System.Serializable]
public class DeleteUserBody
{
    public string email;
}

[Serializable]
public class EditAvatarBody
{
    public string email;
    public string newAvatar;
    public string updateType = "avatar";
}

[Serializable]
public class EditNameBody
{
    public string email;
    public string newFirstname;
    public string newMiddlename;
    public string newLastname;
    public string updateType = "name";
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

    public void EditAvatar(
        string email,
        string avatar,
        Action<ResponseMessage> onSuccess,
        Action<string> onError
    )
    {
        EditAvatarBody body = new()
        {
            email = email,
            newAvatar = avatar,
            updateType = "avatar",
        };

        string jsonData = JsonUtility.ToJson(body);
        string url = $"{Constants.API_URL}/api/users";

        StartCoroutine(
            httpManager.PutRequest<ResponseMessage>(
                url,
                jsonData,
                (r) => onSuccess?.Invoke(r),
                (e) => onError?.Invoke(e)
            )
        );
    }

    public void EditName(
        string email,
        string fname,
        string mname,
        string lname,
        Action<ResponseMessage> onSuccess,
        Action<string> onError
    )
    {
        EditNameBody body = new()
        {
            email = email,
            newFirstname = fname,
            newMiddlename = mname,
            newLastname = lname,
            updateType = "name",
        };

        string jsonData = JsonUtility.ToJson(body);
        string url = $"{Constants.API_URL}/api/users";

        StartCoroutine(
            httpManager.PutRequest<ResponseMessage>(
                url,
                jsonData,
                (r) => onSuccess?.Invoke(r),
                (e) => onError?.Invoke(e)
            )
        );
    }
}
