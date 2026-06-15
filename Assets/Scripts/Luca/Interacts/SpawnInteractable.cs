using System.Collections.Generic;
using UnityEngine;

public class SpawnInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private ObjectGrabbing interactable;
    [SerializeField] private Transform interactableParent;
    [SerializeField] private int poolSize = 3;
    [SerializeField] private float spawnOffset = 0.25f;

    private InteractablePooler<ObjectGrabbing> interactablePooler;
    
    private void Awake()
    {
        interactablePooler = new InteractablePooler<ObjectGrabbing>(interactable, interactableParent, poolSize);
    }

    public void Interact()
    {
        Vector3 offsetPos = new Vector3(transform.position.x, transform.position.y + spawnOffset, transform.position.z);
        interactablePooler.GetFromPool(offsetPos, Quaternion.identity);
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
