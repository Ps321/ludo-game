using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

public class PlayFabManager : Photon.PunBehaviour
{

    private Sprite[] avatarSprites;

    public string PlayFabId;
    public string authToken;
    public bool multiGame = true;
    public bool roomOwner = false;

    public bool opponentReady = false;
    public bool imReady = false;

    public bool isInLobby = false;
    public bool isInMaster = false;
    public string versionName = "1.1";
    public bool bots = false;
    
    private WWWForm form;


    #region Unity Method

    void Awake()
    {
        Debug.Log("Playfab awake");
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        PhotonNetwork.PhotonServerSettings.HostType = ServerSettings.HostingOption.PhotonCloud;
        PhotonNetwork.PhotonServerSettings.PreferredRegion = CloudRegionCode.eu;
        PhotonNetwork.ConnectUsingSettings(versionName);

#if UNITY_IOS
        PhotonNetwork.PhotonServerSettings.Protocol = ConnectionProtocol.Tcp;
#else
        PhotonNetwork.PhotonServerSettings.Protocol = ConnectionProtocol.Udp;
#endif
        Debug.Log("PORT: " + PhotonNetwork.PhotonServerSettings.ServerPort);

        PlayFabSettings.TitleId = StaticString.PlayFabTitleID;

        PhotonNetwork.OnEventCall += this.OnEvent;
        DontDestroyOnLoad(transform.gameObject);
    }
    // Use this for initialization
    void Start()
    {
        Debug.Log("Playfab start");
        PhotonNetwork.BackgroundTimeout = StaticString.photonDisconnectTimeoutLong; ;
        GameManager.Instance.playfabManager = this;
        avatarSprites = GameObject.Find("StaticGameVariablesContainer").GetComponent<StaticGameVariablesController>().avatars;
    }

    void OnDestroy()
    {
        PhotonNetwork.OnEventCall -= this.OnEvent;
    }

    #endregion

    #region Public Method
    public void destroy()
    {
        if (this.gameObject != null)
            DestroyImmediate(this.gameObject);
    }

    public void StartGame()
    {
        PhotonNetwork.room.IsOpen = false;
        PhotonNetwork.room.IsVisible = false;

        CancelInvoke("StartGameWithBots");
        Invoke("startGameScene", 3.0f);

    }
    public void startGameScene()
    {
        if (GameManager.Instance.currentPlayersCount == GameManager.Instance.requiredPlayers)
        {

            LoadGameScene();
            PhotonNetwork.RaiseEvent((int)EnumPhoton.StartGame, null, true, null);          

        }
        else
        {
            if (PhotonNetwork.isMasterClient)
                WaitForNewPlayer();
        }
    }

    public void LoadGameScene()
    {     
        if (!GameManager.Instance.gameSceneStarted)
        {
            SceneManager.LoadScene("StartGameScene");
            GameManager.Instance.gameSceneStarted = true;
        }

    }

    public void WaitForNewPlayer()
    {
        if (PhotonNetwork.isMasterClient && GameManager.Instance.type != MyGameType.Private && GameManager.Instance.type != MyGameType.FourPlayer)
        {
            Debug.Log("START INVOKE");
            CancelInvoke("StartGameWithBots");
            Invoke("StartGameWithBots", 0.0f);
        }
    }

    public void LoadGameWithDelay()
    {
        LoadGameScene();
    }

    #endregion

    #region Private Method
    private string androidUnique()
    {
        AndroidJavaClass androidUnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityPlayerActivity = androidUnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject unityPlayerResolver = unityPlayerActivity.Call<AndroidJavaObject>("getContentResolver");
        AndroidJavaClass androidSettingsSecure = new AndroidJavaClass("android.provider.Settings$Secure");
        return androidSettingsSecure.CallStatic<string>("getString", unityPlayerResolver, "android_id");
    }



    #endregion

