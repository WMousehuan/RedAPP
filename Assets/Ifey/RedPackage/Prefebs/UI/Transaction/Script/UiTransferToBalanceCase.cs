using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Newtonsoft.Json;
using static UiWithdrawalCase;
public class UiTransferToBalanceCase : Popup
{
    private string transferUrl= "/app-api/member/user/updateBrokerage";
    public TMP_InputField value_InputField;
    public float _value
    {
        get
        {
            int.TryParse(value_InputField.text, out int value);
            return value;
        }
    }
    public Text valueLimit_Text;
    float limitValue = 0;

    UiUserBalance uiUserBalance;
    public void Init(UiUserBalance uiUserBalance)
    {
        this.uiUserBalance = uiUserBalance;
        this.limitValue = RedPackageAuthor.Instance.currentUserCommissionBalance;
        valueLimit_Text.text = "The transfer amount is " + limitValue.ToString("F2"); ;
    }

    public void Transfer()
    {
        if (string.IsNullOrEmpty(value_InputField.text))
        {
            MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "The value cannot be empty");
            return;
        }
        float value = this._value;
        if (value <= 0)
        {
            MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "The value cannot be less than or equal to 0");
            return;
        }
        if (value > limitValue)
        {
            MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "The value cannot be greater than the balance");
            return;
        }

        var dataPack = new
        {
            optCash = value,
        };
        UiWaitMask waitMask_Ui = (UiWaitMask)PopupManager.Instance.Open(PopupType.PopupWaitMask);
        string transferUrl = this.transferUrl + "?optCash=" + value;
        UtilJsonHttp.Instance.PutObjectWithParamAuthorizationToken(transferUrl, dataPack, null, (resultData) => {

            ReturnData<string> returnData = JsonConvert.DeserializeObject<ReturnData<string>>(resultData);
            if (string.IsNullOrEmpty(returnData.data))
            {
                //print(returnData.code);
                waitMask_Ui?.ShowResultCase("Failed to transfer Balance", 1);
                return;
            }
            waitMask_Ui?.ShowResultCase("Transfer Success", 1);
            PopupManager.Instance.Close(this);

            RedPackageAuthor.Instance.realUserCommissionBalance -= value;

        }, (code, msg) =>
        {
            waitMask_Ui?.ShowResultCase("Failed to transfer Balance", 1);
            print(code + "||" + msg);
        });

    }

    public void OnEventTransfer()
    {
        Transfer();
    }

}
