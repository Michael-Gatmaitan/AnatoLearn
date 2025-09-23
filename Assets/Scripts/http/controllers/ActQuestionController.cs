using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestionApiResponse<T>
{
    public List<T> data;
}

[System.Serializable]
public class QuestionTOF
{
    public int id;
    public string question;
    public string answer;
}

[System.Serializable]
public class QuestionMCQ
{
    public int id;
    public int act_id;
    public string question;
    public string answer;

    // public string choices;
    public QuestionChoices choices;
}

[System.Serializable]
public class QuestionChoices
{
    public string a;
    public string b;
    public string c;
    public string d;
}

public class ActQuestionController : MonoBehaviour
{
    private HTTPManager httpManager;

    void Awake()
    {
        httpManager = GetComponent<HTTPManager>();
        if (httpManager == null)
        {
            Debug.LogError("Http manager component not found in act qa controller");
        }
    }

    public void GetActQuestionByTopicId<T>(
        int topic_id,
        string actName,
        System.Action<QuestionApiResponse<T>> onSuccess,
        System.Action<string> onError
    )
    {
        int act_type_id = 0;
        if (actName == "mcq")
        {
            act_type_id = 2;
        }
        else if (actName == "tof")
        {
            act_type_id = 3;
        }

        string url =
            $"{Constants.API_URL}/activities/qa?act_type_id={act_type_id}&topic_id={topic_id}";

        StartCoroutine(
            httpManager.GetRequest<QuestionApiResponse<T>>(
                url,
                (response) =>
                {
                    Debug.Log(response);
                    onSuccess?.Invoke(response);
                },
                (error) =>
                {
                    Debug.Log(error);
                    onError?.Invoke(error);
                }
            )
        );
    }

    // public void GetTOFQuestionByTopicId(int topic_id)
}
