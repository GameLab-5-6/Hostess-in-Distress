using System;
using System.Collections;
using UnityEngine;

public enum ObjectType
{
    Default,
    Toy,
    Drink1,
    Drink2,
    Phone,
    Cart,
    Luggage
}

public class PlayerInteract : MonoBehaviour
{
    public static PlayerInteract Instance;
    
    [SerializeField] private Transform cam;
    [SerializeField] private float maxInteractDistance = 3f;
    [SerializeField] private LayerMask interactMask;
    [SerializeField] private LayerMask eventMask;
    public IInteractable currentInteractable;
    private GameObject lastInteractable;
    public IEventable currentEventable;
    private GameObject lastEventable;

    [SerializeField] private GameObject hammer;
    private bool isCharging;
    public float chargeTime = 1f;
    public float chargeAmount;

    private bool hasInteract;
    public static event Action<string> OnHoverInteract;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    
    private void OnEnable()
    {
        InputManager.OnInteraction += HandleInteraction;
        InputManager.OnPunchCharge += StartPunch;
        InputManager.OnPunchRelease += HandlePunch;

        ObjectGrabbing.OnInteract += ChangeInteractStatus;
    }
    
    private void OnDisable()
    {
        InputManager.OnInteraction -= HandleInteraction;
        InputManager.OnPunchCharge -= StartPunch;
        InputManager.OnPunchRelease -= HandlePunch;
        
        ObjectGrabbing.OnInteract -= ChangeInteractStatus;
    }

    private void Start()
    {
        isCharging = false;
        chargeAmount = 0f;
        hammer.SetActive(false);

        hasInteract = false;
    }
    
    private void Update()
    {
        CheckForInteractables();
        CheckForEventables();

        if (isCharging)
        {
            chargeAmount += Time.deltaTime;
            chargeAmount = Mathf.Clamp(chargeAmount, 0f, chargeTime);
        }
    }

    private void ChangeInteractStatus(bool isInteracting) => hasInteract = isInteracting;
    
    private void CheckForInteractables()
    {
        Ray ray = new Ray(cam.position, cam.forward);
        RaycastHit[] hits = Physics.RaycastAll(ray, maxInteractDistance, interactMask);

        if (hits.Length <= 0)
        {
            currentInteractable = null;
            
            if (lastInteractable == null)
                return;
            if (lastInteractable.TryGetComponent(out Outline outlineFalse))
            {
                outlineFalse.enabled = false;
                lastInteractable = null;
                OnHoverInteract?.Invoke("");
            }

            return;
        }
        
        if (hits[0].collider.TryGetComponent(out IInteractable interactable))
        {
            currentInteractable = interactable;
            
            if (hits[0].collider.gameObject != lastInteractable && lastInteractable != null)
            {
                if (lastInteractable.TryGetComponent(out Outline lastOutline))
                    lastOutline.enabled = false;
            }

            if (hits[0].collider.TryGetComponent(out SpawnInteractable spawn))
            {
                OnHoverInteract?.Invoke("E = spawn object");
            }

            if (hits[0].collider.TryGetComponent(out ObjectGrabbing objectGrabbing))
            {
                if (objectGrabbing.isInteracting)
                {
                    OnHoverInteract?.Invoke("MouseScroll = change grab distance | Q = min distance | R = max distance | E = Let go of object");
                }
                else
                {
                    OnHoverInteract?.Invoke("E = interact");
                }
            }
            
            //Outlines
            if (hits[0].collider.TryGetComponent(out Outline outlineTrue))
            {
                lastInteractable = hits[0].collider.gameObject;
                outlineTrue.OutlineColor = Color.white;
                outlineTrue.enabled = true;
            }
        }
    }
    
    private void CheckForEventables()
    {
        if (hasInteract)
            return;
        
        Ray ray = new Ray(cam.position, cam.forward);
        RaycastHit[] hits = Physics.RaycastAll(ray, maxInteractDistance, eventMask);

        if (hits.Length <= 0)
        {
            currentEventable = null;
            
            if (lastEventable == null)
                return;
            if (lastEventable.TryGetComponent(out Outline outlineFalse))
            {
                outlineFalse.enabled = false;
                lastEventable = null;
                if (currentInteractable == null)
                    OnHoverInteract?.Invoke("");
            }
            
            return;
        }
        
        if (hits[0].collider.TryGetComponent(out IEventable eventable))
        {
            currentEventable = eventable;
            
            //Outlines
            if (hits[0].collider.TryGetComponent(out EventBase eventBase))
            {
                if (hits[0].collider.TryGetComponent(out Outline outlineTrue) && eventBase.isActive)
                {
                    lastEventable = hits[0].collider.gameObject;
                    outlineTrue.enabled = true;
                    OnHoverInteract?.Invoke("E = interact | Hold Left Click = knockout");
                }
            }
        }
    }
    
    private void HandleInteraction()
    {
        currentInteractable?.Interact();
        currentEventable?.Solution();
    }
    
    private void StartPunch() => isCharging = true;

    private void HandlePunch()
    {
        if (chargeAmount >= chargeTime)
        {
            currentEventable?.Knockout();
            StartCoroutine(HammerAnimation());
        }

        chargeAmount = 0f;
        isCharging = false;
    }

    private IEnumerator HammerAnimation()
    {
        hammer.SetActive(true);
        
        yield return new WaitForSeconds(0.25f);
        
        hammer.SetActive(false);
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawLine(cam.transform.position, cam.transform.position + cam.transform.forward * maxInteractDistance);
    // }
}
