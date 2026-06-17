using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EventManager : MonoBehaviour
{
    [SerializeField] private float eventSpawnRate = 10f;
    private float timer;
    private List<IEventable> eventsList;

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
    }

    private void Update()
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
}
