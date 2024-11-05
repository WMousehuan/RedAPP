using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UserManager;

public class UIPOPSignup : Popup
{
    public TMP_InputField uerId;
    public TMP_InputField loginPsd;
    public TMP_InputField nickname;
    public TMP_InputField superiorId;

    public TMP_Dropdown phoneArae_DropDown;
    public Text areaCode_Text;
    public TMP_InputField phoneNumber_InputField;
    public TMP_InputField code_InputField;

    [HideInInspector]
    string userSignUpUrl = "/app-api/member/auth/register";
    private string verifyUrl = "/app-api/member/auth/send-sms-code";
    public override void OnEnable()
    {
        base.OnEnable();
        superiorId.text = UserManager.encryptSuperiorId;
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
        UserManager.Instance.PhoneNumberSendCode(phoneNumber_InputField.text, UserManager.VerifySceneType.SignUp);
    }
    public void openLogin()
    {
        MonoSingleton<PopupManager>.Instance.Close(this);
        //MonoSingleton<PopupManager>.Instance.CloseAllPopup();
        //MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupLogin);
    }
    public enum AreaType//区号
    {
        China = 86,//86
        USA = 1,//1
        UE = 44,//44
        Japan = 81,//81
        India = 91,//91
    }
    public void PhoneNumberSendCode(AreaType areaType, string phoneNumber, VerifySceneType sceneType)
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
    public void signUpClick()
    {
        AppAuthRegistReqVO appAuthRegistReqVO = new AppAuthRegistReqVO();
        appAuthRegistReqVO.username = uerId.text;
        appAuthRegistReqVO.password = loginPsd.text;
        appAuthRegistReqVO.payPassword = loginPsd.text;
        appAuthRegistReqVO.nickname = nickname.text;
        appAuthRegistReqVO.mobile = string.IsNullOrEmpty(phoneNumber_InputField.text) ? "" : string.Format("({0})", UserManager.Instance.currentAreaCode) + phoneNumber_InputField.text;
        appAuthRegistReqVO.code = string.IsNullOrEmpty(phoneNumber_InputField.text) ? "" :code_InputField.text;
        appAuthRegistReqVO.encryptSuperiorId = superiorId.text;
        if (appAuthRegistReqVO.password.Length < 6)
        {
            MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "Minimum 6 digits for Passward");
            return;
        }
        if (appAuthRegistReqVO.username.Length < 4)
        {
            MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "The minimum length of the username is 4 digits");
            return;
        }
        if (appAuthRegistReqVO.nickname.Length < 4)
        {
            MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "The nickname length of the username is 4 digits");
            return;
        }
        if (phoneNumber_InputField.text.Length > 0  && appAuthRegistReqVO.code.Length < 4)
        {
            MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "Minimum 4 digits for Verify Code");
            return;
        }
        UiWaitMask waitMask_Ui = (UiWaitMask)PopupManager.Instance.Open(PopupType.PopupWaitMask);
        UtilJsonHttp.Instance.PostRequestWithParamAuthorizationToken(userSignUpUrl, appAuthRegistReqVO, new SignUpInfoInterface(this), (resultData) => {
            waitMask_Ui?.ShowResultCase("Success", 0);
        }, (code, msg) => {
            waitMask_Ui?.ShowResultCase("fail", 0);
        });
    }
}


//get userInfo
public class SignUpInfoInterface : HttpInterface
{
    public UIPOPSignup source_Ctrl;
    public SignUpInfoInterface(UIPOPSignup source_Ctrl)
    {
        this.source_Ctrl = source_Ctrl;
    }
    public void Success(string result)
    {
        UserManager.tempUserId = source_Ctrl.uerId.text;
        MonoSingleton<PopupManager>.Instance.CloseAllPopup();
        // 实现 Success 方法的逻辑
        MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonInfo, "Success", "Success Plz login.", OnEventClickToLogin, OnEventClickToLogin);
        Debug.Log("Success Signup new user go To Login!");
    }

    public void Fail(JObject json)
    {
        // 实现 Fail 方法的逻辑
        int code = json["code"].Value<int>();
        //not login
        if (code == 401)
        {
            MonoSingleton<PopupManager>.Instance.CloseAllPopup();
            Debug.Log("User notLogin Show Login UI!");
            MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupLogin);
            //MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupCommonYesNo);
        }
        else if(code == 1004001007)
        {
            //user name exits!
            MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "user name exits,Plz choose another");
        }
        else if (code == 1004001006)
        {
            //user name exits!
            MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "The mobile phone number is already registered");
        }
    }
    public void UnknowError(string errorMsg)
    {
        Debug.Log("SignUpInfoInterface UnknowError=" + errorMsg);
    }

    private void OnEventClickToLogin()
    {
         
            MonoSingleton<PopupManager>.Instance.CloseAllPopup();
            MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupLogin);
        //MonoSingleton<SceneControlManager>.Instance.LoadScene(SceneType.Lobby);

    }
}
