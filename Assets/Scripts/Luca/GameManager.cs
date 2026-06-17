using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float maxSanity = 100f;
    public float currentSanity;
    public float maxSatisfaction = 100f;
    public float currentSatisfaction;
    [SerializeField] private float drainRate = 0.25f;
    [SerializeField] private int activeEvents = 0;

    [Header("Game Settings")] 
    [SerializeField] private float totalGameTime = 300f;
    private float timer;

    [Header("temporary variables")] 
    [SerializeField] private GameObject winHue;
    [SerializeField] private GameObject loseHue;

    public static event Action OnPause, OnResume;
    
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
        winHue.SetActive(false);
        loseHue.SetActive(false);
        
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
        winHue.SetActive(true);
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
        loseHue.SetActive(true);
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
