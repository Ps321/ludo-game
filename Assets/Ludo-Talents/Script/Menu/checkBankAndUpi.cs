using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class checkBankAndUpi : MonoBehaviour
{
    [SerializeField] Button withdrawButton;
    [SerializeField] GameObject addBankBT;
    [SerializeField] GameObject addUpiBT;
    [SerializeField] GameObject spriteBank;
    [SerializeField] GameObject spriteUpi;

    [SerializeField] Text accountnuber;
    [SerializeField] Text upi;

    [SerializeField] Toggle toggleBank;
    [SerializeField] Toggle toggleUpi;

    WWWForm form;
    void Start()
    {
        withdrawButton.onClick.AddListener(_GetBankUpiData);
    }
    public void _GetBankUpiData()
    {
        UIManager.Instance.LoadingPanel.SetActive(true);
        StartCoroutine(GetBankUpiData());
    }

    IEnumerator GetBankUpiData()
    {
       
        form = new WWWForm();
        form.AddField("userid", PlayerPrefs.GetString("userid"));
        WWW w = new WWW(StaticString.bankOrUpi, form);

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
                Debug.Log("Get Bank and UPI " + w.text);
                if (response.status)
                {
                    Player player = response.data;
                    if (player.upi.Length != 0 && player.accountnumber.Length != 0)
                    {
                        toggleUpi.interactable = true;
                        toggleBank.interactable = true;
                        addUpiBT.SetActive(false);
                        addBankBT.SetActive(false);
                        upi.text = player.upi;
                        accountnuber.text = player.accountnumber;                     
                    }
                    else if (player.upi.Length != 0)
                    {
                        addUpiBT.SetActive(false);
                        spriteUpi.SetActive(true);
                        spriteUpi.GetComponent<Image>().sprite = UIManager.Instance.verifySprite;
                        upi.text = player.upi;
                        Debug.Log(player.upi);
                       
                    }
                    else if (player.accountnumber.Length != 0)
                    {
                        addBankBT.SetActive(false);
                        spriteBank.SetActive(true);
                        spriteBank.GetComponent<Image>().sprite = UIManager.Instance.verifySprite;
                        accountnuber.text = player.accountnumber;
                        Debug.Log(player.accountnumber);
                    }


                    if (player.upi.Length == 0 && player.accountnumber.Length != 0)
                    {
                        toggleUpi.interactable = false;
                       
                    }
                    else if (player.upi.Length != 0 && player.accountnumber.Length == 0)
                    {
                        toggleBank.interactable = false;
                    }
                    UIManager.Instance.LoadingPanel.SetActive(false);
                }
                else
                {
                    UIManager.Instance.LoadingPanel.SetActive(false);
                }
            }
        }
    }
}
