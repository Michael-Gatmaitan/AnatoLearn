using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TotalScore
{
    public int id;
    public int user_id;
    public int topic_id;
    public int total_score;
    public int accuracy;
    public string created_at;
    public int time_left;
}

[System.Serializable]
public class TotalScores
{
    public List<TotalScore> data;
}

[Serializable]
public class Count
{
    public int count;
}

public class TotalScoresController : MonoBehaviour
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

    public void GetTotalScoresByUserIdAndTopicId(
        int user_id,
        int topic_id,
        bool get_passed_scores,
        System.Action<TotalScores> onSuccess,
        System.Action<string> onError
    )
    {
        string gps = get_passed_scores == true ? "true" : "false";
        string url =
            $"{Constants.API_URL}/total-scores?user_id={user_id}&topic_id={topic_id}&get_passed_scores={gps}";

        Debug.Log(url);

        StartCoroutine(
            httpManager.GetRequest<TotalScores>(
                url,
                (r) => onSuccess?.Invoke(r),
                (e) => onError?.Invoke(e)
            )
        );
    }

    public void GetTotalAttempts(
        int user_id,
        int topic_id,
        Action<Count> onSuccess,
        Action<string> onError
    )
    {
        string url =
            $"{Constants.API_URL}/total-scores/total-attempts?user_id={user_id}&topic_id={topic_id}";
        Debug.Log(url);
        // http://localhost:8000/total-scores/total-attempts?user_id=7&topic_id=1
        StartCoroutine(
            httpManager.GetRequest<Count>(
                url,
                (r) => onSuccess?.Invoke(r),
                (e) => onError?.Invoke(e)
            )
        );
    }

    public void GetAllTotalScores(
        int user_id,
        bool get_passed_scores,
        Action<TotalScores> onSuccess,
        Action<string> onError
    )
    {
        string url =
            $"{Constants.API_URL}/total-scores/u?user_id={user_id}&get_passed_scores={get_passed_scores}";

        StartCoroutine(
            httpManager.GetRequest<TotalScores>(
                url,
                (r) => onSuccess?.Invoke(r),
                (e) => onError?.Invoke(e)
            )
        );
    }
}
