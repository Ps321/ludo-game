using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;
using PlayFab.ClientModels;
using PlayFab;
using TMPro;
using System.Collections.Generic;
#if UNITY_ANDROID || UNITY_IOS

#endif
public class InitMenuScript : MonoBehaviour
{
    public GameObject playerName;
    public GameObject playerAvatar;
    public GameObject playerAvatarMain;

    //public GameObject backButtonMatchPlayers;
    public GameObject menuCanvas;
    public GameObject changeDialog;
    public GameObject inputNewName;
    public GameObject tooShortText;
    public GameObject rateWindow;
    public GameObject rewardDialogText;

    public GameObject dialog;

    public GameObject GameConfigurationScreen;
    public GameObject GameConfigurationScreenBid; 
    public GameObject GameConfigurationScreenvsComputer;
    public GameObject FourPlayerMenuButton;

    void Start()
    {

        GameManager.Instance.initMenuScript = this;
        if (PlayerPrefs.GetInt(StaticString.SoundsKey, 0) == 0)
        {
            AudioListener.volume = 1;
        }
        else
        {
            AudioListener.volume = 0;
        }

        if (!StaticString.isFourPlayerModeEnabled)
        {
            FourPlayerMenuButton.SetActive(false);
        }

        

        GameManager.Instance.dialog = dialog;

        
        //GameManager.Instance.backButtonMatchPlayers = backButtonMatchPlayers;

        playerName.GetComponent<TMP_Text>().text = GameManager.Instance.nameMy;
        if (GameManager.Instance.avatarMy != null)
            playerAvatar.GetComponent<Image>().sprite = GameManager.Instance.avatarMy;
        playerAvatarMain.GetComponent<Image>().sprite = GameManager.Instance.avatarMy;

      /*  GameManager.Instance.myAvatarGameObject = playerAvatar;
        GameManager.Instance.myNameGameObject = playerName;
*/
               

      /*  if (PlayerPrefs.GetInt("GamesPlayed", 1) % 8 == 0 && PlayerPrefs.GetInt("GameRated", 0) == 0)
        {
            rateWindow.SetActive(true);
            PlayerPrefs.SetInt("GamesPlayed", PlayerPrefs.GetInt("GamesPlayed", 1) + 1);
        }*/

    }

    public void ChangeImage()
    {
        if (GameManager.Instance.avatarMy != null)
            playerAvatar.GetComponent<Image>().sprite = GameManager.Instance.avatarMy;
        playerAvatarMain.GetComponent<Image>().sprite = GameManager.Instance.avatarMy;

    }

    public void QuitApp()
    {
        PlayerPrefs.SetInt("GameRated", 1);
#if UNITY_ANDROID
        Application.OpenURL( StaticString.localaddress);
#elif UNITY_IPHONE
        Application.OpenURL("itms-apps://itunes.apple.com/app/id" + StaticString.ITunesAppID);
#endif
        //Application.Quit();
    }

    public void ShowGameConfiguration(int index)
    {
        GameManager.Instance.showTargetLines = true;
        GameManager.Instance.mode = MyGameMode.Master;

        switch (index)
        {
            case 2:
                GameManager.Instance.type = MyGameType.TwoPlayer;
                Debug.Log(GameManager.Instance.type);
                break;

            case 4:
                GameManager.Instance.type = MyGameType.FourPlayer;
                Debug.Log(GameManager.Instance.type);
                break;         
          
        }
    }


    public void TakeScreenshot()
    {
        ScreenCapture.CaptureScreenshot("TestScreenshot.png");
    }

    public void backToMenuFromTableSelect()
    {
        GameManager.Instance.offlineMode = false;
     
        menuCanvas.SetActive(true);

    }

    public void showChangeDialog()
    {
        changeDialog.SetActive(true);
    }


    
}
