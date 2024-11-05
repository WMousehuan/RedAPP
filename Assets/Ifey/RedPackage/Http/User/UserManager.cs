using Assets.Ifey.RedPackage.Prefebs.UI.Login.Script;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public enum VerifySceneType
    {
        None,
        Login,
        ChangePhoneNumber,
        ChangePassword,
        FogetPassword,
        SignUp
    }
    public enum AreaType
    {
        China,//86
        USA,//1
        UE,//44
        Japan,//81
        India,//91
    }
    [HideInInspector]
    public AppMemberUserInfoRespVO appMemberUserInfoRespVO = null; //userInfo

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
            EventManager.Instance.DispatchEvent(GameEventType.GetUserAvatar.ToString());
        }
    }
    private string loginUrl = "/app-api/member/auth/login/username";
    private string loginByPhoneNumberUrl = "/app-api/member/auth/sms-login";
    private string logoutUrl = "/app-api/member/auth/logout";
    private string verifyUrl = "/app-api/member/auth/send-sms-code";
    private string userMainInfoUrl = "/app-api/member/user/get"; //get userInfo Url

    public TextAsset areaCode_TextAsset;
    public ObjectGroup<AreaType, AreaCodeData> areaCodeData_Group;
    public int currentAreaTypeIndex = 4;
    public string currentAreaCode
    {
        get
        {
            return areaCodeData_Group[currentAreaTypeIndex].target.areaCode;
        }
    }
    public static string encryptSuperiorId = "";

    public static string tempUserId;


    private string getAvatarsUrl = "/app-api/member/avatar/page";
    public static ObjectGroup<string, (bool, Texture2D)> avatarData_Group = null;
    public System.Action<Texture2D, int> loadedAvatarAction = null;
    public bool isLoadingAvatarData = false;
    private void Start()
    {
        currentAvatar_Texture = defaultTexture;

        GetAvatarDatas();

        //string[] areaDataStages= areaCode_TextAsset.text.Split("\r\n");
        //for(int i=0;i< areaDataStages.Length; i++)
        //{
        //    areaCodeData_Dictionary.Add()
        //}
    }

    private void OnDestroy()
    {
        EventManager.Instance?.UnRegist(GameEventType.LoadedAvatarTexture.ToString(), this.GetInstanceID());
    }
    public void UserLogin(string id,string password)
    {
        try
        {

            AppAuthUsernameLoginReqVO appAuthUsernameLoginReqVO = new AppAuthUsernameLoginReqVO();
            appAuthUsernameLoginReqVO.username = id;
            appAuthUsernameLoginReqVO.password = password;
            if (appAuthUsernameLoginReqVO.password.Length < 6)
            {
                MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "Minimum 6 digits for Passward");
                return;
            }
            if (appAuthUsernameLoginReqVO.username.Length < 4)
            {
                MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "The minimum length of the username is 4 digits");
                return;
            }
            UiWaitMask waitMask_Ui = (UiWaitMask)PopupManager.Instance.Open(PopupType.PopupWaitMask);
            //when start the game,get the userInfo
            UtilJsonHttp.Instance.PostRequestWithParamAuthorizationToken(loginUrl, appAuthUsernameLoginReqVO, new UserLoginInterface(), (resultData) => {
                waitMask_Ui?.ShowResultCase("Success", 0);
                foreach (VerifySceneType type in Enum.GetValues(typeof(VerifySceneType)))
                {
                    PlayerPrefs.DeleteKey("VerifyStampTime"+ type.ToString());
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
                foreach(VerifySceneType type in Enum.GetValues(typeof(VerifySceneType)))
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
    public void PhoneNumberSendCode(string phoneNumber, VerifySceneType sceneType)
    {
        try
        {
            var data = new
            {
                mobile = string.Format("({0})", currentAreaCode) + phoneNumber,
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
    public void UserLogout()
    {
        UtilJsonHttp.Instance.PostRequestWithParamAuthorizationToken(logoutUrl, null, null, (resultData) => {
            UiHintCase.instance.Show("Logout Success");
        }, (code, msg) => {
            Debug.Log("Logout Fail");
        });
    }

    public void GetUserMainInfo(System.Action<bool> loadedAction = null)
    {

        //RedPackageAuthor.Instance.authorizationValue = "1";
        //RedPackageAuthor.Instance.refreshTokenAuthorizationValue = "1";
        //when start the game,get the userInfo
        UiWaitMask waitMask_Ui = (UiWaitMask)PopupManager.Instance.Open(PopupType.PopupWaitMask);
        waitMask_Ui.Init("Sign in");
        UtilJsonHttp.Instance.GetRequestWithAuthorizationToken(userMainInfoUrl, new GetUserInfoInterface(this), (resultData) =>
        {
            waitMask_Ui.ShowResultCase("Success", 0);
            loadedAction?.Invoke(true);
            EventManager.Instance.DispatchEvent(GameEventType.Login.ToString());
            if (avatarData_Group != null && avatarData_Group[appMemberUserInfoRespVO.avatar].Item2 != null)
            {
                currentAvatar_Texture = avatarData_Group[appMemberUserInfoRespVO.avatar].Item2;
            }
   
        }, (code, msg) => {
            waitMask_Ui.ShowResultCase("Fail", 0);
            loadedAction?.Invoke(false);
           
            UiHintCase.instance.Show("Login Error");
            waitMask_Ui = (UiWaitMask)PopupManager.Instance.Open(PopupType.PopupWaitMask);
            waitMask_Ui.Init("ReLogin");
            IEPool_Manager.instance.WaitTimeToDo("", 3, null, () => {
                waitMask_Ui.ShowResultCase("", 0);
                GetUserMainInfo(loadedAction);
            });
        });
    }

    public void GetAvatarDatas()
    {
        if (avatarData_Group == null && !string.IsNullOrEmpty(getAvatarsUrl))
        {
            if (isLoadingAvatarData)
            {
                return;
            }
            isLoadingAvatarData = true;
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
                            EventManager.Instance.DispatchEvent(GameEventType.LoadedAvatarTexture.ToString(), texture2d, index, avatarData_Group[index].key);
                        }, () =>
                        {
                            avatarData_Group[index].target = (false, null);
                        });

                    }
                }
                isLoadingAvatarData = false;
            }, (code, msg) => {
                isLoadingAvatarData = false;
            });
        }
        EventManager.Instance.Regist(GameEventType.LoadedAvatarTexture.ToString(), this.GetInstanceID(), objects =>
        {
            Texture2D texture2d = (Texture2D)objects[0];
            //int realIndex = (int)objects[1];
            string avatarUrl = (string)objects[2];

            if (appMemberUserInfoRespVO != null && appMemberUserInfoRespVO.avatar == avatarUrl)
            {
                currentAvatar_Texture = texture2d;
            }

        });
    }
    public void SetAvatarRawImageByUrl(RawImage avatar_RawImage,string avatarUrl)
    {
        if (string.IsNullOrEmpty(avatarUrl))
        {
            return;
        }
        if (avatarData_Group.ContainsKey(avatarUrl))
        {
  
            if (avatarData_Group[avatarUrl].Item2 != null)
            {
                avatar_RawImage.texture = avatarData_Group[avatarUrl].Item2;
                return;
            }
            int currentIndex =avatarData_Group.GetIndexByKey(avatarUrl);
            if (UserManager.avatarData_Group[avatarUrl].Item1 == true)
            {

                System.Action<Texture2D, int> loadedAvatarAction = null;
                loadedAvatarAction = (texture2D, index) => {
                    if (index == currentIndex)
                    {
                        if (avatar_RawImage != null)
                        {
                            avatar_RawImage.texture = texture2D;
                        }
                    }
                };
                this.loadedAvatarAction = loadedAvatarAction;
                return;
            }
            else
            {
                GeneralTool_Ctrl.DownloadImage(avatarUrl, (texture2D) =>
                {
                    if (avatar_RawImage != null)
                    {
                        avatar_RawImage.texture = texture2D;
                    }
                    avatarData_Group[currentIndex].target = (true, texture2D);
                    this.loadedAvatarAction?.Invoke(texture2D, currentIndex);
                    EventManager.Instance.DispatchEvent(GameEventType.LoadedAvatarTexture.ToString(), texture2D, currentIndex, avatarData_Group[currentIndex].key);
                }, () =>
                {
                    avatarData_Group[currentIndex].target = (false, null);
                });
            }
        }
        else
        {
            GeneralTool_Ctrl.DownloadImage(avatarUrl, (texture2D) =>
            {
                if (avatar_RawImage != null)
                {
                    avatar_RawImage.texture = texture2D;
                }

            }, () =>
            {
            });
        }
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
        EventManager.Instance.DispatchEvent(GameEventType.GetUserData.ToString());
#if UNITY_EDITOR
        Debug.Log(result);
        Debug.Log("Success Get User info!");
#endif
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
    public float balance {  get; set; }//总余额
    public float commission { get; set; }//佣金余额
    public int experience { get; set; }
    public int userType { get; set; }
    public Level level { get; set; }
    public bool? brokerageEnabled { get; set; }
}
[System.Serializable]
public struct AreaCodeData
{
    public string areaCode;
    public Sprite areaFlag_Sprite;
}