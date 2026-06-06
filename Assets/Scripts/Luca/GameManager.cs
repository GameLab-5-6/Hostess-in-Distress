using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static event Action OnPause, OnResume;
    
    private void OnEnable()
    {
        InputManager.OnPauseRequested += PauseGame;
        InputManager.OnResumeRequested += ResumeGame;
    }

    private void OnDisable()
    {
        InputManager.OnPauseRequested -= PauseGame;
        InputManager.OnResumeRequested -= ResumeGame;
    }

    private void PauseGame()
    {
        if (Time.timeScale != 0f)
        {
            Debug.Log("Pause Game");
            Time.timeScale = 0f;
            OnPause?.Invoke();
            InputManager.OnPauseAllowed?.Invoke();
        }
    }

    private void ResumeGame()
    {
        if (Time.timeScale == 0f)
        {
            Debug.Log("Resume Game");
            Time.timeScale = 1f;
            OnResume?.Invoke();
            InputManager.OnResumeAllowed?.Invoke();
        }
    }
}
