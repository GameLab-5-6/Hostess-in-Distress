using System;
using UnityEngine;

public class BabyEvent : MonoBehaviour, IEventable
{
    [SerializeField] private GameObject isActiveIndicator;
    
    [SerializeField] private float timeBeforeReactivation;
    private float timer;
    
    private bool canActivate, isActive;

    public static event Action<int> OnUpdateActiveEvents;
    public static event Action OnEventSolution, OnEventKnockout;

    private void Start()
    {
        canActivate = false;
        isActive = false;
        isActiveIndicator.SetActive(false);
    }

    private void Update()
    {
        if (!canActivate)
            return;

        if (!isActive)
        {
            timer += Time.deltaTime;

            if (timer >= timeBeforeReactivation)
            {
                Activate();
                timer = 0f;
            }
        }
    }
    
    public void Activate()
    {
        isActiveIndicator.SetActive(true);
        canActivate = true;
        isActive = true;
        OnUpdateActiveEvents?.Invoke(1);
    }

    public void Solution()
    {
        isActiveIndicator.SetActive(false);
        isActive = false;
        OnUpdateActiveEvents?.Invoke(-1);
        OnEventSolution?.Invoke();
    }

    public void Knockout()
    {
        isActiveIndicator.SetActive(false);
        canActivate = false;
        isActive = false;
        OnUpdateActiveEvents?.Invoke(-1);
        OnEventKnockout?.Invoke();
    }

    public bool IsActive() => isActive;
}
