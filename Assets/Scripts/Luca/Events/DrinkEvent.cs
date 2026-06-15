using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class DrinkEvent : EventBase
{
    [Header("Fail")] 
    public float failSatisfactionChange;
    public float failSanityChange;
    
    [SerializeField] private Transform overlapPosition;
    public float overlapArea;
    [SerializeField] private LayerMask interactMask;

    private int randomDrink;
    private ObjectType drinkType;
    
    public static event Action<int> OnUpdateActiveEvents;
    public static event Action<float, float> OnEventFail;
    
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
                    if (obj.objectType == drinkType)
                    {
                        SolutionWithObject();
                        col.gameObject.SetActive(false);
                    }
                    else
                    {
                        FailEvent();
                        col.gameObject.SetActive(false);
                    }
                }
            }
        }
    }

    public override void Activate()
    {
        base.Activate();

        randomDrink = Random.Range(0, 2);
        switch (randomDrink)
        {
            case 0:
                drinkType = ObjectType.Drink1;
                outline.OutlineColor = Color.red;
                break;
            
            case 1:
                drinkType = ObjectType.Drink2;
                outline.OutlineColor = Color.blue;
                break;
        }
    }

    private void FailEvent()
    {
        OnUpdateActiveEvents?.Invoke(-1);
        OnEventFail?.Invoke(failSatisfactionChange, failSanityChange);
    }

    private void SolutionWithObject()
    {
        base.Solution();
    }

    public override void Solution()
    {
        
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
}
