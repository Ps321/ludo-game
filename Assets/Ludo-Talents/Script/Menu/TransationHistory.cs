using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;

public class TransationHistory : MonoBehaviour
{
    private WWWForm form;
    [SerializeField] GameObject myPrefab;


    private void Start()
    {
        StartCoroutine(TransationHistoryData());
    }

    IEnumerator TransationHistoryData()
    {
        form = new WWWForm();
        form.AddField("id", PlayerPrefs.GetString("userid"));

        using (UnityWebRequest www = UnityWebRequest.Post(StaticString.transationHistory, form))
        {
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                print(www.error);
            }
            else
            {
                if (www.isDone)
                {                   
                    Response<Trans_History[]> response = JsonUtility.FromJson<Response<Trans_History[]>>(www.downloadHandler.text);
                    Debug.Log("TransationHistoryData " + www.downloadHandler.text);
                    if (response.status)
                    {
                        Trans_History[] transHistory = response.data;
                      

                        for (int i = 0; i < transHistory.Length; i++)
                        {
                            // optima code
                            if (this.gameObject.transform.childCount <= transHistory.Length)
                            {
                                for (int j = 0; j < transHistory.Length; j++)
                                {
                                    this.gameObject.transform.GetChild(i).gameObject.SetActive(true);
                                    this.gameObject.transform.GetChild(i).GetChild(1).GetComponent<Trans_Date>().MyDateDis(transHistory[i].created_at);
                                    this.gameObject.transform.GetChild(i).GetChild(3).GetComponent<Trans_Date>().MyTimeDis(transHistory[i].created_at);
                                    this.gameObject.transform.GetChild(i).GetChild(4).GetComponent<TMP_Text>().text = transHistory[i].message;
                                    this.gameObject.transform.GetChild(i).GetChild(5).GetComponent<TMP_Text>().text = transHistory[i].type;
                                    this.gameObject.transform.GetChild(i).GetChild(7).GetComponent<TMP_Text>().text = transHistory[i].description;
                                    this.gameObject.transform.GetChild(i).GetChild(8).GetComponent<TMP_Text>().text = transHistory[i].amount;
                                }

                            }
                            else
                            {
                                for (int j = 0; j < transHistory.Length; j++)
                                {
                                    this.gameObject.transform.GetChild(i).gameObject.SetActive(true);
                                    this.gameObject.transform.GetChild(i).GetChild(1).GetComponent<Trans_Date>().MyDateDis(transHistory[i].created_at);
                                    this.gameObject.transform.GetChild(i).GetChild(3).GetComponent<Trans_Date>().MyTimeDis(transHistory[i].created_at);
                                    this.gameObject.transform.GetChild(i).GetChild(4).GetComponent<TMP_Text>().text = transHistory[i].message;
                                    this.gameObject.transform.GetChild(i).GetChild(5).GetComponent<TMP_Text>().text = transHistory[i].type;
                                    this.gameObject.transform.GetChild(i).GetChild(7).GetComponent<TMP_Text>().text = transHistory[i].description;
                                    this.gameObject.transform.GetChild(i).GetChild(8).GetComponent<TMP_Text>().text = transHistory[i].amount;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
