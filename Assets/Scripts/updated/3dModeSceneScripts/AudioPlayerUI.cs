using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using System.Collections.Generic;


/// Plays voice over and bgmusic.

public class AudioPlayerUI : MonoBehaviour
{
    private UIDocument uiDoc;
    private Dictionary<Button, VisualElement> buttonToBarFill = new();
    private AudioSource voiceSource;

    // Expose BGM as a static property
    public static AudioSource BgmSource { get; private set; }

    private Button currentButton;
    private VisualElement currentBarFill;
    private AudioClip currentClip;

    void Awake()  // <-- assign early so others can use it in OnEnable
    {
        var audioSources = GetComponents<AudioSource>();
        if (audioSources.Length < 2)
        {
            Debug.LogError("❌ [AudioPlayerUI] Need 2 AudioSources: one for BGM, one for VoiceOvers.");
            return;
        }

        BgmSource = audioSources[0];
        voiceSource = audioSources[1];
        voiceSource.loop = false;

        Debug.Log($"[AudioPlayerUI] Awake → BgmSource assigned? {BgmSource != null}, VoiceSource assigned? {voiceSource != null}");
    }

    void Start()
    {   
        SceneData.LanguageVersion = "englishVersion";    

        Debug.Log("[AudioPlayerUI] Start() running...");
        PlayBgMusic();

        uiDoc = GetComponent<UIDocument>();
        var root = uiDoc.rootVisualElement;
        if (root == null)
        {
            Debug.LogError("❌ [AudioPlayerUI] UIDocument rootVisualElement is NULL!");
            return;
        }

        var allButtons = root.Query<Button>().ToList();
        Debug.Log($"[AudioPlayerUI] Found {allButtons.Count} buttons in UI.");

        foreach (var btn in allButtons)
        {
            if (!btn.name.EndsWith("PlayBtn")) continue;

            string baseName = btn.name.Replace("PlayBtn", "");
            string containerName = baseName + "DescriptionCon";
            var container = root.Q<VisualElement>(containerName);
            if (container == null)
            {
                Debug.LogWarning($"❌ [AudioPlayerUI] No container found: {containerName}");
                continue;
            }

            var barFill = container.Q<VisualElement>("audioProgressFill");
            if (barFill == null)
            {
                Debug.LogWarning($"❌ [AudioPlayerUI] No audioProgressFill in container: {containerName}");
                continue;
            }

            buttonToBarFill[btn] = barFill;
            btn.clicked += () => OnPlayClicked(btn, baseName);
            Debug.Log($"[AudioPlayerUI] Hooked up Play button: {btn.name} → {containerName}");

            var exitBtn = container.Q<Button>("exitDescriptionConBtn");
            if (exitBtn != null)
            {
                exitBtn.clicked += OnExitClicked;
                Debug.Log($"[AudioPlayerUI] Hooked up Exit button for {containerName}");
            }
            else
            {
                Debug.LogWarning($"❌ [AudioPlayerUI] exitDescriptionConBtn not found in: {containerName}");
            }
        }
    }

    private void PlayBgMusic()
    {
        AudioClip bgMusic = Resources.Load<AudioClip>("uiAudio/3dModeBgMusic");
        if (bgMusic == null)
        {
            Debug.LogError("❌ [AudioPlayerUI] Could not load 'uiAudio/3dModeBgMusic'");
            return;
        }

        Debug.Log("🎶 [AudioPlayerUI] Starting background music...");
        BgmSource.loop = true;
        BgmSource.clip = bgMusic;

        // read saved value from VolumeController
        int saved = PlayerPrefs.GetInt("BgmVolume", 50);
        float volume = saved / 100f;
        BgmSource.volume = volume;

        Debug.Log($"[AudioPlayerUI] Applied saved BGM volume: {saved} ({volume:0.00})");
        BgmSource.Play();
    }


    void Update()
    {
        if (voiceSource != null && voiceSource.isPlaying && currentBarFill != null && currentClip != null)
        {
            float progress = voiceSource.time / currentClip.length;
            currentBarFill.style.width = new Length(progress * 100f, LengthUnit.Percent);
            Debug.Log($"[AudioPlayerUI] Voice progress: {progress * 100f:0.0}%");
        }
    }

    void OnPlayClicked(Button clickedBtn, string baseName)
    {
        Debug.Log($"▶️ [AudioPlayerUI] Play button clicked: {clickedBtn.name}, baseName={baseName}");

        string audioFileName = (SceneData.LanguageVersion == "tagalogVersion")
            ? baseName.ToLower() + "Tag"
            : baseName.ToLower();

        Debug.Log($"[AudioPlayerUI] Trying to load clip: Audio/{audioFileName}");
        AudioClip clip = Resources.Load<AudioClip>("Audio/" + audioFileName);
        if (clip == null)
        {
            Debug.LogError($"❌ [AudioPlayerUI] Audio/{audioFileName} not found!");
            return;
        }

        if (voiceSource.isPlaying && clickedBtn == currentButton)
        {
            Debug.Log("[AudioPlayerUI] Voice already playing → Pausing.");
            voiceSource.Pause();
            return;
        }

        if (BgmSource.isPlaying)
        {
            Debug.Log("[AudioPlayerUI] Pausing BGM while voice plays...");
            BgmSource.Pause();
        }

        voiceSource.Stop();
        voiceSource.clip = clip;
        voiceSource.time = 0;
        voiceSource.Play();
        Debug.Log($"[AudioPlayerUI] Playing voice clip: {clip.name}, length={clip.length:0.00}s");

        StartCoroutine(ResumeBGMAfterVoice(clip.length));

        currentButton = clickedBtn;
        currentClip = clip;
        currentBarFill = buttonToBarFill[clickedBtn];
    }

    void OnExitClicked()
    {
        Debug.Log("[AudioPlayerUI] Exit button clicked.");

        if (voiceSource.isPlaying)
        {
            Debug.Log("[AudioPlayerUI] Stopping voice playback.");
            voiceSource.Stop();
        }

        if (currentBarFill != null)
        {
            currentBarFill.style.width = new Length(0, LengthUnit.Percent);
            Debug.Log("[AudioPlayerUI] Reset progress bar.");
        }

        currentButton = null;
        currentClip = null;
        currentBarFill = null;

        if (!BgmSource.isPlaying)
        {
            Debug.Log("[AudioPlayerUI] Resuming BGM after exit.");
            BgmSource.UnPause();
        }
    }

    IEnumerator ResumeBGMAfterVoice(float delay)
    {
        Debug.Log($"[AudioPlayerUI] Will try to resume BGM after {delay:0.00}s...");
        yield return new WaitForSeconds(delay);

        if (!voiceSource.isPlaying && !BgmSource.isPlaying)
        {
            Debug.Log("[AudioPlayerUI] Voice finished → Resuming BGM.");
            BgmSource.UnPause();
        }
        else
        {
            Debug.Log("[AudioPlayerUI] Voice still playing or BGM already running, not resuming.");
        }
    }
}
