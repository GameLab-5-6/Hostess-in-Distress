using UnityEngine;

public class BabyEvent : EventParent
{
    [SerializeField] private Transform overlapPosition;
    [SerializeField] private float overlapArea;
    [SerializeField] private LayerMask interactMask;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        
        if (isActive)
        {
            Collider[] readInteractable = Physics.OverlapSphere(overlapPosition.position, overlapArea, interactMask);

            if (readInteractable.Length <= 0)
                return;

            foreach (Collider col in readInteractable)
            {
                if (col.TryGetComponent(out ObjectGrabbing obj))
                {
                    if (obj.objectType == ObjectType.Toy)
                    {
                        SolutionWithObject();
                    }
                }
            }
        }
    }

    public override void Activate()
    {
        base.Activate();
    }

    public override void Solution()
    {
        
    }

    private void SolutionWithObject()
    {
        base.Solution();
    }

    public override void Knockout()
    {
        base.Knockout();
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(overlapPosition.position, overlapArea);
    }
    
    // private Outline outline;
    //
    // [SerializeField] private float timeBeforeReactivation;
    // private float timer;
    // [SerializeField] private Transform overlapPosition;
    // [SerializeField] private float overlapArea;
    // [SerializeField] private LayerMask interactMask;
    //
    // [Header("Solution")]
    // [SerializeField] private float solSatisfactionGained;
    // [SerializeField] private float solSanityChange;
    // [Header("Knockout")]
    // [SerializeField] private float koSatisfactionChange;
    // [SerializeField] private float koSanityChange;
    //
    // private bool canActivate, isActive;
    //
    // public static event Action<int> OnUpdateActiveEvents;
    // public static event Action<float, float> OnEventSolution, OnEventKnockout;
    //
    // private void Awake()
    // {
    //     outline = GetComponent<Outline>();
    // }
    //
    // private void Start()
    // {
    //     canActivate = false;
    //     isActive = false;
    //     outline.enabled = false;
    //     gameObject.layer = LayerMask.NameToLayer("Default");
    // }
    //
    // private void Update()
    // {
    //     if (!canActivate)
    //         return;
    //
    //     if (!isActive)
    //     {
    //         timer += Time.deltaTime;
    //
    //         if (timer >= timeBeforeReactivation)
    //         {
    //             Activate();
    //             timer = 0f;
    //         }
    //     }
    //
    //     if (isActive)
    //     {
    //         Collider[] readInteractable = Physics.OverlapSphere(overlapPosition.position, overlapArea, interactMask);
    //
    //         if (readInteractable.Length <= 0)
    //             return;
    //
    //         foreach (Collider col in readInteractable)
    //         {
    //             if (col.TryGetComponent(out ObjectGrabbing obj))
    //             {
    //                 if (obj.objectType == ObjectType.Toy)
    //                 {
    //                     SolutionWithObject();
    //                 }
    //             }
    //         }
    //     }
    // }
    //
    // public void Activate()
    // {
    //     canActivate = true;
    //     isActive = true;
    //     outline.enabled = true;
    //     gameObject.layer = LayerMask.NameToLayer("Eventable");
    //     
    //     OnUpdateActiveEvents?.Invoke(1);
    // }
    //
    // public void Solution()
    // {
    //     //since an object reading is needed this solution will do nothing, however it is possible to give UI directions with it
    // }
    //
    // private void SolutionWithObject()
    // {
    //     isActive = false;
    //     outline.enabled = false;
    //     gameObject.layer = LayerMask.NameToLayer("Default");
    //     
    //     OnUpdateActiveEvents?.Invoke(-1);
    //     OnEventSolution?.Invoke(solSatisfactionGained, solSanityChange);
    // }
    //
    // public void Knockout()
    // {
    //     canActivate = false;
    //     isActive = false;
    //     outline.enabled = false;
    //     gameObject.layer = LayerMask.NameToLayer("Default");
    //     
    //     OnUpdateActiveEvents?.Invoke(-1);
    //     OnEventKnockout?.Invoke(koSatisfactionChange, koSanityChange);
    // }
    //
    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.blue;
    //     Gizmos.DrawWireSphere(overlapPosition.position, overlapArea);
    // }
}
