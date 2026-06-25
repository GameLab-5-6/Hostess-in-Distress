using UnityEngine;

public class ExclamationPointMovement : MonoBehaviour
{
    private void Update()
    {
        transform.LookAt(Camera.main.transform);
    }
}
