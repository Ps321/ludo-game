using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeFillter : MonoBehaviour
{
    #region Instance
    private static GameModeFillter _instance;
    public static GameModeFillter Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GameModeFillter>();
            }
            return _instance;
        }

    }
    #endregion

    [SerializeField] List<GameObject> eventsGameObjects = new List<GameObject>();
    [SerializeField]GameObject contentGameobj;
   
    public void FillterDisplay(GameObject eventsGameObject)
    {
        eventsGameObjects.Add(eventsGameObject);
    }

    private void Update()
    {
        for (int i = 0; i < eventsGameObjects.Count; i++)
        {
            if (eventsGameObjects[i] == null)
            {
                eventsGameObjects.RemoveAt(i);
            }

        }
    }

    public void BattelAll()
    {
        for (int i = 0; i < eventsGameObjects.Count; i++)
        {
            eventsGameObjects[i].SetActive(true);

        }
    }
    public void Battel1v1()
    {
        for (int i = 0; i < eventsGameObjects.Count; i++)
        {
            if (eventsGameObjects[i].transform.name == "1v1 Battle")
            {
                eventsGameObjects[i].SetActive(true);
            }else
            {
                eventsGameObjects[i].SetActive(false);
            }
        }
    }

    public void Winner1()
    {
        for (int i = 0; i < eventsGameObjects.Count; i++)
        {
            if (eventsGameObjects[i].transform.name == "1 Winner")
            {
                eventsGameObjects[i].SetActive(true);
            }
            else
            {
                eventsGameObjects[i].SetActive(false);
            }
        }
    }

    public void Winner2()
    {
        for (int i = 0; i < eventsGameObjects.Count; i++)
        {
            if (eventsGameObjects[i].transform.name == "2 Winner")
            {
                eventsGameObjects[i].SetActive(true);
            }
            else
            {
                eventsGameObjects[i].SetActive(false);
            }
        }
    }

    public void Winner3()
    {
        for (int i = 0; i < eventsGameObjects.Count; i++)
        {
            if (eventsGameObjects[i].transform.name == "3 Winner")
            {
                eventsGameObjects[i].SetActive(true);
            }
            else
            {
                eventsGameObjects[i].SetActive(false);
            }
        }
    }

    public void Winner4()
    {
        for (int i = 0; i < eventsGameObjects.Count; i++)
        {
            if (eventsGameObjects[i].transform.name == "4 Winner")
            {
                eventsGameObjects[i].SetActive(true);
            }
            else
            {
                eventsGameObjects[i].SetActive(false);
            }
        }
    }

}
