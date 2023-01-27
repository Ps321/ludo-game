using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AddUPI : MonoBehaviour
{
    [SerializeField] TMP_InputField upiName;
    [SerializeField] TMP_Text infoText;
    WWWForm form;

    public void CheckDetails()
    {

        if (upiName.text.Length !=0)
        {
            StartCoroutine(ADDUpiDeatil());
        }
        else
        {
            infoText.text = $"<color=red> {"Enter UPI"} </color>";
        }
    }

    IEnumerator ADDUpiDeatil()
    {
        form = new WWWForm();

        form.AddField("userid", PlayerPrefs.GetString("userid"));
        form.AddField("upi", upiName.text);
       

        WWW w = new WWW(StaticString.addUpiDetails, form);

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
                    SliderMenuAnim.Instance.UPI(1);
                    upiName.text = "";                  
                    infoText.text = "";
                }
            }

        }
    }
}
