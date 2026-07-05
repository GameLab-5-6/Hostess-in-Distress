using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    
    [SerializeField] private Transform cam;
    [SerializeField] private float speed = 5f;
    [SerializeField] bool isWalking;

    [SerializeField] private GameObject audioPrefab;
    [SerializeField] private AudioClip walkingClip;
    [Range(0f, 1f)] [SerializeField] private float walkingVolume;
    private float audioTimer;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioPrefab = Instantiate(audioPrefab, transform.position, Quaternion.identity, transform);
        audioPrefab.GetComponent<AudioSource>().clip = walkingClip;
        audioPrefab.GetComponent<AudioSource>().volume = walkingVolume;
        audioPrefab.GetComponent<AudioSource>().spatialBlend = 0f;
    }

    private void Start()
    {
        audioTimer = walkingClip.length;
    }

    private void Update()
    {
        if (isWalking)
        {
            if (audioTimer >= walkingClip.length)
            {
                audioPrefab.GetComponent<AudioSource>().Play();
                audioTimer = 0f;
            }
            else
            {
                audioTimer += Time.deltaTime;
                audioPrefab.GetComponent<AudioSource>().UnPause();
                    
            }
        }
        else
        {
            audioPrefab.GetComponent<AudioSource>().Pause();
        }
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

            isWalking = true;
        }
        else
        {
            //removes sliding when moving
            rb.linearVelocity = new Vector3(0f, rb.linearVelocity.y, 0f);

            isWalking = false;
        }
    }
}
