using UnityEngine;

public class Object : MonoBehaviour, IInteractable
{
    private Rigidbody rb;
    
    private bool isInteracting;
    private float distanceOnInteract;
    private Camera cam;

    [SerializeField] private float force = 50f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float damping = 5f;

    private void Awake()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (isInteracting)
        {
            Vector3 targetPosition = cam.transform.position + cam.transform.forward * distanceOnInteract;
            
            Vector3 direction = targetPosition - rb.transform.position;

            Vector3 velocity = direction * force;

            if (velocity.magnitude > maxSpeed)
            {
                velocity = velocity.normalized * maxSpeed;
            }
            
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, velocity, Time.fixedDeltaTime * damping);
            
            //rb.MovePosition(Vector3.MoveTowards(rb.transform.position, targetPosition, 100f * Time.fixedDeltaTime));
        }
    }
    
    public void Interact()
    {
        isInteracting = !isInteracting;
        
        if (isInteracting)
        {
            rb.useGravity = false;
            distanceOnInteract = Vector3.Distance(transform.position, Camera.main.transform.position);
        }
        else
        {
            rb.useGravity = true;
        }
    }
}
