using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PricePoolMode : MonoBehaviour
{
    public int eventID;
    public int tp_id;
    string mName;
    string pricePool;

    public void GetID(int id, int TP, string modename ,string pPool)
    {        
        eventID = id;
        tp_id = TP;
        mName = modename;
        pricePool = pPool;
    }

    public void BTEventID()
    {
        if (tp_id == 2)
        {           
            Display_Two_Ranks.Instance.OnShowPricePool(eventID, pricePool, mName);
            SliderMenuAnim.Instance.OnprizePoolDisplayTwo(0);
        }
        else if (tp_id == 4)
        {           
            Display_Four_Ranks.Instance.OnShowPricePool(eventID, pricePool, mName);
            SliderMenuAnim.Instance.OnprizePoolDisplayFour(0);
        }            
    }
}
