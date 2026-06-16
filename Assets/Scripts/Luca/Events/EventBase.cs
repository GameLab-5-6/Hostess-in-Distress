using System;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public abstract class EventBase : MonoBehaviour, IEventable
{
    protected Outline outline;
    
    public float timeBeforeReactivation;
    protected float timer;
    
    [Header("Solution")]
    public float solSatisfactionChange;
    public float solSanityChange;
    [Header("Knockout")]
    public float koSatisfactionChange;
    public float koSanityChange;
    
    protected bool canActivate;
    public bool isActive;

    public static event Action<int> OnUpdateActiveEvents;
    public static event Action<float, float> OnEventSolution, OnEventKnockout;
    
    protected virtual void Awake()
    {
        outline = GetComponent<Outline>();
    }
    
    protected virtual void Start()
    {
        canActivate = false;
        isActive = false;
        outline.enabled = false;
        gameObject.layer = LayerMask.NameToLayer("Default");
    }
    
    protected virtual void Update()
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

    public virtual void Activate()
    {
        canActivate = true;
        isActive = true;
        outline.enabled = true;
        gameObject.layer = LayerMask.NameToLayer("Eventable");
        
        OnUpdateActiveEvents?.Invoke(1);
    }

    public virtual void Solution()
    {
        isActive = false;
        outline.enabled = false;
        gameObject.layer = LayerMask.NameToLayer("Default");
        
        OnUpdateActiveEvents?.Invoke(-1);
        OnEventSolution?.Invoke(solSatisfactionChange, solSanityChange);
    }

    public virtual void Knockout()
    {
        canActivate = false;
        isActive = false;
        outline.enabled = false;
        gameObject.layer = LayerMask.NameToLayer("Default");
        
        OnUpdateActiveEvents?.Invoke(-1);
        OnEventKnockout?.Invoke(koSatisfactionChange, koSanityChange);
    }
}
