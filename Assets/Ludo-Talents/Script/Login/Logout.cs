
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Logout : MonoBehaviour
{
   
    public void LogoutGame()
    {
        PlayerPrefs.SetString("Logout", "Logout");
        PlayerPrefs.Save();
        if (PlayerPrefs.HasKey("Login"))
        {
            PlayerPrefs.DeleteKey("Login");
        }


        if (PlayerPrefs.HasKey("phone"))
        {
            PlayerPrefs.DeleteKey("userid");
            PlayerPrefs.DeleteKey("username");
            PlayerPrefs.DeleteKey("phone");
            PlayerPrefs.DeleteKey("email");
        }

        if (PlayerPrefs.HasKey("OnSignIn"))
        {
            PlayerPrefs.DeleteKey("OnSignIn");
            PlayerPrefs.DeleteKey("userid");
            PlayerPrefs.DeleteKey("username");
            PlayerPrefs.DeleteKey("phone");
            PlayerPrefs.DeleteKey("email");
            
        }
        GameManager.Instance.playfabManager.destroy();
        GameManager.Instance.gameController.destroy();
        GameManager.Instance.connectionLost.destroy();
        GameManager.Instance.avatarMy = null;
        GameManager.Instance.logged = false;
        GameManager.Instance.resetAllData();
        PhotonNetwork.Disconnect();
        LoginloadloginScene();
    }
    public void LoginloadloginScene()
    {
        SceneManager.LoadScene("Login");
    }
   


}
