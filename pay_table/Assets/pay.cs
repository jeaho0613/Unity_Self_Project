using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pay : MonoBehaviour
{
    public InputField inputField;
    public Text totalPay, text2;
    public float gold;

    void Start()
    {

    }

    void Update()
    {

    }

    public void okBnt()
    {
        string strPay = inputField.text;
        float pay = float.Parse(strPay);
        gold += checkPay(pay);
        totalPay.text = "총 급여 : " + gold.ToString();

    }

    public float checkPay(float pay)
    {
        if (pay >= 110)
        {
            pay = pay * 300;
            Debug.Log($"pay * 300 으로 계산됐습니다. : {pay}");
            text2.text = $"pay * 300 으로 계산됐습니다. : {pay}";
            return pay;
        }
        else if (pay >= 90 && pay <= 109)
        {
            pay = pay * 300 * 0.9f;
            Debug.Log($"pay * 300 * 0.9f 으로 계산됐습니다. : {pay}");
            text2.text = $"pay * 300 * 0.9f 으로 계산됐습니다. : {pay}";
            return pay;
        }
        else if (pay >= 70 && pay <= 89)
        {
            pay = pay * 300 * 0.8f;
            Debug.Log($"pay * 300 * 0.8f 으로 계산됐습니다. : {pay}");
            text2.text = $"pay * 300 * 0.8f 으로 계산됐습니다. : {pay}";
            return pay;
        }
        else if (pay >= 50 && pay <= 69)
        {
            pay = pay * 300 * 0.7f;
            Debug.Log($"pay * 300 * 0.7f으로 계산됐습니다. : {pay}");
            text2.text = $"pay * 300 * 0.7f으로 계산됐습니다. : {pay}";
            return pay;
        }
        else if (pay >= 30 && pay <= 49)
        {
            pay = pay * 300 * 0.6f;
            Debug.Log($"pay * 300 * 0.6f으로 계산됐습니다. : {pay}");
            text2.text = $"pay * 300 * 0.6f으로 계산됐습니다. : {pay}";
            return pay;
        }
        else
        {
            Debug.LogError("범주에서 벗어납니다.");
            text2.text = $"범주에서 벗어납니다. : {pay}";
        }
        return pay;
    }
}
