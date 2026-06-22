using System.Collections.Generic;
using UnityEngine;

public class AudioObjectPooler : MonoBehaviour
{
    public static AudioObjectPooler Instance;

    [SerializeField] private GameObject audioSourcePrefab;
    [SerializeField] private int poolSize = 10;

    public List<GameObject> pooledObjects;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        pooledObjects = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(audioSourcePrefab);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }

        GameObject obj = Instantiate(audioSourcePrefab);
        obj.SetActive(false);
        pooledObjects.Add(obj);
        return obj;
    }
}