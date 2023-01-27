using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AddMoneyWithBt : MonoBehaviour
{

    public InputField inputAmount;
    public int amounts;
    public string amount;

  
  

    public void AddAmount(int index)
    {
        if (inputAmount.text.Length == 0)
        {
            amounts = 0;
        }
        else
        {
            amount = inputAmount.text;
            amounts = int.Parse(amount);
        }


        switch (index)
        {
            case 1:
                amounts = amounts + 50;
                inputAmount.text = amounts.ToString();
                break;
            case 2:
                amounts = amounts + 100;
                inputAmount.text = amounts.ToString();
                break;
            case 3:
                amounts = amounts + 200;
                inputAmount.text = amounts.ToString();
                break;


        }
    }
}
