using UnityEngine;
using UnityEngine.UIElements;

public class VolumeController : MonoBehaviour
{
    private SliderInt volumeSlider;      // BGM
    private SliderInt sfxVolumeSlider;   // SFX

    private VisualElement bgmFillBar;    // Fill bar for BGM
    private VisualElement sfxFillBar;    // Fill bar for SFX

    private void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        // === BG MUSIC SLIDER ===
        volumeSlider = root.Q<SliderInt>("volumeSlider");
        if (volumeSlider == null)
        {
            Debug.LogError("❌ [VolumeController] BGM Slider NOT found in UXML!");
            return;
        }

        int savedBgm = PlayerPrefs.GetInt("BgmVolume", 50);
        volumeSlider.value = savedBgm;
        Debug.Log($"[VolumeController] Loaded saved BGM volume: {savedBgm}");

        volumeSlider.RegisterValueChangedCallback(evt =>
        {
            PlayerPrefs.SetInt("BgmVolume", evt.newValue);
            PlayerPrefs.Save();
            Debug.Log($"🎚️ [VolumeController] BGM volume changed → {evt.newValue}");
            UpdateBgmFill(evt.newValue);
        });

        // Add custom fill bar for BGM
        var bgmTracker = volumeSlider.Q<VisualElement>("unity-tracker");
        if (bgmTracker != null)
        {
            bgmFillBar = new VisualElement { name = "bgm-fill-bar" };
            bgmFillBar.style.backgroundColor =  new Color(1f, 0.7f, 0.2f); // orange fill
            bgmFillBar.style.height = Length.Percent(100);
            bgmFillBar.style.width = Length.Percent(0);
            bgmFillBar.style.position = Position.Absolute;
            bgmFillBar.style.left = 0;
            bgmFillBar.style.top = 0;

            bgmTracker.Add(bgmFillBar);

            // update once on load
            UpdateBgmFill(savedBgm);
        }

        // === SFX SLIDER ===
        sfxVolumeSlider = root.Q<SliderInt>("sfxVolumeSlider");
        if (sfxVolumeSlider == null)
        {
            Debug.LogError("❌ [VolumeController] SFX Slider NOT found in UXML!");
            return;
        }

        int savedSfx = PlayerPrefs.GetInt("SfxVolume", 50);
        sfxVolumeSlider.value = savedSfx;
        Debug.Log($"[VolumeController] Loaded saved SFX volume: {savedSfx}");

        sfxVolumeSlider.RegisterValueChangedCallback(evt =>
        {
            PlayerPrefs.SetInt("SfxVolume", evt.newValue);
            PlayerPrefs.Save();
            Debug.Log($"🎚️ [VolumeController] SFX volume changed → {evt.newValue}");
            UpdateSfxFill(evt.newValue);
        });

        // Add custom fill bar for SFX
        var sfxTracker = sfxVolumeSlider.Q<VisualElement>("unity-tracker");
        if (sfxTracker != null)
        {
            sfxFillBar = new VisualElement { name = "sfx-fill-bar" };
            sfxFillBar.style.backgroundColor = new Color(1f, 0.7f, 0.2f); // orange fill
            sfxFillBar.style.height = Length.Percent(100);
            sfxFillBar.style.width = Length.Percent(0);
            sfxFillBar.style.position = Position.Absolute;
            sfxFillBar.style.left = 0;
            sfxFillBar.style.top = 0;

            sfxTracker.Add(sfxFillBar);

            // update once on load
            UpdateSfxFill(savedSfx);
        }
    }

    private void UpdateBgmFill(int value)
    {
        float percent = Mathf.InverseLerp(volumeSlider.lowValue, volumeSlider.highValue, value);
        if (bgmFillBar != null)
            bgmFillBar.style.width = Length.Percent(percent * 100f);
    }

    private void UpdateSfxFill(int value)
    {
        float percent = Mathf.InverseLerp(sfxVolumeSlider.lowValue, sfxVolumeSlider.highValue, value);
        if (sfxFillBar != null)
            sfxFillBar.style.width = Length.Percent(percent * 100f);
    }
}
