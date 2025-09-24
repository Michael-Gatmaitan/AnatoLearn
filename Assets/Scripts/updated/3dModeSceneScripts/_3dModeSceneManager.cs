// using UnityEngine;
// using UnityEngine.UIElements;
// using UnityEngine.SceneManagement;
// using System;

// public class RedIncorrectScoreBtnHandler : MonoBehaviour
// {
//     private void OnEnable()
//     {
//         var root = GetComponent<UIDocument>().rootVisualElement;
//         var incorrectScoreRedbox = root.Q<VisualElement>("incorrectScore");

//         if (incorrectScoreRedbox != null)
//         {
//             incorrectScoreRedbox.RegisterCallback<ClickEvent>(evt =>
//             {
//                 SceneData.showScorePage = true;
//                 SceneManager.LoadScene("UIScene1");
//             });
//         }
//         else
//         {
//             Debug.LogWarning("did not clcikec!!");
//         }
//     }
// }


using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class _3dModeSceneManager : MonoBehaviour
{
    private UserTagViewsController userTagViewsController;
    private static UserTopicProgressController userTopicProgressController;

    private void OnEnable()
    {
        userTagViewsController = GetComponent<UserTagViewsController>();
        userTopicProgressController = GetComponent<UserTopicProgressController>();

        var root = GetComponent<UIDocument>().rootVisualElement;
        var finishBtn = root.Q<VisualElement>("finishBtn");
        if (finishBtn != null)
        {
            finishBtn.RegisterCallback<ClickEvent>(evt =>
            {
                // SceneData.showScorePage = true;
                // SceneData.showProgressionPage = true;
                // SceneManager.LoadScene("UIScene1");

                // Check validity of user tag views at current topic
                userTagViewsController.CheckValidity(
                    UserState.Instance.Id,
                    UserState.Instance.TopicId,
                    (r) =>
                    {
                        Debug.Log(
                            $"Result for validity: {r.comparison_result}\nTotal tags in this topic: {r.tags_count_for_topic}\nUser viewed tags: {r.usertagviews_count_for_tags_in_topic}"
                        );

                        if (r.comparison_result)
                        {
                            // Enable the "Finish Button"
                            Debug.Log("You can quiz button now unlocked");
                            // SceneData.showScorePage = true;
                            // SceneData.showProgressionPage = true;
                            UserState.Instance.SetShowProgressionPage(true);
                            SceneManager.LoadScene("UIScene1");

                            userTopicProgressController.UpdateUserTopicProgress(
                                UserState.Instance.Id,
                                UserState.Instance.TopicId,
                                "explore",
                                (r) =>
                                {
                                    Debug.Log("Explore unlocked: " + r);
                                },
                                (e) =>
                                {
                                    Debug.LogError(e);
                                }
                            );
                        }
                        else
                        {
                            // Do nothing
                            Debug.Log("You need to view all body parts to proceed");
                        }
                    },
                    (e) => Debug.Log(e)
                );
            });
        }
        else
        {
            UnityEngine.Debug.LogWarning("did not clcikec!!");
        }

        var incorrectScoreRedbox = root.Q<VisualElement>("incorrectScore"); //edited 6-20
        if (incorrectScoreRedbox != null)
        {
            incorrectScoreRedbox.RegisterCallback<ClickEvent>(evt =>
            {
                SceneData.showScorePage = true;
                SceneManager.LoadScene("UIScene1");
            });
        }
        else
        {
            UnityEngine.Debug.LogWarning("did not clcikec!!");
        }

        // var yesHomeBtn = root.Q<VisualElement>("yesHomeBtn");
        // if (yesHomeBtn != null)
        // {
        //     yesHomeBtn.RegisterCallback<ClickEvent>(evt =>
        //     {
        //         // Subscribe to sceneLoaded event
        //         SceneManager.sceneLoaded += OnSceneLoaded;

        //         // Load UIScene1
        //         SceneManager.LoadScene("UIScene1");
        //     });
        // }
        // else
        // {
        //     // Debug.LogWarning("yesHomeBtn not found!");
        // }

        //heart3dMode btn in 3dMode of circulatory:
        var heart3dModeBtn = root.Q<Button>("heart3dModeBtn"); //edited 6-20
        heart3dModeBtn?.RegisterCallback<ClickEvent>(eevt =>
        {
            SceneManager.LoadScene("3dModeCirculatory_HeartScene");
            UnityEngine.Debug.Log("3dModeCirculatory_HeartScene LOADED");
            // SceneData.takingCirculatoryHeartTapMe = true;
        });
    }

    // private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    // {
    //     if (scene.name == "UIScene1")
    //     {
    //         // Find the UIScreenManager in the newly loaded scene
    //         // UIScreenManager uiScreenManager = FindObjectOfType<UIScreenManager>();
    //         UIScreenManager uiScreenManager = FindFirstObjectByType<UIScreenManager>();

    //         if (uiScreenManager != null)
    //         {
    //             uiScreenManager.ShowHomePage();
    //             // Debug.Log("Homepage displayed!");
    //         }
    //         else
    //         {
    //             // Debug.LogWarning("UIScreenManager not found in UIScene1!");
    //         }

    //         // Always unsubscribe after use to avoid duplicate calls
    //         SceneManager.sceneLoaded -= OnSceneLoaded;
    //     }
    // }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            UserState.Instance.SetShowProgressionPage(true);
            SceneManager.LoadScene("UIScene1");
            return;
        }
    }
}
