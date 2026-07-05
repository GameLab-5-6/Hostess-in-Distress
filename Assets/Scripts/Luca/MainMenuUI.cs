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

    public static event Action<float, float, float> OnPrefsUpdated;
    
    private void Awake()
    {
        //Switches to UI Inputs at start
        InputManager.OnPauseAllowed?.Invoke();
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
        
        mainSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.5f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
        bgSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
    }
    
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
    }

    public void UpdateVolumePref()
    {
        OnPrefsUpdated?.Invoke(mainSlider.value, sfxSlider.value, bgSlider.value);
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
