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
    [SerializeField] private Transform bubblePosition;
    public GameObject drink1Bubble;
    public GameObject drink2Bubble;

    private int randomDrink;
    private ObjectType drinkType;

    public string interactText;
    
    public static event Action<int> OnUpdateActiveEvents;
    public static event Action<float, float> OnEventFail;
    public static event Action<string> OnInteraction;
    
    protected override void Awake()
    {
        base.Awake();
        drink1Bubble = Instantiate(drink1Bubble, bubblePosition.position, Quaternion.identity, transform);
        drink2Bubble = Instantiate(drink2Bubble, bubblePosition.position, Quaternion.identity, transform);
    }

    protected override void Start()
    {
        base.Start();
        drink1Bubble.SetActive(false);
        drink2Bubble.SetActive(false);
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
                drink1Bubble.SetActive(true);
                break;
            
            case 1:
                drinkType = ObjectType.Drink2;
                drink2Bubble.SetActive(true);
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
        drink1Bubble.SetActive(false);
        drink2Bubble.SetActive(false);
    }

    public override void Solution()
    {
        OnInteraction?.Invoke(interactText);
    }

    public override void Knockout()
    {
        base.Knockout();
        drink1Bubble.SetActive(false);
        drink2Bubble.SetActive(false);
    }
    
    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.blue;
    //     Gizmos.DrawWireSphere(overlapPosition.position, overlapArea);
    // }
}
