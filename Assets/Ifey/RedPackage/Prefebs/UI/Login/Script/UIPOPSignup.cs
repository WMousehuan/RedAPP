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
    [HideInInspector]
    string userSignUpUrl = "/app-api/member/auth/register";

    public override void OnEnable()
    {
        base.OnEnable();
        superiorId.text = UserManager.Instance.encryptSuperiorId;
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
        UtilJsonHttp.Instance.PostRequestWithParamAuthorizationToken(userSignUpUrl, appAuthRegistReqVO, new SignUpInfoInterface());
    }
}


//get userInfo
public class SignUpInfoInterface : HttpInterface
{
    public void Success(string result)
    {
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