    #region PlayFab
    public void Login()
    {
        string customId = "";
        if (PlayerPrefs.HasKey("unique_identifier"))
        {
            customId = PlayerPrefs.GetString("unique_identifier");
        }
        else
        {
            customId = System.Guid.NewGuid().ToString();
            PlayerPrefs.SetString("unique_identifier", customId);
        }

        Debug.Log("UNIQUE IDENTIFIER: " + customId);

        LoginWithCustomIDRequest request = new LoginWithCustomIDRequest()
        {
            TitleId = PlayFabSettings.TitleId,
            CreateAccount = true,
            CustomId = customId //SystemInfo.deviceUniqueIdentifier
        };

        PlayFabClientAPI.LoginWithCustomID(request, (result) =>
        {
            PlayFabId = result.PlayFabId;
            Debug.Log("Got PlayFabID: " + PlayFabId);           

            Dictionary<string, string> data = new Dictionary<string, string>();
            if (result.NewlyCreated)
            {
                string name = result.PlayFabId;
                name = GameManager.Instance.userName;
                Debug.Log("(new account)");
                setInitNewAccountData();

            }
            else
            {
                CheckIfFirstTitleLogin(PlayFabId);
                Debug.Log("(existing account)");
            }
            data.Add("PlayerName", PlayerPrefs.GetString("username"));
            data.Add("AvatarIndex", PlayerPrefs.GetInt("ImageIndex").ToString());
            data.Add("LoggedType", "Guest");
            UpdateUserTitleDisplayNameRequest displayNameRequest = new UpdateUserTitleDisplayNameRequest()
            {
                DisplayName = GameManager.Instance.playfabManager.PlayFabId
            };

            PlayFabClientAPI.UpdateUserTitleDisplayName(displayNameRequest, (response) =>
            {
                Debug.Log("Title Display name updated successfully");
            }, (error) =>
            {
                Debug.Log("Title Display name updated error: " + error.Error);

            }, null);

            GameManager.Instance.myPlayerData.UpdateUserData(data);
            GameManager.Instance.nameMy = name;
            PlayerPrefs.SetString("LoggedType", "Guest");
            PlayerPrefs.Save();
            GetPhotonToken();


        },
            (error) =>
            {
                Debug.Log("Error logging in player with custom ID:");
                Debug.Log(error.ErrorMessage);
                GameManager.Instance.connectionLost.showDialog();
            });
    }

    public void setInitNewAccountData()
    {
        Dictionary<string, string> data = MyPlayerData.InitialUserData();
        GameManager.Instance.myPlayerData.UpdateUserData(data);
    }

    public void CheckIfFirstTitleLogin(string id)
    {
        GetUserDataRequest getdatarequest = new GetUserDataRequest()
        {
            PlayFabId = id,

        };

        PlayFabClientAPI.GetUserData(getdatarequest, (result) =>
        {
            Dictionary<string, UserDataRecord> data = result.Data;

            if (!data.ContainsKey(MyPlayerData.TitleFirstLoginKey))
            {
                Debug.Log("First login for this title. Set initial data");
                setInitNewAccountData();
            }

        }, (error) =>
        {
            Debug.Log("Data updated error " + error.ErrorMessage);
        }, null);
    }

    void GetPhotonToken()
    {

        GetPhotonAuthenticationTokenRequest request = new GetPhotonAuthenticationTokenRequest();
        request.PhotonApplicationId = StaticString.PhotonAppID.Trim();

        PlayFabClientAPI.GetPhotonAuthenticationToken(request, OnPhotonAuthenticationSuccess, OnPlayFabError);
    }

    void OnPhotonAuthenticationSuccess(GetPhotonAuthenticationTokenResult result)
    {
        string photonToken = result.PhotonCustomAuthenticationToken;
        Debug.Log(string.Format("Yay, logged in session token: {0}", photonToken));
        PhotonNetwork.AuthValues = new AuthenticationValues();
        PhotonNetwork.AuthValues.AuthType = CustomAuthenticationType.Custom;
        PhotonNetwork.AuthValues.AddAuthParameter("username", this.PlayFabId);
        PhotonNetwork.AuthValues.AddAuthParameter("Token", result.PhotonCustomAuthenticationToken);
        PhotonNetwork.AuthValues.UserId = this.PlayFabId;
        PhotonNetwork.playerName = this.PlayFabId;
        authToken = result.PhotonCustomAuthenticationToken;
        getPlayerDataRequest();

    }

