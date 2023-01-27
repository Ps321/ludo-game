using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BankMode : MonoBehaviour
{
    [SerializeField] Toggle toggle;
    [SerializeField] GameObject spriteBank;
    void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.RemoveAllListeners();
        toggle.onValueChanged.AddListener((OnSwitch) =>
        {
            if (OnSwitch)
            {
                toggle.isOn = true;
                spriteBank.SetActive(true);               
                spriteBank.GetComponent<Image>().sprite = UIManager.Instance.verifySprite;
                PlayerPrefs.SetString("mode", "bank");
                PlayerPrefs.Save();
                Debug.Log(PlayerPrefs.GetString("mode"));
            }
            else
            {
                toggle.isOn = false;
                spriteBank.SetActive(false);
                if (PlayerPrefs.HasKey("mode"))
                {
                    PlayerPrefs.DeleteKey("mode");
                }
            }
        }
      );
    }
}
