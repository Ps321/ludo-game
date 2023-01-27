using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Display_Two_Ranks : MonoBehaviour
{
    #region Instance
    private static Display_Two_Ranks _instance;
    public static Display_Two_Ranks Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<Display_Two_Ranks>();
            }
            return _instance;
        }

    }
    #endregion

    public GameObject ranksObject;
    WWWForm form;
    int gameId;

    public TMP_Text modeNames;
    public TMP_Text entryFees;

    public void OnShowPricePool(int eventId, string entryFee, string modeName)
    {

        gameId = eventId;
        entryFees.text = entryFee;
        modeNames.text = modeName;
        Debug.Log("eventID" + gameId + "pricePool " + entryFee + "mName" + modeName);
        StartCoroutine(OnShowPricePool());
    }

    IEnumerator OnShowPricePool()
    {
        form = new WWWForm();
        form.AddField("id", gameId);

        WWW w = new WWW(StaticString.prizePools, form);

        yield return w;

        if (w.error != null)
        {
            Debug.Log("<color=red>" + w.text + "</color>");//error
        }
        else
        {
            if (w.isDone)
            {
                Response<RankPool[]> response = JsonUtility.FromJson<Response<RankPool[]>>(w.text);
                Debug.Log("ShowPricePool " + w.text);

                if (response.status)
                {
                    RankPool[] rankPools = response.data;

                  //  for (int i = 0; i < ranksObject.transform.childCount; i++)
                  //  {
                        ranksObject.transform.GetChild(0).transform.GetChild(1).GetComponentInChildren<Text>().text = rankPools[0].win_amount;
                        ranksObject.transform.GetChild(1).transform.GetChild(1).GetComponentInChildren<Text>().text = rankPools[1].win_amount;
                   // }
                }
            }
        }
    }
}
