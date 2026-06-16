using System.Collections.Generic;
using UnityEngine;

public class SpawnInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private ObjectGrabbing interactable;
    [SerializeField] private Transform interactableParent;
    [SerializeField] private int poolSize = 3;
    [SerializeField] private float spawnOffset = 0.25f;

    [Header("Drinks")]
    [SerializeField] private bool isDrink;
    [SerializeField] private float drinkDistance;
    [SerializeField] private LayerMask eventMask;
    private bool allowInteract;

    private InteractablePooler<ObjectGrabbing> interactablePooler;
    
    private void Awake()
    {
        interactablePooler = new InteractablePooler<ObjectGrabbing>(interactable, interactableParent, poolSize);
    }

    public void Interact()
    {
        if (isDrink)
        {
            allowInteract = false;
            
            Collider[] colliders = Physics.OverlapSphere(transform.position, drinkDistance, eventMask);
            
            foreach (Collider col in colliders)
            {
                if (col.TryGetComponent<DrinkEvent>(out DrinkEvent drinkEvent))
                {
                    if (drinkEvent.isActive)
                        allowInteract = true;
                }
            }
        }

        if (!allowInteract && isDrink)
            return;
        
        Vector3 offsetPos = new Vector3(transform.position.x, transform.position.y + spawnOffset, transform.position.z);
        interactablePooler.GetFromPool(offsetPos, Quaternion.identity);
    }
    
    private void OnDrawGizmos()
    {
        if (!isDrink)
            return;
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, drinkDistance);
    }
}

public class InteractablePooler<T> where T : ObjectGrabbing
{
    private T prefab;
    private Transform parent;
    private int poolSize;
    private List<T> interactables = new List<T>();

    public InteractablePooler(T prefab, Transform parent, int poolSize)
    {
        this.prefab = prefab;
        this.parent = parent;
        this.poolSize = poolSize;
        InitializePool();
    }

    private void InitializePool()
    {
        interactables = new List<T>();
        for (int i = 0; i < poolSize; i++)
        {
            T interactable = GameObject.Instantiate(prefab, parent);
            interactable.gameObject.SetActive(false);
            interactables.Add(interactable);
        }
    }

    public T GetFromPool(Vector3 position, Quaternion rotation)
    {
        foreach (var interactable in interactables)
        {
            if (!interactable.gameObject.activeInHierarchy)
            {
                interactable.transform.position = position;
                interactable.transform.rotation = rotation;
                interactable.gameObject.SetActive(true);
                return interactable;
            }
        }

        T newInteractable = Object.Instantiate(prefab, position, rotation, parent);
        interactables.Add(newInteractable);
        return newInteractable;
    }
}
