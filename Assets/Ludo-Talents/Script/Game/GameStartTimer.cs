using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameStartTimer : MonoBehaviour
{

    public TMP_Text timeCount;
    public int timeRemaining = 10;

    

    void Start()
    {
        timeCount = GetComponent<TMP_Text>();
        
        StartCoroutine(TimerCount());
    }

    IEnumerator TimerCount()
    {
        StartCoroutine(BidAMounts());
        while (timeRemaining >= 0)
        {
            timeCount.text = timeRemaining.ToString();
            yield return new WaitForSeconds(1f);
            timeRemaining--;
        }
        GameManager.Instance.allReadyPlayer = true;
        SceneManager.LoadScene("GameScene");
       
    }
    IEnumerator BidAMounts()
    {
        WWWForm form = new WWWForm();

        form.AddField("userid", PlayerPrefs.GetString("userid"));
        form.AddField("eventid", GameManager.Instance.eventIDs);
        form.AddField("amount", PlayerPrefs.GetString("bidamount", "bidamount"));

        WWW w = new WWW(StaticString.bid, form);

        yield return w;

        if (w.error != null)
        {
            Debug.Log("<color=red>" + w.text + "</color>");//error
        }
        else
        {
            if (w.isDone)
            {               
                Debug.Log("BidAMounts " + w.text);
            }

        }
    }
}
