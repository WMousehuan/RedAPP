using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//All the user manager
public class RedPackageAuthor : MonoSingleton<RedPackageAuthor>
{
    [HideInInspector]
    public string authorizationKey = "authorizationKey";
    [HideInInspector]
    public string refreshTokenAuthorizationKey = "refreshTokenAuthorizationKey";
    string refreshTokenUrl = "/app-api/member/auth/refresh-token";

    [HideInInspector]
    string userBalanceKey = "userBalanceKey";
    public string authorizationValue
    {
        get
        {
            return PlayerPrefs.GetString(authorizationKey, "").ToString();
        }
        set
        {
            PlayerPrefs.SetString(authorizationKey,value);
        }
    }

    public string refreshTokenAuthorizationValue
    {
        get
        {
            return PlayerPrefs.GetString(refreshTokenAuthorizationKey, "").ToString();
        }
        set
        {
            PlayerPrefs.SetString(refreshTokenAuthorizationKey, value);
        }
    }


    public float userBalance
    {
        get
        {
            return PlayerPrefs.GetFloat(userBalanceKey, 0);
        }
        set
        {

            PlayerPrefs.SetFloat(userBalanceKey, value);
            EventManager.Instance.DispatchEvent(GameType.CoinUpdate.ToString(), this.GetInstanceID());
        }
    }

    public string userNickName { get; set; }
    // Start is called before the first frame update
    // 调用SendRequestWithParamAuthorizationToken方法，将refreshToken作为参数传递
    public void CallRefreshTokenAPI()
    {
        //RefreshTokenParam param = new RefreshTokenParam
        //{
        //    refreshToken = refreshTokenAuthorizationValue
        //};
        // Create a new form
        WWWForm refreshToken = new WWWForm();
        refreshToken.AddField("refreshToken", refreshTokenAuthorizationValue);
        Debug.Log("刷新令牌，refreshTokenAuthorizationValue="+ refreshTokenAuthorizationValue);
        UtilJsonHttp.Instance.PostRequestWithParamAuthorizationToken(refreshTokenUrl+"?refreshToken="+ refreshTokenAuthorizationValue, refreshToken, new RefreshTokenParamHttpInterface());
    }
    public class RefreshTokenParamHttpInterface : HttpInterface
    {
        public void Success(string result)
        {
            MonoSingleton<PopupManager>.Instance.CloseAllPopup();
            ReturnData<UserLoginReturnData> responseData = JsonConvert.DeserializeObject<ReturnData<UserLoginReturnData>>(result);
            // 实现 Success 方法的逻辑
            RedPackageAuthor.Instance.authorizationValue = responseData.data.accessToken;
            RedPackageAuthor.Instance.refreshTokenAuthorizationValue = responseData.data.refreshToken;
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
                Debug.Log("User notLogin Show Login UI!");
                MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupLogin);
                //MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupCommonYesNo);
            }
            else if (code == 407)
            {
                MonoSingleton<PopupManager>.Instance.CloseAllPopup();
                Debug.Log("User notLogin 407  Show Login UI!");
                MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupLogin);
                //user name exits!
                MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "user name exits,Plz choose another");
            }
            else if (code == 400)
            {
                MonoSingleton<PopupManager>.Instance.CloseAllPopup();
                MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupLogin);
                Debug.Log("refreshTokenUrl Fail 400 Show Login UI!");
            }
            else
            {
                MonoSingleton<PopupManager>.Instance.CloseAllPopup();
                MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupLogin);
                Debug.Log("refreshTokenUrl Others Show Login UI!");
            }

        }

        public void UnknowError(string errorMsg)
        {
             
        }
    }

    // 创建一个包含refreshToken属性的类
    public class RefreshTokenParam
    {
        public string refreshToken { get; set; }
    }
}


