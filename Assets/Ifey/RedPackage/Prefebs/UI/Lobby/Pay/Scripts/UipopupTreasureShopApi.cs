using Assets.Ifey.RedPackage.Prefebs.UI.Lobby.Channel.Cheasure.Scripts.Bean;
using Gley.EasyIAP;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UipopupTreasureShopApi : MonoBehaviour
{
    //[HideInInspector]
    //string createOrderApi = "/app-api/red/cash-recharge/create";

    //public void BuyCoinHttp(double coinNumber, double rewardAmount)
    //{
    //    try
    //    {
    //        Debug.Log("Start grab pkg!");
    //        AppCashRechargeSaveReqVO appCashRechargeSaveReqVO = new AppCashRechargeSaveReqVO();
    //        appCashRechargeSaveReqVO.optCash = coinNumber;
    //        //when start the game,get the userInfo
    //        UtilJsonHttp.Instance.PostRequestWithParamAuthorizationToken(createOrderApi, appCashRechargeSaveReqVO, new UipopupTreasureShopApiRespond(this, coinNumber + rewardAmount));
    //    }
    //    catch (Exception e)
    //    {
    //        Debug.LogError("UipopupTreasureShopApi An exception occurred: " + e.Message);
    //        // Handle the exception, for example display an error message or log the exception
    //    }
    //}
    private string createRechargeUrl = "/app-api/red/order/toRecharge";//发起订单
    private string cancleRechargeUrl = "/app-api/red/order/cancelRecharge/{0}";//取消订单
    private string catchRechargeUrl = "/app-api/red/cash-recharge/page";//查询订单
    private IEnumerator catchRechargeIE;
    public void BuyCoinHttp(double coinNumber, double rewardAmount)
    {
        try
        {
            Debug.Log("Start grab pkg!");
            AppCashRechargeSaveReqVO appCashRechargeSaveReqVO = new AppCashRechargeSaveReqVO();
            appCashRechargeSaveReqVO.optCash = coinNumber;
            UiWaitMask waitMask_Ui = (UiWaitMask)PopupManager.Instance.Open(PopupType.PopupWaitMask);
            UtilJsonHttp.Instance.PostRequestWithParamAuthorizationToken(createRechargeUrl, appCashRechargeSaveReqVO, null, (Action<string>)((resultData) =>
            {
                //print(resultData);
                ReturnData<PurchaseOrderDataVO> returnData = JsonConvert.DeserializeObject<ReturnData<PurchaseOrderDataVO>>(resultData);
                if (!string.IsNullOrEmpty(returnData.data.payUrl))
                {
                    UiPurchaseCase.instance.closeAction = () => {
                        waitMask_Ui?.ShowResultCase("Cancle recharge", 0);
                    }; 
                    UiPurchaseCase.instance.timeOverAction = () => {
                        UiPurchaseCase.instance.Close();
                        IEPool_Manager.instance.StopIE(catchRechargeIE);
                        waitMask_Ui?.ShowResultCase("Cancle recharge", 1);
                    };
                    UiPurchaseCase.instance.OpenUrl(returnData.data.payUrl);
                    
                    long timeStamp = 0;
                    if (long.TryParse(returnData.data.expirationTime, out timeStamp))
                    {

                    }
                    System.Action loopAction = null;
                    int stateIndex = 0;
                    float catchTime = 20;
                    float limitTime = timeStamp - GeneralTool_Ctrl.GetTimeStamp();
                    UiPurchaseCase.instance.targetStampTime = timeStamp;
                    loopAction = () => {
                        limitTime = timeStamp - GeneralTool_Ctrl.GetTimeStamp();
                        if (limitTime<=0)
                        {
                            UiPurchaseCase.instance.Close();
                            waitMask_Ui?.ShowResultCase("Recharge timeout", 1);
                            return;
                        }
                        switch (stateIndex)
                        {
                            case 0:
                                catchTime = 20;
                                break;
                            case 1:
                                catchTime = 15;
                                break;
                            case 2:
                                catchTime = 10;
                                break;
                            default:
                                catchTime = 5;
                                break;
                        }
                        stateIndex++;
                        IEPool_Manager.instance.WaitTimeToDo("RechargeCatch" + returnData.data.orderNo, catchTime, null, () => {
                            string catchRechargeUrl = this.catchRechargeUrl + "?orderNo=" + returnData.data.orderNo;
                            UtilJsonHttp.Instance.GetRequestWithAuthorizationToken(catchRechargeUrl, null, (resultData) => {
                                //查询订单状态 未完成
                                 ReturnData<PageResultPacketSendRespVO<PurchaseOrderDataVO>> returnData = JsonConvert.DeserializeObject<ReturnData<PageResultPacketSendRespVO<PurchaseOrderDataVO>>>(resultData);
                                PurchaseOrderDataVO purchaseOrderDataVO = returnData.data.list[0];
                                switch (purchaseOrderDataVO.rechargeStatus)
                                {
                                    case 0://正在充值
                                        loopAction?.Invoke();
                                        break;
                                    case 1://充值成功
                                        UiPurchaseCase.instance.Close();
                                        waitMask_Ui?.ShowResultCase("Recharge successful", 1);
                                        UIManager.Instance.ShowGetCoinEffect(base.transform, new Vector2(0, 100), () => {
                                            RedPackageAuthor.Instance.realUserBalance += (purchaseOrderDataVO.optCash + purchaseOrderDataVO.awardCash);
                                        }, 10);
                                        break;
                                    case 2://充值失败
                                        UiPurchaseCase.instance.Close();
                                        waitMask_Ui?.ShowResultCase("Recharge Failed", 1);
                                        break;
                                    case 3://充值取消
                                        UiPurchaseCase.instance.Close();
                                        waitMask_Ui?.ShowResultCase("Cancle recharge", 1);
                                        break;
                                }
                            }, (code,msg) => {
                                loopAction?.Invoke();
                            });
                        });
                    };
                    loopAction?.Invoke();
                }

            }));
        }
        catch (Exception e)
        {
            Debug.LogError("UipopupTreasureShopApi An exception occurred: " + e.Message);
            // Handle the exception, for example display an error message or log the exception
        }
    }
    public void MakeBuyProductThrillGame(double indexProduct, double rewardAmount)
    {

        BuyCoinHttp(indexProduct, rewardAmount);

    }
}



