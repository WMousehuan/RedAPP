using Assets.Ifey.RedPackage.Prefebs.UI.Lobby.Channel.Cheasure.Scripts.Bean;
using Gley.EasyIAP;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BestHTTP.JSON.LitJson;

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
    [HideInInspector]
    string rechargeUrl = "/app-api/red/order/toRecharge";

    public void BuyCoinHttp(double coinNumber, double rewardAmount)
    {
        try
        {
            Debug.Log("Start grab pkg!");
            AppCashRechargeSaveReqVO appCashRechargeSaveReqVO = new AppCashRechargeSaveReqVO();
            appCashRechargeSaveReqVO.optCash = coinNumber;

            UtilJsonHttp.Instance.PostRequestWithParamAuthorizationToken(rechargeUrl, appCashRechargeSaveReqVO, null, (resultData) =>
            {
                print(resultData);
                JsonData jsonData = JsonMapper.ToObject(resultData);
                if (jsonData.ContainsKey("data")&& jsonData["data"]!=null)
                {
                    string url = jsonData["data"].ToString();
                    if (!string.IsNullOrEmpty(url))
                    {
#if UNITY_EDITOR || PLATFORM_ANDROID
                        Application.OpenURL(url);
#elif UNITY_WEBGL
                        WebMessage_Ctrl.SendMessageToWeb("openUrl^"+url);
#endif
                        //System.Action loopChackResultAction = null;
                        //loopChackResultAction = () => 
                        //{
                        //    IEPool_Manager.instance.WaitTimeToDo(null, 5, null, () => {
                                
                        //    });
                        //};
                    }
                }
            });
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
        RedPackageAuthor.Instance.userBalance += (float)value;
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
}
