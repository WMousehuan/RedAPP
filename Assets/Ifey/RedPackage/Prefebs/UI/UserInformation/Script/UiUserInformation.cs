using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NativeFilePickerNamespace;
using System.IO;
using UnityEngine.Networking;
using System.Text;
public class Ui_UserInformation : Popup
{
    public enum UserInfoType
    {
        nickname,
        avatar,
        sex,
    }

    public RawImage avatar_RawImage;
    public Texture2D defaultAvatar_Texture;
    public InputField userName_InputField;

    public Button setName_Button;
    public Transform setNameEidit_Case;

    public string uploadUserDataUrl = "/app-api/member/user/update";
    public string updateFileUrl = "/app-api/infra/file/upload";
    public override void Start()
    {
      
        base.Start();
        OnEventSwitchSetNameState(false);
        RefreshUserInformation();
        EventManager.Instance.Regist(typeof(GetUserInfoInterface).ToString(), this.GetInstanceID(), (objects) =>
        {
            string sign = (string)objects[0];
            switch (sign)
            {
                case "GetAvatar":
                    RefreshUserInformation();
                    break;
            }
        });
    }
    private void OnDestroy()
    {
        EventManager.Instance.UnRegist(typeof(GetUserInfoInterface).ToString(), this.GetInstanceID());
    }
    public void RefreshUserInformation()
    {
        userName_InputField.text = UserManager.Instance.appMemberUserInfoRespVO.nickname;
        if (UserManager.Instance.currentAvatar_Texture!=null)
        {
            avatar_RawImage.texture = UserManager.Instance.currentAvatar_Texture;
        }
        else
        {
            avatar_RawImage.texture = defaultAvatar_Texture;
        }

    }

    public void OnEventSetAvatar()
    {

#if UNITY_EDITOR
        NativeFilePicker.PickFile((path) => {
            SetAvatarByPath(path);
        }, "image/*");
#elif PLATFORM_ANDROID
         NativeFilePicker.PickFile((path) => {
          SetAvatarByPath(path);
        }, "image/*");
#elif UNITY_WEBGL
        clickSelectFileBtn(this.gameObject.name);
#endif
    }
    /// <summary>
    /// �ϴ���Ϣ(�����Ĳ� ��Ϣ���ᱻ����)
    /// </summary>
    /// <param name="keyValues"></param>
    public string GetUploadDataString(params (UserInfoType, string)[] keyValues)
    {
        string uploadData = "{";
        for (int i = 0; i < keyValues.Length; i++)
        {
            switch (keyValues[i].Item1)
            {
                case UserInfoType.avatar:
                case UserInfoType.nickname:
                    uploadData += string.Format("\"{0}\":\"{1}\"", keyValues[i].Item1.ToString(), keyValues[i].Item2);
                    break;
                case UserInfoType.sex:
                    uploadData += string.Format("\"{0}\":{1}", keyValues[i].Item1.ToString(), keyValues[i].Item2);
                    break;
            }
        }
        uploadData += "}";

        return uploadData;

    }
    public void SetAvatarByPath(string path)
    {
        if (File.Exists(path))
        {
            print(Path.GetFileName(path));
            // ���ļ�·����ȡͼƬ����
            byte[] imageData = File.ReadAllBytes(path);

            UiWaitMask waitMask_Ui = (UiWaitMask)PopupManager.Instance.Open(PopupType.PopupWaitMask);
            UtilJsonHttp.Instance.PostFileWithParamAuthorizationToken(updateFileUrl + string.Format("?path={0}", Path.GetFileName(path)), Path.GetFileName(path), imageData, new PostAvatarFileInterface(this), (requestData) =>
            {
                JObject json = JObject.Parse(requestData);
                string url = json["data"].Value<string>();
                print(url);
                UtilJsonHttp.Instance.PutContentWithParamAuthorizationToken(uploadUserDataUrl, GetUploadDataString((UserInfoType.avatar, url)), new PostAvatarFileInterface(this), (requestData) =>
                {               
                    // ����Texture2D������ͼƬ����
                    Texture2D texture = new Texture2D(2, 2, TextureFormat.ASTC_8x8, false);
                    texture.name = path;
                    texture.LoadImage(imageData);
                    UserManager.Instance.currentAvatar_Texture = texture;
                    avatar_RawImage.texture = texture;
                    waitMask_Ui.ShowResultCase("Success", 1);
                }, () =>
                {
                    waitMask_Ui.ShowResultCase("Fail", 1);
                });

            }, () =>
            {
                avatar_RawImage.texture = defaultAvatar_Texture;
            });

        }
        else
        {
            Debug.LogWarning("Image file not found: " + path);
        }
    }
    public void OnEventUploadUserName()
    {
        if(userName_InputField.text == UserManager.Instance.appMemberUserInfoRespVO.nickname)
        {
            return;
        }
        UiWaitMask waitMask_Ui = (UiWaitMask)PopupManager.Instance.Open(PopupType.PopupWaitMask);
        UtilJsonHttp.Instance.PutContentWithParamAuthorizationToken(uploadUserDataUrl, GetUploadDataString((UserInfoType.nickname, userName_InputField.text)), new PostAvatarFileInterface(this), (requestData) =>
        {
            UserManager.Instance.appMemberUserInfoRespVO.nickname = userName_InputField.text;
            OnEventSwitchSetNameState(false);
            waitMask_Ui.ShowResultCase("Success", 1);
        }, () =>
        {
            OnEventSwitchSetNameState(false);
            waitMask_Ui.ShowResultCase("Fail", 1);
        });
    }
    public void OnEventSwitchSetNameState(bool isSwitch)
    {
        userName_InputField.interactable = isSwitch;
        if (isSwitch)
        {
            userName_InputField.ActivateInputField();
        }

        setName_Button.gameObject.SetActive(!isSwitch);
        setNameEidit_Case.gameObject.SetActive(isSwitch);
        userName_InputField.text = UserManager.Instance.appMemberUserInfoRespVO.nickname;
    }
}

public class PutUserInformationInterface : HttpInterface
{
    Ui_UserInformation ui_UserInformation;
    public PutUserInformationInterface(Ui_UserInformation source)
    {
        ui_UserInformation = source;
    }
    public void Success(string result)
    {

    }
    public void Fail(JObject json)
    {
        // ʵ�� Fail �������߼�
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
            //user name exits!
            MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "user name exits,Plz choose another");
        }

        else if (code == 406 || code == 1004003000 || code == 400)
        {
            Debug.Log("Login name or psd error!");
            //user name exits!
            MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "Login name or psd error!");
        }

    }

    public void UnknowError(string errorMsg)
    {
        Debug.Log("GetUserDetailInterface UnknowError=" + errorMsg);
    }
}
public class PostAvatarFileInterface : HttpInterface
{
    Ui_UserInformation ui_UserInformation;
    public PostAvatarFileInterface(Ui_UserInformation source)
    {
        ui_UserInformation = source;
    }
    public void Success(string result)
    {

    }
    public void Fail(JObject json)
    {
        // ʵ�� Fail �������߼�
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
            //user name exits!
            MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "user name exits,Plz choose another");
        }

        else if (code == 406 || code == 1004003000 || code == 400)
        {
            Debug.Log("Login name or psd error!");
            //user name exits!
            MonoSingleton<PopupManager>.Instance.OpenCommonPopup(PopupType.PopupCommonAlarm, "Error", "Login name or psd error!");
        }

    }

    public void UnknowError(string errorMsg)
    {
        Debug.Log("GetUserDetailInterface UnknowError=" + errorMsg);
    }
}