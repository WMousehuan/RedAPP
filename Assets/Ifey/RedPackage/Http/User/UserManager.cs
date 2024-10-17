using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;
using static RedPackageAuthor;

public class UserManager : MonoSingleton<UserManager>
{
    [HideInInspector]
    public AppMemberUserInfoRespVO appMemberUserInfoRespVO =null; //userInfo
    [SerializeField]
    private Texture2D _currentAvatar_Texture;
    public Texture2D currentAvatar_Texture
    {
        get
        {
            return _currentAvatar_Texture;
        }
        set
        {
            _currentAvatar_Texture = value;
            EventManager.Instance.DispatchEvent(typeof(GetUserInfoInterface).ToString(), "GetAvatar");
        }
    }
    [HideInInspector]
    public string userMainInfoUrl = "/app-api/member/user/get"; //get userInfo Url
    public static string encryptSuperiorId = "";
  
    public void GetUserMainInfo()
    {
        //RedPackageAuthor.Instance.authorizationValue = "1";
        //RedPackageAuthor.Instance.refreshTokenAuthorizationValue = "1";
        //when start the game,get the userInfo
        UtilJsonHttp.Instance.GetRequestWithAuthorizationToken(userMainInfoUrl, new GetUserInfoInterface(this), (resultData) => {
            EventManager.Instance.DispatchEvent(typeof(UserManager).ToString(), "LoginIn");
            GeneralTool_Ctrl.DownloadImage(appMemberUserInfoRespVO.avatar, (texture) =>
            {
                currentAvatar_Texture = texture;
            });
        });
    }

    
}

 
//get userInfo
public class GetUserInfoInterface : HttpInterface
{
    UserManager userManager;
    public GetUserInfoInterface(UserManager source)
    {
        userManager = source;
    } 
    public void Success(string result)
    {
        // 实现 Success 方法的逻辑
        ReturnData<AppMemberUserInfoRespVO> responseData = JsonConvert.DeserializeObject<ReturnData<AppMemberUserInfoRespVO>>(result);
        userManager.appMemberUserInfoRespVO = responseData.data;
        // 实现 Success 方法的逻辑
        RedPackageAuthor.Instance.userBalance = responseData.data.balance;
        RedPackageAuthor.Instance.userNickName = responseData.data.nickname;
        EventManager.Instance.DispatchEvent(typeof(GetUserInfoInterface).ToString(), "UpdateData");
        Debug.Log("Success Get User info!");
    }

    public void Fail(JObject json)
    {
        // 实现 Fail 方法的逻辑
        int code = json["code"].Value<int>();
        //not login
        if (code == 401)
        {
            Debug.Log("User notLogin CallRefreshTokenAPI!");
            //MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupLogin);
            //MonoSingleton<PopupManager>.Instance.Open(PopupType.PopupCommonYesNo);
            MonoSingleton<RedPackageAuthor>.Instance.CallRefreshTokenAPI();
        }

    }

    public void UnknowError(string errorMsg)
    {
        Debug.Log("GetUserInfoInterface UnknowError="+ errorMsg) ;
    }
}



public class Level
{
    public int id { get; set; }
    public string name { get; set; }
    public int level { get; set; }
    public string icon { get; set; }
}

public class AppMemberUserInfoRespVO
{
    public int id { get; set; }
    public string nickname { get; set; }
    public string avatar { get; set; }
    public string mobile { get; set; }
    public int sex { get; set; }
    public int point { get; set; }
    public float balance {  get; set; }
    public int experience { get; set; }
    public int userType { get; set; }
    public Level level { get; set; }
    public bool? brokerageEnabled { get; set; }
}
