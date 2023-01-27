using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class User_Name : MonoBehaviour
{
    [SerializeField] TMP_InputField userName;
  
    [SerializeField] TMP_Text errorText;
    public GameObject usernamePanel;
    public GameObject loadingPanel;
    WWWForm form;

    PlayFabManager playFabManager;
 

    private void Awake()
    {
        playFabManager = GameObject.Find("PlayfabManager").GetComponent<PlayFabManager>();
        loadingPanel.SetActive(false);
    }
   void Start()
    {
        userName.text = PlayerPrefs.GetString("username");
    }

    public void UserNames()
    {
        if (PlayerPrefs.GetString("phone").Length != 0)
        {
            if (userName.text.Length != 0)
            {
                loadingPanel.SetActive(true);
                StartCoroutine(InsertIntoDataBaseRegiterPhone());
            }
            else
            {
                errorText.text = "<color=red>" + "Enter the Username" + "</color>";
            }
            
        }
        else
        {
            Debug.Log(PlayerPrefs.GetString("phone"));
        }
    }

    IEnumerator InsertIntoDataBaseRegiterPhone()
    {
        form = new WWWForm();
        form.AddField("username", userName.text);
        form.AddField("phone", PlayerPrefs.GetString("phone"));

        WWW w = new WWW(StaticString.loginurl, form);

        yield return w;

        if (w.error != null)
        {
            Debug.Log("<color=red>" + w.text + "</color>");//error
        }

        else
        {
            if (w.isDone)
            {
                Response<Player> response = JsonUtility.FromJson<Response<Player>>(w.text);
                Debug.Log("Check " + w.text);    

                if (response.status)
                {
                    Player player = response.data;
                    if (response.message == "User registered")
                    {
                        Debug.Log(response.message);
                        PlayerPrefs.SetString("userid", player.userid);
                        PlayerPrefs.SetString("username", player.username);
                        PlayerPrefs.SetInt("ImageIndex", player.profile_img);
                        GameManager.Instance.userName = player.username;
                        PlayerPrefs.Save();                                                
                        playFabManager.Login();
                    }
                }
                else
                {
                    Debug.Log(response.message);
                    loadingPanel.SetActive(false);
                }
            }
        }
    }

    private void OnDisable()
    {
        userName.text = "";
        errorText.text = "";
    }
}
