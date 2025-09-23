using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

/// <summary>
/// Plays sound effects for UI buttons in the 3D mode.
/// </summary>
public class UIButtonSoundManager : MonoBehaviour
{
    // [SerializeField] private int audioSourceIndex = 3; // default → 3rd AudioSource in Inspector
    private AudioSource audioSource;
    private Dictionary<string, AudioClip> buttonSounds;

    // List of UXML button names that should play sounds
    private readonly string[] buttonNames =
    {
        "menuBtn",
        "homeBtn",
        "langBtn",
        "hideBtn",
        "tagBtn",
        "boneClassBtn",
        "boneDivBtn",
        "funFactBtn",
        "heart3dModeBtn",
        "neuronsBtn",
        "finishBtn"
    };

    private void Awake()
    {
        Debug.Log("🎬 UIButtonSoundManager Awake()");

        // Get all AudioSources from this GameObject
        var sources = GetComponents<AudioSource>();
        // if (sources.Length <= audioSourceIndex)
        // {
        //     Debug.LogError($"❌ Not enough AudioSources! Need at least {audioSourceIndex + 1} on {gameObject.name}");
        //     return;
        // }

        audioSource = sources[2];
        // Debug.Log($"✅ Using AudioSource[{audioSourceIndex}] on {gameObject.name}");

        // Find UI Document
        var uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null)
        {
            Debug.LogError("❌ No UIDocument found on this GameObject!");
            return;
        }

        var root = uiDocument.rootVisualElement;
        if (root == null)
        {
            Debug.LogError("❌ UIDocument rootVisualElement is NULL!");
            return;
        }

        // Load sounds
        buttonSounds = new Dictionary<string, AudioClip>();
        foreach (var btnName in buttonNames)
        {
            var clip = Resources.Load<AudioClip>($"uiAudio/{btnName}");
            if (clip != null)
            {
                buttonSounds[btnName] = clip;
                Debug.Log($"✅ Loaded sound for {btnName}");
            }
            else
            {
                Debug.LogWarning($"⚠️ No sound found at Resources/uiAudio/{btnName}");
            }
        }

        // Hook buttons
        foreach (var btnName in buttonNames)
        {
            var button = root.Q<Button>(btnName);
            if (button != null)
            {
                string captured = btnName; // prevent closure issue
                button.clicked += () => PlayButtonSound(captured);
                Debug.Log($"🎯 Hooked sound effect to {btnName}");
            }
            else
            {
                Debug.LogWarning($"⚠️ Button {btnName} not found in UXML!");
            }
        }
    }

    /// <summary>
    /// Plays the sound effect associated with a button.
    /// </summary>
    private void PlayButtonSound(string btnName)
{
    if (audioSource == null)
    {
        Debug.LogError("❌ UIButtonSoundManager: AudioSource is missing!");
        return;
    }

    if (buttonSounds.TryGetValue(btnName, out AudioClip clip) && clip != null)
    {
        Debug.Log($"🔊 Playing sound for {btnName}");

        int saved = PlayerPrefs.GetInt("SfxVolume", 50);
        float volume = saved / 100f;

        // Use PlayOneShot with volume
        audioSource.PlayOneShot(clip, volume);
    }
    else
    {
        Debug.LogWarning($"⚠️ No clip assigned for {btnName}");
    }
}

}
