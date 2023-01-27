using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManadgers : MonoBehaviour
{
    public void GameScene()
    {
        SceneManager.LoadScene("GameScene");
    }
    public void Transaction_History()
    {
        SceneManager.LoadScene("Transaction_History");
    } 
    public void Help_Desk()
    {
        SceneManager.LoadScene("Help_Desk");
    }

    public void MenuScenes()
    {
        SceneManager.LoadScene("Menu");
    }
    public void UserNameTextScenes()
    {
        SceneManager.LoadScene("user_name");
    }  
    public void TM_Event()
    {
        SceneManager.LoadScene("TM_Event");
    }

}
