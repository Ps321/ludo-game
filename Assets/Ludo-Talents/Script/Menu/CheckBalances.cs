using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CheckBalances : MonoBehaviour
{
    [SerializeField] TMP_Text homeBalance;
    [SerializeField] TMP_Text balance;
    [SerializeField] TMP_Text addedAmount;
    [SerializeField] TMP_Text winningAmount;
    [SerializeField] TMP_Text withdrawWwinningAmount;
    [SerializeField] TMP_Text bounceAmount;

    [Header("Button")]
    [SerializeField] Button homeButton;
    [SerializeField] Button eventButton;

    WWWForm form;

    private void Start()
    {
        StartCoroutine(LoadingPanel());     
        StartCoroutine(CheckBalance());

        /*homeButton.onClick.AddListener(_CheckBal);
        eventButton.onClick.AddListener(_CheckBal);*/

    }
    public void _CheckBal()
    {
        StartCoroutine(CheckBalance());
    }

    IEnumerator CheckBalance()
    {
        form = new WWWForm();
        form.AddField("userid", PlayerPrefs.GetString("userid"));
        WWW w = new WWW(StaticString.balance, form);
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
                Debug.Log("CheckBalance " + w.text);
                if (response.status)
                {
                    Player player = response.data;
                    balance.text = StaticString.ruppeSymbol + player.balance;
                    homeBalance.text = StaticString.ruppeSymbol + player.balance;
                    winningAmount.text = StaticString.ruppeSymbol + player.winningamount;
                    withdrawWwinningAmount.text = StaticString.ruppeSymbol + player.winningamount;
                    addedAmount.text = StaticString.ruppeSymbol + player.addedamount;
                    bounceAmount.text = StaticString.ruppeSymbol + player.bonusamount;
                    GameManager.Instance.winAmount = player.winningamount;
                    GameManager.Instance.checkbalforplay = player.balance;
                }
            }
        }
        yield return new WaitForSeconds(10f);
        StartCoroutine(CheckBalance());
    }

    IEnumerator LoadingPanel()
    {
        UIManager.Instance.LoadingPanel.SetActive(true);
        yield return new WaitForSeconds(3f);
        UIManager.Instance.LoadingPanel.SetActive(false);
    }
}
