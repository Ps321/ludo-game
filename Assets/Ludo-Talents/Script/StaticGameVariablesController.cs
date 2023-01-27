using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticGameVariablesController : MonoBehaviour
{

    public Sprite[] avatars;

 
    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(transform.gameObject);

    }

    // Update is called once per frame
    public void destroy()
    {
        if (this.gameObject != null)
            DestroyImmediate(this.gameObject);
    }
}
