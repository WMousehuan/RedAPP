using Assets.Ifey.RedPackage.Prefebs.UI.Lobby.Channel.Scripts;
using Assets.Ifey.RedPackage.Prefebs.UI.Login.Script;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Ifey.RedPackage.Prefebs.UI.Lobby.Channel.Cheasure.Scripts.Bean;
using System.Drawing;

/// <summary>
/// 抢红包
/// </summary>
public class TreasureGrab : MonoBehaviour
{
    [HideInInspector]
    string grabUrl = "/app-api/red/packet-receive/getRedPacket";
    public PackageItem packageItem;
    public void grabPkg()
    {
        try
        {
            Debug.Log("Start grab pkg!");
            AppPacketReceiveSaveReqVO appPacketReceiveSaveReqVO = new AppPacketReceiveSaveReqVO();
            appPacketReceiveSaveReqVO.redPacketSendId = packageItem.packetSendRespVO.id; //红包ID
            //appPacketReceiveSaveReqVO.channelId = packageItem.packetSendRespVO.channelId;//校验抢包资格
            //when start the game,get the userInfo
            UtilJsonHttp.Instance.PostRequestWithParamAuthorizationToken(grabUrl, appPacketReceiveSaveReqVO, new TreasureGrabRespond(this));
        }
        catch (Exception e)
        {
            Debug.LogError("An exception occurred: " + e.Message);
            // Handle the exception, for example display an error message or log the exception
        }
    }
}



public class TreasureGrabRespond : HttpInterface
{
    public FailPubDo failPubDo = new FailPubDo();
    TreasureGrab treasureGrab;
    // 构造方法
    public TreasureGrabRespond(TreasureGrab treasureGrab)
    {
        this.treasureGrab = treasureGrab;
    }
    public void Success(string result)
    {
        //MonoSingleton<PopupManager>.Instance.CloseAllPopup();
        ReturnData<AppPacketReceiveRespVO> responseData = JsonConvert.DeserializeObject<ReturnData<AppPacketReceiveRespVO>>(result);
        // 实现 Success 方法的逻辑
        Debug.Log("Success TreasureGrabRespond=" + responseData.code.ToString());
        //显示明细
        treasureGrab.packageItem.openPackageDetailResultClick();
        MonoSingleton<UIManager>.Instance.ShowGetCoinEffect(this.treasureGrab.transform.parent, new Vector2(0f, 100f), Coins, 10); //show coin effect
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
            string msg = json["msg"].Value<string>();
            if (code == 1022000003)
            {
                MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "U already Grabed!");
            }
            else if (code == 1)
            {
                MonoSingleton<PopupManager>.Instance.CloseAllPopup();
                MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "All coin have been Grab.");
            }
            else {
                MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Info", msg);
            }
        }

    }

    public void UnknowError(string errorMsg)
    {
        Debug.LogError("TreasureGrabRespond" + errorMsg);
    }
}

public class AppPacketReceiveSaveReqVO
{
    public long redPacketSendId { get; set; }
    public long channelId { get; set; } // 红包游戏ID
    public long memberId { get; set; } //用户ID

    public override string ToString()
    {
        return $"AppPacketReceiveSaveReqVO [redPacketSendId={redPacketSendId}, memberId={memberId}]";
    }
}