
using UnityEngine.UI;
using UnityEngine;
using TMPro;


public class PlayerInfromation : MonoBehaviour
{
    public Sprite defaultAvatar;

    public GameObject window;
    public GameObject avatar;
    public GameObject playername;
    public GameObject totalGamesPlayedValue;
    public GameObject GamesWonValue;
    public GameObject GamesLosseValue;


    public static object instance;
    private PlayerInfromation() { }

    public static PlayerInfoController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new PlayerInfromation();
            }
            return (PlayerInfoController)instance;
        }
    }

    void Start()
    {     
        defaultAvatar = avatar.GetComponent<Image>().sprite;       
    }

    public void ShowPlayerInfo(Sprite avatarSprite, string name, MyPlayerData data)
    {
        window.SetActive(true);
        FillData(avatarSprite, name, data);

    }

    public void FillData(Sprite avatarSprite, string name, MyPlayerData data)
    {

        if (avatarSprite == null)
        {
            avatar.GetComponent<Image>().sprite = defaultAvatar;
            playername.GetComponent<Text>().text = name;
        }
        else
        {
            avatar.GetComponent<Image>().sprite = avatarSprite;
            playername.GetComponent<Text>().text = name;
        }
        totalGamesPlayedValue.GetComponent<TMP_Text>().text = (data.GetPlayedGamesCount()).ToString();
        GamesWonValue.GetComponent<TMP_Text>().text = (data.GetFourPlayerWins() + data.GetTwoPlayerWins()).ToString();
        GamesLosseValue.GetComponent<TMP_Text>().text = ((data.GetPlayedGamesCount()) -(data.GetTwoPlayerWins())).ToString();


    }
    
}
