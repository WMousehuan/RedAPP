using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static RedPackageAuthor;
using static UnityEngine.GraphicsBuffer;

public class UserManager : MonoSingleton<UserManager>
{
    [HideInInspector]
    public AppMemberUserInfoRespVO appMemberUserInfoRespVO =null; //userInfo

    public Texture2D defaultTexture;

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
            EventManager.Instance.DispatchEvent(GameType.GetUserAvatar.ToString());
        }
    }
    [HideInInspector]
    public string userMainInfoUrl = "/app-api/member/user/get"; //get userInfo Url
    public static string encryptSuperiorId = "";

    public static string tempUserId;


    private string getAvatarsUrl = "/app-api/member/avatar/page";
    public static ObjectGroup<string, (bool, Texture2D)> avatarData_Group = null;
    public System.Action<Texture2D,int> loadedAvatarAction = null;
    private void Start()
    {
        currentAvatar_Texture = defaultTexture;

        if (avatarData_Group == null && !string.IsNullOrEmpty(getAvatarsUrl))
        {
            UtilJsonHttp.Instance.GetRequestWithAuthorizationToken(getAvatarsUrl, null, (resultData) =>
            {
                ReturnData<PageResultPacketSendRespVO<ReserveAvatarData>> returnData = JsonConvert.DeserializeObject<ReturnData<PageResultPacketSendRespVO<ReserveAvatarData>>>(resultData);
                if (avatarData_Group == null)
                {
                    avatarData_Group = new ObjectGroup<string, (bool, Texture2D)>();
                    for (int i = 0; i < returnData.data.list.Length; i++)
                    {
                        int index = i;
                        avatarData_Group.Add(returnData.data.list[index].avatarUrl, (false, null));
                        GeneralTool_Ctrl.DownloadImage(avatarData_Group[index].key, (texture2d) =>
                        {
                            avatarData_Group[index].target = (true, texture2d);
                            loadedAvatarAction?.Invoke(texture2d, i);
                            EventManager.Instance.DispatchEvent(GameType.LoadedAvatarTexture.ToString(), texture2d, index, avatarData_Group[index].key);
                        }, () =>
                        {
                            avatarData_Group[index].target = (false, null);
                        });
                        
                    }
                }
            });
        }
        EventManager.Instance.Regist(GameType.LoadedAvatarTexture.ToString(), this.GetInstanceID(), objects =>
        {
            Texture2D texture2d = (Texture2D)objects[0];
            //int realIndex = (int)objects[1];
            string avatarUrl = (string)objects[2];

            if (appMemberUserInfoRespVO!=null&& appMemberUserInfoRespVO.avatar== avatarUrl)
            {
                currentAvatar_Texture = texture2d;
            }

        });
    }
    private void OnDestroy()
    {
        EventManager.Instance.UnRegist(GameType.LoadedAvatarTexture.ToString(), this.GetInstanceID());
    }
    public void GetUserMainInfo(System.Action<bool> loadedAction = null)
    {

        //RedPackageAuthor.Instance.authorizationValue = "1";
        //RedPackageAuthor.Instance.refreshTokenAuthorizationValue = "1";
        //when start the game,get the userInfo
        UtilJsonHttp.Instance.GetRequestWithAuthorizationToken(userMainInfoUrl, new GetUserInfoInterface(this), (resultData) =>
        {
            loadedAction?.Invoke(true);
            EventManager.Instance.DispatchEvent(typeof(UserManager).ToString(), "LoginIn");
            if (avatarData_Group != null && avatarData_Group[appMemberUserInfoRespVO.avatar].Item2 != null)
            {
                currentAvatar_Texture = avatarData_Group[appMemberUserInfoRespVO.avatar].Item2;
            }
            //GeneralTool_Ctrl.DownloadImage(appMemberUserInfoRespVO.avatar, (texture) =>
            //{
            //    currentAvatar_Texture = texture;
            //}, () => {
            //    loadedAction?.Invoke(false);
            //});
        }, () => {
            loadedAction?.Invoke(false);
        });
    }

    
}

 
//get userInfo
public class GetUserInfoInterface : HttpInterface
{
    UserManager source_Ctrl;
    public GetUserInfoInterface(UserManager source)
    {
        source_Ctrl = source;
    } 
    public void Success(string result)
    {
        if (source_Ctrl == null)
        {
            return;
        }
        // 实现 Success 方法的逻辑
        ReturnData<AppMemberUserInfoRespVO> responseData = JsonConvert.DeserializeObject<ReturnData<AppMemberUserInfoRespVO>>(result);
        source_Ctrl.appMemberUserInfoRespVO = responseData.data;
        // 实现 Success 方法的逻辑
        RedPackageAuthor.Instance.userBalance = responseData.data.balance;
        RedPackageAuthor.Instance.userNickName = responseData.data.nickname;
        EventManager.Instance.DispatchEvent(GameType.GetUserData.ToString());
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
