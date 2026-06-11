using UnityEngine;

public enum ObjectType
{
    Default,
    Toy
}

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private Transform cam;
    [SerializeField] private float maxInteractDistance = 5f;
    [SerializeField] private LayerMask interactMask;
    [SerializeField] private LayerMask eventMask;
    public IInteractable currentInteractable;
    public IEventable currentEventable;
    
    private bool isCharging;
    public float chargeTime = 1f;
    public float chargeAmount;
    
    private void OnEnable()
    {
        InputManager.OnInteraction += HandleInteraction;
        InputManager.OnPunchCharge += StartPunch;
        InputManager.OnPunchRelease += HandlePunch;
    }
    
    private void OnDisable()
    {
        InputManager.OnInteraction -= HandleInteraction;
        InputManager.OnPunchCharge -= StartPunch;
        InputManager.OnPunchRelease -= HandlePunch;
    }

    private void Start()
    {
        isCharging = false;
        chargeAmount = 0f;
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
    
    private void CheckForInteractables()
    {
        Ray ray = new Ray(cam.position, cam.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxInteractDistance, interactMask))
        {
            if (hit.collider.TryGetComponent(out IInteractable interactable))
            {
                currentInteractable = interactable;
                return;
            }
        }
        currentInteractable = null;
    }
    
    private void HandleInteraction()
    {
        currentInteractable?.Interact();
        currentEventable?.Solution();
    }

    private void CheckForEventables()
    {
        Ray ray = new Ray(cam.position, cam.forward);
        {
            if (Physics.Raycast(ray, out RaycastHit hit, maxInteractDistance, eventMask))
            {
                if (hit.collider.TryGetComponent(out IEventable eventable))
                {
                    currentEventable = eventable;
                    return;
                }
            }
        }
        currentEventable = null;
    }

    private void StartPunch() => isCharging = true;

    private void HandlePunch()
    {
        if (chargeAmount >= chargeTime)
        {
            currentEventable?.Knockout();
        }

        chargeAmount = 0f;
        isCharging = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(cam.transform.position, cam.transform.position + cam.transform.forward * maxInteractDistance);
    }
}
