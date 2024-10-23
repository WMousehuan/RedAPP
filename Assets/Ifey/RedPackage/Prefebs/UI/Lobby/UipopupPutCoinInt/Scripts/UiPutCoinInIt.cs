using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using TMPro;
using Assets.Ifey.RedPackage.Prefebs.UI.Login.Script;
using Assets.Ifey.RedPackage.Prefebs.UI.Lobby.UipopupPutCoinInt.Scripts;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using EasyUI.Toast;
using UnityEngine.Purchasing;

public class UiPutCoinInIt : Popup
{
    public TMP_InputField inputFieldAmount;
    public TMP_InputField inputFieldPassward;
    NumberOfPlayerHorizontal numberOfPlayerHorizontal;
    BombNumberHorizontal bombNumberHorizontal;
    [HideInInspector]
    //submit the 
    string sendRedPacketUrl = "/app-api/red/packet-send/sendRedPacket";
    public override void Start()
    {
        base.Start();
        numberOfPlayerHorizontal = GetComponentInChildren<NumberOfPlayerHorizontal>();
        numberOfPlayerHorizontal.startInitPlayersNumberCHeckbox();

        bombNumberHorizontal = GetComponentInChildren<BombNumberHorizontal>();
        bombNumberHorizontal.startInitPlayersNumberCHeckbox();

        inputFieldAmount.onValueChanged.AddListener(OnInputValueChanged);
        Debug.Log($"UiPutCoinInIt on Start!");
    }
    // Start is called before the first frame update
    public void OnInputValueChanged(string value)
    {
        // 使用正则表达式保留输入中的数字部分
        string newValue = Regex.Replace(value, "[^0-9]", "");

        // 更新InputField的值
        inputFieldAmount.text = newValue;
    }

    public void submitBtnClick()
    {
        if (inputFieldAmount.text == null || inputFieldAmount.text.Length == 0)
        {
            MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Info", "Plz input the accmout！");
            return;
        }

        if (inputFieldPassward.text == null || inputFieldPassward.text.Length == 0)
        {
            MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Info", "Plz input the passward！");
        }

        if (inputFieldPassward.text.Length < 6)
        {
            MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Info", "Minimum 6 digits for Passward");
            return;
        }
        AppPacketSendSaveReqVO appPacketSendSaveReqVO = new AppPacketSendSaveReqVO();
        appPacketSendSaveReqVO.redAmount = int.Parse(inputFieldAmount.text);
        appPacketSendSaveReqVO.payPassword = inputFieldPassward.text;
        appPacketSendSaveReqVO.channelId = long.Parse(PlayerTreasureGameData.Instance.entranceChannelId);
        //选中的玩法
        PlayersCheckBox selectPlayMethon = numberOfPlayerHorizontal.getOnSelectPlayer();
        if (selectPlayMethon == null || selectPlayMethon.id == null)
        {
            MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "Plz select the numebr of treasure!");
            return;
        }
        appPacketSendSaveReqVO.playId = selectPlayMethon.id;

        BombNumberCheckBox bombNumberCheckBox = bombNumberHorizontal.getBombNumberCheckBox();
        appPacketSendSaveReqVO.thunderNo = (int)bombNumberCheckBox.id;
        Debug.Log("submit sendCoinPkg=" + appPacketSendSaveReqVO.ToString());
        UiWaitMask waitMask_Ui = (UiWaitMask)PopupManager.Instance.Open(PopupType.PopupWaitMask);
        UtilJsonHttp.Instance.PostRequestWithParamAuthorizationToken(sendRedPacketUrl, appPacketSendSaveReqVO, new submitPutCoinInItHttpCallBack(), (resultData) =>
        {
            waitMask_Ui?.ShowResultCase("Success", 1);
        }
        ,() =>
        {
            waitMask_Ui?.ShowResultCase("Fail", 0);
        });
    }
}

public class submitPutCoinInItHttpCallBack : HttpInterface
{
    public FailPubDo failPubDo = new FailPubDo();
    public void Success(string result)
    {
        Debug.Log("submitPutCoinInItHttpCallBack Success");
        MonoSingleton<PopupManager>.Instance.CloseAllPopup();
        MonoSingleton<UserManager>.Instance.GetUserMainInfo();
        MonoSingleton<SceneControlManager>.Instance.LoadScene(SceneType.Lobby, SceneChangeEffect.Color);
        SoundSFX.Play(SFXIndex.DailyBonusGet);
        MonoSingleton<UIOverlayEffectManager>.Instance.ShowEffectRibbonFireworks();
        Toast.Show("Successful send chest! ", 3f, ToastColor.Yellow);
        EventManager.Instance.DispatchEvent(typeof(submitPutCoinInItHttpCallBack).ToString(), "Refresh");
    }

    public void Fail(JObject json)
    {
        if (!failPubDo.failPubdo(json))
        {
            int code = json["code"].Value<int>();
            if (code == 1004001004)
            {
                MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupShopCoin, enableBackCloseButton: true);
                MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Info", "Not enough coin to sent!");
                Debug.Log("Not enough coin to sent!!");
                return;           
            }
            else if (code == 1022001001)
            {
                
                MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Info", json["msg"].Value<string>());
                Debug.Log("submitPutCoinInItHttpCallBack "+ json["msg"].Value<string>());
                return;
            }
            else
            { 
                MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "Send coin Fail!");
            }

        }
    }

    public void UnknowError(string errorMsg)
    {
        Debug.Log("submitPutCoinInItHttpCallBack UnknowError=" + errorMsg);
    }
}
