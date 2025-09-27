using UnityEngine;

public class UserState : MonoBehaviour
{
    public static UserState Instance { get; private set; }
    public string Username { get; private set; }
    public string Email { get; private set; }
    public int Id;

    public int TopicId { get; private set; }
    public int ActivityId { get; private set; }

    public int CurrentTapScore = 5;
    public int CurrentMCQScore = -1;
    public int CurrentTOFScore = -1;

    // MCQ and TOF time
    public float QuizTimeRemaining = 300f;

    // States
    public bool isFromTapMe = false;
    public bool showProgressionPage = false;

    private void Awake()
    {
        // Ensure only one instance exists
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        TopicId = 0;
        DontDestroyOnLoad(gameObject); // Persist between scenes
    }

    public void SetShowProgressionPage(bool t)
    {
        showProgressionPage = t;
    }

    public bool GetShowProgressionPage()
    {
        return showProgressionPage;
    }

    public void SetQuizTimeRemaining(float time)
    {
        QuizTimeRemaining = time;
    }

    public float GetQuizTimeRemaining()
    {
        return QuizTimeRemaining;
    }

    public void SetUserData(int id, string username, string email)
    {
        Id = id;
        Username = username;
        Email = email;

        // Save to PlayerPrefs
        PlayerPrefs.SetInt("id", id);
        PlayerPrefs.SetString("username", username);
        PlayerPrefs.SetString("email", email);
        PlayerPrefs.Save();
    }

    public void LoadUserData()
    {
        if (PlayerPrefs.HasKey("id"))
        {
            Id = PlayerPrefs.GetInt("id");
            Username = PlayerPrefs.GetString("username");
            Email = PlayerPrefs.GetString("email");
        }
    }

    public void ClearUserData()
    {
        Id = 0;
        Username = null;
        Email = null;

        isFromTapMe = false;

        PlayerPrefs.DeleteKey("id");
        PlayerPrefs.DeleteKey("username");
        PlayerPrefs.DeleteKey("email");
    }

    public bool IsLoggedIn()
    {
        return !string.IsNullOrEmpty(Username) && Id != 0;
    }

    public void SetTopicId(int topicId)
    {
        TopicId = topicId;
        // switch (systemName)
        // {
        //     case "skeletal":
        //         TopicId = 1;
        //         // BodySystem = BodySystem.SkeletalSystem;
        //         break;
        //     case "muscular":
        //         TopicId = 2;
        //         break;
        //     case "nervous":
        //         TopicId = 7; // ??
        //         // BodySystem = BodySystem.NervousSystem;
        //         break;
        //     default:
        //         TopicId = 0;
        //         Debug.LogError($"Invalid system name to set UserState.TopicId: {systemName}");
        //         break;
        // }

        Debug.Log($"UserData.TopicID is set to {TopicId}");
    }

    public int SetActivityId(string activity)
    {
        switch (activity)
        {
            case "tapme":
                ActivityId = 1;
                break;
            case "mcq":
                ActivityId = 2;
                break;
            case "tof":
                ActivityId = 3;
                break;
            default:
                Debug.LogError("Activity not found");
                break;
        }

        Debug.Log($"UserData.ActivityId set to {ActivityId}");
        return ActivityId;
    }

    public int SetTapScore(int score)
    {
        CurrentTapScore = score;
        return CurrentTapScore;
    }

    public int SetMCQScore(int score)
    {
        CurrentMCQScore = score;
        return CurrentMCQScore;
    }

    public int SetTOFScore(int score)
    {
        CurrentTOFScore = score;
        return CurrentMCQScore;
    }

    public void ResetAllScores()
    {
        CurrentTapScore = -1;
        CurrentMCQScore = -1;
        CurrentTOFScore = -1;
    }
}
