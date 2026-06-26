using UnityEngine;

public class ImageLookAtCamera : MonoBehaviour
{
    private void Update()
    {
        transform.LookAt(Camera.main.transform);
    }
}
