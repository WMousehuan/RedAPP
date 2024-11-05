using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Newtonsoft.Json;
public class UiWithdrawalCase : Popup
{
    //public enum AmountType
    //{
    //    Default,
    //    Commission,//佣金
    //}
    public BalanceType balanceType;

    public string withdrawalUrl = "/app-api/red/order/toWithdraw";

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

    public TMP_InputField payeeUserAccount_InputField;
    public float _payeeUserAccount
    {
        get
        {
            int.TryParse(payeeUserAccount_InputField.text, out int payeeUserAccount);
            return payeeUserAccount;
        }
    }
    public float limitValue;
    public enum PayCodeType
    {
        WALLET,//电子钱包
        NBK,//网银
        ITT,//国际电汇
        VC//虚拟币
    }
    public Dropdown paycode_DropDown;
    public enum BizCodeType
    {
        NUBAN,//尼日
        BASA,//南非
        CPF,//巴西
        IMPS,//印度
        NEFT,//印度
        RTGS,//印度
        UPI,//印度
    }
    public Dropdown bizCode_DropDown;
    public enum CurrencyType
    {
        NGN,//尼日
        BRL,//南非
        ZAR,//巴西
        INR,//印度
    }
    public Dropdown currency_DropDown;

    private void Awake()
    {
        paycode_DropDown.options.Clear();
        foreach (string type in Enum.GetNames(typeof(PayCodeType)))
        {
            if ((int)Enum.Parse<PayCodeType>(type) >= -1)
            {
                paycode_DropDown.options.Add(new Dropdown.OptionData( type.ToString()));
            }
        }
        bizCode_DropDown.options.Clear();
        foreach (string type in Enum.GetNames(typeof(BizCodeType)))
        {
            if ((int)Enum.Parse<BizCodeType>(type) >= -1)
            {
                bizCode_DropDown.options.Add(new Dropdown.OptionData(type.ToString()));
            }
        }
        currency_DropDown.options.Clear();
        foreach (string type in Enum.GetNames(typeof(CurrencyType)))
        {
            if ((int)Enum.Parse<CurrencyType>(type) >= -1)
            {
                currency_DropDown.options.Add(new Dropdown.OptionData(type.ToString()));
            }
        }
    }
    public void Init(BalanceType balanceType, float limitValue)
    {
        this.balanceType = balanceType;
        this.limitValue = limitValue;
        valueLimit_Text.text = "The withdrawable amount is " + limitValue;
    }

    public void Withdarwal()
    {
        if (string.IsNullOrEmpty(value_InputField.text))
        {
            MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "The value cannot be empty");
            return;
        }
        
        if (this._value <= 0)
        {
            MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "The value cannot be less than or equal to 0");
            return;
        }
        if (this._value > limitValue)
        {
            MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "The value cannot be greater than the balance");
            return;
        }
        if (string.IsNullOrEmpty(payeeUserAccount_InputField.text))
        {
            MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "Account Number cannot be empty");
        }
        var dataPack = new
        {
            optCase = this._value,
            optType = (int)balanceType,
            payeeUserAccount = this._payeeUserAccount,
            payCode = ((PayCodeType)paycode_DropDown.value).ToString(),
            bizCode = ((BizCodeType)bizCode_DropDown.value).ToString(),
            currency = ((CurrencyType)currency_DropDown.value).ToString(),
        }; 
        UiWaitMask waitMask_Ui = (UiWaitMask)PopupManager.Instance.Open(PopupType.PopupWaitMask);
        UtilJsonHttp.Instance.PutObjectWithParamAuthorizationToken(withdrawalUrl, dataPack, null, (resultData) => {
            
            ReturnData<bool> returnData = JsonConvert.DeserializeObject<ReturnData<bool>>(resultData);
            switch (returnData.data)
            {
                case true:
                    waitMask_Ui?.ShowResultCase("Successfully initiated withdrawal", 1);
                    OnEventClose();
                    IEPool_Manager.instance.WaitTimeToDo("", 4, null, () => {
                        UserManager.Instance.GetUserMainInfo();
                    });
                    break;
                case false:
                    waitMask_Ui?.ShowResultCase("Failed to initiate withdrawal", 1);
                    break;
            }
        }, (code,msg) => {
            waitMask_Ui?.ShowResultCase("Failed to initiate withdrawal", 1);
        });
    }
}
