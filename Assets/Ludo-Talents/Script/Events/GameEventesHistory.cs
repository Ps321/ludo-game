using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Database;

public class GameEventesHistory : MonoBehaviour
{
    #region Instance
    private static GameEventesHistory _instance;
    public static GameEventesHistory Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GameEventesHistory>();
            }
            return _instance;
        }

    }
    #endregion

    WWWForm form;
    public int idEvent;
    DatabaseReference databaseReference;
    public List<int> IDs = new List<int>();

    private void Start()
    {        
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        StartEvents();
    }

    public void StartEvents()
    {
        StartCoroutine(GetGames());       
    }
   

    IEnumerator GetGames()
    {
        form = new WWWForm();
        WWW w = new WWW(StaticString.gameEvent, form);
        yield return w;
        if (w.error != null)
        {
            Debug.Log("<color=red>" + w.text + "</color>");//error
        }
        else
        {
            if (w.isDone)
            {
                Response<GameEvents[]> response = JsonUtility.FromJson<Response<GameEvents[]>>(w.text);
                Debug.Log("_GameEventesHistory" + w.text);
                if (response.status)
                {
                    GameEvents[] gamesEvents = response.data;                 

                    for (int i = 0; i < gamesEvents.Length; i++)
                    {
                        GameObject g = ObjectPool.ShareInstance.GetPoolObject();
                        if (!IDs.Contains(gamesEvents[i].id))
                        {
                            // clone Game Objects
                            g.SetActive(true);
                            g.transform.name = gamesEvents[i].mode;

                            GameModeFillter.Instance.FillterDisplay(g);

                            //GameObecjt Name  - mode
                            g.transform.GetChild(0).GetComponent<TMP_Text>().text = gamesEvents[i].mode;

                            //GameObecjt Name -Bg_PrizePoolBT
                            g.transform.GetChild(2).GetComponent<Button>().GetComponentInChildren<TMP_Text>().text = StaticString.ruppeSymbol + gamesEvents[i].prize_pool;
                            g.transform.GetChild(2).GetComponent<PricePoolMode>().GetID(gamesEvents[i].id, gamesEvents[i].t_p, gamesEvents[i].mode, gamesEvents[i].prize_pool);

                            //GameObecjt Name -Text_joined
                            g.transform.GetChild(4).GetComponent<TotalJoinedPlayerEvents>().GetEventsIDs(gamesEvents[i].id);

                            //GameObecjt Name -Bg_entryfeeBT
                            g.transform.GetChild(6).GetComponent<Button>().GetComponentInChildren<TMP_Text>().text = StaticString.ruppeSymbol + gamesEvents[i].enrty_fee.ToString();
                            g.transform.GetChild(6).GetComponent<BidAmount>().BIDAmount(gamesEvents[i].enrty_fee, gamesEvents[i].t_p, gamesEvents[i].tournament_id, gamesEvents[i].prize_pool, gamesEvents[i].id,gamesEvents[i].mode);

                            //GameObecjt Name -Text_Time
                            g.transform.GetChild(8).GetComponent<TimerCountdowns>().CurrentTime(g, gamesEvents[i].time, gamesEvents[i].id);
                            IDs.Add(gamesEvents[i].id);
                            StartCoroutine(loadJoinedPlayterCount(gamesEvents[i].id.ToString()));
                        }
                    }
                   // UIManager.Instance.LoadingPanel.SetActive(false);
                }
                else
                {
                   // UIManager.Instance.LoadingPanel.SetActive(false);
                }
               
            }
        }      
    }

    #region Firebase Total Joined Player
    IEnumerator loadJoinedPlayterCount(string eventIDss)
    {
        var DBTask = databaseReference.Child("Events").Child(eventIDss).GetValueAsync();
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {

        }
        else if (DBTask.Result.Value == null)
        {
            StartCoroutine(updateJoinedPlayter(eventIDss));
            StartCoroutine(updateJoinedPlayterCount(eventIDss));
        }
        else
        {
        }
    }

    IEnumerator updateJoinedPlayter(string eventIDs)
    {
        var DBTask = databaseReference.Child("Events").Child(eventIDs).Child("eventId").SetValueAsync(eventIDs);
        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {

        }
        else
        {

        }
    }

    IEnumerator updateJoinedPlayterCount(string eventIDss)
    {
        var DBTask = databaseReference.Child("Events").Child(eventIDss).Child("counter").SetValueAsync((UnityEngine.Random.Range(10, 20)));

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        { }
        else
        {
        }
    }

    #endregion

}
