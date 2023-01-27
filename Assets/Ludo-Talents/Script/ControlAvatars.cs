using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ControlAvatars : MonoBehaviour
{
    public AudioSource playerJoin;
    public AudioSource playerLeft;
    public GameObject FailedToJoinRoomWindow;
    public GameObject FailedToJoinRoomText;
    public GameObject startButtonPrivate;

    public GameObject prefab;
    private List<GameObject> avatars;
    private GameObject lastAvatar;
    private Sprite restoreSprite;
    private int addAvatars = 10;
    public bool foundPlayer = false;
    public bool playerRejected = false;
    public float speed;
    private float speed1;
    public bool foundCancel = false;
    private GameObject OpponentAvatar;
    public Text opponentNameText;
    public Sprite noAvatarSprite;

    public GameObject menuCanvas;
    public GameObject titleCanvas;
    public GameObject matchPlayersCanvas;

    public GameObject AvatarFrameMy;
    public GameObject AvatarFrameOpponent;
    public GameObject vsText;
    public GameObject centerCoins;
    public GameObject leftcoins;
    public GameObject rightCoins;

    public GameObject oppontentCoinImage;
    public GameObject myCoinImage;
    public GameObject oppontentPayoutCoins;
    public GameObject myPayoutCoins;
    public GameObject centerPayoutCoins;

   
    private AudioSource[] audioSources;
    public GameObject cantPlayNowOppo;
    public GameObject longTimeMessage;
    // Use this for initialization
    public float waitingOpponentTime = 0;
    public GameObject messageBubbleText;
    public GameObject messageBubble;
    public bool opponentActive = true;
    public bool GameSceneLoaded = false;

    GameObject gb;
    private List<int> playerDisconnectedIndexes = new List<int>();
    void Awake()
    {
        audioSources = GetComponents<AudioSource>();
    }

    void Start()
    {
        gb = GameObject.Find("SliderMenuAnim");
        GameManager.Instance.controlAvatars = this;
    }

    public void CancelWaitingForPlayer()
    {
        PhotonNetwork.LeaveRoom();
    }


    public void ShowJoinFailed(string error)
    {
        FailedToJoinRoomWindow.SetActive(true);
    }

    public void reset()
    {    

        Debug.Log("Two and four player ");
        PhotonNetwork.BackgroundTimeout = StaticString.photonDisconnectTimeoutLong;

        if (GameManager.Instance.type == MyGameType.TwoPlayer)
        {
            GameManager.Instance.requiredPlayers = 2;

        }
        else if (GameManager.Instance.type == MyGameType.FourPlayer)
        {
            GameManager.Instance.requiredPlayers = 4;
        }
        gb.transform.GetComponent<SliderMenuAnim>().MatchPlayer(0);
    }
     
    public void PlayerJoined(int index, string id)
    {
        Debug.Log("PLAYJOINED");
        GameManager.Instance.currentPlayersCount++;

        if (GameManager.Instance.opponentsIDs.Contains(id))
        {
            Debug.Log("Current players count: " + GameManager.Instance.currentPlayersCount);

            if (GameManager.Instance.currentPlayersCount >= GameManager.Instance.requiredPlayers)
            {
                if (PhotonNetwork.isMasterClient)
                    GameManager.Instance.playfabManager.StartGame();
            }
            else
            {
                if (PhotonNetwork.isMasterClient)
                {
                    GameManager.Instance.playfabManager.WaitForNewPlayer();
                    Debug.Log("INVOKE PLAYJOINED");
                }
            }
        }

    }

    public void PlayerDisconnected(int index)
    {
        playerLeft.Play();
        GameManager.Instance.currentPlayersCount--;
        GameManager.Instance.opponentsIDs[index] = null;
        GameManager.Instance.opponentsNames[index] = null;
        GameManager.Instance.opponentsAvatars[index] = null;
        Debug.Log("Current players count: " + GameManager.Instance.currentPlayersCount);
    }

    public void showLongTimeMessage()
    {
        if (!foundPlayer && gameObject.activeSelf)
            longTimeMessage.SetActive(true);
    }



    public void hideLongTimeMessage()
    {
        longTimeMessage.SetActive(false);
    }

    

    public void playerDisconnected()
    {
        StopAllCoroutines();
        rightCoins.SetActive(false);
        leftcoins.SetActive(false);
        cantPlayNowOppo.SetActive(true);
        PhotonNetwork.LeaveRoom();
        Invoke("cancelGame", 5.0f);
    }

    private void cancelGame()
    {
        cantPlayNowOppo.SetActive(false);
        matchPlayersCanvas.SetActive(false);
        PhotonNetwork.BackgroundTimeout = StaticString.photonDisconnectTimeoutLong; ;
        Debug.Log("Timeout 1");
        //reset ();
    }

    public void StartGamePrivate()
    {
        //PhotonNetwork.RaiseEvent((int)EnumPhoton.BeginPrivateGame, null, true, null);
        GameManager.Instance.playfabManager.StartGame();
    }

    private void startGame()
    {
        GameObject.Find("PlayFabManager").GetComponent<PlayFabManager>().imReady = true;
        if (!GameManager.Instance.offlineMode)
        {

            PhotonNetwork.RaiseEvent(199, GameManager.Instance.cueIndex + "-" + GameManager.Instance.cueTime, true, null);
        }

        if (PhotonNetwork.playerList.Length < 2)
        {
            playerDisconnected();
        }

        //SceneManager.LoadScene ("GameScene");
        //reset ();
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            PhotonNetwork.SendOutgoingCommands();
            Debug.Log("Application pause");
        }
        else
        {
            PhotonNetwork.SendOutgoingCommands();
            Debug.Log("Application resume");
        }
    }

    public void hideMessageBubble()
    {
        messageBubble.GetComponent<Animator>().Play("HideBubble");
    }

    public IEnumerator updateMessageBubbleText()
    {
        yield return new WaitForSeconds(1.0f * 2);
        waitingOpponentTime -= 1;
        if (!GameManager.Instance.opponentDisconnected)
            messageBubbleText.GetComponent<Text>().text = StaticString.waitingForOpponent + " " + waitingOpponentTime;
        if (waitingOpponentTime > 0 && !opponentActive && !GameManager.Instance.opponentDisconnected)
        {
            StartCoroutine(updateMessageBubbleText());
        }
    }
}
