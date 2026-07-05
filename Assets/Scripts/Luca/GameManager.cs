using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float maxSanity = 100f;
    public float currentSanity;
    public float maxSatisfaction = 100f;
    public float currentSatisfaction;
    [SerializeField] private float drainRate = 0.25f;
    [SerializeField] private int activeEvents = 0;

    [Header("Game Settings")] 
    public float totalGameTime = 300f;
    private float timer;

    public static event Action OnPause, OnResume, OnGameWin, OnGameLose;
    
    private void OnEnable()
    {
        InputManager.OnPauseRequested += PauseGame;
        InputManager.OnResumeRequested += ResumeGame;
        
        //Event event actions
        EventBase.OnUpdateActiveEvents += UpdateActiveEvents;
        EventBase.OnEventSolution += UpdateBarValues;
        EventBase.OnEventKnockout += UpdateBarValues;
    }

    private void OnDisable()
    {
        InputManager.OnPauseRequested -= PauseGame;
        InputManager.OnResumeRequested -= ResumeGame;
        
        EventBase.OnUpdateActiveEvents -= UpdateActiveEvents;
        EventBase.OnEventSolution -= UpdateBarValues;
        EventBase.OnEventKnockout -= UpdateBarValues;
    }

    private void Start()
    {
        activeEvents = 0;
        currentSanity = maxSanity;
        currentSatisfaction = maxSatisfaction;
    }

    private void Update()
    {
        GameTime();
        StatusCheck();
        
        currentSanity -= Time.deltaTime * drainRate * activeEvents;
    }
    
    private void UpdateActiveEvents(int amount) => activeEvents += amount;

    private void UpdateBarValues(float satisfaction, float sanity)
    {
        currentSatisfaction += satisfaction;
        if (currentSatisfaction > maxSatisfaction)
            currentSatisfaction = maxSatisfaction;
        
        currentSanity += sanity;
        if (currentSanity > maxSanity)
            currentSanity = maxSanity;
    }

    private void GameTime()
    {
        timer += Time.deltaTime;

        if (timer >= totalGameTime)
        {
            WinGame();
        }
    }

    private void WinGame()
    {
        OnGameWin?.Invoke();
        Time.timeScale = 0f;
        AudioListener.pause = true;
    }

    private void StatusCheck()
    {
        if (currentSanity <= 0f || currentSatisfaction <= 0f)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        OnGameLose?.Invoke();
        Time.timeScale = 0f;
        AudioListener.pause = true;
    }
    
    private void PauseGame()
    {
        if (Time.timeScale != 0f)
        {
            Time.timeScale = 0f;
            AudioListener.pause = true;
            OnPause?.Invoke();
            InputManager.OnPauseAllowed?.Invoke();
        }
    }

    public void ResumeGame()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
            AudioListener.pause = false;
            OnResume?.Invoke();
            InputManager.OnResumeAllowed?.Invoke();
        }
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
