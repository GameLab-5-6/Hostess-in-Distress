using UnityEngine;

public class ObjectGrabbing : MonoBehaviour, IInteractable
{
    [HideInInspector] public Rigidbody rb;
    private Transform cam;
    
    [SerializeField] private bool isInteracting;
    
    private float distanceOnInteract;
    [SerializeField] private float force = 50f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float damping = 5f;
    [SerializeField] private float rotationSpeed = 20f;
    [SerializeField] private float distanceChangeAmount = 0.5f;
    [SerializeField] private float maxDistance = 3f;
    [SerializeField] private float minDistance = 1.5f;
    
    public ObjectType objectType;

    [Header("Phone Grab")] 
    [SerializeField] private int interactTimesToGrab;
    private int interactedTimes;
    [SerializeField] private float decayRate;
    private float decayTimer;
    [HideInInspector] public bool isInEvent = false;

    private void Awake()
    {
        cam = Camera.main.transform;
        rb = GetComponent<Rigidbody>();
    }
    
    private void Start()
    {
        if (objectType == ObjectType.Cart)
        {
            rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        }
        else if (objectType == ObjectType.Phone)
        {
            rb.useGravity = false;
        }
    }

    private void Update()
    {
        if (objectType != ObjectType.Phone)
            return;
        
        decayTimer += Time.deltaTime;

        if (decayTimer >= decayRate)
        {
            decayTimer = 0f;
            if (interactedTimes <= 0)
                return;
            interactedTimes--;
        }
    }

    private void FixedUpdate()
    {
        if (isInteracting)
        {
            // TELEKINESIS
            // Vector3 targetPosition = cam.transform.position + cam.transform.forward * distanceOnInteract;
            //
            // Vector3 direction = targetPosition - rb.transform.position;
            //
            // Vector3 velocity = direction * force;
            //
            // if (velocity.magnitude > maxSpeed)
            // {
            //     velocity = velocity.normalized * maxSpeed;
            // }
            //
            // rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, velocity, Time.fixedDeltaTime * damping);

            HandlePosition();
            HandleRotation();
        }
    }

    private void HandlePosition()
    {
        if (objectType != ObjectType.Cart)
        {
            InputManager.GetGrabDistance(out float distance);
            distanceOnInteract += distance * distanceChangeAmount;
            distanceOnInteract = Mathf.Clamp(distanceOnInteract, minDistance, maxDistance);
        }
        else
        {
            distanceOnInteract = minDistance;
        }

        Vector3 idealPoint = cam.position + cam.forward * distanceOnInteract;
        
        Vector3 direction = idealPoint - rb.position;
        Vector3 velocity = direction * force;
        if (velocity.magnitude > maxSpeed) 
            velocity = velocity.normalized * maxSpeed;
        
        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, velocity, Time.fixedDeltaTime * damping);
    }

    void HandleRotation()
    {
        Quaternion targetRot = Quaternion.LookRotation(cam.forward, Vector3.up);

        if (objectType == ObjectType.Cart)
        {
            targetRot = new Quaternion(0f, targetRot.y, 0f, targetRot.w);
        }
        
        targetRot.Normalize();

        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, Time.fixedDeltaTime * rotationSpeed));
    }

    public void Interact()
    {
        if (objectType != ObjectType.Phone || !isInEvent)
        {
            isInteracting = !isInteracting;
            
            if (isInteracting)
            {
                if (objectType == ObjectType.Cart)
                {
                    rb.constraints = RigidbodyConstraints.FreezePositionY;
                }
            
                rb.useGravity = false;
                rb.freezeRotation = true;
                distanceOnInteract = Vector3.Distance(transform.position, Camera.main.transform.position);
            }
            else
            {
                if (objectType == ObjectType.Cart)
                {
                    rb.constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
                }
            
                rb.useGravity = true;
                rb.freezeRotation = false;
            }
        }
        else
        {
            interactedTimes++;

            if (interactedTimes >= interactTimesToGrab)
            {
                isInteracting = true;
                rb.constraints = RigidbodyConstraints.None;
                isInEvent = false;
            }
        }
    }
}
