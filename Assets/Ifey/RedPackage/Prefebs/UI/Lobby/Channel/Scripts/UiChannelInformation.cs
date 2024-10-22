using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UiChannelInformation : Popup
{
    private string deleteUrl = "/app-api/red/channel/delete";
    private string updateDataUrl = "/app-api/red/channel/update";
    private string getPromotionUrl = "/app-api/red/promotion-setting/getActiveSetting";//获得推广链接Url
    //public InputField channelName_TextField;
    //public InputField minValue_TextField;
    //public InputField maxValue_TextField;
    //public InputField durationTime_TextField;
    public TMP_InputField channelName_TextField;
    public TMP_InputField minValue_TextField;
    public TMP_InputField maxValue_TextField;
    public TMP_InputField durationTime_TextField;

    public GameObject roomEditor_Case;

    public InputField promotionLink_InputField;
    public Button copy_Button;
    // Update is called once per frame
    public override void Start()
    {
        base.Start();
        if (LobbyManager.Instance == null || LobbyManager.Instance.currnetChannelData == null)
        {
            PopupManager.Instance.Close();
            return;
        }
        bool isSelfChannel = LobbyManager.Instance.currnetChannelData.memberId == UserManager.Instance.appMemberUserInfoRespVO.id;
        roomEditor_Case.gameObject.SetActive(isSelfChannel);
        channelName_TextField.interactable = isSelfChannel;
        minValue_TextField.interactable = isSelfChannel;
        maxValue_TextField.interactable = isSelfChannel;
        durationTime_TextField.interactable = isSelfChannel;
        if (isSelfChannel)
        {
            promotionLink_InputField.text = "Loading...";
            copy_Button.gameObject.SetActive(false);
            UtilJsonHttp.Instance.GetRequestWithAuthorizationToken(getPromotionUrl, null, (resultData) =>
            {
                JObject json = JObject.Parse(resultData);
                string dataContent = json["data"].ToString();
                JObject data = JObject.Parse(dataContent);
                if (promotionLink_InputField != null)
                {
                    string promotionUrl = data["promotionLink"].Value<string>();
                    string confusionNumber = "";
                    for(int i = 0; i < 3; i++)
                    {
                        confusionNumber += Random.Range(0, 10).ToString();
                    }
                    if (promotionUrl.Contains("?"))
                    {
                        promotionUrl += "&channelId=" + PlayerTreasureGameData.Instance.entranceChannelId + confusionNumber;
                    }
                    else
                    {
                        promotionUrl += "?channelId=" + PlayerTreasureGameData.Instance.entranceChannelId + confusionNumber;
                    }
                    promotionLink_InputField.text = promotionUrl;
                }
                copy_Button?.gameObject?.SetActive(true);
            }, () =>
            {
            });
        }


        channelName_TextField.text = LobbyManager.Instance.currnetChannelData.channelName.ToString();
        minValue_TextField.text = LobbyManager.Instance.currnetChannelData.redMinMoney.ToString();

        maxValue_TextField.text = LobbyManager.Instance.currnetChannelData.redMaxMoney.ToString();


        durationTime_TextField.text = LobbyManager.Instance.currnetChannelData.redExpirationTime.ToString();


       
       

    }
    void Update()
    {
        //PlayerTreasureGameData.Instance.entranceChannelId
    }

    public void OnEventDeleteChannel()
    {
        SoundSFX.Play(SFXIndex.ButtonClick);
        PopupManager.Instance.Open<UiAgreeCase>(PopupType.PopupAgreeCase).Init(() =>
        {
            UiWaitMask waitMask_Ui = (UiWaitMask)PopupManager.Instance.Open(PopupType.PopupWaitMask);
            UtilJsonHttp.Instance.DeleteParamAuthorizationToken(deleteUrl + "?id="+PlayerTreasureGameData.Instance.entranceChannelId,new CommonHttpInterface(), (requestData) =>
            {
                waitMask_Ui?.ShowResultCase("Success", 1, () => {
                    MonoSingleton<SceneControlManager>.Instance?.LoadScene(SceneType.Title, SceneChangeEffect.Color);
                });
            }, () =>
            {
                waitMask_Ui?.ShowResultCase("Fail", 1);
            });
        }, "Are you sure to delete the channel?","Yes","No");
    }
    public void OnEventUpdateChannel()
    {
        SoundSFX.Play(SFXIndex.ButtonClick);
        if (string.IsNullOrEmpty(channelName_TextField.text))
        {
            MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Info", "Channel name cannot be empty!");
            //PopupManager.Instance.
            print("频道名称不能为空");
            return;
        }
        if (string.IsNullOrEmpty(minValue_TextField.text))
        {
            MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Info", "Value cannot be empty!");
            return;
        }
        if (string.IsNullOrEmpty(maxValue_TextField.text))
        {
            MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Info", "Value cannot be empty!");
            return;
        }
        if (string.IsNullOrEmpty(durationTime_TextField.text))
        {
            MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Info", "Duration Time cannot be empty!");
            return;
        }
        float min_Value = float.Parse(minValue_TextField.text);
        float max_Value = float.Parse(maxValue_TextField.text);
        if (min_Value < 0 || max_Value < 0)
        {
            MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Info", "The value cannot be less than or equal to 0");
            return;
        }
        if (min_Value > 1000 || max_Value > 1000)
        {
            MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Info", "The value cannot be greater than 1000");
            return;
        }
        if (min_Value > max_Value)
        {
            MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Info", "The minimum value cannot be greater than the maximum value!");
            return;
        }
        int durationTime = int.Parse(durationTime_TextField.text);
        if (durationTime < 0)
        {
            MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Info", "The value cannot be less than or equal to 0!");
            return;
        }
        if (durationTime > 1000)
        {
            MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Info", "The value cannot be greater than 1000!");
            return;
        }
        var dataObject = new
        {
            id = PlayerTreasureGameData.Instance.entranceChannelId,
            channelName = channelName_TextField.text,
            redMinMoney = min_Value,
            redMaxMoney = max_Value,
            redExpirationTime = durationTime,
        };
        LobbyManager.Instance.currnetChannelData.channelName = dataObject.channelName;
        LobbyManager.Instance.currnetChannelData.redMinMoney = dataObject.redMinMoney;
        LobbyManager.Instance.currnetChannelData.redMaxMoney = dataObject.redMaxMoney;
        LobbyManager.Instance.currnetChannelData.redExpirationTime = dataObject.redExpirationTime;
        UiWaitMask waitMask_Ui = (UiWaitMask)PopupManager.Instance.Open(PopupType.PopupWaitMask);
        UtilJsonHttp.Instance.PutObjectWithParamAuthorizationToken(updateDataUrl, dataObject, new CommonHttpInterface(), (requestData) =>
        {
            waitMask_Ui?.ShowResultCase("Success", 1, () =>
            {
                PopupManager.Instance?.Close();
            });
        }, () =>
        {
            waitMask_Ui?.ShowResultCase("Fail", 1);
        });
    }
    public void OnEventCopyUrl()
    {
        GUIUtility.systemCopyBuffer = promotionLink_InputField.text;
        WebMessage_Ctrl.SendMessageToWeb("clipbord^" + promotionLink_InputField.text);
        UiHintCase.instance.Show("Copied link");
    }
}
