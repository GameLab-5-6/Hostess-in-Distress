using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject tutorialPanel;

    [SerializeField] private Slider mainSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider bgSlider;

    public static event Action<string, float> OnPrefsUpdated;
    
    private void Awake()
    {
        //Switches to UI Inputs at start
        InputManager.OnPauseAllowed?.Invoke();
        Time.timeScale = 0f;
    }
    
    private void OnEnable()
    {
        InputManager.OnResumeRequested += ReturnToMainMenu;
    }

    private void OnDisable()
    {
        InputManager.OnResumeRequested -= ReturnToMainMenu;
    }

    private void Start()
    {
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
        tutorialPanel.SetActive(false);
        
        mainSlider.value = PlayerPrefs.GetFloat(AudioManager.MASTER_VOLUME_KEY, 0.5f);
        sfxSlider.value = PlayerPrefs.GetFloat(AudioManager.SFX_VOLUME_KEY, 0.5f);
        bgSlider.value = PlayerPrefs.GetFloat(AudioManager.MUSIC_VOLUME_KEY, 0.5f);
    }
    
    public void PlayGame()
    {
        SceneManager.LoadScene("Scenes/Main");
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }

    public void UpdateMainPref()
    {
        string key = AudioManager.MASTER_VOLUME_KEY;
        float volume = mainSlider.value;
        
        OnPrefsUpdated?.Invoke(key, volume);
    }

    public void UpdateSFXPref()
    {
        string key = AudioManager.SFX_VOLUME_KEY;
        float volume = sfxSlider.value;
        
        OnPrefsUpdated?.Invoke(key, volume);
    }

    public void UpdateBGPref()
    {
        string key = AudioManager.MUSIC_VOLUME_KEY;
        float volume = bgSlider.value;
        
        OnPrefsUpdated?.Invoke(key, volume);
    }

    public void OpenTutorial()
    {
        tutorialPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }
    
    public void ReturnToMainMenu()
    {
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
        tutorialPanel.SetActive(false);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
