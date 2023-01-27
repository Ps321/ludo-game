using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firebase.Auth;
using UnityEngine.SceneManagement;

public class Phone_Auth : MonoBehaviour
{
    [SerializeField] TMP_InputField phoneNumber;
    [SerializeField] TMP_InputField otpNo;
    [SerializeField] TMP_Text infoText;
    [SerializeField] TMP_Text SentOTPtext;

    [SerializeField] GameObject loginPanel;
    [SerializeField] GameObject verifyPanel;
    [SerializeField] GameObject loadingPanel;

    FirebaseAuth firebaseAuth;
    PhoneAuthProvider provider;
    private uint phoneAuthTimeoutMs = 60 * 1000;
    private string verificationId;

    WWWForm form;
    PlayFabManager playFabManager;

    void Start()
    {

        firebaseAuth = FirebaseAuth.DefaultInstance;
        playFabManager = GameObject.Find("PlayfabManager").GetComponent<PlayFabManager>();

    }
    public void Login()
    {
        int i = int.Parse(phoneNumber.text);
        if (i == 10)
        {
            OtpSent();

        }
        else
        {
            infoText.text = "Enter 10 Digit Number";
        }
    }
    public void OtpSent()
    {
        loadingPanel.SetActive(true);
        provider = PhoneAuthProvider.GetInstance(firebaseAuth);
        provider.VerifyPhoneNumber("+91" + phoneNumber.text, phoneAuthTimeoutMs, null,
          verificationCompleted: (credential) =>
          {
              infoText.text = credential.ToString();
              Debug.Log("credential " +credential);
              // Auto-sms-retrieval or instant validation has succeeded (Android only).
              // There is no need to input the verification code.
              // `credential` can be used instead of calling GetCredential().
          },
          verificationFailed: (error) =>
          {
              infoText.text = " verification code was not sent.";
              // The verification code was not sent.
              // `error` contains a human readable explanation of the problem.
          },
          codeSent: (id, token) =>
          {
              verificationId = id;
              infoText.text = " Verification code was successfully sent via SMS";
              SentOTPtext.text = "OTP sent to +91 " + phoneNumber.text;
              PlayerPrefs.SetString("phone", phoneNumber.text);
              PlayerPrefs.Save();

              loginPanel.SetActive(false);
              verifyPanel.SetActive(true);
              loadingPanel.SetActive(false);
             
          },
          codeAutoRetrievalTimeOut: (id) =>
          {
              
          });
    }

    public void Verify_OTP() // attched Login scence in verify Button
    {
        loadingPanel.SetActive(true);
        Credential credential = provider.GetCredential(verificationId, otpNo.text);

        firebaseAuth.SignInWithCredentialAsync(credential).ContinueWith(task => {
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " +
                               task.Exception);
                infoText.text = "OTP Worng! Enter the correct OTP";
                loadingPanel.SetActive(false);
                return;
            }
           
            FirebaseUser newUser = task.Result;
            Debug.Log("User signed in successfully");
            infoText.text = "<color=green>" + "Otp successfully" + "</color>";
            StartCoroutine(Checkuser());
           
        });

    }
    public void CheckUsers()
    {
        PlayerPrefs.SetString("phone", "1234567899");
        PlayerPrefs.Save();
        loadingPanel.SetActive(true);
        StartCoroutine(Checkuser());

    }

    IEnumerator Checkuser()
    {
       
        form = new WWWForm();
        form.AddField("phone", PlayerPrefs.GetString("phone"));
        WWW w = new WWW(StaticString.checkUser, form);

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
                    Player player = response.data;
                    if (response.message == "Success")
                    {
                        //infoText.text = "<color=green>" + "successfully" + "</color>";
                        PlayerPrefs.SetString("userid", player.userid);                        
                        PlayerPrefs.SetString("username", player.username);
                        PlayerPrefs.SetInt("ImageIndex", player.profile_img);
                        GameManager.Instance.userName = player.username;
                        PlayerPrefs.Save();
                        playFabManager.Login();
                        verifyPanel.SetActive(false);
                        loadingPanel.SetActive(true);
                    }
                }
                else
                {
                    Debug.Log(response.message);
                    if (PlayerPrefs.HasKey("username"))
                    {
                        PlayerPrefs.DeleteKey("username");
                    }
                    SceneManager.LoadScene("user_name");
                }
            }
        }
    }

    public void ChangeScene()
    {
        verifyPanel.SetActive(false);
        loginPanel.SetActive(true);
    }

    private void OnDisable()
    {
        phoneNumber.text = "";
        otpNo.text = "";
    }
}
