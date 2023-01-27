using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UPIMode : MonoBehaviour
{
    [SerializeField] Toggle toggle;
    [SerializeField] GameObject spriteUpi;

    void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.RemoveAllListeners();
        toggle.onValueChanged.AddListener((OnSwitch) =>
        {
            if (OnSwitch)
            {
                toggle.isOn = true;
                spriteUpi.SetActive(true);
                spriteUpi.GetComponent<Image>().sprite = UIManager.Instance.verifySprite;
                PlayerPrefs.SetString("mode", "upi");
                PlayerPrefs.Save();
                Debug.Log(PlayerPrefs.GetString("mode"));

            }
            else
            {
                toggle.isOn = false;
                spriteUpi.SetActive(false);
                if (PlayerPrefs.HasKey("mode"))
                {
                    PlayerPrefs.DeleteKey("mode");
                }
            }
        }
      );
    }
}
