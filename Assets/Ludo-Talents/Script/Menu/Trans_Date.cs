using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Trans_Date : MonoBehaviour
{
    TMP_Text myText;
   
    public void MyDateDis(string mydate)
    {
        myText = GetComponent<TMP_Text>();
        /*  string[] Separate = mydate.Split(' ');
         // string OilServiceDate = Separate[1];
          string OilServiceDate = Convert.ToDateTime(mydate).ToShortDateString();
          Debug.Log(OilServiceDate);*/

        System.DateTime theTime = System.DateTime.Parse(mydate);
        string date = theTime.Day + "-" + theTime.Month + "-" + theTime.Year;
        myText.text = date;
        
    }

    public void MyTimeDis(string mydate)
    {
        myText = GetComponent<TMP_Text>();
        System.DateTime theTime = System.DateTime.Parse(mydate);
        string time = theTime.Hour + ":" + theTime.Minute;
        myText.text = time;

    }
}