public class UipopupTreasureShopApiRespond : HttpInterface
{
    public FailPubDo failPubDo = new FailPubDo();
    UipopupTreasureShopApi source_Ctrl;
    double value = 0;
    // 构造方法
    public UipopupTreasureShopApiRespond(UipopupTreasureShopApi uipopupTreasureShopApi, double value)
    {
        this.source_Ctrl = uipopupTreasureShopApi;
        this.value = value;
    }
    public void Success(string result)
    {
        if (source_Ctrl == null)
        {
            return;
        }
        //refresh ammount
        ReturnData<string> responseData = JsonConvert.DeserializeObject<ReturnData<string>>(result);
        if (source_Ctrl != null)
            MonoSingleton<UIManager>.Instance.ShowGetCoinEffect(source_Ctrl.transform.parent, new Vector2(0f, 100f), Coins, 10); //show coin effect
        // 实现 Success 方法的逻辑
        Debug.Log("Success TreasureGrabRespond=" + responseData.code.ToString());
        MonoSingleton<PopupManager>.Instance.CloseAllPopup();
        //显示明细

    }
    void Coins()
    {
        RedPackageAuthor.Instance.realUserBalance += (float)value;
        //MonoSingleton<UserManager>.Instance.GetUserMainInfo();
    }

    public void Fail(JObject json)
    {
        if (!failPubDo.failPubdo(json))
        {
            int code = json["code"].Value<int>();
            if (code == 1022000003)
            {
                MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "U already Grabed!");
            }
            else if (code == 1)
            {
                MonoSingleton<PopupManager>.Instance.CloseAllPopup();
                MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "All coin have been Grab.");
            }
            else
            {
                MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "Grab money Fail!");
            }
        }

    }

    public void UnknowError(string errorMsg)
    {
        Debug.LogError("TreasureGrabRespond" + errorMsg);
    }

    
    //public class OrderDataVO
    //{
    //    public string orderNo;
    //    public string rechargeStatus;
    //    public string createTime;
    //    public string expirationTime;
    //}
}
public class PurchaseOrderDataVO
{
    public int id;
    public string orderNo;
    public string payUrl;
    public float optCash;
    public float awardCash;
    public int balanceType;
    public int rechargeStatus;
    public int optCashStatus;
    public string createTime;
    public string expirationTime;
}
