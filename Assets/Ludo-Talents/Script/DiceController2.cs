using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceController2 : MonoBehaviour
{

    public GameObject mainDice;


    public void FinishAnim()
    {
        mainDice.GetComponent<GameDiceController>().SetDiceValue();
    }

}
