using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Withdraw : MonoBehaviour
{
    [SerializeField] InputField amount;
    [SerializeField] Text info;

    WWWForm form;
    private void Start()
    {    
        if (PlayerPrefs.HasKey("mode"))
        {
            PlayerPrefs.DeleteKey("mode");
        }
    }

    public void WithDrawRequestbank()
    {
        int amountswithdraw = int.Parse(amount.text);
      
        if (GameManager.Instance.winAmount != 0)
        {
            if (GameManager.Instance.winAmount >= amountswithdraw && (amountswithdraw >= 30 && amountswithdraw <= 25000))
            {
                if (PlayerPrefs.GetString("mode").Length == 0)
                {
                    info.text = "<color=red>" + "Please Select Mode" + "</color>";
                }
                else
                {
                    StartCoroutine(InsertIntoDataBaseBank());
                }
              
            }
            else
            {
                info.text = "<color=red>" + "Minimum Withdraw limit is ₹30" + "</color>";
            }
        }
        else
        {
            info.text = "<color=red>" + "Money not enough" + "</color>";
        }

    }


    IEnumerator InsertIntoDataBaseBank()
    {
        form = new WWWForm();

        form.AddField("userid", PlayerPrefs.GetString("userid"));
        form.AddField("amount", amount.text);
        form.AddField("mode", PlayerPrefs.GetString("mode"));    

        WWW w = new WWW(StaticString.withdrawRequest, form);

        yield return w;

        if (w.error != null)
        {
            Debug.Log("<color=red>" + w.text + "</color>");
        }
        else
        {
            if (w.isDone)
            {
                Response<Player> response = JsonUtility.FromJson<Response<Player>>(w.text);
                Debug.Log("Check " + w.text);

                if (response.status)
                {
                    Debug.Log("<color=green>" + response.message + "</color>");
                    if (PlayerPrefs.HasKey("mode"))
                    {
                        PlayerPrefs.DeleteKey("mode");
                    }
                    SliderMenuAnim.Instance.WithdrawMoney(1);
                    amount.text = "";
                }            

            }
        }
    }    
}
