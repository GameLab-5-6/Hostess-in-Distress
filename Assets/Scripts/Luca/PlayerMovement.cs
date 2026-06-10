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
            Vector3 camDirection = cam.forward * direction.z + cam.right * direction.x;
            Vector3 movement = camDirection.normalized * (speed * Time.fixedDeltaTime);
            rb.MovePosition(rb.position + movement);
        }
    }
}
