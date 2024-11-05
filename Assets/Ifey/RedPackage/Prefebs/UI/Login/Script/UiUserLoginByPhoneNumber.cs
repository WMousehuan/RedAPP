using Assets.Ifey.RedPackage.Prefebs.UI.Login.Script;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UserManager;

public class UiUserLoginByPhoneNumber : Popup
{
    //public TMP_InputField AreaCode_InputField;
    public TMP_InputField phoneNumber_InputField;
    public TMP_Dropdown phoneArae_DropDown;
    public Text areaCode_Text;
    public TMP_InputField code_InputField;

    private string loginUrl = "/app-api/member/auth/sms-login";
    private string verifyUrl = "/app-api/member/auth/send-sms-code";
    private string loginByPhoneNumberUrl = "/app-api/member/auth/sms-login";
    public override void Start()
    {
        base.Start();
        List<TMP_Dropdown.OptionData> optionDatas = new List<TMP_Dropdown.OptionData>();
        for (int i = 0; i < UserManager.Instance.areaCodeData_Group.Count; i++)
        {
            ObjectPack<UserManager.AreaType, AreaCodeData> areaCodeDataPack = UserManager.Instance.areaCodeData_Group[i];
            optionDatas.Add(new TMP_Dropdown.OptionData(areaCodeDataPack.key.ToString(), areaCodeDataPack.target.areaFlag_Sprite));
        }
        phoneArae_DropDown.onValueChanged.AddListener((index) =>
        {
            UserManager.Instance.currentAreaTypeIndex = index;
            areaCode_Text.text = string.Format("({0})", UserManager.Instance.currentAreaCode);
        });
        phoneArae_DropDown.ClearOptions();
        phoneArae_DropDown.AddOptions(optionDatas);
        phoneArae_DropDown.value = UserManager.Instance.currentAreaTypeIndex;
    }

    public void OnEventSendCode()
    {
        if (string.IsNullOrEmpty(phoneNumber_InputField.text))
        {
            MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "Phone number cannot be empty");
            return;
        }
        
        UserManager.Instance.PhoneNumberSendCode( phoneNumber_InputField.text, UserManager.VerifySceneType.Login);
    }
    public void OnEventUserLogin()
    {
        UserManager.Instance.UserLoginByPhoneNumber( phoneNumber_InputField.text, code_InputField.text);
    }
    public void openSignUp()
    {
        //MonoSingleton<PopupManager>.Instance.CloseAllPopup();
        MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupSignup);
    }
    public void OnEventChangeSignUp()
    {
        MonoSingleton<PopupManager>.Instance.CloseAllPopup();
        MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupLogin);
    }
    public enum AreaType//ÇøºÅ
    {
        China=86,//86
        USA=1,//1
        UE=44,//44
        Japan=81,//81
        India=91,//91
    }
    public void PhoneNumberSendCode(AreaType areaType,string phoneNumber, VerifySceneType sceneType)
    {
        try
        {
            var data = new
            {
                mobile = string.Format("({0})", (int)areaType) + phoneNumber,
                scene = (int)sceneType
            };
            //UiWaitMask waitMask_Ui = (UiWaitMask)PopupManager.Instance.Open(PopupType.PopupWaitMask);
            //when start the game,get the userInfo
            UtilJsonHttp.Instance.PostRequestWithParamAuthorizationToken(verifyUrl, data, null, (resultData) =>
            {
                int currnetTimeSteamp = Utils.ConvertToTimestamp(DateTime.Now);
                PlayerPrefs.SetInt("VerifyStampTime" + sceneType.ToString(), currnetTimeSteamp + 60);
                EventManager.Instance.DispatchEvent(GameEventType.SetVerifyStampTime.ToString(), currnetTimeSteamp + 60, sceneType);
                UiHintCase.instance.Show("Send verify code success");
                //waitMask_Ui?.ShowResultCase("Send verify code success", 0);
            }, (code, msg) =>
            {
                Debug.Log("Send verify code  fail");
                string content = "Change Phone Fail.\r\ncode=" + code;
                switch (code)
                {
                    case 1004001008:
                        content = "This phone number is not registered!";
                        break;
                    case 1002014002:
                        content = "The verification code has been used!";
                        break;
                    case 1002014001:
                        content = "The verification code has expired!";
                        break;
                    case 1002014000:
                        content = "Verification code does not exist!";
                        break;
                }
                MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", content);
            });
        }
        catch (Exception e)
        {
            Debug.LogError("An exception occurred: " + e.Message);
            // Handle the exception, for example display an error message or log the exception
        }
    }
    public void UserLoginByPhoneNumber(string phoneNumber, string code)
    {
        try
        {
            AppAuthPhoneNumberReqVO appAuthPhoneNumberLoginReqVO = new AppAuthPhoneNumberReqVO();
            appAuthPhoneNumberLoginReqVO.mobile = string.Format("({0})", UserManager.Instance.currentAreaCode) + phoneNumber;
            appAuthPhoneNumberLoginReqVO.code = code;
            if (appAuthPhoneNumberLoginReqVO.mobile.Length <= 0)
            {
                MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "Phone number cannot be empty");
                return;
            }
            if (appAuthPhoneNumberLoginReqVO.code.Length < 4)
            {
                MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "Minimum 4 digits for Verify Code");
                return;
            }

            UiWaitMask waitMask_Ui = (UiWaitMask)PopupManager.Instance.Open(PopupType.PopupWaitMask);
            //when start the game,get the userInfo
            UtilJsonHttp.Instance.PostRequestWithParamAuthorizationToken(loginByPhoneNumberUrl, appAuthPhoneNumberLoginReqVO, new UserLoginInterface(), (resultData) => {
                waitMask_Ui?.ShowResultCase("Success", 0);
                foreach (VerifySceneType type in Enum.GetValues(typeof(VerifySceneType)))
                {
                    PlayerPrefs.DeleteKey("VerifyStampTime" + type.ToString());
                }
            }, (code, msg) => {
                waitMask_Ui?.ShowResultCase("fail", 0);
            });
        }
        catch (Exception e)
        {
            Debug.LogError("An exception occurred: " + e.Message);
            // Handle the exception, for example display an error message or log the exception
        }
    }
}
