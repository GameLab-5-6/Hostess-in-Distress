using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EventManager : MonoBehaviour
{
    private List<ILuggage> luggageList;
    [Header("Luggage Event")]
    [SerializeField] private float minTimeBeforeReactivation = 20f;
    [SerializeField] private float maxTimeBeforeReactivation = 60f;
    public float luggageOverlapRadius;
    public Vector3 luggageMoveToAmount;
    
    private float randomTime;
    private float luggageTimer;

    private List<IEventable> eventsList;
    [Header("Passenger Events")]
    [SerializeField] private float eventSpawnRate = 10f;
    private float timer;

    [Header("Baby Event")] 
    public float babyTimeBeforeReactivation;
    public float babySolSatisfactionChange;
    public float babySolSanityChange;
    public float babyKoSatisfactionChange;
    public float babyKoSanityChange;
    public float babyOverlapArea;
    
    [Header("Child Event")] 
    public float childTimeBeforeReactivation;
    public float childSolSatisfactionChange;
    public float childSolSanityChange;
    public float childKoSatisfactionChange;
    public float childKoSanityChange;
    
    [Header("Drink Event")]
    public float drinkTimeBeforeReactivation;
    public float drinkSolSatisfactionChange;
    public float drinkSolSanityChange;
    public float drinkKoSatisfactionChange;
    public float drinkKoSanityChange;
    public float drinkFailSatisfactionChange;
    public float drinkFailSanityChange;
    public float drinkOverlapArea;

    [Header("Music Event")] 
    public GameObject musicPhonePrefab;
    public float musicTimeBeforeReactivation;
    public float musicSolSatisfactionChange;
    public float musicSolSanityChange;
    public float musicKoSatisfactionChange;
    public float musicKoSanityChange;
    public float musicOverlapArea;


    //for the IEventables to be placed into the list they all need to be children of this EventManager object
    private void Awake()
    {
        eventsList = new List<IEventable>();
        EventBase[] events = FindObjectsByType<EventBase>(FindObjectsSortMode.None);
        foreach (var e in events)
        {
            if (e.TryGetComponent(out IEventable eventable))
            {
                eventsList.Add(eventable);
            }
        }

        luggageList = new List<ILuggage>();
        LuggageEvent[] luggage = FindObjectsByType<LuggageEvent>(FindObjectsSortMode.None);
        foreach (var l in luggage)
        {
            if (l.TryGetComponent(out ILuggage eventable))
            {
                luggageList.Add(eventable);
            }
        }
    }

    private void Start()
    {
        randomTime = Random.Range(minTimeBeforeReactivation, maxTimeBeforeReactivation);
    }

    private void Update()
    {
        EventSpawning();
        LuggageEventSpawing();
    }

    private void EventSpawning()
    {
        if (eventsList.Count <= 0)
            return;
        
        timer += Time.deltaTime;
        if (timer >= eventSpawnRate)
        {
            int randomEvent = Random.Range(0, eventsList.Count);
            eventsList[randomEvent].Activate();
            eventsList.RemoveAt(randomEvent);

            timer = 0f;
        }
    }

    private void LuggageEventSpawing()
    {
        if (luggageList.Count <= 0)
            return;
        
        luggageTimer += Time.deltaTime;
        if (luggageTimer >= randomTime)
        {
            int randomLuggage = Random.Range(0, luggageList.Count);
            luggageList[randomLuggage].TriggerEvent();
            luggageList.RemoveAt(randomLuggage);
            
            randomTime = Random.Range(minTimeBeforeReactivation, maxTimeBeforeReactivation);
            luggageTimer = 0f;
        }
    }
}
