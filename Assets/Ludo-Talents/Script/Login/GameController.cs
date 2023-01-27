using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    #region Instance
    private static GameController _instance;
    public static GameController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<GameController>();
            }
            return _instance;
        }

    }
    #endregion

    private PlayFabManager playFabManager;
    GameObject controlAvatars;
    #region Unity Method
    void Awake()
    {
        GameManager.Instance.gameController = this;
        playFabManager = GameObject.Find("PlayfabManager").GetComponent<PlayFabManager>();
        DontDestroyOnLoad(transform.gameObject);
        if (!GameManager.Instance.logged)
        {

#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
    		{
                initSession();
    		}
#else
            initSession();
#endif

            GameManager.Instance.logged = true;
        }
    }
    #endregion


    #region Private Method

    private void initSession()
    {

        if (PlayerPrefs.HasKey("Login") && PlayerPrefs.HasKey("mobile"))
        { playFabManager.Login(); }


    }
    #endregion



    #region public Method

    public void startRandomGamevscomputer()
    {
        controlAvatars = GameObject.Find("ControlAvatars");
        controlAvatars.GetComponent<ControlAvatars>().reset();
      
        playFabManager.JoinRoomAndStartGame();
        //playFabManager.PlaywithBots();
    }

    public void startRandomGame()
    {

        controlAvatars = GameObject.Find("ControlAvatars");
        controlAvatars.GetComponent<ControlAvatars>().reset();

        playFabManager.JoinRoomAndStartGame();
       // playFabManager.OnPhotonRandomJoinFailed();
        
    }

    public void destroy()
    {
        if (this.gameObject != null)
            DestroyImmediate(this.gameObject);
    }

    #endregion

}
