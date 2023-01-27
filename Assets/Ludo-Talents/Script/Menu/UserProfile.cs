using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.Networking;

public class UserProfile : MonoBehaviour
{
    [SerializeField] Button userProfileBt;
    [SerializeField] TMP_Text userName;
    [SerializeField] TMP_Text bal;
    [SerializeField] TMP_Text referral_code;
    [SerializeField] Text mobileNo;
    [SerializeField] Text bankAccountNo;
    [SerializeField] Text upiId;
    [SerializeField] GameObject upiBT;
    [SerializeField] GameObject upiVerifyImage;
    [SerializeField] InputField referRedeem;
    [SerializeField] GameObject referRedeemBT;

    [Space]
    [SerializeField] Sprite[] sprite;
    [SerializeField] GameObject addBT;
    [SerializeField] GameObject verifyBankBT;
    WWWForm form;

    private void Start()
    {
        StartCoroutine(CheckProfile());
        userProfileBt.onClick.AddListener(CheckProfiles);
    }



    public void ShareCoderefcode()
    {
        NativeShare share = new NativeShare();
        string shareText = StaticString.SharePrivateLinkMessage + PlayerPrefs.GetString("refer_code") + $"\n\nDownload {StaticString.appName} Game Now \n";
#if UNITY_ANDROID
        shareText += StaticString.shareLink;

#elif UNITY_IOS
        //shareText += StaticString.shareLink + StaticStrings.ITunesAppID;
        shareText += StaticString.shareLink + StaticString.ITunesAppID;
#endif
        share.Share(shareText, null, null, "OpenWhatsAppHelp");
    }

    public void OpenWhatsAppHelp()
    {

        Application.OpenURL(StaticString.Whatsapphelp);

    }

    public void CheckProfiles()
    {
        UIManager.Instance.LoadingPanel.SetActive(true);
        StartCoroutine(CheckProfile());
    }

    IEnumerator CheckProfile()
    {
        form = new WWWForm();

        form.AddField("userid", PlayerPrefs.GetString("userid"));

        WWW w = new WWW(StaticString.checkProfiles, form);

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
                Debug.Log("_CheckProfile " + w.text);

                if (response.status)
                {
                    Player player = response.data;
                    userName.text = player.username;
                    mobileNo.text = player.phone;
                    referral_code.text = player.referral_code;
                    bal.text = StaticString.ruppeSymbol + player.balance;
                    PlayerPrefs.SetString("refer_code", player.referral_code);
                    PlayerPrefs.Save();

                    if (player.refer_to.Length != 0)
                    {
                        referRedeemBT.SetActive(false);
                        referRedeem.text = player.refer_to;
                        referRedeem.interactable = false;
                    }

                    if (player.accountnumber.Length != 0)
                    {
                        bankAccountNo.text = "xxxxxx" + player.accountnumber.Substring(player.accountnumber.Length - 4, 4);
                        if (player.bankstatus == "verified")
                        {
                            addBT.SetActive(false);
                            verifyBankBT.SetActive(true);
                            verifyBankBT.GetComponent<Image>().sprite = sprite[0];
                        }
                        else if (player.bankstatus == "unverified")
                        {
                            addBT.SetActive(true);
                        }
                        else if (player.bankstatus == "pending")
                        {
                            addBT.SetActive(false);
                            verifyBankBT.SetActive(true);
                            verifyBankBT.GetComponent<Image>().sprite = sprite[1];
                        }

                    }
                    else
                    {
                        addBT.SetActive(true);
                    }

                    if (player.upi.Length != 0)
                    {
                        upiId.text = "" + player.upi;
                        upiBT.SetActive(false);
                        upiVerifyImage.SetActive(true);
                    }
                    UIManager.Instance.LoadingPanel.SetActive(false);
                }
            }
        }
    }

    public void OnReferRedemCode()
    {
        StartCoroutine(_ReferCodeRedem());
    }

    IEnumerator _ReferCodeRedem()
    {
        WWWForm form = new WWWForm();
        form.AddField("referral_code", referRedeem.text);
        form.AddField("userid", PlayerPrefs.GetString("userid"));
        using (UnityWebRequest w = UnityWebRequest.Post(StaticString.ReferRedem, form))
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

                    Response<Player> response = JsonUtility.FromJson<Response<Player>>(w.downloadHandler.text);
                    Debug.Log("_ReferCodeRedem " + w.downloadHandler.text);
                    if (response.status)
                    {
                        Player player = response.data;

                        if (player.refer_to.Length != 0)
                        {
                            referRedeemBT.SetActive(false);
                            referRedeem.interactable = false;
                            referRedeem.text = player.refer_to;
                        }
                    }
                }
            }
        }

    }
}
