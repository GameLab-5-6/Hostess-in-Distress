using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    
    [SerializeField] private Transform cam;
    [SerializeField] private float speed = 5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (InputManager.IsMoving(out Vector3 direction))
        {
            //moves the player relative to camera look direction
            Vector3 camDirection = cam.forward * direction.z + cam.right * direction.x;
            //taking out the y-axis so it doesn't fly
            camDirection.y = 0f;
            camDirection.Normalize();
            
            //calculates the velocity of player
            Vector3 velocity = camDirection * (speed * Time.fixedDeltaTime);
            velocity.y = rb.linearVelocity.y;
            rb.linearVelocity = velocity;
        }
        else
        {
            //removes sliding when moving
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);
        }
    }
}
