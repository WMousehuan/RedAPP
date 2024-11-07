using Assets.Ifey.RedPackage.Prefebs.UI.Lobby.Channel.Scripts;
using Assets.Ifey.RedPackage.Prefebs.UI.Login.Script;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Ifey.RedPackage.Prefebs.UI.Lobby.Channel.Cheasure.Scripts.Bean;

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
            switch (packageItem.packetSendRespVO.redStatus)
            {
                case 0:
                    if (!packageItem.packetSendRespVO.isGrabed)
                    {
                        UiWaitMask waitMask_Ui = (UiWaitMask)PopupManager.Instance.Open(PopupType.PopupWaitMask);
                        UtilJsonHttp.Instance.PostRequestWithParamAuthorizationToken(grabUrl, appPacketReceiveSaveReqVO, new TreasureGrabRespond(this, waitMask_Ui));
                    }
                    else
                    {
                        packageItem.OpenPackageDetailResultClick();
                        MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Info", "U already Grabed!");
                    }
                    break;
                case 1:
                    packageItem.OpenPackageDetailResultClick();
                    MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Info", "All already Grabed!");
                    break;
                case 2:
                    packageItem.OpenPackageDetailResultClick();
                    MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Info", "Redpacket expired !");
                    break;
            }
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
    TreasureGrab source_Ctrl;
    UiWaitMask uiWaitMask;
    float AmountOffset = 0;
    // 构造方法
    public TreasureGrabRespond(TreasureGrab treasureGrab, UiWaitMask uiWaitMask)
    {
        this.source_Ctrl = treasureGrab;
        this.uiWaitMask = uiWaitMask;
    }
    public void Success(string result)
    {
        if (uiWaitMask != null)
        {
            uiWaitMask.ShowResultCase("", 0);
        }
        if (source_Ctrl == null)
        {
            return;
        }
        ReturnData<AppPacketReceiveRespVO> responseData = JsonConvert.DeserializeObject<ReturnData<AppPacketReceiveRespVO>>(result);
        // 实现 Success 方法的逻辑
        Debug.Log("Success TreasureGrabRespond=" + responseData.code.ToString());
        source_Ctrl.packageItem.packetSendRespVO.receiveMemberIds += "," + UserManager.Instance.appMemberUserInfoRespVO.id;//红包被多个用户抢那里添加上自己
        source_Ctrl.packageItem.SetPacketSendRespVOInfo(source_Ctrl.packageItem.packetSendRespVO);
        //显示明细
        Transform parent = this.source_Ctrl.transform.parent;
        if (responseData.data.CompensateAmount == null || responseData.data.CompensateAmount == 0)
        {
            AmountOffset += (float)responseData.data.GetAmount.Value;
            UIManager.Instance.ShowGetCoinEffect(parent, new Vector2(0f, 100f), EffectFinishEvent, 10); //show coin effect
        }
        else
        {
            AmountOffset += ((float)responseData.data.GetAmount.Value + (float)responseData.data.CompensateAmount.Value);
            UIManager.Instance.ShowBomb(parent, EffectFinishEvent);
        }



    }
    void EffectFinishEvent()
    {
        PopupManager.Instance.CloseAllPopup();
        source_Ctrl.packageItem.OpenPackageDetailResultClick();
        RedPackageAuthor.Instance.realUserBalance += (AmountOffset);
        //MonoSingleton<UserManager>.Instance.GetUserMainInfo();
    }
    public void Fail(JObject json)
    {
        if (uiWaitMask != null)
        {
            uiWaitMask.ShowResultCase("", 0);
        }
        if (!failPubDo.failPubdo(json))
        {
            int code = json["code"].Value<int>();
            string msg = json["msg"].Value<string>();
            if (code == 1022000003)
            {
                source_Ctrl.packageItem.OpenPackageDetailResultClick();
                MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Info", "U already Grabed!");
            }
            else if (code == 1)
            {
                //MonoSingleton<PopupManager>.Instance.CloseAllPopup();
                source_Ctrl.packageItem.OpenPackageDetailResultClick();
                MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Info", "All coin have been Grab.");
            }
            else if (code==1004001004)
            {
                //SoundSFX.Play(SFXIndex.ButtonClick);
                MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupShopCoin, enableBackCloseButton: true);
                MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Info", msg);
            }
            else 
            {

                source_Ctrl.packageItem.OpenPackageDetailResultClick();
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