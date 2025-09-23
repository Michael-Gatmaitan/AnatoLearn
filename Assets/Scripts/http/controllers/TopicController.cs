using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AllTopicResponse
{
    public List<Topic> data;
}

[System.Serializable]
public class Topic
{
    public int id;
    public string topic_name;
}

public class TopicController : MonoBehaviour
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

    public void GetAllTopics(
        System.Action<AllTopicResponse> onSuccess,
        System.Action<string> onError
    )
    {
        string url = $"{Constants.API_URL}/topics";
        Debug.Log(url);

        StartCoroutine(
            httpManager.GetRequest<AllTopicResponse>(
                url,
                (response) =>
                {
                    if (response != null)
                    {
                        onSuccess?.Invoke(response);
                    }
                },
                (error) => onError?.Invoke(error)
            )
        );
    }

    public void GetTopicById(
        int topic_id,
        System.Action<Topic> onSuccess,
        System.Action<string> onError
    )
    {
        string url = $"{Constants.API_URL}/topics?topic_id={topic_id}";

        StartCoroutine(
            httpManager.GetRequest<Topic>(
                url,
                (r) => onSuccess?.Invoke(r),
                (e) => onError?.Invoke(e)
            )
        );
    }
}
