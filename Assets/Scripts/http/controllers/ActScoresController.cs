using System.Collections.Generic;
using Google.Protobuf.WellKnownTypes;
using Unity.Burst.CompilerServices;
using UnityEngine;

[System.Serializable]
public class ActScore
{
    public int id;
    public int act_type_id;
    public int user_id;
    public int topic_id;
    public float score;
    public string created_at;
    public int total_scores_id;
}

[System.Serializable]
public class ActScorePostResponse
{
    public string message;
    public bool success;
}

[System.Serializable]
public class AllActScoreGetResponse
{
    public List<ActScore> data;
}

[System.Serializable]
public class CreateTotalScoreBody
{
    public ScoresBody scores;
    public int topic_id;
    public int user_id;
    public int time_left;
}

[System.Serializable]
public class ScoresBody
{
    public int tap;
    public int mcq;
    public int tof;
}

[System.Serializable]
public class CreateTotalScoreResponse
{
    public string message;
    public bool success;
}

public class ActScoresController : MonoBehaviour
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

    public void GetActScores(
        int user_id,
        int act_type_id,
        int topic_id,
        System.Action<ActScore> onSuccess,
        System.Action<string> onError
    )
    {
        string url =
            $"{Constants.API_URL}/activity-scores?user_id={user_id}&act_type_id={act_type_id}&topic_id={topic_id}";

        StartCoroutine(
            httpManager.GetRequest<ActScore>(
                url,
                (response) =>
                {
                    if (response != null)
                    {
                        onSuccess?.Invoke(response);
                    }
                    else
                    {
                        onError?.Invoke("No data received from server");
                    }
                },
                (error) =>
                {
                    onError?.Invoke(error);
                }
            )
        );
    }

    public void GetAllActScores(
        System.Action<AllActScoreGetResponse> onSuccess,
        System.Action<string> onError
    )
    {
        string url = $"{Constants.API_URL}/activity-scores";
        Debug.Log(url);

        StartCoroutine(
            httpManager.GetRequest<AllActScoreGetResponse>(
                url,
                (r) =>
                {
                    onSuccess?.Invoke(r);
                },
                (e) =>
                {
                    onError?.Invoke(e);
                }
            )
        );
    }

    public void CreateActScore(
        int user_id,
        int act_type_id,
        int topic_id,
        float score,
        System.Action<ActScorePostResponse> onSuccess,
        System.Action<string> onError
    )
    {
        string url = $"{Constants.API_URL}/activity-scores";

        ActScore newScore = new ActScore
        {
            user_id = user_id,
            act_type_id = act_type_id,
            topic_id = topic_id,
            score = score,
        };

        string jsonData = JsonUtility.ToJson(newScore);

        StartCoroutine(
            httpManager.PostRequest<ActScorePostResponse>(
                url,
                jsonData,
                (response) =>
                {
                    if (response != null)
                    {
                        onSuccess?.Invoke(response);
                    }
                    else
                    {
                        onError?.Invoke("Failed to parse response from server");
                    }
                },
                (error) =>
                {
                    onError?.Invoke(error);
                }
            )
        );
    }

    public void GetActScoresByTotalScoresId(
        int total_scores_id,
        System.Action<AllActScoreGetResponse> onSuccess,
        System.Action<string> onError
    )
    {
        string url =
            $"{Constants.API_URL}/activity-scores/total-scores?total_scores_id={total_scores_id}";

        StartCoroutine(
            httpManager.GetRequest<AllActScoreGetResponse>(
                url,
                (r) => onSuccess?.Invoke(r),
                (e) => onError?.Invoke(e)
            )
        );
    }

    public void CreateActScoresWithTotalScore(
        CreateTotalScoreBody reqBody,
        System.Action<CreateTotalScoreResponse> onSuccess,
        System.Action<string> onError
    )
    {
        string url = $"{Constants.API_URL}/activity-scores/total-scores";
        Debug.Log($"RECORDING ACTS IN TOPIC: {reqBody.topic_id}");
        string jsonData = JsonUtility.ToJson(reqBody);

        StartCoroutine(
            httpManager.PostRequest<CreateTotalScoreResponse>(
                url,
                jsonData,
                (response) =>
                {
                    if (response != null)
                    {
                        onSuccess?.Invoke(response);
                    }
                    else
                    {
                        onError?.Invoke(
                            "Failed to parse || This error is from post create act score with total score"
                        );
                    }
                },
                (error) =>
                {
                    onError?.Invoke(error);
                }
            )
        );
    }
}
