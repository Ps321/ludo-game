using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SliderMenuAnim : MonoBehaviour
{

    #region Instance
    private static SliderMenuAnim _instance;
    public static SliderMenuAnim Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<SliderMenuAnim>();
            }
            return _instance;
        }

    }
    #endregion

    [SerializeField] RectTransform mainMenu, eventCreatedBid, referMenu, settingMenu, myBalance, myProfile, addBankDetails, upi, helpDesk, privacyPolicy;
    [SerializeField] RectTransform termsAndCondition, withdrawDetails, addMoney, confirmPlayFee, matchingObj, contantus, faqPanel, howTOPlay, changeAvatar,prizePoolDisplay;
    [SerializeField] RectTransform display_Two_Ranks, display_Four_Ranks;

    float time = 0.25f;
    Vector2 sidePositionTransform;
    Vector2 downPositionTransform;
    AudioSource audioSource;
    public AudioClip audioClip;

    bool confirmpay,contantuss = false;
    bool homePage = true;

    public GameObject quitScreen;
    void Start()
    {
        sidePositionTransform = new Vector2(1176, 0);
        downPositionTransform = new Vector2(0, -2365);
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip;

    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            if (homePage)
            {
                quitScreen.SetActive(true);
            }
            else if (confirmpay)
            {
                UIManager.Instance.confirm_Payment.SetActive(false);
                confirmPlayFee.DOAnchorPos(downPositionTransform, time);
            }
            else if (contantuss)
            {
                contantus.DOAnchorPos(downPositionTransform, time);
            }
        }
    }

    public void Homepanel(int i)
    {
        audioSource.Play();
        mainMenu.DOAnchorPos(sidePositionTransform, time);
        UIManager.Instance.homePanel.SetActive(true);
        if (i == 0)
        {
            mainMenu.DOAnchorPos(Vector2.zero, time);
            UIManager.Instance.referAndEarnPanel.SetActive(false);
        }
    }

    public void EventCreatedPlayGame(int i)
    {
        audioSource.Play();
        if (i == 0)
        {
            //UIManager.Instance.eventPanel.SetActive(true);
            eventCreatedBid.DOAnchorPos(Vector2.zero, time);
            homePage = false;
        }
        else if (i == 1)
        {
            eventCreatedBid.DOAnchorPos(sidePositionTransform, time);
            //UIManager.Instance.eventPanel.SetActive(false);
            homePage = true;

        }
    }

    public void ReferEarn(int i)
    {
        audioSource.Play();
        UIManager.Instance.referAndEarnPanel.SetActive(true);
        if (i == 0)
        {
            homePage = false;
            referMenu.DOAnchorPos(Vector2.zero, time);
        }      
        else if (i == 1)
        {
            referMenu.DOAnchorPos(sidePositionTransform, time);
           
            mainMenu.DOAnchorPos(Vector2.zero, time);
            UIManager.Instance.homePanel.SetActive(true);
            UIManager.Instance.referAndEarnPanel.SetActive(false);
           
            homePage = true;
        }
    }

    public void Setting(int i)
    {
        audioSource.Play();
        if (i == 0)
        {
            homePage = false;
            settingMenu.DOAnchorPos(Vector2.zero, time);
            StartCoroutine(BackToPostion(2));
        }
        else if (i == 1)
        {           
            settingMenu.DOAnchorPos(sidePositionTransform, time);
            mainMenu.DOAnchorPos(Vector2.zero, time);
            homePage = true;
        }
    }

    public void MyProfile(int i)
    {
        audioSource.Play();
        if (i == 0)
        {
            myProfile.DOAnchorPos(Vector2.zero, time);
        }
        else if (i == 1)
        {
            myProfile.DOAnchorPos(sidePositionTransform, time);
        }
    }

    public void MyBalance(int i)
    {
        audioSource.Play();
        if (i == 0)
        {
            myBalance.DOAnchorPos(Vector2.zero, time);
            StartCoroutine(BackToPostion(1));          
        }
        else if (i == 1)
        {
            myBalance.DOAnchorPos(sidePositionTransform, time);
        }
    }

    public void AddBankDetails(int i)
    {
        audioSource.Play();
        if (i == 0)
        {
            addBankDetails.DOAnchorPos(Vector2.zero, time);
        }
        else if (i == 1)
        {
            addBankDetails.DOAnchorPos(sidePositionTransform, time);
        }
    } 

    public void WithdrawMoney(int i)
    {
        audioSource.Play();
        if (i == 0)
        {
            withdrawDetails.DOAnchorPos(Vector2.zero, time);
        }
        else if (i == 1)
        {
            withdrawDetails.DOAnchorPos(sidePositionTransform, time);
        }
    } 

    public void ADDMoney(int i)
    {
        audioSource.Play();
        if (i == 0)
        {
            addMoney.DOAnchorPos(Vector2.zero, time);
        }
        else if (i == 1 || Input.GetKey(KeyCode.Escape))
        {
            addMoney.DOAnchorPos(sidePositionTransform, time);
        }
    }

    public void MyProfileHome(int i)
    {
        audioSource.Play();
        if (i == 0)
        {
            myProfile.DOAnchorPos(Vector2.zero, time);
        }
        else if (i == 1)
        {
            myProfile.DOAnchorPos(sidePositionTransform, time);
        }
    }

    public void UPI(int i)
    {
        audioSource.Play();
        if (i == 0)
        {
            upi.DOAnchorPos(Vector2.zero, time);
        }
        else if (i == 1)
        {
            upi.DOAnchorPos(sidePositionTransform, time);
        }
    }

    public void HelpDesk(int i)
    {
        audioSource.Play();
        if (i == 0)
        {
            helpDesk.DOAnchorPos(Vector2.zero, time);
        }
        else if (i == 1)
        {
            helpDesk.DOAnchorPos(sidePositionTransform, time);
        }
    }

    public void PrivacyPolicy(int i)
    {
        audioSource.Play();
        if (i == 0)
        {
            privacyPolicy.DOAnchorPos(Vector2.zero, time);
        }
        else if (i == 1)
        {
            privacyPolicy.DOAnchorPos(sidePositionTransform, time);
        }
    }

    public void TermsAndCondition(int i)
    {
        audioSource.Play();
        if (i == 0)
        {
            termsAndCondition.DOAnchorPos(Vector2.zero, time);
        }
        else if (i == 1)
        {
            termsAndCondition.DOAnchorPos(sidePositionTransform, time);
        }
    }

    public void ConfirmAmountPlay(int i)
    {
        audioSource.Play();
        if (i == 0)
        {
            UIManager.Instance.confirm_Payment.SetActive(true);
            confirmpay = true;
            confirmPlayFee.DOAnchorPos(Vector2.zero, time);
        }
        else if (i == 1)
        {
            confirmpay = false;

            UIManager.Instance.confirm_Payment.SetActive(false);
            confirmPlayFee.DOAnchorPos(downPositionTransform, time);
        }
    }

    public void ContantUSS(int i)
    {
        audioSource.Play();
        if (i == 0)
        {
            contantuss = true;
            contantus.DOAnchorPos(Vector2.zero, time);
        }
        else if (i == 1)
        {
            contantuss = false;
            contantus.DOAnchorPos(downPositionTransform, time);
        }
    }

    public void MatchPlayer(int i)
    {
        audioSource.Play();
        if (i == 0)
        {
            matchingObj.DOAnchorPos(Vector2.zero, time);
        }
        else if (i == 1)
        {
            matchingObj.DOAnchorPos(downPositionTransform, time);
        }

    }

    public void FAQ(int i)
    {
        audioSource.Play();
        if (i == 0)
        {
            faqPanel.DOAnchorPos(Vector2.zero, time);
        }
        else if (i == 1)
        {
            faqPanel.DOAnchorPos(sidePositionTransform, time);
        }
    }

    public void HowToPlays(int i)
    {
        audioSource.Play();
        if (i == 0)
        {
            howTOPlay.DOAnchorPos(Vector2.zero, time);
        }
        else if (i == 1)
        {
            howTOPlay.DOAnchorPos(sidePositionTransform, time);
        }
    } 
    public void ChangeAvatars(int i)
    {
        audioSource.Play();
        if (i == 0)
        {
            changeAvatar.DOAnchorPos(Vector2.zero, time);
        }
        else if (i == 1)
        {
            changeAvatar.DOAnchorPos(sidePositionTransform, time);
        }
    } 
    public void OnprizePoolDisplay(int i)
    {
        audioSource.Play();
        if (i == 0)
        {
            prizePoolDisplay.DOAnchorPos(Vector2.zero, time);
        }
        else if (i == 1)
        {
            prizePoolDisplay.DOAnchorPos(sidePositionTransform, time);
        }
    }
    public void OnprizePoolDisplayTwo(int i)
    {
        audioSource.Play();
        if (i == 0)
        {
            display_Two_Ranks.DOAnchorPos(Vector2.zero, time);
        }
        else if (i == 1)
        {
            display_Two_Ranks.DOAnchorPos(sidePositionTransform, time);
        }
    }
    public void OnprizePoolDisplayFour(int i)
    {
        audioSource.Play();
        if (i == 0)
        {
            display_Four_Ranks.DOAnchorPos(Vector2.zero, time);
        }
        else if (i == 1)
        {
            display_Four_Ranks.DOAnchorPos(sidePositionTransform, time);
        }
    }



    #region IEnumerator
    IEnumerator BackToPostion(int i)
    {
        if (i == 1)
        {
            yield return new WaitForSeconds(0.5f);
            settingMenu.DOAnchorPos(sidePositionTransform, time);
        }
        else if (i == 2)
        {
            yield return new WaitForSeconds(0.5f);
            referMenu.DOAnchorPos(sidePositionTransform, time);
        }
        else if (i == 3)
        {
            yield return new WaitForSeconds(0.5f);
            mainMenu.DOAnchorPos(sidePositionTransform, time);
        }
       
    }
    #endregion
}
