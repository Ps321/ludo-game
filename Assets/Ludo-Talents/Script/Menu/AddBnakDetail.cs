using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class AddBnakDetail : MonoBehaviour
{
    [SerializeField] TMP_InputField accountName;
    [SerializeField] TMP_InputField accountNo;
    [SerializeField] TMP_InputField accountConfirmNo;
    [SerializeField] TMP_InputField ifscCode;
    [SerializeField] TMP_Text infoText;
    [SerializeField] GameObject ifscCodePanel;

    WWWForm form;
    public void CheckDetails()
    {

        if (accountNo.text == accountConfirmNo.text)
        {
            StartCoroutine(ADDBankDeatil());
        }
        else
        {
            infoText.text = $"<color=red> {"Check Account Number"} </color>";
        }
    }

    IEnumerator ADDBankDeatil()
    {
        form = new WWWForm();

        form.AddField("userid",PlayerPrefs.GetString("userid"));
        form.AddField("bankholdername", accountName.text);
        form.AddField("accountnumber", accountNo.text);
        form.AddField("ifsccode", ifscCode.text);


        WWW w = new WWW(StaticString.addBankDetails, form);

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
                    SliderMenuAnim.Instance.AddBankDetails(1);
                    accountName.text = "";
                    accountNo.text = "";
                    accountConfirmNo.text = "";
                    ifscCode.text = "";
                    infoText.text = "";
                }
                else
                {
                    if (response.message == "IFSC CODE NOT FOUND")
                    {
                        ifscCodePanel.SetActive(true);
                    }
                }

            }

        }
    }

}
