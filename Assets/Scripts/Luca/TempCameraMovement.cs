using Unity.Cinemachine;
using UnityEngine;

public class TempCameraMovement : MonoBehaviour
{
    //[SerializeField] private Camera cam;
    [SerializeField] private CinemachineSplineDolly dolly;
    [SerializeField] private float speed;
    private float position;

    private void Update()
    {
        position += speed * Time.deltaTime;
        dolly.CameraPosition = position;
    }
}
