using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

[RequireComponent(typeof(Outline))]
public abstract class EventBase : MonoBehaviour, IEventable
{
    private Outline outline;
    
    public float timeBeforeReactivation;
    protected float timer;
    public GameObject exclamationPoint;
    [SerializeField] protected Transform exclamationSpawnPosition;
    public bool followPlayerOnEvent;
    public GameObject passengerHead;
    public Transform targetKoTransform;
    
    [Header("Solution")]
    public float solSatisfactionChange;
    public float solSanityChange;
    public float lerpTimeSol = 1f;
    [Header("Knockout")]
    public float koSatisfactionChange;
    public float koSanityChange;
    public float lerpTimeKnockout = 0.25f;

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

            if (!followPlayerOnEvent)
            {
                Vector3 targetLook = Camera.main.transform.position - passengerHead.transform.position;

                float angle = Vector3.Angle(Vector3.forward, targetLook);
                float clamp = Mathf.Clamp(angle, -90f, 90f);

                Vector3 clampedTarget =
                    Vector3.RotateTowards(Vector3.forward, targetLook, clamp * Mathf.Deg2Rad, Mathf.Infinity);

                passengerHead.transform.rotation = Quaternion.LookRotation(clampedTarget, Vector3.up);
            }
        }
        else
        {
            if (passengerHead != null && followPlayerOnEvent)
            {
                Vector3 targetLook = Camera.main.transform.position - passengerHead.transform.position;

                float angle = Vector3.Angle(Vector3.forward, targetLook);
                float clamp = Mathf.Clamp(angle, -90f, 90f);

                Vector3 clampedTarget =
                    Vector3.RotateTowards(Vector3.forward, targetLook, clamp * Mathf.Deg2Rad, Mathf.Infinity);

                passengerHead.transform.rotation = Quaternion.LookRotation(clampedTarget, Vector3.up);
            }
            
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

        if (exclamationPoint != null)
            exclamationPoint.SetActive(true);
        
        OnUpdateActiveEvents?.Invoke(1);
    }

    public virtual void Solution()
    {
        isActive = false;
        gameObject.layer = LayerMask.NameToLayer("Default");
        outline.enabled = false;
        
        if (exclamationPoint != null)
            exclamationPoint.SetActive(false);
        
        StopAudio();

        AudioManager.Instance.PlaySFX2D(solutionClip, solutionVolume);
        
        OnUpdateActiveEvents?.Invoke(-1);
        OnEventSolution?.Invoke(solSatisfactionChange, solSanityChange);
        
        StartCoroutine(LerpHead(lerpTimeSol, Quaternion.identity, passengerHead.transform.position));
    }

    public virtual void Knockout()
    {
        canActivate = false;
        isActive = false;
        gameObject.layer = LayerMask.NameToLayer("Default");
        outline.enabled = false;
        
        if (exclamationPoint != null)
            exclamationPoint.SetActive(false);
        
        StopAudio();
        
        AudioManager.Instance.PlaySFX2D(koClip, koVolume);
        
        OnUpdateActiveEvents?.Invoke(-1);
        OnEventKnockout?.Invoke(koSatisfactionChange, koSanityChange);
        
        StartCoroutine(LerpHead(lerpTimeKnockout, targetKoTransform.rotation, targetKoTransform.position));
    }

    private IEnumerator LerpHead(float time, Quaternion targetRot, Vector3 targetPos)
    {
        Quaternion startRot = passengerHead.transform.rotation;
        Vector3 startPos = passengerHead.transform.position;
        
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            passengerHead.transform.rotation = Quaternion.Slerp(startRot, targetRot, elapsedTime / time);
            passengerHead.transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        passengerHead.transform.rotation = targetRot;
        passengerHead.transform.position = targetPos;
    }
}
