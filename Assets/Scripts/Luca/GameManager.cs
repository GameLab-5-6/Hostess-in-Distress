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

    public static event Action OnPause, OnResume;
    
    private void OnEnable()
    {
        InputManager.OnPauseRequested += PauseGame;
        InputManager.OnResumeRequested += ResumeGame;

        EventManager.OnBarsUpdated += UpdateBarsValues;
        
        //Event event actions
        BabyEvent.OnUpdateActiveEvents += UpdateActiveEvents;
    }

    private void OnDisable()
    {
        InputManager.OnPauseRequested -= PauseGame;
        InputManager.OnResumeRequested -= ResumeGame;
        
        EventManager.OnBarsUpdated -= UpdateBarsValues;
        
        BabyEvent.OnUpdateActiveEvents -= UpdateActiveEvents;
    }

    private void Start()
    {
        activeEvents = 0;
        currentSanity = maxSanity;
        currentSatisfaction = maxSatisfaction;
    }

    private void Update()
    {
        currentSanity -= Time.deltaTime * drainRate * activeEvents;
    }
    
    private void UpdateActiveEvents(int amount) => activeEvents += amount;

    private void UpdateBarsValues(float satisfaction, float sanity)
    {
        currentSatisfaction += satisfaction;
        if (currentSatisfaction > maxSatisfaction)
            currentSatisfaction = maxSatisfaction;
        
        currentSanity += sanity;
        if (currentSanity > maxSanity)
            currentSanity = maxSanity;
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
