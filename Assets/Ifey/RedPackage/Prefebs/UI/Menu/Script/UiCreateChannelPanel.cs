using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UiCreateChannelPanel : Popup
{
    private string url = "/app-api/red/channel/create";

    public InputField channelName_TextField;
    public InputField minValue_TextField;
    public InputField maxValue_TextField;
    public InputField durationTime_TextField;

    public override void Start()
    {
        base.Start();

        minValue_TextField.text = "10";

        maxValue_TextField.text = "100";

        durationTime_TextField.text = "1";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEventCreateChannel()
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
            id = UserManager.Instance.appMemberUserInfoRespVO.id,
            channelName = channelName_TextField.text,
            redMinMoney = min_Value,
            redMaxMoney = max_Value,
            redExpirationTime = durationTime,
        };
        UiWaitMask waitMask_Ui = (UiWaitMask)PopupManager.Instance.Open(PopupType.PopupWaitMask);
        UtilJsonHttp.Instance.PostRequestWithParamAuthorizationToken(url, dataObject, new CommonHttpInterface(), (requestData) =>
        {
            EventManager.Instance.DispatchEvent(typeof(UiCreateChannelPanel).ToString(), "CreateChannel");
            waitMask_Ui.ShowResultCase("Success", 1, () => {
                PopupManager.Instance.Close();
            });
        }, () =>
        {
            waitMask_Ui.ShowResultCase("Fail", 1);
        });
    }
}