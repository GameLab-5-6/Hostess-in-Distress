using UnityEngine;

public class ObjectGrabbing : MonoBehaviour, IInteractable
{
    private Rigidbody rb;
    private Transform cam;
    
    private bool isInteracting;
    
    private float distanceOnInteract;
    [SerializeField] private float force = 50f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float damping = 5f;
    [SerializeField] private float rotationSpeed = 20f;
    [SerializeField] private float distanceChangeAmount = 0.5f;
    [SerializeField] private float maxDistance = 3f;
    [SerializeField] private float minDistance = 1.5f;

    [Header("Immovable Grabbing")] 
    [SerializeField] private bool isImmovable;

    private void Awake()
    {
        cam = Camera.main.transform;
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        if (isImmovable)
        {
            rb.constraints = RigidbodyConstraints.FreezePositionX;
            rb.constraints = RigidbodyConstraints.FreezePositionZ;
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
        if (!isImmovable)
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

        if (isImmovable)
        {
            targetRot = new Quaternion(0f, targetRot.y, 0f, targetRot.w);
        }
        
        targetRot.Normalize();

        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, Time.fixedDeltaTime * rotationSpeed));
    }

    public void Interact()
    {
        isInteracting = !isInteracting;
        
        if (isInteracting)
        {
            if (isImmovable)
            {
                rb.constraints = RigidbodyConstraints.None;
                rb.constraints = RigidbodyConstraints.FreezePositionY;
            }
            
            rb.useGravity = false;
            rb.freezeRotation = true;
            distanceOnInteract = Vector3.Distance(transform.position, Camera.main.transform.position);
        }
        else
        {
            if (isImmovable)
            {
                rb.constraints = RigidbodyConstraints.FreezePositionX;
                rb.constraints = RigidbodyConstraints.FreezePositionZ;
            }
            
            rb.useGravity = true;
            rb.freezeRotation = false;
        }
    }
}
