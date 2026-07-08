using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject tutorialPanel;
    
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
    }
    
    public void PlayGame()
    {
        InputManager.OnResumeAllowed?.Invoke();
        SceneManager.LoadScene("Scenes/Main");
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        mainMenuPanel.SetActive(false);
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
