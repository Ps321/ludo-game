using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Show_Price_Pool : MonoBehaviour
{
    GameObject ranksObject;
    WWWForm form;
    public TMP_Text modeNames;
    public TMP_Text entryFees;

    private void OnEnable()
    {
        ranksObject = this.gameObject;
        entryFees.text = GameManager.Instance.prizePoolMoney;
        modeNames.text = GameManager.Instance.eventMode;
        StartCoroutine(OnShowPricePool());
    }

    IEnumerator OnShowPricePool()
    {
        form = new WWWForm();
        form.AddField("id", GameManager.Instance.eventIDs);

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

                    for (int i = 0; i < ranksObject.transform.childCount; i++)
                    {
                        ranksObject.transform.GetChild(i).transform.GetChild(1).GetComponentInChildren<Text>().text = rankPools[i].win_amount;

                        /*if (ranksObject.transform.childCount == rankPools.Length)
                        {
                        }
                        else
                        {
                            ranksObject.transform.GetChild(i).transform.GetChild(1).GetComponentInChildren<Text>().text = rankPools[i].win_amount;
                        }*/
                    }
                }
            }
        }
    }

}
