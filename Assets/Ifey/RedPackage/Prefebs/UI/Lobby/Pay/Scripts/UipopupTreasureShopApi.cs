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
    [HideInInspector]
    string createOrderApi = "/app-api/red/cash-recharge/create";

    public void BuyCoinHttp(long coinNumber)
    {
        try
        {
            Debug.Log("Start grab pkg!");
            AppCashRechargeSaveReqVO appCashRechargeSaveReqVO = new AppCashRechargeSaveReqVO();
            appCashRechargeSaveReqVO.optCash = coinNumber;
            //when start the game,get the userInfo
            UtilJsonHttp.Instance.PostRequestWithParamAuthorizationToken(createOrderApi, appCashRechargeSaveReqVO, new UipopupTreasureShopApiRespond(this));
        }
        catch (Exception e)
        {
            Debug.LogError("UipopupTreasureShopApi An exception occurred: " + e.Message);
            // Handle the exception, for example display an error message or log the exception
        }
    }
    public void MakeBuyProductThrillGame(int indexProduct)
    {

        BuyCoinHttp(indexProduct);

    }
}



public class UipopupTreasureShopApiRespond : HttpInterface
{
    public FailPubDo failPubDo = new FailPubDo();
    UipopupTreasureShopApi uipopupTreasureShopApi;
    // 构造方法
    public UipopupTreasureShopApiRespond(UipopupTreasureShopApi uipopupTreasureShopApi)
    {
        this.uipopupTreasureShopApi = uipopupTreasureShopApi;
    }
    public void Success(string result)
    {
        //refresh ammount
        ReturnData<string> responseData = JsonConvert.DeserializeObject<ReturnData<string>>(result);
        MonoSingleton<UIManager>.Instance.ShowGetCoinEffect(uipopupTreasureShopApi.transform.parent, new Vector2(0f, 100f), Coins, 10); //show coin effect
        // 实现 Success 方法的逻辑
        Debug.Log("Success TreasureGrabRespond=" + responseData.code.ToString());
        MonoSingleton<PopupManager>.Instance.CloseAllPopup();
        //显示明细

    }
    void Coins()
    {
        MonoSingleton<UserManager>.Instance.GetUserMainInfo();
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
