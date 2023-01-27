using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppQuit : MonoBehaviour
{

    public GameObject quitScreen;
    // Start is called before the first frame update
    void Start()
    {
        quitScreen.SetActive(false);
    }

    
    public void ScreenQuit()
    {
        Application.Quit();
    }
    
}
