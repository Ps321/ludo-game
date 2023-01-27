using System;
using System.Collections;
using System.Collections.Generic;
using Photon;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameGUIController : PunBehaviour
{
    public AudioSource gameStarts;
    public GameObject TIPObject;
    public GameObject firstPrizeObject;
    public GameObject SecondPrizeObject;
    public GameObject firstPrizeText;
    public GameObject firstPrizeTextScreenWinScreen;
    public GameObject secondPrizeText;
    public GameObject WinCanvas;
    public GameObject LoseCanvas;

    public AudioSource WinSound;
    public AudioSource myTurnSource;
    public AudioSource oppoTurnSource;
    private bool AllPlayersReady = false;
    // LUDO
    public MultiDimensionalGameObject[] PlayersPawns;
    public GameObject[] PlayersDices;
    public GameObject[] HomeLockObjects; 
    public GameObject Sounds;

    [System.Serializable]
    public class MultiDimensionalGameObject
    {
        public GameObject[] objectsArray;
    }

    public GameObject ludoBoard;
    public GameObject[] diceBackgrounds;
    public Sprite diceBgSprite;
    public MultiDimensionalGameObject[] playersPawnsColors;
    public MultiDimensionalGameObject[] playersPawnsMultiple;

    private Color colorRed = new Color(181 / 255, 7 / 255, 10 / 255);
    private Color colorBlue = new Color(36 / 255, 127 / 255, 209 / 255);
    private Color colorYellow = new Color(234 / 255.0f, 186.0f / 255, 7 / 255);
    private Color colorGreen = new Color(128 / 255, 192 / 255, 29 / 255);


    // END LUDO

    public GameObject GameFinishWindow;
    public GameObject ScreenShotController;
    public GameObject PlayerInfoWindow;
    private bool SecondPlayerOnDiagonal = true;

   [SerializeField] private List<string> PlayersIDs;
    public GameObject[] Players;
    public GameObject[] PlayersTimers;
    public GameObject[] ActivePlayers;
    public GameObject[] PlayersAvatarsButton;
    public GameObject[] PlayerScores;
    public GameObject[] PlayerScoresText;

    private List<Sprite> avatars;
    private List<string> names;
    
    private List<PlayerObject> playerObjects;
    private int myIndex;
    private string myId;

    public int Checkpawn = 0;

    private Color[] borderColors = new Color[4] { Color.yellow, Color.green, Color.red, Color.blue };

    private int currentPlayerIndex;

    private int ActivePlayersInRoom;


    private string CurrentPlayerID;

    private List<PlayerObject> playersFinished = new List<PlayerObject>();

    private bool iFinished = false;
    private bool FinishWindowActive = false;

    private float firstPlacePrize;
    private float secondPlacePrize;
    private float threePlacePrize;
    private float fourPlacePrize;
    int prizepoolAmount;
    int rankss;
   

    private int requiredToStart = 0;
   
    private LudoGameController ludoController;


    private WWWForm form;

    // ludo Score 

   [SerializeField] private List<ScoreTest> scoreTests = new List<ScoreTest>();

    //end ludo..
    private static object instance;

    private GameGUIController() { }

    public static GameGUIController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameGUIController();
            }
            return (GameGUIController)instance;
        }
    }

    public List<string> moverNames = new List<string>();
    // Use this for initialization
    void Start()
    {
        requiredToStart = GameManager.Instance.requiredPlayers;

        PhotonNetwork.RaiseEvent((int)EnumPhoton.ReadyToPlay, 0, true, null);


        // LUDO
        // Rotate board and set colors

        Color[] colors = null;
        int rotation = 0;
        

        if (rotation == 0)
        {
            colors = new Color[] { colorRed, colorGreen, colorYellow, colorBlue };
            ludoBoard.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, 0.0f);
        }
        else if (rotation == 1)
        {
            colors = new Color[] { colorBlue, colorYellow, colorGreen, colorRed };
            ludoBoard.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, -90.0f);

        }
        else if (rotation == 2)
        {
            colors = new Color[] { colorYellow, colorBlue, colorRed, colorGreen };
            ludoBoard.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, -180.0f);
        }
        else if (rotation == 3)
        {
            colors = new Color[] { colorGreen, colorRed, colorBlue, colorYellow };
            ludoBoard.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, -270.0f);

        }
        else
        {
            colors = new Color[] { colorBlue, colorYellow, colorGreen, colorRed };
            ludoBoard.GetComponent<RectTransform>().eulerAngles = new Vector3(0, 0, -90);
        }


        for (int i = 0; i < diceBackgrounds.Length; i++)
        {
            diceBackgrounds[i].GetComponent<Image>().sprite = diceBgSprite;
        }

        for (int i = 0; i < playersPawnsColors.Length; i++)
        {
            for (int j = 0; j < playersPawnsColors[i].objectsArray.Length; j++)
            {
                playersPawnsColors[i].objectsArray[j].GetComponent<Image>().color = colors[i];
                playersPawnsMultiple[i].objectsArray[j].GetComponent<Image>().color = colors[i];
            }
        }


        // END LUDO

        // Update player data in playfab
        Dictionary<string, string> data = new Dictionary<string, string>();

        data.Add(MyPlayerData.GamesPlayedKey, (GameManager.Instance.myPlayerData.GetPlayedGamesCount() + 1).ToString());

        GameManager.Instance.myPlayerData.UpdateUserData(data);

        currentPlayerIndex = 0;
        myId = GameManager.Instance.playfabManager.PlayFabId;
        playerObjects = new List<PlayerObject>();
        avatars = GameManager.Instance.opponentsAvatars;
        avatars.Insert(0, GameManager.Instance.avatarMy);

        names = GameManager.Instance.opponentsNames;
        names.Insert(0, GameManager.Instance.nameMy);

        PlayersIDs = new List<string>();
        for (int i = 0; i < GameManager.Instance.opponentsIDs.Count; i++)
        {
            if (GameManager.Instance.opponentsIDs[i] != null)
            {
                PlayersIDs.Add(GameManager.Instance.opponentsIDs[i]);
            }
        }

        PlayersIDs.Insert(0, GameManager.Instance.playfabManager.PlayFabId);

        for (int i = 0; i < PlayersIDs.Count; i++)
        {
            playerObjects.Add(new PlayerObject(names[i], PlayersIDs[i], avatars[i]));
            Debug.Log(name[i]);
            Debug.Log(PlayersIDs[i]);
            Debug.Log(avatars[i]);

        }

        // Bubble sort
        for (int i = 0; i < PlayersIDs.Count; i++)
        {
            for (int j = 0; j < PlayersIDs.Count - 1; j++)
            {
                if (string.Compare(playerObjects[j].id, playerObjects[j + 1].id) == 1)
                {
                    // swaap ids
                    PlayerObject temp = playerObjects[j + 1];
                    playerObjects[j + 1] = playerObjects[j];
                    playerObjects[j] = temp;
                }
            }
        }

        for (int i = 0; i < PlayersIDs.Count; i++)
        {
            Debug.Log(playerObjects[i].id);
        }

        ActivePlayersInRoom = PlayersIDs.Count;

        if (PlayersIDs.Count == 2)
        {
            if (SecondPlayerOnDiagonal)
            {
                Players[1].SetActive(false);
                Players[3].SetActive(false);
                PlayerScoresText[1].SetActive(false);
                PlayerScoresText[3].SetActive(false);
                ActivePlayers = new GameObject[2];
                ActivePlayers[0] = Players[0];
                ActivePlayers[1] = Players[2];

                // LUDO
                for (int i = 0; i < PlayersPawns[1].objectsArray.Length; i++)
                {
                    PlayersPawns[1].objectsArray[i].SetActive(false);
                }


                for (int i = 0; i < PlayersPawns[3].objectsArray.Length; i++)
                {
                    PlayersPawns[3].objectsArray[i].SetActive(false);
                }

                // END LUDO
            }
            else
            {

                // LUDO
                for (int i = 0; i < PlayersPawns[21].objectsArray.Length; i++)
                {
                    PlayersPawns[2].objectsArray[i].SetActive(false);
                }

                for (int i = 0; i < PlayersPawns[3].objectsArray.Length; i++)
                {
                    PlayersPawns[3].objectsArray[i].SetActive(false);
                }

                // END LUDO
                Players[2].SetActive(false);
                Players[3].SetActive(false);
                PlayerScoresText[2].SetActive(false);
                PlayerScoresText[3].SetActive(false);
                ActivePlayers = new GameObject[2];
                ActivePlayers[0] = Players[0];
                ActivePlayers[1] = Players[1];
            }
        }
        else
        {
            ActivePlayers = Players;
        }



        int startPos = 0;
        for (int i = 0; i < playerObjects.Count; i++)
        {
            if (playerObjects[i].id == GameManager.Instance.playfabManager.PlayFabId)
            {
                startPos = i;
                break;
            }
        }
        int index = 0;
        bool addedMe = false;
        myIndex = startPos;
        GameManager.Instance.myPlayerIndex = myIndex;

        for (int i = startPos; ;)
        {
            if (i == startPos && addedMe) break;

            if (PlayersIDs.Count == 2 && SecondPlayerOnDiagonal)
            {
                if (addedMe)
                {
                    playerObjects[i].timer = PlayersTimers[2];

                    string id = playerObjects[i].id;
                    PlayersAvatarsButton[2].GetComponent<Button>().onClick.RemoveAllListeners();
                    PlayersAvatarsButton[2].GetComponent<Button>().onClick.AddListener(() => ButtonClick(id));

                    // LUDO
                    playerObjects[i].dice = PlayersDices[2];
                    playerObjects[i].pawns = PlayersPawns[2].objectsArray;

                    for (int k = 0; k < playerObjects[i].pawns.Length; k++)
                    {
                        playerObjects[i].pawns[k].GetComponent<LudoPawnController>().setPlayerIndex(i);
                    }
                    playerObjects[i].homeLockObjects = HomeLockObjects[2];

                    // END LUDO
                }
                else
                {
                    GameManager.Instance.myPlayerIndex = i;
                    playerObjects[i].timer = PlayersTimers[index];

                    string id = playerObjects[i].id;

                    // LUDO
                    playerObjects[i].dice = PlayersDices[index];
                    playerObjects[i].scoreText = PlayerScores[index];
                    playerObjects[i].pawns = PlayersPawns[index].objectsArray;

                    for (int k = 0; k < playerObjects[i].pawns.Length; k++)
                    {
                        playerObjects[i].pawns[k].GetComponent<LudoPawnController>().setPlayerIndex(i);
                    }
                    playerObjects[i].homeLockObjects = HomeLockObjects[index];
                    // END LUDO
                }
            }
            else
            {

                playerObjects[i].timer = PlayersTimers[index];


                // LUDO
                playerObjects[i].dice = PlayersDices[index];
                playerObjects[i].pawns = PlayersPawns[index].objectsArray;

                for (int k = 0; k < playerObjects[i].pawns.Length; k++)
                {
                    playerObjects[i].pawns[k].GetComponent<LudoPawnController>().setPlayerIndex(i);
                }
                playerObjects[i].homeLockObjects = HomeLockObjects[index];
                // END LUDO

                string id = playerObjects[i].id;
                if (index != 0)
                {
                    PlayersAvatarsButton[index].GetComponent<Button>().onClick.RemoveAllListeners();
                    PlayersAvatarsButton[index].GetComponent<Button>().onClick.AddListener(() => ButtonClick(id));
                }

            }



            playerObjects[i].AvatarObject = ActivePlayers[index];
            ActivePlayers[index].GetComponent<PlayerAvatarController>().Name.GetComponent<Text>().text = playerObjects[i].name;
            ActivePlayers[index].GetComponent<PlayerAvatarController>().playIds = playerObjects[i].id;
           
            if (playerObjects[i].avatar != null)
            {
                ActivePlayers[index].GetComponent<PlayerAvatarController>().Avatar.GetComponent<Image>().sprite = playerObjects[i].avatar;
            }

            index++;

            if (i < PlayersIDs.Count - 1)
            {
                i++;
            }
            else
            {
                i = 0;
            }

            addedMe = true;
        }

        currentPlayerIndex = GameManager.Instance.firstPlayerInGame;
        GameManager.Instance.currentPlayer = playerObjects[currentPlayerIndex];

        GameManager.Instance.playerObjects = playerObjects;

        for (int i = 0; i < playerObjects.Count; i++)
        {
            for (int j = 0; j < ActivePlayers.Length; j++)
            {
                if (playerObjects[i].id.Equals(ActivePlayers[j].GetComponent<PlayerAvatarController>().playIds))
                {
                    ActivePlayers[j].GetComponent<PlayerAvatarController>().moverPositions.text = moverNames[i];

                }
            }
            
        }
       

        // CheckPlayersIfShouldFinishGame();

        // Set prizes. 
        // For 2 players 1st get 2x payout and 2nd 0. 
        // For 4 players 1st get 4x payout and 2nd gets payout
        StartCoroutine(ShowPool());

        if (ActivePlayersInRoom == 2)
        {
            StartCoroutine(ShowPool());         
        }
        else if (ActivePlayersInRoom == 4)
        {
            StartCoroutine(ShowPool());            
        }     

        // LUDO

        // Enable home locks

        if (GameManager.Instance.mode == MyGameMode.Master)
        {
            for (int i = 0; i < GameManager.Instance.playerObjects.Count; i++)
            {
                GameManager.Instance.playerObjects[i].homeLockObjects.SetActive(false);
            }
            GameManager.Instance.needToKillOpponentToEnterHome = true;
        }
        else
        {
            GameManager.Instance.needToKillOpponentToEnterHome = false;
        }       

        // END LUDO

        for (int i = 0; i < playerObjects.Count; i++)
        {
            if (playerObjects[i].id.Contains("_BOT"))
            {
                GameManager.Instance.readyPlayersCount++;
            }
        }

        GameManager.Instance.playerObjects = playerObjects;

        // Check if all players are still in room - if not deactivate
        for (int i = 0; i < playerObjects.Count; i++)
        {
            bool contains = false;
            if (!playerObjects[i].id.Contains("_BOT"))
            {
                for (int j = 0; j < PhotonNetwork.playerList.Length; j++)
                {
                    if (PhotonNetwork.playerList[j].NickName.Equals(playerObjects[i].id))
                    {
                        contains = true;
                        break;
                    }
                }

                if (!contains)
                {
                    GameManager.Instance.readyPlayersCount++;
                    Debug.Log("Ready players: " + GameManager.Instance.readyPlayersCount);
                    setPlayerDisconnected(i);
                }
            }
        }

        CheckPlayersIfShouldFinishGame();

        StartCoroutine(waitForPlayersToStart());


        int value = PlayerPrefs.GetInt(StaticString.SoundsKey);
        if (value == 0)
        {
            PlayerPrefs.SetInt(StaticString.SoundsKey, 1);
            AudioListener.volume = 1;
        }
        else if (value == 1)
        {
            PlayerPrefs.SetInt(StaticString.SoundsKey, 1);
            AudioListener.volume = 1;
        }
    }

    private IEnumerator waitForPlayersToStart()
    {
        Debug.Log("Waiting for players " + GameManager.Instance.readyPlayersCount + " - " + requiredToStart);

        yield return new WaitForSeconds(0.1f);


        if (GameManager.Instance.readyPlayersCount < requiredToStart)
        {
            StartCoroutine(waitForPlayersToStart());
        }
        else if (GameManager.Instance.allReadyPlayer)
        {
            AllPlayersReady = true;
            SetTurn();

            // if (myIndex == 0)
            // {
            //     SetMyTurn();
            //     playerObjects[0].dice.GetComponent<GameDiceController>().DisableDiceShadow();
            // }
            // else
            // {
            //     SetOpponentTurn();
            //     playerObjects[currentPlayerIndex].dice.GetComponent<GameDiceController>().DisableDiceShadow();
            // }

        }
    }

    public int GetCurrentPlayerIndex()
    {
        return currentPlayerIndex;
    }


    private void ButtonClick(string id)
    {

        int index = 0;

        for (int i = 0; i < playerObjects.Count; i++)
        {
            if (playerObjects[i].id == id)
            {
                index = i;
                break;
            }
        }

        CurrentPlayerID = id;

        if (playerObjects[index].AvatarObject.GetComponent<PlayerAvatarController>().Active)
        {
            PlayerInfoWindow.GetComponent<PlayerInfromation>().ShowPlayerInfo(playerObjects[index].avatar, playerObjects[index].name, playerObjects[index].data);
        }

    }

    public void FinishedGame()
    {
        if (GameManager.Instance.currentPlayer.id == PhotonNetwork.player.NickName)
        {
            SetFinishGame(GameManager.Instance.currentPlayer.id, true);
            Debug.Log("Finishid to Auto Play + true" );
        }
        else
        {
            SetFinishGame(GameManager.Instance.currentPlayer.id, false);
        }
    }

    private void SetFinishGame(string id, bool me)
    {
        if (!me || !iFinished)
        {
            Debug.Log("SET FINISH");
            ActivePlayersInRoom--;         

            int index = GetPlayerPosition(id);          

            playersFinished.Add(playerObjects[index]);

            PlayerAvatarController controller = playerObjects[index].AvatarObject.GetComponent<PlayerAvatarController>();
            controller.Name.GetComponent<Text>().text = "";
            controller.Active = false;
            controller.finished = true;

            playerObjects[index].dice.SetActive(false);

           // int position = playersFinished.Count;
            
            if (me)
            {
                CheckWinners();
                DisplayScore();

                int position = GameManager.Instance.ranks;
                
                PhotonNetwork.BackgroundTimeout = StaticString.photonDisconnectTimeoutLong;
                iFinished = true;
                if (ActivePlayersInRoom >= 0)
                {
                    PhotonNetwork.RaiseEvent((int)EnumPhoton.FinishedGame, PhotonNetwork.player.NickName, true, null);
                    Debug.Log("set finish call finish turn");
                    SendFinishTurn();
                }                
                if (GameManager.Instance.type == MyGameType.TwoPlayer)
                {                     
                    if (position == 0)
                    {
                        WinSound.Play();
                        Dictionary<string, string> data = new Dictionary<string, string>();
                        data.Add(MyPlayerData.TotalPlayerWinsKey, (GameManager.Instance.myPlayerData.GetTotalPlayerWinsKey() + 1).ToString());
                        data.Add(MyPlayerData.TwoPlayerWinsKey, (GameManager.Instance.myPlayerData.GetTwoPlayerWins() + 1).ToString());
                        GameManager.Instance.myPlayerData.UpdateUserData(data);

                        rankss = 1;
                        StartCoroutine(Checkwinner());                 
                    }
                    else if (position == 1)
                    {
                        rankss = 2;
                        StartCoroutine(Checkwinner());
                    }
                }
                else if(GameManager.Instance.type == MyGameType.FourPlayer)
                {
                    if (position == 0)
                    {
                        WinSound.Play();

                        Dictionary<string, string> data = new Dictionary<string, string>();
                        data.Add(MyPlayerData.TotalPlayerWinsKey, (GameManager.Instance.myPlayerData.GetTotalPlayerWinsKey() + 1).ToString());
                        data.Add(MyPlayerData.FourPlayerWinsKey, (GameManager.Instance.myPlayerData.GetFourPlayerWins() + 1).ToString());
                        GameManager.Instance.myPlayerData.UpdateUserData(data);

                        rankss = 1;
                        StartCoroutine(Checkwinner());
                        Debug.Log("wincanvas");
                    }
                    else if (position == 1)
                    {
                        rankss = 2;
                        StartCoroutine(Checkwinner());                       
                    }
                    else if (position == 2)
                    {
                        rankss = 3;
                        StartCoroutine(Checkwinner());                   
                    }
                    else if (position == 3)
                    {
                        rankss = 4;
                        StartCoroutine(Checkwinner());                       
                    }

                    
                }
            }
            else if (GameManager.Instance.currentPlayer.isBot)
            {
                SendFinishTurn();
            }      
            CheckPlayersIfShouldFinishGame();
        }
    }

    public int GetPlayerPosition(string id)
    {
        for (int i = 0; i < playerObjects.Count; i++)
        {
            if (playerObjects[i].id.Equals(id))
            {
                return i;
            }
        }
        return -1;
    }

    public void SendFinishTurn()
    {
        if (!FinishWindowActive && ActivePlayersInRoom > 1)
        {
            if (GameManager.Instance.currentPlayer.isBot)
            {
                BotDelay();
            }
            else
            {
                PhotonNetwork.RaiseEvent((int)EnumPhoton.NextPlayerTurn, myIndex, true, null);

                Debug.Log("PLAYER BEFORE: " + currentPlayerIndex);
                
                setCurrentPlayerIndex(myIndex);

                Debug.Log("PLAYER AFTER: " + currentPlayerIndex + " isbot: " + GameManager.Instance.currentPlayer.isBot);

                SetTurn();

                GameManager.Instance.miniGame.setOpponentTurn();
            }
        }
    }


    void Awake()
    {
        PhotonNetwork.OnEventCall += this.OnEvent;
    }


    void OnDestroy()
    {
        PhotonNetwork.OnEventCall -= this.OnEvent;
    }

    private void OnEvent(byte eventcode, object content, int senderid)
    {
        Debug.Log("received event: " + eventcode);
        if (eventcode == (int)EnumPhoton.NextPlayerTurn)
        {
            if (playerObjects[(int)content].AvatarObject.GetComponent<PlayerAvatarController>().Active &&
                currentPlayerIndex == (int)content)
            {
                if (!FinishWindowActive)
                {
                    setCurrentPlayerIndex((int)content);

                    SetTurn();
                }
            }
        }
        else if (eventcode == (int)EnumPhoton.FinishedGame)
        {
            string message = (string)content;
            SetFinishGame(message, false);
        }
    }

    private void SetMyTurn()
    {
        GameManager.Instance.isMyTurn = true;

        if (GameManager.Instance.miniGame != null)
            GameManager.Instance.miniGame.setMyTurn();


        StartTimer();
    }

    private void BotTurn()
    {
        oppoTurnSource.Play();

        GameManager.Instance.isMyTurn = false;
        Debug.Log("Bot Turn");
        StartTimer();

        GameManager.Instance.miniGame.BotTurn(true);

    }

    private void SetTurn()
    {
        if (!GameManager.Instance.LinkFbAccount)
        {
            gameStarts.Play();
            GameManager.Instance.LinkFbAccount = true;
            GameTimeCountDown.Instance.CurrentTime();
        }

        Debug.Log("SET TURN CALLED");
        for (int i = 0; i < playerObjects.Count; i++)
        {
            playerObjects[i].dice.GetComponent<GameDiceController>().EnableDiceShadow();
        }

        playerObjects[currentPlayerIndex].dice.GetComponent<GameDiceController>().DisableDiceShadow();

        GameManager.Instance.currentPlayer = playerObjects[currentPlayerIndex];

        if (playerObjects[currentPlayerIndex].id == myId)
        {
            if (UpdatePlayerTimer.Instance.autoPlayLife != 0)
            {
                SetMyTurn();
            }
            else
            {
                FinishWindowActive = true;
            }
        }
        else if (playerObjects[currentPlayerIndex].isBot)
        {
            BotTurn();
        }
        else
        {
            SetOpponentTurn();
        }
    }

    private void BotDelay()
    {
        if (!FinishWindowActive)
        {
            setCurrentPlayerIndex(currentPlayerIndex);
            SetTurn();
        }

    }

    private void setCurrentPlayerIndex(int current)
    {

        while (true)
        {
            current = current + 1;
            currentPlayerIndex = (current) % playerObjects.Count;
            GameManager.Instance.currentPlayer = playerObjects[currentPlayerIndex];
            if (playerObjects[currentPlayerIndex].AvatarObject.GetComponent<PlayerAvatarController>().Active) break;
        }

    }

    private void SetOpponentTurn()
    {
        Debug.Log("Opponent turn");
        oppoTurnSource.Play();
        GameManager.Instance.isMyTurn = false;

        StartTimer();
    }

    private void StartTimer()
    {
        for (int i = 0; i < playerObjects.Count; i++)
        {
            if (i == currentPlayerIndex)
            {
                playerObjects[currentPlayerIndex].timer.SetActive(true);
            }
            else
            {
                playerObjects[i].timer.SetActive(false);
            }
        }
    }

    public void StopTimers()
    {
        for (int i = 0; i < playerObjects.Count; i++)
        {
            playerObjects[i].timer.SetActive(false);
        }
    }

    public void PauseTimers()
    {
        playerObjects[currentPlayerIndex].timer.GetComponent<UpdatePlayerTimer>().Pause();
    }

    public void restartTimer()
    {
        playerObjects[currentPlayerIndex].timer.GetComponent<UpdatePlayerTimer>().restartTimer();
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        Debug.Log("Player disconnected: " + otherPlayer.NickName);

        for (int i = 0; i < playerObjects.Count; i++)
        {
            if (playerObjects[i].id.Equals(otherPlayer.NickName))
            {
                setPlayerDisconnected(i);
                break;
            }
        }

        CheckPlayersIfShouldFinishGame();
    }

    public override void OnLeftRoom()
    {

    }
    public void CheckPlayersIfShouldFinishGame()
    {
        if (!FinishWindowActive)
        {
            if ((ActivePlayersInRoom == 1 && !iFinished))
            {
                StopAndFinishGame();
                return;
            }

            if (ActivePlayersInRoom == 0)
            {
                StopAndFinishGame();
                return;
            }

            if (iFinished && ActivePlayersInRoom == 1 && CheckIfOtherPlayerIsBot())
            {
                AddBotToListOfWinners();
                StopAndFinishGame();
                return;
            }

            if (ActivePlayersInRoom > 1 && iFinished)
            {
                //TIPButtonObject.SetActive(true);
            }
        }
    }

    public void StopAndFinishGame()
    {
        StopTimers();
        SetFinishGame(PhotonNetwork.player.NickName, true);
        ShowGameFinishWindow();
    }

    public void ShowGameFinishWindow()
    {
        if (!FinishWindowActive)
        {

            FinishWindowActive = true;

            List<PlayerObject> otherPlayers = new List<PlayerObject>();

            for (int i = 0; i < playerObjects.Count; i++)
            {
                PlayerAvatarController controller = playerObjects[i].AvatarObject.GetComponent<PlayerAvatarController>();
                if (controller.Active && !controller.finished)
                {
                    otherPlayers.Add(playerObjects[i]);
                }

                for (int j = 0; j < playersFinished.Count; j++)
                {

                    if (playerObjects[i].Equals(scoreTests[j].playername))
                    {
                        Debug.Log(" i : " + i + " " + " j ; " + j);
                    }
                }
               
            }
            GameFinishWindow.GetComponent<GameFinishWindowController>().showWindow(playersFinished, otherPlayers, firstPlacePrize, secondPlacePrize, threePlacePrize, fourPlacePrize);
        }
    }

    public void AddBotToListOfWinners()
    {
        for (int i = 0; i < playerObjects.Count; i++)
        {
            if (playerObjects[i].id.Contains("_BOT") && playerObjects[i].AvatarObject.GetComponent<PlayerAvatarController>().Active)
            {
                playersFinished.Add(playerObjects[i]);
            }
        }
    }

    public bool CheckIfOtherPlayerIsBot()
    {
        for (int i = 0; i < playerObjects.Count; i++)
        {
            if (playerObjects[i].id.Contains("_BOT") && playerObjects[i].AvatarObject.GetComponent<PlayerAvatarController>().Active)
            {
                playerObjects[i].AvatarObject.GetComponent<PlayerAvatarController>().finished = true;
                return true;
            }
        }
        return false;
    }

    public void setPlayerDisconnected(int i)
    {
        requiredToStart--;
        if (!FinishWindowActive)
        {
            if (!playerObjects[i].AvatarObject.GetComponent<PlayerAvatarController>().finished)
                ActivePlayersInRoom--;

            Debug.Log("Active players: " + ActivePlayersInRoom);
            if (currentPlayerIndex == i && ActivePlayersInRoom > 1)
            {

                setCurrentPlayerIndex(currentPlayerIndex);
                if (AllPlayersReady)
                    SetTurn();
            }

            Debug.Log("finsish.....");
            playerObjects[i].AvatarObject.GetComponent<PlayerAvatarController>().PlayerLeftRoom();

            // LUDO
            playerObjects[i].dice.SetActive(false);
            if (!playerObjects[i].AvatarObject.GetComponent<PlayerAvatarController>().finished)
            {
                for (int j = 0; j < playerObjects[i].pawns.Length; j++)
                {
                    //playerObjects[i].pawns[j].SetActive(false);
                    playerObjects[i].pawns[j].GetComponent<LudoPawnController>().GoToInitPosition(false);
                }
            }
            // END LUDO
        }
    }

    public void LeaveGame(bool finishWindow)
    {
        if (!iFinished || finishWindow)
        {
            PlayerPrefs.SetInt("GamesPlayed", PlayerPrefs.GetInt("GamesPlayed", 1) + 1);
            SceneManager.LoadScene("Menu");
            PhotonNetwork.BackgroundTimeout = StaticString.photonDisconnectTimeoutLong;
            PlayerPrefs.SetInt("TotalGamecount", 1);
            PlayerPrefs.Save();

            //StartCoroutine(SesndGameplayedData());

            //GameManager.Instance.cueController.removeOnEventCall();
            PhotonNetwork.LeaveRoom();

            GameManager.Instance.playfabManager.roomOwner = false;
            GameManager.Instance.roomOwner = false;
            GameManager.Instance.resetAllData();

        }
        else
        {
            ShowGameFinishWindow();
        }
    }

    IEnumerator Checkwinner()
    {
        form = new WWWForm();
        form.AddField("userid", PlayerPrefs.GetString("userid"));
        form.AddField("eventid", GameManager.Instance.eventIDs);
        form.AddField("rank", rankss);

        WWW w = new WWW(StaticString.winnerprizeurl, form);
        yield return w;

        if (w.error != null)
        {

            Debug.Log("<color=red>" + w.text + "</color>");//error
        }
        else
        {
            if (w.isDone)
            {
                if (w.text.Contains("error"))
                {
                    Debug.Log("<color=red>" + w.text + "</color>");//error
                }
                else
                {

                    Debug.Log("<color=green>" + w.text + "</color>");//user exist

                }
            }
        }

        w.Dispose();
    }

    IEnumerator Sendwinnergamedata(string gametype)
    {
        form = new WWWForm();
        form.AddField("id", PlayerPrefs.GetString("userid"));
        form.AddField("status", "win");
        form.AddField("win", PlayerPrefs.GetString("winningamount"));
        form.AddField("bet", PlayerPrefs.GetString("bidamount"));
        Debug.Log(PlayerPrefs.GetString("bidamount"));


        using (UnityWebRequest www = UnityWebRequest.Post(StaticString.gameEnd + PlayerPrefs.GetString("userid"), form))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                print(www.error);
            }
            else
            {
                if (www.isDone)
                {

                    Debug.Log("Checking sendwinnerdata" + www.downloadHandler.text);

                }
            }
        }

    }

    IEnumerator Sendlosergamedata(string gametype)
    {
        form = new WWWForm();
        form.AddField("id", PlayerPrefs.GetString("userid"));
        form.AddField("status", "loose");
        form.AddField("win", 0);
        form.AddField("bet", PlayerPrefs.GetString("bidamount"));
        //form.AddField("gametype", gametype);
        Debug.Log(PlayerPrefs.GetString("bidamount"));

        using (UnityWebRequest www = UnityWebRequest.Post(StaticString.gameEnd + PlayerPrefs.GetString("userid"), form))
        {
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                print(www.error);
            }
            else
            {
                if (www.isDone)
                {
                    Debug.Log("Checking sendloserdata" + www.downloadHandler.text);
                }
            }
        }

    }  


    public void CheckWinners()
    {
        if (GameManager.Instance.type == MyGameType.TwoPlayer)
        {
            scoreTests.Add(new ScoreTest(int.Parse(ActivePlayers[0].GetComponent<PlayerAvatarController>().scoreTexts.text), ActivePlayers[0], ActivePlayers[0].GetComponent<PlayerAvatarController>().playIds));
            scoreTests.Add(new ScoreTest(int.Parse(ActivePlayers[1].GetComponent<PlayerAvatarController>().scoreTexts.text), ActivePlayers[1], ActivePlayers[1].GetComponent<PlayerAvatarController>().playIds));
        }
        else if (GameManager.Instance.type == MyGameType.FourPlayer)
        {
            scoreTests.Add(new ScoreTest(int.Parse(ActivePlayers[0].GetComponent<PlayerAvatarController>().scoreTexts.text), ActivePlayers[0], ActivePlayers[0].GetComponent<PlayerAvatarController>().playIds));
            scoreTests.Add(new ScoreTest(int.Parse(ActivePlayers[1].GetComponent<PlayerAvatarController>().scoreTexts.text), ActivePlayers[1], ActivePlayers[1].GetComponent<PlayerAvatarController>().playIds));
            scoreTests.Add(new ScoreTest(int.Parse(ActivePlayers[2].GetComponent<PlayerAvatarController>().scoreTexts.text), ActivePlayers[2], ActivePlayers[2].GetComponent<PlayerAvatarController>().playIds));
            scoreTests.Add(new ScoreTest(int.Parse(ActivePlayers[3].GetComponent<PlayerAvatarController>().scoreTexts.text), ActivePlayers[3], ActivePlayers[3].GetComponent<PlayerAvatarController>().playIds));
        }
        DataSorts();
    }
    void DataSorts()
    {
        scoreTests.Sort(sortFunc);
    }

    public int sortFunc(ScoreTest a, ScoreTest b)
    {
        if (a.playerScoreText > b.playerScoreText)
        {
            return -1;
        }
        else if (a.playerScoreText < b.playerScoreText)
        {
            return 1;
        }
        return 0;
    }

    public void DisplayScore()
    {
       
        for (int j = 0; j < scoreTests.Count; j++)
        {
            if (scoreTests[j].playFabID.Equals(GameManager.Instance.playfabManager.PlayFabId))
            {
                GameManager.Instance.ranks = j;              
            }
        }
    }

    IEnumerator ShowPool()
    {
        form = new WWWForm();
        form.AddField("id", GameManager.Instance.eventIDs);

        WWW w = new WWW(StaticString.prizePools, form);

        yield return w;

        if (w.error != null)
        {
            Debug.Log("<color=red>" + w.text + "</color>");//error
        }
        else
        {
            if (w.isDone)
            {
                Response<RankPool[]> response = JsonUtility.FromJson<Response<RankPool[]>>(w.text);
                Debug.Log("Check " + w.text);

                if (response.status)
                {
                    RankPool[] rankPools = response.data;
                    firstPlacePrize = int.Parse(rankPools[0].win_amount);
                    secondPlacePrize = int.Parse(rankPools[1].win_amount);
                    threePlacePrize = int.Parse(rankPools[2].win_amount);
                    fourPlacePrize = int.Parse(rankPools[3].win_amount);
                }
            }
        }
    }
}