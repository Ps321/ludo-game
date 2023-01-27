using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DisplayPrizePool : MonoBehaviour
{
    public TMP_Text text;
    public GameObject prizePool;
    public GameObject prizePoolTwo;
    public GameObject prizePoolFour;

    private void Start()
    {
        text.text = StaticString.ruppeSymbol + GameManager.Instance.prizePoolMoney;
    }
    public void BTEventID()
    {        
        StartCoroutine(ClickBtPool());
    }
    IEnumerator ClickBtPool()
    {

        yield return null;
        if (GameManager.Instance.numberOfTotalPlayers == 2)
        {
            prizePoolTwo.SetActive(true);
        }
        else if (GameManager.Instance.numberOfTotalPlayers == 4)
        {
            //prizePool.SetActive(true);
            prizePoolFour.SetActive(true);
        }
        prizePool.SetActive(true);
    }
}