    public void getPlayerDataRequest()
    {
        Debug.Log("Get player data request!!");
        GetUserDataRequest getdatarequest = new GetUserDataRequest()
        {
            PlayFabId = GameManager.Instance.playfabManager.PlayFabId,
        };

        PlayFabClientAPI.GetUserData(getdatarequest, (result) =>
        {

            Dictionary<string, UserDataRecord> data = result.Data;

            GameManager.Instance.myPlayerData = new MyPlayerData(data, true);


            Debug.Log("Get player data request finish!!");
            StartCoroutine(loadSceneMenu());
        }, (error) =>
        {
            Debug.Log("Data updated error " + error.ErrorMessage);
        }, null);
    }

    private IEnumerator loadSceneMenu()
    {
        yield return new WaitForSeconds(0.1f);

        if (isInMaster && isInLobby)
        {
            SceneManager.LoadScene("Menu");
        }
        else
        {
            StartCoroutine(loadSceneMenu());
        }

    }

    void OnPlayFabError(PlayFabError error)
    {
        Debug.Log("Playfab Error: " + error.ErrorMessage);
    }

    #endregion

    #region Photon 

    public override void OnConnectedToMaster()
    {
        isInMaster = true;
        Debug.Log("Connected to master");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined lobby");
        isInLobby = true;
        Debug.Log("PhotonNetwork.insideLobby :- " + PhotonNetwork.countOfPlayers);
    }
    public override void OnCreatedRoom()
    {
        Debug.Log("OnCreatedRoom");
        roomOwner = true;
        GameManager.Instance.roomOwner = true;
        GameManager.Instance.currentPlayersCount = 1;
    }

