using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class FaqScript : MonoBehaviour
{
    public GameObject logginIssue;
    public GameObject gameRelated;
    public GameObject paymentsRelated;
    public GameObject referralRelated;
    public GameObject profileRelated;

    public void LogginIssue(bool a)
    {
        if (a == true)
        {
            a = false;
            logginIssue.SetActive(true);
            gameRelated.SetActive(false);
            paymentsRelated.SetActive(false);
            referralRelated.SetActive(false);
            profileRelated.SetActive(false);
        }
        else
        {
            logginIssue.SetActive(false);
        }
       
    }

    public void GameRelated(bool a)
    {
        if (a == true)
        {
            a = false;
            gameRelated.SetActive(true);
            logginIssue.SetActive(false);
            paymentsRelated.SetActive(false);
            referralRelated.SetActive(false);
            profileRelated.SetActive(false);
        }
        else
        {
            gameRelated.SetActive(false);
        }
    }

    public void PaymentsRelated(bool a)
    {
        if (a == true)
        {
            a = false;
            paymentsRelated.SetActive(true);
            gameRelated.SetActive(false);
            logginIssue.SetActive(false);
            referralRelated.SetActive(false);
            profileRelated.SetActive(false);
        }
        else
        {
            paymentsRelated.SetActive(false);
        }
    }

    public void ReferralRelated(bool a)
    {
        if (a == true)
        {
            a = false;
            referralRelated.SetActive(true);
            gameRelated.SetActive(false);
            paymentsRelated.SetActive(false);
            logginIssue.SetActive(false);
            profileRelated.SetActive(false);
        }
        else
        {
            referralRelated.SetActive(false);
        }
    }

    public void ProfileRelated(bool a)
    {
        if (a == true)
        {
            a = false;
            profileRelated.SetActive(true);
            gameRelated.SetActive(false);
            paymentsRelated.SetActive(false);
            referralRelated.SetActive(false);
            logginIssue.SetActive(false);
        }
        else
        {
            profileRelated.SetActive(false);
        }
    }
}
