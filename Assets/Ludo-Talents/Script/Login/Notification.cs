using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Messaging;

public class Notification : MonoBehaviour
{
    public void Start()
    {
        DontDestroyOnLoad(transform.gameObject);
        FirebaseMessaging.SubscribeAsync("all");
        Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
        Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
    }

    public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    {
        UnityEngine.Debug.Log("Received Registration Token: " + token.Token);
    }

    public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    {
        UnityEngine.Debug.Log("Received a new message from: " + e.Message.From);
    }
}
