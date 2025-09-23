using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;

public class FullscreenVideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public Camera mainCamera;
    public Camera fullscreenCamera;
    public UIDocument uiDocument;
    private VisualElement root;

    // Call this function to start playing the video
    public void PlayFullscreenVideo()
    {
        mainCamera.gameObject.SetActive(false);
        fullscreenCamera.gameObject.SetActive(true);
        videoPlayer.Play();
    }

    // Call this function when the video is finished
    void OnVideoFinished(VideoPlayer vp)
    {
        mainCamera.gameObject.SetActive(true);
        fullscreenCamera.gameObject.SetActive(false);
    }

    void Start()
    {
        // root = uiDocument.rootVisualElement;
        // root.style.display = DisplayStyle.None;
        // Add a listener to know when the video finishes
        videoPlayer.loopPointReached += OnVideoFinished;
    }
}