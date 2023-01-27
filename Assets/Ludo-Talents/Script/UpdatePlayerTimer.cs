using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdatePlayerTimer : MonoBehaviour
{
    private float playerTime;
    public GameObject timerObject;
    private Image timer;
    private bool timeSoundsStarted;
    public AudioSource[] audioSources;
    public GameObject GUIController;
    public bool myTimer;
    public bool paused = false;
    public int autoPlayLife = 3;

    public GameObject heart1, heart2, heart3;
    public Sprite[] hearts;
    public GameObject lifeFinished;
    public GameObject lifeLineObj;
    public AudioSource timeOut;
   
    private static object instance;
    private UpdatePlayerTimer() { }

    public static UpdatePlayerTimer Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new UpdatePlayerTimer();

            }
            return (UpdatePlayerTimer)instance;
        }
    }

    // Use this for initialization
    void Start()
    {
        autoPlayLife = 3;
        heart1.gameObject.SetActive(true);
        heart2.gameObject.SetActive(true);
        heart3.gameObject.SetActive(true);

        timer = gameObject.GetComponent<Image>();
        // ludoController = new LudoGameController();
    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        timer = gameObject.GetComponent<Image>();
    }

    public void Pause()
    {
        paused = true;
        audioSources[0].Stop();
    }

    // Update is called once per frame
    void Update()
    {
       
        if (!paused)
            updateClock();

        if (autoPlayLife > 3)
            autoPlayLife = 3;

        switch (autoPlayLife)
        {
            case 3:

                break;

            case 2:
                heart1.GetComponent<Image>().sprite = hearts[0];
                break;

            case 1:                
                heart2.GetComponent<Image>().sprite = hearts[1];                
                break;

            case 0:
                heart3.GetComponent<Image>().sprite = hearts[2];               
                break;
        }
    }

    public void restartTimer()
    {
        paused = false;
        timer.fillAmount = 1.0f;

    }


    void OnDisable()
    {
        if (timer != null)
        {
            timer.fillAmount = 1.0f;
            paused = false;
            audioSources[0].Stop();
        }
    }

    private void updateClock()
    {
       
        float minus;

        playerTime = GameManager.Instance.playerTime;
        if (GameManager.Instance.offlineMode)
            playerTime = GameManager.Instance.playerTime + GameManager.Instance.cueTime;

        minus = 1f / playerTime * Time.deltaTime;

        timer.fillAmount -= minus;

        if (timer.fillAmount < 0.25f && !timeSoundsStarted)
        {
            audioSources[0].Play();
            timeSoundsStarted = true;
        }

        if (timer.fillAmount == 0)
        {

            Debug.Log("TIME 0");

            audioSources[0].Stop();
            GameManager.Instance.stopTimer = true;
            if (!GameManager.Instance.offlineMode)
            {
                if (myTimer)
                {
                    if (autoPlayLife != 1)
                    {
                        autoPlayLife--;
                        Debug.Log("Life Remaining" + autoPlayLife);
                      
                        if (GameManager.Instance.type == MyGameType.TwoPlayer || GameManager.Instance.type == MyGameType.FourPlayer)
                        {
                            lifeLineObj.SetActive(true);
                            // LudoGameController.Instance.RollDiceForPlayer();
                            GUIController.GetComponent<GameGUIController>().SendFinishTurn();
                            Debug.Log("Calling AutoPlay for Online mode");
                            Update();
                        }

                    }
                    else /*if (autoPlayLife == 1)*/
                    {
                        audioSources[1].Play();
                        paused = true;
                        Debug.Log("Timer call finish turn");
                        GUIController.GetComponent<GameGUIController>().LeaveGame(true);
                       /* GUIController.GetComponent<GameGUIController>().SendFinishTurn();
                        lifeFinished.SetActive(true);
                        GameManager.Instance.wasFault = true;
                        GameManager.Instance.cueController.SetTurnOffline(true);*/
                    }
                }
            }
            else
            {
                GameManager.Instance.wasFault = true;
                GameManager.Instance.cueController.SetTurnOffline(true);
            }
            
            //PhotonNetwork.RaiseEvent(9, null, true, null);
        }
    }
    
}


