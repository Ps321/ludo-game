using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;

public class TimerCountdowns : MonoBehaviour
{

    private Sprite[] avatarSprites;
    public TMP_Text textDisplay;
    public GameObject myobject;

    int second = 0;
    DateTime startDate, endData, p_startTime, puseTime;
    TimeSpan t;
    int ids;
    private void Start()
    {
        textDisplay = GetComponent<TMP_Text>();
        avatarSprites = GameObject.Find("StaticGameVariablesContainer").GetComponent<StaticGameVariablesController>().avatars;

    }
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            puseTime = DateTime.Now;
        }
        else
        {
            var dtt = (DateTime.Now - puseTime).TotalSeconds;
            p_startTime = DateTime.Now.AddSeconds(dtt);
            CancelInvoke("TimeC");

            InvokeRepeating("TimeC", 0f, 1f);
            Debug.Log("pause" + pause);
        }
    }
    public void CurrentTime(GameObject startTime , string datime, int id)
    {
        ids = id;
        myobject = startTime;
        second = 0;
        //endData = DateTime.Now.AddMinutes(startTime);

        p_startTime = DateTime.Now;        
        DateTime dtt = DateTime.Parse(datime);
        var diffInSeconds = (dtt - p_startTime).TotalSeconds;
        endData = DateTime.Now.AddSeconds(diffInSeconds);
        CancelInvoke("TimeC");
        InvokeRepeating("TimeC", 0.0f, 1.0f);
    }

    void TimeC()
    {      
        second++;
        startDate = p_startTime.AddSeconds(second);

        if (startDate > endData)
        {
            textDisplay.text = "00:00:00";
            if (GameManager.Instance.showTargetLines == false)
            {
                myobject.SetActive(false);
                myobject.transform.SetAsLastSibling();
            }
            
            textDisplay.color = Color.white;

            for (int i = 0; i < GameEventesHistory.Instance.IDs.Count; i++)
            {
                if (ids == GameEventesHistory.Instance.IDs[i])
                {
                    GameEventesHistory.Instance.IDs.RemoveAt(i);
                    GameEventesHistory.Instance.StartEvents();
                }
            }
            return;
        }

        t = endData - startDate;

        if (t.Seconds < 0)
        {
            textDisplay.text = "00:00:00";           
            return;
        }



        if (t.TotalHours < 1)
        {
            textDisplay.text = "00" + ":" + t.Minutes.ToString("00") + ":" + t.Seconds.ToString("00");

            if (t.TotalMinutes < 1)
            {
                textDisplay.color = Color.red;               
                if (t.TotalSeconds <= 12)
                {
                    GameManager.Instance.EnterEvents = false;
                    if (GameManager.Instance.showTargetLines == true && ids == GameManager.Instance.StarteventID)
                    {
                        if (PlayerPrefs.HasKey("EventConfrim"))
                        {
                            PlayerPrefs.DeleteKey("EventConfrim");
                        }
                        GameManager.Instance.gameController.startRandomGame();                        
                    }
                    if (GameManager.Instance.addBotPlayers && t.TotalSeconds <= 3)
                    {
                       Timertake();                       
                    }
                }
            }
        }
        else
        {
            textDisplay.text =(Math.Floor(t.TotalHours)).ToString("00") + ":" + t.Minutes.ToString("00") + ":" + t.Seconds.ToString("00");
        }
    }

    void Timertake()
    {           
        Debug.Log("calling bot matching");
        GameManager.Instance.gameController.startRandomGamevscomputer();
        PhotonNetwork.room.IsOpen = false;
        PhotonNetwork.room.IsVisible = false;

        if (PhotonNetwork.isMasterClient)
        {
            Debug.Log("Back in Loop TimerTake");
            PhotonNetwork.RaiseEvent((int)EnumPhoton.StartWithBots, null, true, null);
        }

        for (int i = 0; i < GameManager.Instance.requiredPlayers - 1; i++)
        {
            if (GameManager.Instance.opponentsIDs[i] == null)
            {
                AddBot(i);
            }
        }
    }
    public void AddBot(int i)
    {
        GameManager.Instance.opponentsAvatars[i] = avatarSprites[UnityEngine.Random.Range(0, avatarSprites.Length - 1)];
        GameManager.Instance.opponentsIDs[i] = "_BOT" + i;
        int k= UnityEngine.Random.Range(0, 42);
        GameManager.Instance.opponentsNames[i] = StaticString.BotNames[k];
        Debug.Log("Name: " + GameManager.Instance.opponentsNames[i]);
        if (GameManager.Instance.type == MyGameType.TwoPlayer || GameManager.Instance.type == MyGameType.FourPlayer)
        {
            Debug.Log("Game Type " + GameManager.Instance.type);

            //StartCoroutine(cutBidAmount());
        }
        GameManager.Instance.controlAvatars.PlayerJoined(i, "_BOT" + i);
    }
}
