using System;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private Slider mainSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider bgSlider;
    
    [SerializeField] private AudioClip testSfx;
    [SerializeField][Range(0f, 1f)] private float testSfxVolume;

    public static event Action<string, float> OnPrefsUpdated;

    private void Awake()
    {
        mainSlider.onValueChanged.AddListener(UpdateMainPref);
        sfxSlider.onValueChanged.AddListener(UpdateSFXPref);
        bgSlider.onValueChanged.AddListener(UpdateBGPref);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(AudioManager.MASTER_VOLUME_KEY, mainSlider.value);
        PlayerPrefs.SetFloat(AudioManager.SFX_VOLUME_KEY, sfxSlider.value);
        PlayerPrefs.SetFloat(AudioManager.BG_VOLUME_KEY, bgSlider.value);
    }

    private void Start()
    {
        mainSlider.value = PlayerPrefs.GetFloat(AudioManager.MASTER_VOLUME_KEY, 0.5f);
        sfxSlider.value = PlayerPrefs.GetFloat(AudioManager.SFX_VOLUME_KEY, 0.5f);
        bgSlider.value = PlayerPrefs.GetFloat(AudioManager.BG_VOLUME_KEY, 0.5f);
    }
    
    public void UpdateMainPref(float value)
    {
        string key = AudioManager.MASTER_VOLUME_KEY;
        float volume = value;

        OnPrefsUpdated?.Invoke(key, volume);
    }

    public void UpdateSFXPref(float value)
    {
        string key = AudioManager.SFX_VOLUME_KEY;
        float volume = value;
        
        OnPrefsUpdated?.Invoke(key, volume);
    }

    public void UpdateBGPref(float value)
    {
        string key = AudioManager.BG_VOLUME_KEY;
        float volume = value;
        
        OnPrefsUpdated?.Invoke(key, volume);
    }

    public void PlayTestSFX()
    {
        AudioManager.Instance.PlaySFX2D(testSfx, testSfxVolume);
    }
}
