using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPOPSignup : Popup
{
    public TMP_InputField uerId;
    public TMP_InputField loginPsd;
    public TMP_InputField nickname;
    public TMP_InputField superiorId;
    public TMP_InputField phoneNumber_InputField;
    public TMP_InputField code_InputField;
    [HideInInspector]
    string userSignUpUrl = "/app-api/member/auth/register";

    public override void OnEnable()
    {
        base.OnEnable();
        superiorId.text = UserManager.encryptSuperiorId;
    }
    public void openLogin()
    {
        MonoSingleton<PopupManager>.Instance.CloseAllPopup();
        MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupLogin);
    }

    public void signUpClick()
    {

        AppAuthRegistReqVO appAuthRegistReqVO = new AppAuthRegistReqVO();
        appAuthRegistReqVO.username = uerId.text;
        appAuthRegistReqVO.password = loginPsd.text;
        appAuthRegistReqVO.payPassword = loginPsd.text;
        appAuthRegistReqVO.nickname = nickname.text;
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
        if(phoneNumber_InputField!=null&& phoneNumber_InputField.text.Length < 13)
        {
            return;
        }
        if (code_InputField != null && string.IsNullOrEmpty(code_InputField.text))
        {
            return;
        }
        UiWaitMask waitMask_Ui = (UiWaitMask)PopupManager.Instance.Open(PopupType.PopupWaitMask);
        UtilJsonHttp.Instance.PostRequestWithParamAuthorizationToken(userSignUpUrl, appAuthRegistReqVO, new SignUpInfoInterface(this), (resultData) => {
            waitMask_Ui?.ShowResultCase("Success", 0);
        }, () => {
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
        else if(code == 407)
        {
            //user name exits!
            MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "user name exits,Plz choose another");
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
