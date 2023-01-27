
using UnityEngine;


public class StartScriptController : MonoBehaviour
{

    public GameObject LoginCanvas;
    public GameObject splashCanvas;
    PlayFabManager playFabManager;

    void Start()
    {
        playFabManager = GameObject.Find("PlayfabManager").GetComponent<PlayFabManager>();

        if (PlayerPrefs.HasKey("Logout"))
        {
            LoginCanvas.SetActive(true);
            splashCanvas.SetActive(false);
        }

        
        if (PlayerPrefs.HasKey("userid"))
        {
            playFabManager.Login();
            LoginCanvas.SetActive(false);
            splashCanvas.SetActive(true);
        }
        else
        {
            LoginCanvas.SetActive(true);
        }
        Debug.Log("START SCRIPT");
    }
}
