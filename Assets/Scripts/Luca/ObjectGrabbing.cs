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

    private void Awake()
    {
        cam = Camera.main.transform;
        rb = GetComponent<Rigidbody>();
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

        rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, Time.fixedDeltaTime * rotationSpeed));
    }

    public void Interact()
    {
        isInteracting = !isInteracting;
        
        if (isInteracting)
        {
            rb.useGravity = false;
            rb.freezeRotation = true;
            distanceOnInteract = Vector3.Distance(transform.position, Camera.main.transform.position);
        }
        else
        {
            rb.useGravity = true;
            rb.freezeRotation = false;
        }
    }
}
