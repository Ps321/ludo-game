using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerInfoController : MonoBehaviour
{

    public GameObject window;

    public GameObject avatar;
    public GameObject playername;

    public Sprite avatarSprite;

    public GameObject TotalEarningsValue;
    public GameObject CurrentBalanceValue;
    public GameObject WinningAmount;
    public GameObject GamesWonValue;
    public GameObject WinRateValue;
    public GameObject TwoPlayerWinsValue;
    public GameObject FourPlayerWinsValue;
    public GameObject FourPlayerWinsText;
    public GameObject GamesPlayedValue;
    private int index;
    public Sprite defaultAvatar;
    private WWWForm form;
    public GameObject addFriendButton;
    public GameObject editProfileButton;
    public GameObject EditButton;
    

    public static object instance;
    private PlayerInfoController() { }

    public static PlayerInfoController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerInfoController();
            }
            return (PlayerInfoController)instance;
        }
    }

    void Start()
    {
        if (!StaticString.isFourPlayerModeEnabled)
        {
            FourPlayerWinsValue.SetActive(false);
            FourPlayerWinsText.SetActive(false);
        }

        defaultAvatar = avatar.GetComponent<Image>().sprite;
        InvokeRepeating("PlayerDataLoader", 1f, 1f);
    }

  

    public void ShowPlayerInfo(int index)
    {
        this.index = index;
        window.SetActive(true);

        if (index == 0)
        {
            FillData(GameManager.Instance.avatarMy, GameManager.Instance.nameMy, GameManager.Instance.myPlayerData);
            addFriendButton.SetActive(false);
            editProfileButton.SetActive(true);
        }
        else
        {
            addFriendButton.SetActive(true);
            editProfileButton.SetActive(false);
            Debug.Log("Player info " + index);

            FillData(GameManager.Instance.playerObjects[index].avatar, GameManager.Instance.playerObjects[index].name, GameManager.Instance.playerObjects[index].data);
        }
    }

    public void ShowPlayerInfo(Sprite avatarSprite, string name, MyPlayerData data)
    {
        editProfileButton.SetActive(false);
        addFriendButton.SetActive(true);

        window.SetActive(true);

        FillData(avatarSprite, name, data);
    }



    public void FillData(Sprite avatarSprite, string name, MyPlayerData data)
    {

        if (avatarSprite == null)
        {
            avatar.GetComponent<Image>().sprite = defaultAvatar;
        }
        else
        {
            avatar.GetComponent<Image>().sprite = avatarSprite;
        }
        

    }

    public void PlayerDataLoader()
    {
        //StartCoroutine(PlayerInfo());

    }

    IEnumerator PlayerInfo()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(StaticString.checkProfiles + PlayerPrefs.GetString("userid")))
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
                    Debug.Log("Check " + www.downloadHandler.text);
                    Response<Player> response = JsonUtility.FromJson<Response<Player>>(www.downloadHandler.text);

                    if (response.status)
                    {
                        
                        Player p = response.data;
                       /* GamesPlayedValue.GetComponent<Text>().text = p.total_played;
                        GamesWonValue.GetComponent<Text>().text = p.total_win;
                        TwoPlayerWinsValue.GetComponent<Text>().text = p.total_win;
                        TotalEarningsValue.GetComponent<Text>().text = "" + p.total_earning;*/
                     

                        if (p.total_played != "0")
                        {
                            double winRatio = ((double)int.Parse(p.total_win )/ int.Parse(p.total_played)) * 100;
                           // WinRateValue.GetComponent<Text>().text = winRatio.ToString() + "%";
                            
                        }
                        else
                        {
                           // WinRateValue.GetComponent<Text>().text = "0%";
                          
                        }
                    }


                }
            }
        }
    }

}


