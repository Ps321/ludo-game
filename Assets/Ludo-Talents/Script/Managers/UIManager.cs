using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UIManager : MonoBehaviour
{ 
    #region Instance
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<UIManager>();
            }
            return _instance;
        }

    }
    #endregion

    #region Variable
    [Space]
    [Header("Login Scene")]
    public GameObject loginPanel;
    public GameObject verifyPanel;
    public GameObject LoadingPanel;

    [Space]
    [Header("Menu Scene")]
    public GameObject userNamePanel;
    public GameObject homePanel;
    public GameObject referAndEarnPanel;
    public GameObject eventPanel;
    public GameObject settingPanel;
    public GameObject myProfile;
    public GameObject myProfileHome;
    public GameObject myBalance;
    public GameObject helpDesk;
    public GameObject howToPlay;
    public GameObject faq;
    public GameObject privacyPolicy;
    public GameObject termsAndConditions;
    public GameObject confirm_Payment;
    

    [Space]
    [Header("Add Bank Fild")]
    public GameObject addBank;
    public GameObject addUpi;
    public GameObject withDrawMoney;
    public GameObject prizePool;
    public GameObject prizePoolTwo;
    public GameObject prizePoolFour;


    [Space]
    [Header("Sprite")]
    public Sprite verifySprite;
    public Sprite joinedEvents;


    #endregion

}