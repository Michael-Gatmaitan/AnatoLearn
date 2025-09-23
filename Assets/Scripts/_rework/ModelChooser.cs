using System;
using System.Collections.Generic;
using UnityEngine;

public class ModelChooser : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public List<GameObject> animators;
    public List<GameObject> tagClickManagers;

    private GameObject activeAnimator;
    private GameObject activeTCM;

    void Start()
    {
        Debug.Log("--- Model chooser ---");

        try
        {
            int topic_id = UserState.Instance.TopicId;

            Debug.Log($"Current topic id: {topic_id}");

            // Choose the animatore and it's TCM (TagClickManager)
            if (topic_id == 0)
            {
                activeAnimator = animators[0];
                activeTCM = tagClickManagers[0];
            }
            else
            {
                activeAnimator = animators[topic_id - 1];
                activeTCM = tagClickManagers[topic_id - 1];
            }

            Debug.Log($"Topic id from model chooser: {topic_id}");

            if (activeAnimator == null || activeTCM == null)
            {
                Debug.Log("Active animator is null");
                return;
            }

            // Then activate them both
            activeAnimator.SetActive(true);
            activeTCM.SetActive(true);
        }
        catch (Exception)
        {
            Debug.Log("Error has occured, entering dev mode");
            activeAnimator = animators[0];
            activeTCM = tagClickManagers[0];
            activeAnimator.SetActive(true);
            activeTCM.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update() { }
}
