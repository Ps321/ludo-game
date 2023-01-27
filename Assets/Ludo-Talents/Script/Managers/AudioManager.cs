using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AudioManager : MonoBehaviour
{
    public GameObject Sounds;

    void Start()
    {
        if (PlayerPrefs.GetInt(StaticString.SoundsKey, 0) == 1)
        {
            Sounds.GetComponent<Toggle>().isOn = false;
        }
        Sounds.GetComponent<Toggle>().onValueChanged.RemoveAllListeners();
        Sounds.GetComponent<Toggle>().onValueChanged.AddListener((value) =>
        {
            PlayerPrefs.SetInt(StaticString.SoundsKey, value ? 0 : 1);
            if (value)
            {
                AudioListener.volume = 1;

            }
            else
            {
                AudioListener.volume = 0;
            }
        }
        );
    }

}