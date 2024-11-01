using Assets.Ifey.RedPackage.Prefebs.UI.Login.Script;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UiUserLogin : Popup
{
    public TMP_InputField uerId;
    public TMP_InputField loginPsd;
    [HideInInspector]
    string loginUrl = "/app-api/member/auth/login/username";
    public override void Start()
    {
        base.Start();
        uerId.text = UserManager.tempUserId;
    }
    public void openSignUp()
    {
        //MonoSingleton<PopupManager>.Instance.CloseAllPopup();
        MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupSignup);
    }
    public void OnEventChangeLogin()
    {
        MonoSingleton<PopupManager>.Instance.CloseAllPopup();
        MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupLoginByPhoneNumber);
    }
    public void userLogin()
    {
        UserManager.Instance.UserLogin(uerId.text, loginPsd.text);
    }
}


//get userInfo
public class UserLoginInterface : HttpInterface
{
    public void Success(string result)
    {
        MonoSingleton<PopupManager>.Instance.CloseAllPopup();
        
        ReturnData<UserLoginReturnData> responseData = JsonConvert.DeserializeObject<ReturnData<UserLoginReturnData>>(result);
        // 实现 Success 方法的逻辑
        RedPackageAuthor.Instance.authorizationValue = responseData.data.accessToken;
        RedPackageAuthor.Instance.refreshTokenAuthorizationValue = responseData.data.refreshToken;
        PlayerPrefs.SetString("LastLoginDateTime", DateTime.Now.ToString());
        UserManager.Instance.GetUserMainInfo();
        Debug.Log(result);
        Debug.Log("Success User Login Success!");
    }

    public void Fail(JObject json)
    {
        // 实现 Fail 方法的逻辑
        int code = json["code"].Value<int>();
        //not login
        if (code == 401)
        {
            MonoSingleton<PopupManager>.Instance.CloseAllPopup();
            //Debug.Log("User notLogin Show Login UI!");
            MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupLogin);
            //MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupCommonYesNo);
        }
        else if (code == 407)
        {
            //user name exits!
            MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "user name exits,Plz choose another");
        }

        else if (code == 406 || code == 1004003000 || code == 400)
        {
            //Debug.Log("Login name or psd error!");
            //user name exits!
            MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "Login name or psd error!");

        }
        else if(code== 1004001008)
        {
            MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "This phone number is not registered!");
        }
        else if(code== 1002014002)
        {
            MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "The verification code has been used!");
        }
        else if (code == 1002014001)
        {
            MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "The verification code has expired!");
        }
        else if (code == 1002014000)
        {
            MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "Verification code does not exist!");
        }
        if (json.ContainsKey("msg"))
        {
            Debug.Log("msg:" + json["msg"].Value<string>());
        }
    }

    public void UnknowError(string errorMsg)
    {
        Debug.Log("UserLoginInterface UnknowError=" + errorMsg);
    }
}
