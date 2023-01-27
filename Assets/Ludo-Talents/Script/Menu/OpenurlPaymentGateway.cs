using UnityEngine;
using UnityEngine.UI; 

public class OpenurlPaymentGateway : MonoBehaviour
{
    [SerializeField] InputField amountAdd;
    public void Openpaymentgateway()
    {
        Application.OpenURL(StaticString.paymentgateway + $"?user_id={PlayerPrefs.GetString("userid")}&name={PlayerPrefs.GetString("username")}&amount={int.Parse(amountAdd.text)}&mobile={ PlayerPrefs.GetString("phone")}");
        SliderMenuAnim.Instance.MyBalance(1);
        SliderMenuAnim.Instance.ADDMoney(1);

    }
}
