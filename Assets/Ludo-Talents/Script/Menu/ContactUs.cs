using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ContactUs : MonoBehaviour
{
    [SerializeField] InputField email;
    [SerializeField] InputField massage;
    [SerializeField] Text info;
    [SerializeField] GameObject emailobj;
    public void BTClickEmail()
    {
        /* string i = "ludogodcontest@gmail.com";
         Application.OpenURL(i);
         SliderMenuAnim.Instance.ContantUSS(1);*/
        //StartCoroutine(_SendEmail());
    }

    public void BTClickTelegram()
    {
        //Application.OpenURL("https://web.telegram.org/k/#@LudoGodContest");
        SliderMenuAnim.Instance.ContantUSS(1);
    }

    public void BTClickInsta()
    {
        //Application.OpenURL("https://www.instagram.com/ludodhani");
        SliderMenuAnim.Instance.ContantUSS(1);
    }

    IEnumerator _SendEmail()
    {
        WWWForm form = new WWWForm();
        form.AddField("email", email.text);
        form.AddField("message", massage.text);
        using (UnityWebRequest w = UnityWebRequest.Post(StaticString.ContactUs, form))
        {
            yield return w.SendWebRequest();
            if (w.result != UnityWebRequest.Result.Success)
            {
                print(w.error);
            }
            else
            {
                if (w.isDone)
                {
                   // Response<Player> response = JsonUtility.FromJson<Response<Player>>(w.downloadHandler.text);
                    Debug.Log(w.downloadHandler.text);
                    info.text = w.downloadHandler.text;
                    emailobj.SetActive(false);
                }
            }
        }
    }
}

