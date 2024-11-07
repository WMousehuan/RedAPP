using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Newtonsoft.Json;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using static UiRechargeDetail;
public class UiWithdrawalCase : Popup
{
    //public enum AmountType
    //{
    //    Default,
    //    Commission,//Ӷ��
    //}
    //public BalanceType balanceType;

    public UiUserBalance uiUserBalance;

    public string withdrawalUrl = "/app-api/red/order/toWithdraw";
    public string catchWithdrawalUrl = "/app-api/red/cash-withdraw/page";
    public string withdrawalRuleUrl = "";

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
    public TMP_InputField userName_InputField;
    public TMP_InputField ifscCode_InputField;
    public float limitValue;
    public enum PayCodeType
    {
        WALLET,//����Ǯ��
        NBK,//����
        ITT,//���ʵ��
        VC//�����
    }
    public Dropdown paycode_DropDown;
    public enum BizCodeType
    {
        NUBAN,//����
        BASA,//�Ϸ�
        CPF,//����
        IMPS,//ӡ��
        NEFT,//ӡ��
        RTGS,//ӡ��
        UPI,//ӡ��
    }
    public Dropdown bizCode_DropDown;
    public enum CurrencyType
    {
        NGN,//����
        BRL,//�Ϸ�
        ZAR,//����
        INR,//ӡ��
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
        paycode_DropDown.value = 0;
        bizCode_DropDown.options.Clear();
        foreach (string type in Enum.GetNames(typeof(BizCodeType)))
        {
            if ((int)Enum.Parse<BizCodeType>(type) >= -1)
            {
                bizCode_DropDown.options.Add(new Dropdown.OptionData(type.ToString()));
            }
        }
        bizCode_DropDown.value = 3;
        currency_DropDown.options.Clear();
        foreach (string type in Enum.GetNames(typeof(CurrencyType)))
        {
            if ((int)Enum.Parse<CurrencyType>(type) >= -1)
            {
                currency_DropDown.options.Add(new Dropdown.OptionData(type.ToString()));
            }
        }
        currency_DropDown.value = 3;
    }
    public void Init(UiUserBalance uiUserBalance, float limitValue)
    {
        this.uiUserBalance = uiUserBalance;
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
        if (string.IsNullOrEmpty(userName_InputField.text))
        {
            MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "Name of payee cannot be empty");
            return;
        }
        if (string.IsNullOrEmpty(ifscCode_InputField.text))
        {
            MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "IFSC Code cannot be empty");
            return;
        }
        var dataPack = new
        {
            optCash = this._value,
            optType = (int)this.uiUserBalance.balanceType,
            payeeUserAccount = this.payeeUserAccount_InputField.text,
            payeeBranchCode = this.ifscCode_InputField.text,
            payeeUsername = this.userName_InputField.text,
            payCode = ((PayCodeType)paycode_DropDown.value).ToString(),
            bizCode = ((BizCodeType)bizCode_DropDown.value).ToString(),
            currency = ((CurrencyType)currency_DropDown.value).ToString(),
        }; 
        UiWaitMask waitMask_Ui = (UiWaitMask)PopupManager.Instance.Open(PopupType.PopupWaitMask);
        UtilJsonHttp.Instance.PutObjectWithParamAuthorizationToken(withdrawalUrl, dataPack, null, (resultData) => {
            
            ReturnData<string> returnData = JsonConvert.DeserializeObject<ReturnData<string>>(resultData);
            if (string.IsNullOrEmpty(returnData.data))
            {
                print(returnData.code);
                waitMask_Ui?.ShowResultCase("Failed to initiate withdrawal", 1);
                return;
            }
            waitMask_Ui?.ShowResultCase("Withdrawing is in progress", 1);
            //waitMask_Ui.Init("Withdrawing is in progress");

            switch (uiUserBalance.balanceType)
            {
                case  BalanceType.Commission:
                    RedPackageAuthor.Instance.withdrawalCommissionBalanceAmount += this._value;
                    break;
                case BalanceType.Default:
                    RedPackageAuthor.Instance.withdrawalBalanceAmount += this._value;
                    break;
            }
            PopupManager.Instance.Close(this);
            uiUserBalance.OnEnable();
            //System.Action loopAction = null;
            //int stateIndex = 0;
            //float catchTime = 20;
            //loopAction = () => {
            //    switch (stateIndex)
            //    {
            //        default:
            //            catchTime = 5;
            //            break;
            //    }
            //    stateIndex++;
            //    IEPool_Manager.instance.WaitTimeToDo("WithdrawalCatch" + returnData.data, catchTime, null, () => {
            //        string catchWithdrawalUrl = this.catchWithdrawalUrl + "?orderNo=" + returnData.data;
            //        UtilJsonHttp.Instance.GetRequestWithAuthorizationToken(catchWithdrawalUrl, null, (resultData) => {
            //            //��ѯ����״̬ δ���
            //            ReturnData<PageResultPacketSendRespVO<PurchaseOrderDataVO>> returnData = JsonConvert.DeserializeObject<ReturnData<PageResultPacketSendRespVO<PurchaseOrderDataVO>>>(resultData);
            //            PurchaseOrderDataVO purchaseOrderDataVO = returnData.data.list[0];
            //            switch (purchaseOrderDataVO.optCashStatus)
            //            {
            //                case 0://��������
            //                    loopAction?.Invoke();
            //                    break;
            //                case 1://���ֳɹ�
            //                    waitMask_Ui?.ShowResultCase("Withdrawal successful", 1);
            //                    switch (this.uiUserBalance.balanceType)
            //                    {
            //                        case BalanceType.Default:
            //                            RedPackageAuthor.Instance.realUserBalance -= this._value;
            //                            //RedPackageAuthor.Instance.withdrawalBalanceAmount-= this._value;
            //                            break;
            //                        case BalanceType.Commission:
            //                            RedPackageAuthor.Instance.realUserCommissionBalance -= this._value;
            //                            break;
            //                    }
            //                    break;
            //                case 2://����ʧ��
            //                    switch (returnData.code)
            //                    {
            //                        case 1022004001:
            //                            MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "Withdrawals have exceeded 2 times, please try again tomorrow");
            //                            waitMask_Ui?.ShowResultCase("Failed to initiate withdrawal", 0);
            //                            break;
            //                        case 1004001004:
            //                            MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "Insufficient account balance");
            //                            waitMask_Ui?.ShowResultCase("Failed to initiate withdrawal", 0);
            //                            break;
            //                        case 1004001006:
            //                            MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "Insufficient commission");
            //                            waitMask_Ui?.ShowResultCase("Failed to initiate withdrawal", 0);
            //                            break;
            //                        default:
            //                            waitMask_Ui?.ShowResultCase("Failed to initiate withdrawal", 1);
            //                            break;
            //                    }
            //                    waitMask_Ui?.ShowResultCase("Withdrawal Failed", 0);
            //                    break;
            //                case 3://����ȡ��
            //                    waitMask_Ui?.ShowResultCase("Withdrawal Cancle", 1);
            //                    break;
            //            }
            //        }, (code, msg) => {
            //            loopAction?.Invoke();
            //        });
            //    });
            //};
            //loopAction?.Invoke();
        }, (code,msg) => 
        {
            switch (code)
            {
                case 1022004001:
                    MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "Today's withdrawal frequency exceeded the limit, please try again tomorrow");
                    waitMask_Ui?.ShowResultCase("Failed to initiate withdrawal", 0);
                    break;
                case 1004001004:
                    MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "Insufficient account balance");
                    waitMask_Ui?.ShowResultCase("Failed to initiate withdrawal", 0);
                    break;
                case 1004001006:
                    MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "Insufficient commission");
                    waitMask_Ui?.ShowResultCase("Failed to initiate withdrawal", 0);
                    break;
                default:
                    waitMask_Ui?.ShowResultCase("Failed to initiate withdrawal", 1);
                    break;
            }
            print(code+"||"+msg);
        });
    }
}
