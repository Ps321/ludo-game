using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
public class BidAmount : MonoBehaviour
{
    #region Instance
    private static BidAmount _instance;
    public static BidAmount Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<BidAmount>();
            }
            return _instance;
        }

    }
    #endregion


    int totalPlayer;
    int bidAmounts;
    GameObject scritpObj;
    public int eventID;
    string prizepool;
    public Button button;
    public Sprite sprite;
    int k;
    string eventMode;
    
    private void Awake()
    {
        scritpObj = GameObject.Find("SliderMenuAnim");
        button = GetComponent<Button>();
    }

    public void BIDAmount(int gameAmounts , int t_p , int tournament_id , string pricepool , int id,string mode)
    {
        bidAmounts = gameAmounts;        
        totalPlayer = t_p;
        GameManager.Instance.tournament_id = tournament_id;
        prizepool = pricepool;
        eventID = id;
        eventMode = mode;
        
    }

    private void Update()
    {
        k = eventID;
        if (GameManager.Instance.EnterEvents == true && k == GameManager.Instance.eventIDs)
        {
            button.enabled = false;
            button.transform.GetComponent<Button>().GetComponentInChildren<TMP_Text>().text = "";
            this.button.GetComponent<Button>().image.sprite = sprite;
            GameManager.Instance.StarteventID = k;
            GameManager.Instance.eventMode = eventMode;
        }
    }

    public void BTClick()
    {
        if (GameManager.Instance.EnterEvents == false)
        {
            if (GameManager.Instance.checkbalforplay <= bidAmounts && GameManager.Instance.EnterEvents == false)
            {
                SliderMenuAnim.Instance.ADDMoney(0);
            }
            else
            {
                GameManager.Instance.eventIDs = eventID;
                GameManager.Instance.payoutCoins = bidAmounts;
                GameManager.Instance.numberOfTotalPlayers = totalPlayer;
                GameManager.Instance.prizePoolMoney = prizepool;
                scritpObj.transform.GetComponent<SliderMenuAnim>().ConfirmAmountPlay(0);
               
            }
        }
       
        
    }

    public void ONTest(string eventsId)
    {
        //  k = 0eventsId;
        //if (GameManager.Instance.EnterEvents == true && k == GameManager.Instance.eventIDs)

        if (string.IsNullOrEmpty(eventsId))
            return;
        Debug.Log(eventID);

        if(GameManager.Instance.eventIdsBT.Contains(Convert.ToInt16(eventsId)))
        {
            button.enabled = false;
            button.transform.GetComponent<Button>().GetComponentInChildren<TMP_Text>().text = "";
            this.button.GetComponent<Button>().image.sprite = sprite;
            GameManager.Instance.StarteventID = k;
            GameManager.Instance.eventMode = eventMode;
        }
        else
        {
            button.enabled = true;            
        }
    }
}