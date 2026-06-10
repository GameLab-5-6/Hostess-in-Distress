using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EventManager : MonoBehaviour
{
    [SerializeField] private float eventSpawnRate = 10f;
    private float timer;
    private List<IEventable> eventsList;
    
    [Header("Solution")] 
    [SerializeField] private float satisfactionGained = 10f;

    [Header("Knockout")] 
    [SerializeField] private float satisfactionLost = -20f;
    [SerializeField] private float sanityGained = 20f;

    //first float will be the satisfaction value, second will be the sanity value
    public static event Action<float, float> OnBarsUpdated;

    //for the IEventables to be placed into the list they all need to be children of this EventManager object
    private void Awake()
    {
        eventsList = new List<IEventable>();
        foreach (Transform child in transform)
        {
            if (child.TryGetComponent(out IEventable eventable))
            {
                eventsList.Add(eventable);
            }
        }
    }

    private void OnEnable()
    {
        BabyEvent.OnEventSolution += SolutionUpdate;
        BabyEvent.OnEventKnockout += KnockoutUpdate;
    }

    private void OnDisable()
    {
        BabyEvent.OnEventSolution -= SolutionUpdate;
        BabyEvent.OnEventKnockout -= KnockoutUpdate;
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

    private void SolutionUpdate()
    {
        OnBarsUpdated?.Invoke(satisfactionGained, 0f);
    }

    private void KnockoutUpdate()
    {
        OnBarsUpdated?.Invoke(satisfactionLost, sanityGained);
    }
}
