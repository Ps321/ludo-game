using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Response<T>
{

    public bool status;
    public string message;
    public T data;
}
