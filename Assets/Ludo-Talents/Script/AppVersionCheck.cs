using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppVersionCheck : MonoBehaviour
{
    public GameObject updateObj;

    private void Awake()
    {
        StartCoroutine(CheckVersion());
    }

    IEnumerator CheckVersion()
    {
        WWW w = new WWW(StaticString.version);

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
                Debug.Log("CheckVersion " + w.text);
                if (response.status)
                {
                    Player player = response.data;

                    if (Application.version == player.version)
                    {
                        updateObj.SetActive(false);
                    }
                    else
                    {
                        updateObj.SetActive(true);
                    }
                }
            }

        }
    }


    public void urlWeb()
    {
        Application.OpenURL(StaticString.shareLink);
    }
}
