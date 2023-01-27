using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;

public class GameTimeCountDown : MonoBehaviour
{
    #region Instance
    private static GameTimeCountDown _instance;
    public static GameTimeCountDown Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GameTimeCountDown>();
            }
            return _instance;
        }

    }
    #endregion

    float totalTime;
    TMP_Text textDisplay;
 
    int second = 0;
    DateTime startTime, endTime, p_startTime, puseTime;
    TimeSpan t;

    public GameGUIController gUIController;
    private LudoGameController ludoController;

    private void Start()
    {
        ludoController = GameObject.Find("GameSpecific").GetComponent<LudoGameController>();
        GameManager.Instance.gameTimeCountDown = this;

        Debug.Log("GameManager.Instance.type : - " + GameManager.Instance.type);
        if (GameManager.Instance.type == MyGameType.TwoPlayer)
        {
            totalTime = 8;
        }
        else if (GameManager.Instance.type == MyGameType.FourPlayer)
        {
            totalTime = 10;
        }
       
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
    public void CurrentTime()
    {
        textDisplay = GetComponent<TMP_Text>();     
        second = 0;
        endTime = DateTime.Now.AddMinutes(totalTime);
        p_startTime = DateTime.Now;

        CancelInvoke("TimeC");
        InvokeRepeating("TimeC", 0.0f,1.0f);
    }

    void TimeC()
    {
        second++;
        startTime = p_startTime.AddSeconds(second);

        if (startTime > endTime)
        {
            textDisplay.text = "00:00";
            GameManager.Instance.finishedTimes = true;
            
            if (GameManager.Instance.finishedTimes)
            {
                Debug.Log("FINISHSSSS");
                ludoController.gUIController.FinishedGame();
               
            }
            return;
        }


        t = endTime - startTime;

        if (t.Seconds < 0)
        {
            textDisplay.text = "00:00";

            return;
        }



        if (t.TotalHours < 1)
        {
            textDisplay.text = t.Minutes.ToString("00") + ":" + t.Seconds.ToString("00");

            if (t.TotalMinutes < 3)
            {
                textDisplay.color = Color.red;

                if (t.TotalSeconds <= 0)
                {
                    GameManager.Instance.finishedTimes = true;
                }
            }
        }
        else
        {
            textDisplay.text = t.Minutes.ToString("00") + ":" + t.Seconds.ToString("00");
        }
    }


}