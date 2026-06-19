using System.Collections;
using UnityEngine;

public class LuggageEvent : MonoBehaviour, ILuggage
{
    private GameObject luggage;
    private Vector3 origin;
    public float overlapRadius = 0.25f;
    [SerializeField] private LayerMask interactableMask;
    public Vector3 moveToAmount;
    public bool onOppositeSide;

    [SerializeField] private float minTimeBeforeReactivation = 20f;
    [SerializeField] private float maxTimeBeforeReactivation = 60f;
    private float randomTime;
    private float timer;

    private bool isActive;
    
    private void Start()
    {
        luggage = transform.GetChild(0).gameObject;
        origin = luggage.transform.position;
    }

    private void Update()
    {
        if (!isActive)
            return;
        
        Collider[] colliders = Physics.OverlapSphere(origin, overlapRadius, interactableMask);
    
        if (colliders.Length <= 0)
            return;
    
        foreach (Collider col in colliders)
        {
            if (col.TryGetComponent<ObjectGrabbing>(out ObjectGrabbing grabbing))
            {
                if (grabbing.objectType == ObjectType.Luggage)
                {
                    grabbing.isInteracting = false;
                    isActive = false;
                    luggage = grabbing.gameObject;
                    FixLuggage();
                }
            }
        }
    }

    public void TriggerEvent()
    {
        Vector3 targetPos = luggage.transform.position + moveToAmount;

        StartCoroutine(MoveLuggage(targetPos, 1f));
    }
    
    public void FixLuggage()
    {
        Vector3 targetPos = origin;

        StartCoroutine(MoveLuggage(targetPos, 1f));
    }

    private IEnumerator MoveLuggage(Vector3 targetPos, float time)
    {
        Vector3 startPos = luggage.transform.position;
        
        //Debug.Log("Start Position: " + startPos + "\n Target Position: " + targetPos);
        
        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            luggage.transform.position = Vector3.Lerp(startPos, targetPos, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        if (!isActive)
            isActive = true;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(origin, overlapRadius);
    }
}
