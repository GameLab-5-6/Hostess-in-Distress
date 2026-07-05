using System;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public abstract class EventBase : MonoBehaviour, IEventable
{
    private Outline outline;
    
    public float timeBeforeReactivation;
    protected float timer;
    public GameObject exclamationPoint;
    [SerializeField] protected Transform exclamationSpawnPosition;
    
    [Header("Solution")]
    public float solSatisfactionChange;
    public float solSanityChange;
    [Header("Knockout")]
    public float koSatisfactionChange;
    public float koSanityChange;

    [Header("Audio")] 
    public GameObject clipPrefab;
    public AudioClip eventClip;
    public float eventVolume;
    public AudioClip solutionClip;
    public float solutionVolume;
    public AudioClip koClip;
    public float koVolume;
    public float timeInBetweenAudio;
    protected float audioTimer;
    
    protected bool canActivate;
    public bool isActive;

    public static event Action<int> OnUpdateActiveEvents;
    public static event Action<float, float> OnEventSolution, OnEventKnockout;
    
    protected virtual void Awake()
    {
        outline = GetComponent<Outline>();
        if (exclamationPoint != null)
        {
            exclamationPoint = Instantiate(exclamationPoint, exclamationSpawnPosition.position, Quaternion.identity, exclamationSpawnPosition);
            exclamationPoint.SetActive(false);
        }
        
        clipPrefab = Instantiate(clipPrefab, transform.position, Quaternion.identity, transform);
        clipPrefab.GetComponent<AudioSource>().clip = eventClip;
        clipPrefab.GetComponent<AudioSource>().volume = eventVolume;
        clipPrefab.GetComponent<AudioSource>().spatialBlend = 1f;
        clipPrefab.SetActive(false);
    }
    
    protected virtual void Start()
    {
        canActivate = false;
        isActive = false;
        gameObject.layer = LayerMask.NameToLayer("Default");
        outline.enabled = false;
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
        else
        {
            if (audioTimer < eventClip.length + timeInBetweenAudio)
            {
                audioTimer += Time.deltaTime;
            }
            else
            {
                PlayAudio();
                audioTimer = 0f;
            }
        }
    }

    protected void PlayAudio()
    {
        clipPrefab.SetActive(true);
        clipPrefab.GetComponent<AudioSource>().Play();
    }

    protected void StopAudio()
    {
        clipPrefab.GetComponent<AudioSource>().Stop();
        clipPrefab.SetActive(false);
    }

    public virtual void Activate()
    {
        canActivate = true;
        isActive = true;
        gameObject.layer = LayerMask.NameToLayer("Eventable");
        audioTimer = eventClip.length + timeInBetweenAudio;

        exclamationPoint.SetActive(true);
        
        OnUpdateActiveEvents?.Invoke(1);
    }

    public virtual void Solution()
    {
        isActive = false;
        gameObject.layer = LayerMask.NameToLayer("Default");
        
        exclamationPoint.SetActive(false);
        StopAudio();

        AudioManager.Instance.PlaySFX2D(solutionClip, solutionVolume);
        
        OnUpdateActiveEvents?.Invoke(-1);
        OnEventSolution?.Invoke(solSatisfactionChange, solSanityChange);
    }

    public virtual void Knockout()
    {
        canActivate = false;
        isActive = false;
        gameObject.layer = LayerMask.NameToLayer("Default");
        
        exclamationPoint.SetActive(false);
        StopAudio();
        
        AudioManager.Instance.PlaySFX2D(koClip, koVolume);
        
        OnUpdateActiveEvents?.Invoke(-1);
        OnEventKnockout?.Invoke(koSatisfactionChange, koSanityChange);
    }
}
