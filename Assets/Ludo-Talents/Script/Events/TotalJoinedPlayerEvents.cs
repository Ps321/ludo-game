using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using Firebase.Database;

public class TotalJoinedPlayerEvents : MonoBehaviour
{
    #region Instance
    private static TotalJoinedPlayerEvents _instance;
    public static TotalJoinedPlayerEvents Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<TotalJoinedPlayerEvents>();
            }
            return _instance;
        }

    }
    #endregion

    int eventIDss;
    TMP_Text textA;
    DatabaseReference databaseReference;
    int ee;
    private void Start()
    {
        textA = GetComponent<TMP_Text>();
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
    }
    public void GetEventsIDs(int id)
    {
        eventIDss = id;
        InvokeRepeating("TT", 1f, 1f);
    }

    public void SaveData()
    {

        StartCoroutine(updateJoinedPlayterCount());
    }
    IEnumerator updateJoinedPlayterCount()
    {
        var DBTask = databaseReference.Child("Events").Child(GameManager.Instance.eventIDs.ToString()).Child("counter").SetValueAsync(int.Parse(textA.text) + 1);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {

        }
        else
        {

        }
    }

    public void TT()
    {
        StartCoroutine(loadJoinedPlayterCount());
    }
    IEnumerator loadJoinedPlayterCount()
    {
        yield return new WaitForSecondsRealtime(1);
        var DBTask = databaseReference.Child("Events").Child(eventIDss.ToString()).GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {

        }
        else if (DBTask.Result.Value == null)
        {

        }
        else
        {
            DataSnapshot dataSnapshot = DBTask.Result;

            textA.text = dataSnapshot.Child("counter").Value.ToString();
        }
    }
    private void OnDisable()
    {
        CancelInvoke("TT");
    }

}
