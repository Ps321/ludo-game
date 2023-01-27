using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool ShareInstance;
    public List<GameObject> poolObjects;
    public GameObject objectToPool;
    public int amountsPool = 20;
    public bool willGrow = true;

    private void Awake()
    {
        ShareInstance = this;
    }

    void Start()
    {
        poolObjects = new List<GameObject>();
        for (int i = 0; i < amountsPool; i++)
        {
            GameObject obj = Instantiate(objectToPool, transform);
            obj.SetActive(false);
            poolObjects.Add(obj);
        }

    }
    public GameObject GetPoolObject()
    {

        for (int i = 0; i < poolObjects.Count; i++)
        {
            if (!poolObjects[i].activeInHierarchy)
            {
                return poolObjects[i];
            }
        }

        if (willGrow)
        {
            GameObject obj = Instantiate(objectToPool, transform);
            poolObjects.Add(obj);
            return obj;
        }
        return null;
    }
}
