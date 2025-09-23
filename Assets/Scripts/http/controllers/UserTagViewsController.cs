using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserTagViews
{
    public List<UserTagView> data;
}

[System.Serializable]
public class UserTagView
{
    public int id;
    public int user_id;
    public int tag_id;
    public bool is_viewed;
}

[System.Serializable]
public class Validity
{
    public bool comparison_result;
}

[System.Serializable]
public class CreateUserTagViewResponse
{
    public int id;
    public int user_id;
    public int tag_id;
    public bool is_viewed;
}

[System.Serializable]
public class CreateUserTagViewBody
{
    public int user_id;
    public string tag_name;
}

[System.Serializable]
public class ResultsByUserIdAndTopicId
{
    public int id;
    public int user_id;
    public string name;
}

public class DataResultByUserIdAndTopicId
{
    public List<ResultsByUserIdAndTopicId> data;
}

class UserTagViewsController : MonoBehaviour
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

    public void GetUserTagViewsByUserId(
        int user_id,
        // string tagname = "",
        Action<UserTagViews> onSuccess,
        Action<string> onError
    )
    {
        string url = $"{Constants.API_URL}/user-tag-views?user_id={user_id}";

        StartCoroutine(
            httpManager.GetRequest<UserTagViews>(
                url,
                (r) => onSuccess?.Invoke(r),
                (e) => onError?.Invoke(e)
            )
        );
    }

    // Individual checking
    public void GetUserTagViewsByUserIdOrTagName(
        int user_id,
        string tagname,
        Action<UserTagView> onSuccess,
        Action<string> onError
    )
    {
        string url = $"{Constants.API_URL}/user-tag-views?user_id={user_id}&tag_name={tagname}";

        StartCoroutine(
            httpManager.GetRequest<UserTagView>(
                url,
                (r) => onSuccess?.Invoke(r),
                (e) => onError?.Invoke(e)
            )
        );
    }

    public void CreateUserTagView(
        int user_id,
        string tagName,
        Action<CreateUserTagViewResponse> onSuccess,
        Action<string> onError
    )
    {
        string url = $"{Constants.API_URL}/user-tag-views";

        CreateUserTagViewBody newUserTagView = new() { user_id = user_id, tag_name = tagName };
        string jsonData = JsonUtility.ToJson(newUserTagView);

        StartCoroutine(
            httpManager.PostRequest<CreateUserTagViewResponse>(
                url,
                jsonData,
                (r) => onSuccess?.Invoke(r),
                (e) => onError?.Invoke(e)
            )
        );
    }

    public void CheckValidity(
        int user_id,
        int topic_id,
        Action<Validity> onSuccess,
        Action<string> onError
    )
    {
        string url =
            $"{Constants.API_URL}/user-tag-views/check-validity?user_id={user_id}&topic_id={topic_id}";

        StartCoroutine(
            httpManager.GetRequest<Validity>(
                url,
                (r) => onSuccess?.Invoke(r),
                (e) => onError?.Invoke(e)
            )
        );
    }

    public void GetUserTagViewsByUserIdAndTopicId(
        int user_id,
        int topic_id,
        Action<DataResultByUserIdAndTopicId> onSuccess,
        Action<string> onError
    )
    {
        string url =
            $"{Constants.API_URL}/user-tag-views/by-uid-and-tid?user_id={user_id}&topic_id={topic_id}";

        Debug.Log($"URL: {url}");

        StartCoroutine(
            httpManager.GetRequest<DataResultByUserIdAndTopicId>(
                url,
                (r) =>
                {
                    Debug.Log("From root of iser tag views: " + JsonUtility.ToJson(r));
                    onSuccess?.Invoke(r);
                },
                (e) => onError?.Invoke(e)
            )
        );
    }
}
