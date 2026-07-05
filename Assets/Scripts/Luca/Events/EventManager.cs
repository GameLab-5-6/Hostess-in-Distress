using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class EventManager : MonoBehaviour
{
    [Header("Start Game Events")] 
    [SerializeField] private float[] startEventTime;
    private int startEventIndex = 0;
    
    private List<ILuggage> luggageList;
    [Header("Luggage Event")]
    [SerializeField] private float minTimeBeforeReactivation = 20f;
    [SerializeField] private float maxTimeBeforeReactivation = 60f;
    public float luggageOverlapRadius;
    public Vector3 luggageMoveToAmount;
    public AudioClip luggageClip;
    [Range(0f, 1f)] public float luggageVolume;
    
    private float randomTime;
    private float luggageTimer;

    private List<IEventable> eventsList;
    [Header("Passenger Events")]
    [SerializeField] private float eventSpawnRate = 10f;
    private float timer;
    public GameObject clipPrefab;
    public AudioClip solutionClip;
    [Range(0f, 1f)] public float solutionVolume;
    public AudioClip koClip;
    [Range(0f, 1f)] public float koVolume;
    public GameObject exclamationPoint;
    
    [Header("Baby Event")] 
    public float babyTimeBeforeReactivation;
    public float babySolSatisfactionChange;
    public float babySolSanityChange;
    public float babyKoSatisfactionChange;
    public float babyKoSanityChange;
    public float babyOverlapArea;
    public AudioClip babyClip;
    public float babyTimeInBetweenAudio;
    [Range(0f, 1f)] public float babyVolume;
    
    [Header("Child Event")] 
    public float childTimeBeforeReactivation;
    public float childSolSatisfactionChange;
    public float childSolSanityChange;
    public float childKoSatisfactionChange;
    public float childKoSanityChange;
    public AudioClip childClip;
    public float childTimeInBetweenAudio;
    [Range(0f, 1f)] public float childVolume;
    
    [Header("Drink Event")]
    public float drinkTimeBeforeReactivation;
    public float drinkSolSatisfactionChange;
    public float drinkSolSanityChange;
    public float drinkKoSatisfactionChange;
    public float drinkKoSanityChange;
    public float drinkFailSatisfactionChange;
    public float drinkFailSanityChange;
    public float drinkOverlapArea;
    public AudioClip drinkClip;
    public float drinkTimeInBetweenAudio;
    [Range(0f, 1f)] public float drinkVolume;
    public GameObject drink1Bubble;
    public GameObject drink2Bubble;

    [Header("Music Event")] 
    public GameObject musicPhonePrefab;
    public float musicTimeBeforeReactivation;
    public float musicSolSatisfactionChange;
    public float musicSolSanityChange;
    public float musicKoSatisfactionChange;
    public float musicKoSanityChange;
    public float musicOverlapArea;
    public AudioClip musicClip;
    public float musicTimeInBetweenAudio;
    [Range(0f, 1f)] public float musicVolume;
    
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

        if (startEventIndex < startEventTime.Length)
        {
            StartEventSpawning();
            return;
        }
        
        timer += Time.deltaTime;
        if (timer >= eventSpawnRate)
        {
            int randomEvent = Random.Range(0, eventsList.Count);
            eventsList[randomEvent].Activate();
            eventsList.RemoveAt(randomEvent);

            timer = 0f;
        }
    }

    private void StartEventSpawning()
    {
        timer += Time.deltaTime;
        if (timer >= startEventTime[startEventIndex])
        {
            switch (startEventIndex)
            {
                case 0:
                    ChildEvent[] cEvents = FindObjectsByType<ChildEvent>(FindObjectsSortMode.None);
                    int randomChild = Random.Range(0, cEvents.Length);
                    int cEventIndex = eventsList.IndexOf(cEvents[randomChild]);
                    eventsList[cEventIndex].Activate();
                    eventsList.RemoveAt(cEventIndex);
                    break;
                
                case 1:
                    BabyEvent[] bEvents = FindObjectsByType<BabyEvent>(FindObjectsSortMode.None);
                    int randomBaby = Random.Range(0, bEvents.Length);
                    int bEventIndex = eventsList.IndexOf(bEvents[randomBaby]);
                    eventsList[bEventIndex].Activate();
                    eventsList.RemoveAt(bEventIndex);
                    break;
                
                case 2:
                    ChildEvent[] dEvents = FindObjectsByType<ChildEvent>(FindObjectsSortMode.None);
                    int randomDrink = Random.Range(0, dEvents.Length);
                    int dEventIndex = eventsList.IndexOf(dEvents[randomDrink]);
                    eventsList[dEventIndex].Activate();
                    eventsList.RemoveAt(dEventIndex);
                    break;
                
                case 3:
                    ChildEvent[] mEvents = FindObjectsByType<ChildEvent>(FindObjectsSortMode.None);
                    int randomMusic = Random.Range(0, mEvents.Length);
                    int mEventIndex = eventsList.IndexOf(mEvents[randomMusic]);
                    eventsList[mEventIndex].Activate();
                    eventsList.RemoveAt(mEventIndex);
                    break;
                
                default:
                    int randomEvent = Random.Range(0, eventsList.Count);
                    eventsList[randomEvent].Activate();
                    eventsList.RemoveAt(randomEvent);
                    break;
            }

            startEventIndex++;
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
