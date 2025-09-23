using System;
using UnityEngine;

[System.Serializable]
public class UserTopicProgressResponse
{
    public int id;
    public int user_id;
    public int topic_id;
    public bool lesson_completed;
    public bool explore_completed;
    public bool activities_completed;
}

[System.Serializable]
public class UserTopicProgressBody
{
    public int user_id;
    public int topic_id;
}

public class UpdateTopicProgressBody
{
    public int user_id;
    public int topic_id;
    public string update_type;
}

public class UserTopicProgressController : MonoBehaviour
{
    private HTTPManager httpManager;

    void Awake()
    {
        httpManager = GetComponent<HTTPManager>();
        if (httpManager == null)
        {
            Debug.LogError("HTTPManager component not found!");
        }
        else
        {
            Debug.Log("User topic progress controller all ready set!!!");
        }
    }

    public void GetOrCreateUserTopicProgressController(
        int topic_id,
        int user_id,
        Action<UserTopicProgressResponse> onSuccess,
        Action<string> onError
    )
    {
        UserTopicProgressBody body = new UserTopicProgressBody
        {
            user_id = user_id,
            topic_id = topic_id,
        };

        string jsonData = JsonUtility.ToJson(body);
        string url =
            $"{Constants.API_URL}/user-topic-progress?topic_id={topic_id}&user_id={user_id}";

        StartCoroutine(
            httpManager.PostRequest<UserTopicProgressResponse>(
                url,
                jsonData,
                (r) => onSuccess?.Invoke(r),
                (e) => onError?.Invoke(e)
            )
        );
    }

    public void UpdateUserTopicProgress(
        int user_id,
        int topic_id,
        string update_type,
        Action<UserTopicProgressResponse> onSuccess,
        Action<string> onError
    )
    {
        UpdateTopicProgressBody body = new UpdateTopicProgressBody
        {
            user_id = user_id,
            topic_id = topic_id,
            update_type = update_type,
        };

        string url = $"{Constants.API_URL}/user-topic-progress";

        string requestBody = JsonUtility.ToJson(body);

        StartCoroutine(
            httpManager.PutRequest<UserTopicProgressResponse>(
                url,
                requestBody,
                (r) => onSuccess?.Invoke(r),
                (e) => onError?.Invoke(e)
            )
        );
    }
}
