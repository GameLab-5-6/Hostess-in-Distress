using System;
using System.Collections;
using UnityEngine;

public class LuggageEvent : MonoBehaviour, ILuggage
{
    private GameObject luggage;
    private Vector3 origin;
    public float overlapRadius = 0.25f;
    [SerializeField] private GameObject hatboxPivot;
    [SerializeField] private LayerMask interactableMask;
    public Vector3 moveToAmount;
    public bool onOppositeSide;

    public AudioClip luggageClip;
    public float luggageVolume;
    public AudioClip solutionClip;
    public float solutionVolume;

    private bool isActive;

    public static event Action<int> OnUpdateLuggageEvents;
    
    private void Start()
    {
        luggage = transform.GetChild(0).gameObject;
        origin = luggage.transform.position;
        luggage.layer = LayerMask.NameToLayer("Default");
    }

    private void Update()
    {
        if (!isActive)
            return;
        
        Collider[] colliders = Physics.OverlapSphere(origin, overlapRadius, interactableMask);
    
        if (colliders.Length <= 0)
            return;
    
        foreach (Collider col in colliders)
        {
            if (col.TryGetComponent<ObjectGrabbing>(out ObjectGrabbing grabbing))
            {
                if (grabbing.objectType == ObjectType.Luggage)
                {
                    grabbing.isInteracting = false;
                    isActive = false;
                    luggage = grabbing.gameObject;
                    luggage.layer = LayerMask.NameToLayer("Default");
                    grabbing.outline.enabled = false;
                    
                    AudioManager.Instance.PlaySFX2D(solutionClip, solutionVolume);
                    
                    FixLuggage();
                }
            }
        }
    }

    public void TriggerEvent()
    {
        Vector3 targetPos = luggage.transform.position + moveToAmount;
        luggage.layer = LayerMask.NameToLayer("Interactable");

        StartCoroutine(MoveLuggageStart(targetPos, 1f));
        
        OnUpdateLuggageEvents?.Invoke(1);
    }
    
    public void FixLuggage()
    {
        Vector3 targetPos = origin;

        StartCoroutine(MoveLuggageSolved(targetPos, 1f));
        
        OnUpdateLuggageEvents?.Invoke(-1);
    }

    private IEnumerator MoveLuggageStart(Vector3 targetPos, float time)
    {
        Vector3 startPos = luggage.transform.position;
        
        //Debug.Log("Start Position: " + startPos + "\n Target Position: " + targetPos);
        
        float elapsedTime = 0f;

        Quaternion hatboxRotation;

        if (onOppositeSide)
            hatboxRotation = Quaternion.Euler(0f, 0f, -90f);
        else
            hatboxRotation = Quaternion.Euler(0f, 0f, 90f);

        while (elapsedTime < time)
        {
            hatboxPivot.transform.rotation = Quaternion.Lerp(Quaternion.identity, hatboxRotation, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        elapsedTime = 0f;

        while (elapsedTime < time)
        {
            luggage.transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        AudioManager.Instance.PlaySFX3D(luggageClip, luggageVolume, luggage.transform.position);
        
        if (!isActive)
            isActive = true;
    }
    
    private IEnumerator MoveLuggageSolved(Vector3 targetPos, float time)
    {
        Vector3 startPos = luggage.transform.position;
        
        //Debug.Log("Start Position: " + startPos + "\n Target Position: " + targetPos);
        
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            luggage.transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / time);
            luggage.transform.rotation = Quaternion.Lerp(luggage.transform.rotation, Quaternion.identity, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        elapsedTime = 0f;

        while (elapsedTime < time)
        {
            hatboxPivot.transform.rotation = Quaternion.Lerp(hatboxPivot.transform.rotation, Quaternion.identity, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        if (!isActive)
            isActive = true;
    }
    
    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.blue;
    //     Gizmos.DrawWireSphere(origin, overlapRadius);
    // }
}
