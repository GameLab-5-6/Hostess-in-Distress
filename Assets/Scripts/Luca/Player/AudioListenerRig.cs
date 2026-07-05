using UnityEngine;

public class AudioListenerRig : MonoBehaviour
{
    private static AudioListenerRig instance;
    private Camera cam;
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void LateUpdate()
    {
        if (cam == null)
        {
            cam = Camera.main;
            if (cam == null)
                return;
        }

        transform.position = cam.transform.position;
        transform.rotation = cam.transform.rotation;
    }
}
