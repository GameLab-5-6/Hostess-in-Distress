using System;
using UnityEngine;

public class ChildEvent : MonoBehaviour, IEventable
{
    private Outline outline;
    
    [SerializeField] private float timeBeforeReactivation;
    private float timer;
    
    [Header("Solution")]
    [SerializeField] private float solSatisfactionGained;
    [SerializeField] private float solSanityChange;
    [Header("Knockout")]
    [SerializeField] private float koSatisfactionChange;
    [SerializeField] private float koSanityChange;
    
    private bool canActivate, isActive;

    public static event Action<int> OnUpdateActiveEvents;
    public static event Action<float, float> OnEventSolution, OnEventKnockout;

    private void Awake()
    {
        outline = GetComponent<Outline>();
    }
    
    private void Start()
    {
        canActivate = false;
        isActive = false;
        outline.enabled = false;
        gameObject.layer = LayerMask.NameToLayer("Default");
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
        canActivate = true;
        isActive = true;
        outline.enabled = true;
        gameObject.layer = LayerMask.NameToLayer("Eventable");
        
        OnUpdateActiveEvents?.Invoke(1);
    }

    public void Solution()
    {
        isActive = false;
        outline.enabled = false;
        gameObject.layer = LayerMask.NameToLayer("Default");
        
        OnUpdateActiveEvents?.Invoke(-1);
        OnEventSolution?.Invoke(solSatisfactionGained, solSanityChange);
    }

    public void Knockout()
    {
        canActivate = false;
        isActive = false;
        outline.enabled = false;
        gameObject.layer = LayerMask.NameToLayer("Default");
        
        OnUpdateActiveEvents?.Invoke(-1);
        OnEventKnockout?.Invoke(koSatisfactionChange, koSanityChange);
    }
}
