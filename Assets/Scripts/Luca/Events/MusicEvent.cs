using System;
using UnityEngine;

public class MusicEvent : EventBase
{
    public GameObject phonePrefab;
    private bool hasPhone;
    
    [SerializeField] private Transform overlapPosition;
    public float overlapArea;
    [SerializeField] private LayerMask interactMask;
    
    public string interactText;

    public static event Action<string> OnInteraction;
    
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();

        phonePrefab = Instantiate(phonePrefab, overlapPosition.position, Quaternion.identity, transform);
        phonePrefab.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();
        
        if (isActive)
        {
            Collider[] readInteractable = Physics.OverlapSphere(overlapPosition.position, overlapArea, interactMask);

            hasPhone = false;
            
            foreach (Collider interactable in readInteractable)
            {
                if (interactable.TryGetComponent(out ObjectGrabbing obj))
                {
                    if (obj.objectType == ObjectType.Phone)
                    {
                        hasPhone = true;

                        phonePrefab.TryGetComponent<IInteractable>(out IInteractable interact);
                        if (PlayerInteract.Instance.currentInteractable != interact)
                        {
                            obj.outline.OutlineColor = Color.red;
                            obj.outline.enabled = true;
                        }
                    }
                }
            }

            if (hasPhone)
                return;
            
            SolutionWithObject();
        }
    }
    
    public override void Activate()
    {
        phonePrefab.transform.position = overlapPosition.position;
        phonePrefab.transform.rotation = Quaternion.identity;
        phonePrefab.SetActive(true);
        if (phonePrefab.TryGetComponent<ObjectGrabbing>(out ObjectGrabbing obj))
        {
            obj.isInEvent = true;
            obj.rb.constraints = RigidbodyConstraints.FreezePosition;
        }

        if (phonePrefab.TryGetComponent<Animator>(out Animator anim))
        {
            anim.SetBool("isActive", true);
        }
        
        hasPhone = true;
        
        base.Activate();
    }

    public override void Solution()
    {
        OnInteraction?.Invoke(interactText);
    }

    private void SolutionWithObject()
    {
        base.Solution();
    }

    public override void Knockout()
    {
        base.Knockout();
        phonePrefab.SetActive(false);
    }
    
    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.blue;
    //     Gizmos.DrawWireSphere(overlapPosition.position, overlapArea);
    // }
}
