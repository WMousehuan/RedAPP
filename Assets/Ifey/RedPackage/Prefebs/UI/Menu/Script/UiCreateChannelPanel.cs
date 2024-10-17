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
        minValue_TextField.onValueChanged.AddListener((content) => {
            if (string.IsNullOrEmpty(content))
            {
                minValue_TextField.SetTextWithoutNotify("1");
            }
            float min_Value = 1;
            if (float.TryParse(minValue_TextField.text, out float value_0))
            {
                min_Value = value_0;
            }
            float max_Value = 1;
            if (float.TryParse(maxValue_TextField.text, out float value_1))
            {
                max_Value = value_1;
            }
            if (min_Value > 1001)
            {
                minValue_TextField.SetTextWithoutNotify("1000");
            }
            if (min_Value < 1)
            {
                minValue_TextField.SetTextWithoutNotify("1");
            }
            if (min_Value > max_Value)
            {
                minValue_TextField.SetTextWithoutNotify(max_Value.ToString());
            }
        });

        maxValue_TextField.text = "100";
        maxValue_TextField.onValueChanged.AddListener((content) =>
        {
            if (string.IsNullOrEmpty(content))
            {
                maxValue_TextField.SetTextWithoutNotify("1");
            }
            float min_Value = 1;
            if (float.TryParse(minValue_TextField.text, out float value_0))
            {
                min_Value = value_0;
            }
            float max_Value = 1;
            if (float.TryParse(maxValue_TextField.text, out float value_1))
            {
                max_Value = value_1;
            }
            if (max_Value > 1001)
            {
                maxValue_TextField.SetTextWithoutNotify("1000");
            }
            if (max_Value < 1)
            {
                maxValue_TextField.SetTextWithoutNotify("1");
            }
            if (max_Value < min_Value)
            {
                maxValue_TextField.SetTextWithoutNotify(min_Value.ToString());
            }
        });

        durationTime_TextField.text = "1";
        durationTime_TextField.onValueChanged.AddListener((content) => {
            if (string.IsNullOrEmpty(content))
            {
                durationTime_TextField.SetTextWithoutNotify("1");
            }
            float durationTime_Value = float.Parse(durationTime_TextField.text);
            if (durationTime_Value < 1)
            {
                durationTime_TextField.SetTextWithoutNotify("1");
            }
            if (durationTime_Value > 100)
            {
                durationTime_TextField.SetTextWithoutNotify("100");
            }
        });
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
        float min_Value =float.Parse( minValue_TextField.text);
        float max_Value = float.Parse(maxValue_TextField.text);
        int durationTime = int.Parse(durationTime_TextField.text);
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