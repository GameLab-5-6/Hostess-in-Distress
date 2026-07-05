using UnityEngine;

public class PlayerSpawnPoint : MonoBehaviour
{
    [SerializeField] private GameObject player;
    
    private void Start()
    {
        player.transform.position = transform.position;
    }
}
