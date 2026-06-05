using UnityEngine;

public class Object : MonoBehaviour, IInteractable
{
    private Rigidbody rb;
    
    private bool isInteracting;
    private float distanceOnInteract;
    private Camera cam;

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
            
            rb.MovePosition(Vector3.MoveTowards(rb.transform.position, targetPosition, 100f * Time.fixedDeltaTime));
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
