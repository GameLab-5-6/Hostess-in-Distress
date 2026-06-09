using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [SerializeField] private float eventSpawnRate = 10f;
    private float timer;
    private List<IEventable> eventsList;

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

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= eventSpawnRate)
        {
            int randomEvent = Random.Range(0, eventsList.Count);
            eventsList[randomEvent].Activate();

            timer = 0f;
        }
    }
}