    public override void OnPhotonCustomRoomPropertiesChanged(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {

        Debug.Log("Custom properties changed: " + DateTime.Now.ToString());
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer player)
    {
        GameManager.Instance.opponentDisconnected = true;

        GameManager.Instance.invitationID = "";

        if (GameManager.Instance.controlAvatars != null)
        {
            Debug.Log("PLAYER DISCONNECTED " + player.NickName);
            if (PhotonNetwork.room.PlayerCount > 1)
            {
                GameManager.Instance.controlAvatars.startButtonPrivate.GetComponent<Button>().interactable = true;
            }
            else
            {
                GameManager.Instance.controlAvatars.startButtonPrivate.GetComponent<Button>().interactable = false;
            }


            int index = GameManager.Instance.opponentsIDs.IndexOf(player.NickName);
            GameManager.Instance.controlAvatars.PlayerDisconnected(index);
        }
    }

    #endregion

    #region Events
    private void OnEvent(byte eventcode, object content, int senderid)
    {

        Debug.Log("Received event: " + (int)eventcode + " Sender ID: " + senderid);

        if (eventcode == (int)EnumPhoton.BeginPrivateGame)
        {
            //StartGame();
            LoadGameScene();
        }
        else if (eventcode == (int)EnumPhoton.StartWithBots && senderid != PhotonNetwork.player.ID)
        {
            LoadBots();
        }
        else if (eventcode == (int)EnumPhoton.StartGame)
        {
            //Invoke("LoadGameWithDelay", UnityEngine.Random.Range(1.0f, 5.0f));
            //PhotonNetwork.LeaveRoom();
            LoadGameScene();
        }
        else if (eventcode == (int)EnumPhoton.ReadyToPlay)
        {
            GameManager.Instance.readyPlayersCount++;
            Debug.Log(GameManager.Instance.readyPlayersCount);
            
            //LoadGameScene();
        }

    }


    #endregion

    #region BotAdd

    public void StartGameWithBots()
    {
        if (PhotonNetwork.isMasterClient)
        {
            if (PhotonNetwork.room.PlayerCount < GameManager.Instance.requiredPlayers)
            {
                Debug.Log("Master Client");
                // PhotonNetwork.RaiseEvent((int)EnumPhoton.StartWithBots, null, true, null);
                LoadBots();
            }
        }
        else
        {
            Debug.Log("Not Master client");
        }
    }

    public void LoadBots()
    {
        Debug.Log("Close room - add bots");
        PhotonNetwork.room.IsOpen = false;
        PhotonNetwork.room.IsVisible = false;

        if (PhotonNetwork.isMasterClient)
        {
            //Invoke("AddBots", 0.0f);
            AddBots();
        }
        else
        {
            AddBots();
        }

    }

    public void AddBots()
    {
        // Add Bots here

        Debug.Log("Add Bots with delay");

        if (PhotonNetwork.room.PlayerCount < GameManager.Instance.requiredPlayers)
        {

            if (PhotonNetwork.isMasterClient)
            {
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
    }

    public void AddBot(int i)
    {
        GameManager.Instance.opponentsAvatars[i] = avatarSprites[UnityEngine.Random.Range(0, avatarSprites.Length - 1)];
        GameManager.Instance.opponentsIDs[i] = "_BOT" + i;
        GameManager.Instance.opponentsNames[i] = "Guest" + UnityEngine.Random.Range(100000, 999999);
        Debug.Log("Name: " + GameManager.Instance.opponentsNames[i]);
        GameManager.Instance.controlAvatars.PlayerJoined(i, "_BOT" + i);
    }

    #endregion     

    /*public override void OnMasterClientSwitched(PhotonPlayer newMasterClient)
    {
        if (GameManager.Instance.controlAvatars != null && GameManager.Instance.type == MyGameType.Private)
        {
            PhotonNetwork.LeaveRoom();
            GameManager.Instance.controlAvatars.ShowJoinFailed("Room closed");
        }
        else
        {
            if (newMasterClient.NickName == PhotonNetwork.player.NickName)
            {
                Debug.Log("Im new master client");
                WaitForNewPlayer();
            }
        }

    }*/

    public void JoinRoomAndStartGame()
    {
        if (GameManager.Instance.JoinEvents == true)
        {
            GameManager.Instance.JoinEvents = false;
            ExitGames.Client.Photon.Hashtable expectedCustomRoomProperties = new ExitGames.Client.Photon.Hashtable() {
            {"m", GameManager.Instance.mode.ToString() +  GameManager.Instance.type.ToString() + GameManager.Instance.payoutCoins.ToString() + GameManager.Instance.tournament_id.ToString()}
         };

            Debug.Log("Create Room: " + GameManager.Instance.mode.ToString() + " " + GameManager.Instance.type.ToString() + " " + GameManager.Instance.payoutCoins.ToString() + " " + GameManager.Instance.tournament_id);

            StartCoroutine(TryToJoinRandomRoom(expectedCustomRoomProperties));
        }
    }

    public IEnumerator TryToJoinRandomRoom(ExitGames.Client.Photon.Hashtable roomOptions)
    {
        while (true)
        {
            if (isInLobby && isInMaster)
            {
                PhotonNetwork.JoinRandomRoom(roomOptions, 0);
                break;
            }
            else
            {
                yield return new WaitForSeconds(0.05f);
            }
        }
    }

    public void OnPhotonRandomJoinFailed()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.CustomRoomPropertiesForLobby = new String[] { "m", "v" };

        string BotMoves = generateBotMoves();

        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() {
            { "m", GameManager.Instance.mode.ToString() +  GameManager.Instance.type.ToString() + GameManager.Instance.payoutCoins.ToString() + GameManager.Instance.tournament_id.ToString()},
            {"bt", BotMoves},
            {"fp", UnityEngine.Random.Range(0, GameManager.Instance.requiredPlayers)}
         };

        roomOptions.MaxPlayers = (byte)GameManager.Instance.requiredPlayers;
        //roomOptions.IsVisible = true;
        StartCoroutine(TryToCreateGameAfterFailedToJoinRandom(roomOptions));

    }

    public IEnumerator TryToCreateGameAfterFailedToJoinRandom(RoomOptions roomOptions)
    {
        while (true)
        {
            if (isInLobby && isInMaster)
            {
                string roomName = "";
                for (int i = 0; i < 6; i++)
                {
                    roomName = roomName + UnityEngine.Random.Range(0, 10);
                }
                PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);

                break;
            }
            else
            {
                yield return new WaitForSeconds(0.05f);
            }
        }
    }

    public string generateBotMoves()
    {
        // Generate BOT moves
        string BotMoves = "";
        int BotCount = 100;
        // Generate dice values
        for (int i = 0; i < BotCount; i++)
        {
            BotMoves += (UnityEngine.Random.Range(1, 7)).ToString();
            if (i < BotCount - 1)
            {
                BotMoves += ",";
            }
        }

        BotMoves += ";";

        // Generate delays
        float minValue = GameManager.Instance.playerTime / 10;
        if (minValue < 1.5f) minValue = 1.5f;
        for (int i = 0; i < BotCount; i++)
        {
            BotMoves += (UnityEngine.Random.Range(minValue, GameManager.Instance.playerTime / 8)).ToString();
            if (i < BotCount - 1)
            {
                BotMoves += ",";
            }
        }
        return BotMoves;
    }

    public void extractBotMoves(string data)
    {
        GameManager.Instance.botDiceValues = new List<int>();
        GameManager.Instance.botDelays = new List<float>();
        string[] d1 = data.Split(';');


        string[] diceValues = d1[0].Split(',');
        for (int i = 0; i < diceValues.Length; i++)
        {
            GameManager.Instance.botDiceValues.Add(int.Parse(diceValues[i]));
        }

        string[] delays = d1[1].Split(',');
        for (int i = 0; i < delays.Length; i++)
        {
            GameManager.Instance.botDelays.Add(float.Parse(delays[i]));
        }
    }

    public override void OnLeftLobby()
    {
        isInLobby = false;
        isInMaster = false;
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("OnJoinedRoom");


        if (GameManager.Instance.mode == MyGameMode.Quick)
        {
            if (PhotonNetwork.room.CustomProperties.ContainsKey("bt"))
            {
                extractBotMoves(PhotonNetwork.room.CustomProperties["bt"].ToString());
            }
            if (PhotonNetwork.room.PlayerCount == 1)
            {
                GameManager.Instance.roomOwner = true;
                WaitForNewPlayer();
            }
        }

        GameManager.Instance.addBotPlayers = true;
        GameManager.Instance.avatarOpponent = null;

        Debug.Log("Players in room " + PhotonNetwork.room.PlayerCount);

        GameManager.Instance.currentPlayersCount = 1;

        Debug.Log(GameManager.Instance.requiredPlayers);
        if (PhotonNetwork.room.PlayerCount >= GameManager.Instance.requiredPlayers)
        {

            PhotonNetwork.room.IsOpen = false;
            PhotonNetwork.room.IsVisible = false;
            //StartCoroutine(SendbidAmount());
            //SendPlayerCounttodb();
        }

        if (!roomOwner)
        {
            for (int i = 0; i < PhotonNetwork.otherPlayers.Length; i++)
            {

                int ii = i;
                int index = GetFirstFreeSlot();

                GameManager.Instance.opponentsIDs[index] = PhotonNetwork.otherPlayers[ii].NickName;

                GetUserDataRequest getdatarequest = new GetUserDataRequest()
                {
                    PlayFabId = PhotonNetwork.otherPlayers[ii].NickName,

                };

                string otherID = PhotonNetwork.otherPlayers[ii].NickName;


                PlayFabClientAPI.GetUserData(getdatarequest, (result) =>
                {
                    Dictionary<string, UserDataRecord> data = result.Data;

                    if (data.ContainsKey("LoggedType"))
                    {
                        if (data.ContainsKey("PlayerName"))
                        {
                            GameManager.Instance.opponentsNames[index] = data["PlayerName"].Value;
                            //GameManager.Instance.controlAvatars.PlayerJoined(index);
                            bool fbAvatar = false;
                            int avatarIndex = 0;

                            avatarIndex = int.Parse(data[MyPlayerData.AvatarIndexKey].Value.ToString());

                            getOpponentData(data, index, fbAvatar, avatarIndex, otherID);
                        }
                        else
                        {
                            Debug.Log("ERROR");
                        }

                    }
                    else
                    {
                        Debug.Log("ERROR");
                    }

                }, (error) =>
                {
                    Debug.Log("Get user data error: " + error.ErrorMessage);
                }, null);
            }
        }

    }   

    public override void OnReceivedRoomListUpdate()
    {
        RoomInfo[] rooms = PhotonNetwork.GetRoomList();
        foreach (RoomInfo room in rooms)
        {
            Debug.Log("room" + room);
            if (room != null)
            {

                if (room.PlayerCount == 1)
                {
                    
                }
            }
        }
    }

    public override void OnLeftRoom()
    {
        Debug.Log("OnLeftRoom called");
        roomOwner = false;
        GameManager.Instance.roomOwner = false;

        //StartCoroutine(Sendlosergamedata("TwoPlayer"));

        GameManager.Instance.resetAllData();
    }

    public int GetFirstFreeSlot()
    {
        int index = 0;
        for (int i = 0; i < GameManager.Instance.opponentsIDs.Count; i++)
        {
            if (GameManager.Instance.opponentsIDs[i] == null)
            {
                index = i;
                break;
            }
        }
        return index;
    }
    public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
    {
        Debug.Log("Failed to join room");

        if (GameManager.Instance.type == MyGameType.Private)
        {
            if (GameManager.Instance.controlAvatars != null)
            {
                GameManager.Instance.controlAvatars.ShowJoinFailed(codeAndMsg[1].ToString());
            }
        }
        else
        {
            GameManager.Instance.gameController.startRandomGame();
        }
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        CancelInvoke("StartGameWithBots");

        Debug.Log("New player joined " + newPlayer.NickName);
        Debug.Log("Players Count: " + GameManager.Instance.currentPlayersCount);



        if (PhotonNetwork.room.PlayerCount >= GameManager.Instance.requiredPlayers)
        {

            PhotonNetwork.room.IsOpen = false;
            PhotonNetwork.room.IsVisible = false;
            //StartCoroutine(SendbidAmount());
            //SendPlayerCounttodb();
        }



        int index = GetFirstFreeSlot();

        GameManager.Instance.opponentsIDs[index] = newPlayer.NickName;
        GetUserDataRequest getdatarequest = new GetUserDataRequest()
        {
            PlayFabId = newPlayer.NickName,
        };

        PlayFabClientAPI.GetUserData(getdatarequest, (result) =>
        {
            Dictionary<string, UserDataRecord> data = result.Data;

            if (data.ContainsKey("LoggedType"))
            {
                if (data.ContainsKey("PlayerName"))
                {
                    GameManager.Instance.opponentsNames[index] = data["PlayerName"].Value;                   
                    bool fbAvatar = false;
                    int avatarIndex = 0;

                    avatarIndex = int.Parse(data[MyPlayerData.AvatarIndexKey].Value.ToString());

                    getOpponentData(data, index, fbAvatar, avatarIndex, newPlayer.NickName);
                }
                else
                {
                    Debug.Log("ERROR");
                }

            }
            else
            {
                Debug.Log("ERROR");
            }

        }, (error) =>
        {
            Debug.Log("Get user data error: " + error.ErrorMessage);
        }, null);
    }
    
    private void getOpponentData(Dictionary<string, UserDataRecord> data, int index, bool fbAvatar, int avatarIndex, string id)
    {
        if (data.ContainsKey("PlayerName"))
        {
            GameManager.Instance.opponentsNames[index] = data["PlayerName"].Value;
        }
        else
        {
            GameManager.Instance.opponentsNames[index] = "Guest857643";
        }

        if (data.ContainsKey("PlayerAvatarUrl") && fbAvatar)
        {
            //StartCoroutine(loadImageOpponent(data["PlayerAvatarUrl"].Value, index, id));
        }
        else
        {
            Debug.Log("GET OPPONENT DATA: " + avatarIndex);
            GameManager.Instance.opponentsAvatars[index] = GameObject.Find("StaticGameVariablesContainer").GetComponent<StaticGameVariablesController>().avatars[avatarIndex];
            //GameManager.Instance.opponentsAvatars[index] = null;
            GameManager.Instance.controlAvatars.PlayerJoined(index, id);
        }

    }

  
   
}
